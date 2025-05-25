// filepath: d:\PetProjects\MentorSync\src\MentorSync.UI\src\features\auth\services\api.ts
import axios, {
    AxiosError,
    AxiosResponse,
    InternalAxiosRequestConfig,
} from "axios";
import {
    ApiErrorResponse,
    getAuthTokens,
    removeAuthTokens,
} from "./authStorage";

// Create an axios instance with default configuration
const api = axios.create({
    baseURL: import.meta.env.VITE_API_URL || "api",
    headers: {
        "Content-Type": "application/json",
    },
});

// Flag to prevent multiple refresh token requests
let isRefreshing = false;
// Store failed requests to retry them after token refresh
let failedQueue: Array<{
    resolve: (value?: unknown) => void;
    reject: (reason?: unknown) => void;
    config: InternalAxiosRequestConfig;
}> = [];

// Process the failed requests queue
const processQueue = (
    error: AxiosError | null,
    token: string | null = null
) => {
    failedQueue.forEach(({ resolve, reject, config }) => {
        if (!error && token) {
            // Update the Authorization header
            config.headers.Authorization = `Bearer ${token}`;
            resolve(axios(config));
        } else {
            reject(error);
        }
    });

    // Reset the queue
    failedQueue = [];
};

// Request interceptor: Add authorization header with Bearer token if available
api.interceptors.request.use(
    (config: InternalAxiosRequestConfig): InternalAxiosRequestConfig => {
        const authTokens = getAuthTokens();

        if (authTokens?.token) {
            config.headers = config.headers || {};
            config.headers.Authorization = `Bearer ${authTokens.token}`;
        }

        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

// Response interceptor: Format error responses and handle token refresh
api.interceptors.response.use(
    (response: AxiosResponse) => {
        return response;
    },
    async (error: AxiosError) => {
        // Get the config that was used for the request
        const originalRequest = error.config as InternalAxiosRequestConfig & {
            _retry?: boolean;
        };

        if (error.response) {
            const apiError = error.response.data as ApiErrorResponse;

            // Log the error details
            console.error("API Error:", {
                status: apiError.status || error.response.status,
                title: apiError.title || "API Error",
                detail: apiError.detail || error.message,
                type: apiError.type,
                instance: apiError.instance,
            });

            // If the error is due to an expired token (401) and we haven't tried to refresh the token yet
            if (error.response.status === 401 && !originalRequest._retry) {
                if (isRefreshing) {
                    // If already refreshing, add the request to the queue
                    return new Promise((resolve, reject) => {
                        failedQueue.push({
                            resolve,
                            reject,
                            config: originalRequest,
                        });
                    });
                }

                originalRequest._retry = true;
                isRefreshing = true;

                try {
                    // Get the current tokens
                    const tokens = getAuthTokens();

                    if (!tokens?.refreshToken) {
                        // No refresh token, redirect to login
                        removeAuthTokens();
                        isRefreshing = false;
                        processQueue(error, null);

                        // Allow the app to handle the redirect via AuthContext
                        return Promise.reject(error);
                    }

                    // Call the refresh token endpoint
                    const response = await axios.post(
                        `${
                            import.meta.env.VITE_API_URL || "api"
                        }/users/refresh-token`,
                        {
                            token: tokens.token,
                            refreshToken: tokens.refreshToken,
                        }
                    );

                    // Store the new tokens
                    const { token, refreshToken, expiration } = response.data;

                    if (token) {
                        // Update tokens in localStorage
                        localStorage.setItem(
                            "mentorsync_auth",
                            JSON.stringify({ token, refreshToken, expiration })
                        );

                        // Process the queue of failed requests
                        isRefreshing = false;
                        processQueue(null, token);

                        // Retry the original request with the new token
                        originalRequest.headers.Authorization = `Bearer ${token}`;
                        return axios(originalRequest);
                    } else {
                        // Refresh token failed, redirect to login
                        removeAuthTokens();
                        isRefreshing = false;
                        processQueue(error, null);
                    }
                } catch (refreshError) {
                    // Refresh token request failed
                    console.error("Token refresh failed:", refreshError);
                    removeAuthTokens();
                    isRefreshing = false;
                    processQueue(error, null);
                }
            } else if (error.response.status === 403) {
                // Handle forbidden error (e.g., user doesn't have required permissions)
                console.warn(
                    "Access forbidden. User might not have required permissions."
                );
            }
        } else if (error.request) {
            // The request was made but no response was received
            console.error("Network Error:", error.request);
        } else {
            // Something happened in setting up the request
            console.error("Request Error:", error.message);
        }

        return Promise.reject(error);
    }
);

export default api;
