import axios, {
    AxiosError,
    AxiosResponse,
    InternalAxiosRequestConfig,
} from "axios";
import { toast } from "react-toastify";
import {
    ApiErrorResponse,
    getAuthTokens,
    removeAuthTokens,
} from "../../features/auth/services/authStorage";

// Create an axios instance with default configuration
const api = axios.create({
    baseURL: import.meta.env.VITE_API_URL || "api",
    headers: {
        "Content-Type": "application/json",
    },
});

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

// Response interceptor: Format error responses
api.interceptors.response.use(
    (response: AxiosResponse) => {
        return response;
    },
    (error: AxiosError) => {
        if (error.response) {
            const apiError = error.response.data as ApiErrorResponse;
            const errorMessage =
                apiError.detail ||
                apiError.title ||
                "Виникла помилка при виконанні запиту";

            // Log the error details
            console.error("API Error:", {
                status: apiError.status || error.response.status,
                title: apiError.title || "API Error",
                detail: apiError.detail || error.message,
                type: apiError.type,
                instance: apiError.instance,
            });

            // Handle authentication errors (401)
            if (error.response.status === 401) {
                console.warn("Authentication error: Token may have expired");
                // Simply remove tokens and let the app handle redirect via AuthContext
                removeAuthTokens();
                toast.error("Сесія закінчилася. Будь ласка, увійдіть знову.");
            } else if (error.response.status === 403) {
                // Handle forbidden error (e.g., user doesn't have required permissions)
                console.warn(
                    "Access forbidden. User might not have required permissions."
                );
                toast.error("У вас немає доступу до цього ресурсу.");
            } else if (error.response.status === 404) {
                toast.error("Ресурс не знайдено.");
            } else if (error.response.status >= 500) {
                toast.error("Помилка сервера. Будь ласка, спробуйте пізніше.");
            } else {
                // Show a toast with the specific error message for other status codes
                toast.error(errorMessage);
            }
        } else if (error.request) {
            // The request was made but no response was received
            console.error("Network Error:", error.request);
            toast.error("Помилка мережі. Перевірте ваше з'єднання.");
        } else {
            // Something happened in setting up the request
            console.error("Request Error:", error.message);
            toast.error("Виникла помилка при виконанні запиту.");
        }

        return Promise.reject(error);
    }
);

export default api;
