import { getAuthTokens } from "../services/authStorage";
import { jwtDecode } from "jwt-decode";

/**
 * Interface for JWT payload structure
 */
interface JwtPayload {
    sub?: string; // subject (user ID)
    email?: string;
    role?: string;
    exp?: number; // expiration timestamp
    [key: string]: unknown; // other claims
}

/**
 * Check if the current token is valid and not expired
 */
export const isTokenValid = (): boolean => {
    try {
        const tokens = getAuthTokens();
        if (!tokens?.token) return false;

        const decodedToken = jwtDecode<JwtPayload>(tokens.token);
        if (!decodedToken.exp) return false;

        // Convert exp to milliseconds and check if expired
        // Add a buffer of 5 minutes to avoid edge cases
        const bufferMs = 5 * 60 * 1000;
        return decodedToken.exp * 1000 > Date.now() + bufferMs;
    } catch (error) {
        console.error("Error validating token:", error);
        return false;
    }
};

/**
 * Extract user information from the JWT token
 */
export const getUserFromToken = (): JwtPayload | null => {
    try {
        const tokens = getAuthTokens();
        if (!tokens?.token) return null;

        return jwtDecode<JwtPayload>(tokens.token);
    } catch (error) {
        console.error("Error decoding token:", error);
        return null;
    }
};

export const getUserId = (): number => {
    // First try to get the ID from the JWT token
    const tokenPayload = getUserFromToken();
    const nameIdentifier =
        tokenPayload?.[
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
        ];
    if (nameIdentifier) {
        return parseInt(String(nameIdentifier), 10);
    }

    return parseInt(localStorage.getItem("userId") || "0", 10);
};

export const getUserRole = (): string => {
    // First try to get the ID from the JWT token
    const user = getUserFromToken();
    const tokenRole =
        user?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

    return typeof tokenRole === "string"
        ? tokenRole
        : localStorage.getItem("userRole") || "";
};

/**
 * Get the remaining time in seconds before the token expires
 */
export const getTokenTimeRemaining = (): number | null => {
    try {
        const tokens = getAuthTokens();
        if (!tokens?.token) return null;

        const decodedToken = jwtDecode<JwtPayload>(tokens.token);
        if (!decodedToken.exp) return null;

        const remaining = decodedToken.exp * 1000 - Date.now();
        return remaining > 0 ? Math.floor(remaining / 1000) : 0;
    } catch (error) {
        console.error("Error calculating token time:", error);
        return null;
    }
};

/**
 * Check if the current user has a specific role
 */
export const hasRole = (role: string | string[]): boolean => {
    try {
        const user = getUserFromToken();
        const tokenRole =
            user?.[
                "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
            ];
        if (!tokenRole) return false;

        const roles = Array.isArray(role) ? role : [role];
        return typeof tokenRole === "string" && roles.includes(tokenRole);
    } catch (error) {
        console.error("Error checking user role:", error);
        return false;
    }
};
