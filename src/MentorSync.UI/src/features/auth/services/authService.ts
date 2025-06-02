// filepath: d:\PetProjects\MentorSync\src\MentorSync.UI\src\features\auth\services\authService.ts
import api from "../../../shared/services/api";
import { saveAuthTokens, removeAuthTokens, getAuthTokens } from "./authStorage";

export interface RegisterRequest {
    email: string;
    userName: string;
    role: string;
    password: string;
    confirmPassword: string;
}

export interface LoginRequest {
    email: string;
    password: string;
}

export interface RefreshTokenRequest {
    token: string;
    refreshToken: string;
}

export interface AuthResponse {
    token?: string;
    refreshToken?: string;
    expiration?: string;
    needOnboarding?: boolean;
    success: boolean;
    message?: string;
    userId?: number;
}

// Error type for better error handling
type ApiErrorType = {
    response?: {
        data?: {
            message?: string;
            title?: string;
            detail?: string;
        };
    };
};

export const authService = {
    register: async (data: RegisterRequest): Promise<AuthResponse> => {
        try {
            const response = await api.post("/users/register", data);
            return {
                ...response.data,
                success: true,
            };
        } catch (error) {
            const err = error as ApiErrorType;
            return {
                success: false,
                message:
                    err.response?.data?.message ||
                    err.response?.data?.detail ||
                    "Помилка реєстрації",
            };
        }
    },

    login: async (data: LoginRequest): Promise<AuthResponse> => {
        try {
            const response = await api.post("/users/login", data); // If login was successful, save the tokens to localStorage
            if (response.data.token) {
                saveAuthTokens({
                    token: response.data.token,
                    refreshToken: response.data.refreshToken,
                    expiration: response.data.expiration,
                    needOnboarding: response.data.needOnboarding,
                    userId: response.data.userId,
                });

                // Save the needOnboarding flag to localStorage for easier access across components
                if (response.data.needOnboarding !== undefined) {
                    localStorage.setItem(
                        "needOnboarding",
                        response.data.needOnboarding.toString()
                    );
                }
            }

            return {
                ...response.data,
                success: true,
            };
        } catch (error) {
            const err = error as ApiErrorType;
            return {
                success: false,
                message:
                    err.response?.data?.message ||
                    err.response?.data?.detail ||
                    "Помилка входу",
            };
        }
    },

    refreshToken: async (): Promise<AuthResponse> => {
        try {
            const tokens = getAuthTokens();

            if (!tokens?.token || !tokens?.refreshToken) {
                throw new Error("No tokens available");
            }

            const response = await api.post("/users/refresh-token", {
                token: tokens.token,
                refreshToken: tokens.refreshToken,
            });

            if (response.data.token) {
                saveAuthTokens({
                    token: response.data.token,
                    refreshToken: response.data.refreshToken,
                    expiration: response.data.expiration,
                });

                return {
                    ...response.data,
                    success: true,
                };
            }

            throw new Error("Failed to refresh token");
        } catch (error) {
            const err = error as ApiErrorType;
            // Token refresh failed, logout user
            removeAuthTokens();
            return {
                success: false,
                message:
                    err.response?.data?.message ||
                    err.response?.data?.detail ||
                    "Помилка оновлення токену",
            };
        }
    },
    logout: (): void => {
        removeAuthTokens();
        localStorage.removeItem("needOnboarding");
        localStorage.removeItem("userId");
        localStorage.removeItem("userRole");
    },
};
