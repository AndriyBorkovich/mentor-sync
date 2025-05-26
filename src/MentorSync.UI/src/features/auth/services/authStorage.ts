/**
 * Interface representing the API error response model
 */
export interface ApiErrorResponse {
    type: string;
    title: string;
    status: number;
    detail: string;
    instance: string;
    [key: string]: unknown; // For additional properties
}

/**
 * Interface for auth token storage
 */
export interface AuthTokens {
    token: string;
    refreshToken?: string;
    expiration?: string;
    userId?: number;
    needOnboarding?: boolean;
}

/**
 * Storage key for auth tokens in localStorage
 */
export const AUTH_STORAGE_KEY = "mentorsync_auth";

/**
 * Get stored auth tokens from localStorage
 */
export const getAuthTokens = (): AuthTokens | null => {
    try {
        const tokens = localStorage.getItem(AUTH_STORAGE_KEY);
        if (!tokens) return null;
        return JSON.parse(tokens);
    } catch (error) {
        console.error("Failed to parse auth tokens from localStorage", error);
        return null;
    }
};

/**
 * Save auth tokens to localStorage
 */
export const saveAuthTokens = (tokens: AuthTokens): void => {
    localStorage.setItem(AUTH_STORAGE_KEY, JSON.stringify(tokens));
};

/**
 * Remove auth tokens from localStorage
 */
export const removeAuthTokens = (): void => {
    localStorage.removeItem(AUTH_STORAGE_KEY);
};
