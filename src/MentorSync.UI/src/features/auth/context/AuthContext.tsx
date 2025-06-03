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

// Define User interface
export interface User {
    id: string | number;
    // Add any other user properties that your application uses
}

interface AuthContextType {
    isAuthenticated: boolean;
    isLoading: boolean;
    needsOnboarding: boolean;
    logout: () => void;
    loginUser: (response: any) => void; // Function to handle login
    refreshAuthToken: () => Promise<boolean>;
    verifyAuthStatus: () => Promise<void>; // Function to manually verify auth status
    user: User | null; // Updated to use the User interface
}

const defaultAuthContext: AuthContextType = {
    isAuthenticated: false,
    isLoading: true,
    needsOnboarding: false,
    logout: () => {},
    loginUser: () => {}, // Default empty function
    verifyAuthStatus: async () => {}, // Default empty function
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
    const [user, setUser] = useState<User | null>(null);

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
    }, []); // Function to verify user authentication state
    const verifyAuth = useCallback(async (): Promise<void> => {
        setIsLoading(true);
        console.log("Verifying authentication state...");

        try {
            const tokens = getAuthTokens();
            console.log(
                "Auth tokens from storage:",
                tokens
                    ? {
                          hasToken: !!tokens.token,
                          hasRefreshToken: !!tokens.refreshToken,
                          expiration: tokens.expiration,
                          userId: tokens.userId,
                          needsOnboarding: tokens.needOnboarding,
                      }
                    : "No tokens found"
            );

            if (tokens?.token) {
                if (tokens.expiration && isTokenValid(tokens.expiration)) {
                    // Token is valid and not expired
                    console.log(
                        "Token is valid - setting authenticated state to true"
                    );
                    setIsAuthenticated(true);

                    // Set user if userId exists
                    if (tokens.userId) {
                        setUser({ id: tokens.userId });
                    }

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
                } else if (tokens.refreshToken) {
                    // Token is expired but we have a refresh token
                    console.log("Token expired, attempting to refresh...");
                    const refreshSuccess = await refreshAuthToken();
                    console.log(
                        "Token refresh result:",
                        refreshSuccess ? "success" : "failed"
                    );
                    setIsAuthenticated(refreshSuccess);
                } else {
                    // No valid tokens
                    console.log("No valid tokens found");
                    setIsAuthenticated(false);
                }
            } else {
                console.log(
                    "No auth token found - setting authenticated state to false"
                );
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
    }, [isTokenValid, refreshAuthToken, verifyAuth]); // New function to handle login
    const loginUser = useCallback((response: any) => {
        if (response?.success && response?.token) {
            console.log(
                "loginUser called: Updating auth state to authenticated"
            );
            setIsAuthenticated(true);
            if (response.needOnboarding !== undefined) {
                setNeedsOnboarding(response.needOnboarding);
            }
            if (response.userId) {
                setUser({ id: response.userId });
            }
            console.log("Auth state updated after login: authenticated=true");
        } else {
            console.warn("loginUser called with invalid response:", response);
        }
    }, []);

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
                loginUser,
                verifyAuthStatus: verifyAuth, // Expose verifyAuth as verifyAuthStatus
                refreshAuthToken,
                user,
            }}
        >
            {children}
        </AuthContext.Provider>
    );
};
