import React, {
    createContext,
    useContext,
    useState,
    useEffect,
    ReactNode,
    useCallback,
} from "react";
import { getAuthTokens } from "../services/authStorage";
import { authService } from "../services/authService";

interface AuthContextType {
    isAuthenticated: boolean;
    isLoading: boolean;
    needsOnboarding: boolean;
    logout: () => void;
    refreshAuthToken: () => Promise<boolean>;
    user: unknown | null; // Replace with your user type
}

const defaultAuthContext: AuthContextType = {
    isAuthenticated: false,
    isLoading: true,
    needsOnboarding: false,
    logout: () => {},
    refreshAuthToken: async () => false,
    user: null,
};

// eslint-disable-next-line react-refresh/only-export-components
export const AuthContext = createContext<AuthContextType>(defaultAuthContext);

// eslint-disable-next-line react-refresh/only-export-components
export const useAuth = () => useContext(AuthContext);

interface AuthProviderProps {
    children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [needsOnboarding, setNeedsOnboarding] = useState<boolean>(false);
    const [user, setUser] = useState<unknown | null>(null);

    // Function to check if the token is valid and not expired
    const isTokenValid = useCallback((expiration?: string): boolean => {
        if (!expiration) return false;

        const expirationDate = new Date(expiration);
        const now = new Date();

        // Add some buffer (5 min) to avoid edge cases
        const bufferMs = 5 * 60 * 1000;
        return expirationDate.getTime() > now.getTime() + bufferMs;
    }, []);

    // Function to refresh the auth token
    const refreshAuthToken = useCallback(async (): Promise<boolean> => {
        try {
            const result = await authService.refreshToken();
            if (result.success && result.token) {
                setIsAuthenticated(true);
                // Check if onboarding status is in the refresh response
                if (result.needOnboarding !== undefined) {
                    setNeedsOnboarding(result.needOnboarding);
                }
                return true;
            }

            setIsAuthenticated(false);
            return false;
        } catch (error) {
            console.error("Failed to refresh token:", error);
            setIsAuthenticated(false);
            return false;
        }
    }, []);

    // Function to verify user authentication state
    const verifyAuth = useCallback(async (): Promise<void> => {
        setIsLoading(true);

        try {
            const tokens = getAuthTokens();

            if (tokens?.token) {
                if (tokens.expiration && isTokenValid(tokens.expiration)) {
                    // Token is valid and not expired
                    setIsAuthenticated(true);
                    // Check if onboarding flag is stored in tokens
                    if (tokens.needOnboarding !== undefined) {
                        setNeedsOnboarding(tokens.needOnboarding);
                    } else {
                        // Otherwise check localStorage for a stored value
                        const needsOnboardingStr =
                            localStorage.getItem("needOnboarding");
                        if (needsOnboardingStr !== null) {
                            setNeedsOnboarding(needsOnboardingStr === "true");
                        }
                    }
                    // You could fetch user profile data here
                } else if (tokens.refreshToken) {
                    // Token is expired but we have a refresh token
                    const refreshSuccess = await refreshAuthToken();
                    setIsAuthenticated(refreshSuccess);
                } else {
                    // No valid tokens
                    setIsAuthenticated(false);
                }
            } else {
                setIsAuthenticated(false);
            }
        } catch (error) {
            console.error("Auth verification failed:", error);
            setIsAuthenticated(false);
        } finally {
            setIsLoading(false);
        }
    }, [isTokenValid, refreshAuthToken]);

    useEffect(() => {
        // Check if user is authenticated on mount
        verifyAuth();

        // Setup interval to periodically check token validity
        const authCheckInterval = setInterval(() => {
            const tokens = getAuthTokens();
            if (tokens?.expiration && !isTokenValid(tokens.expiration)) {
                refreshAuthToken();
            }
        }, 5 * 60 * 1000); // Check every 5 minutes

        return () => clearInterval(authCheckInterval);
    }, [isTokenValid, refreshAuthToken, verifyAuth]);

    const logout = useCallback(() => {
        authService.logout();
        localStorage.removeItem("needOnboarding");
        localStorage.removeItem("userId");
        localStorage.removeItem("userRole");
        setIsAuthenticated(false);
        setNeedsOnboarding(false);
        setUser(null);
    }, []);

    return (
        <AuthContext.Provider
            value={{
                isAuthenticated,
                isLoading,
                needsOnboarding,
                logout,
                refreshAuthToken,
                user,
            }}
        >
            {children}
        </AuthContext.Provider>
    );
};
