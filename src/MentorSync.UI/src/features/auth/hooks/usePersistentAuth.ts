import { useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

/**
 * Hook for handling persistent authentication logic across page refreshes
 * and redirecting based on authentication state.
 */
export const usePersistentAuth = (
    redirectWhenAuthed?: string,
    redirectWhenNotAuthed?: string
) => {
    const { isAuthenticated, isLoading } = useAuth();
    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
        // Only redirect when auth state is confirmed (not loading)
        if (!isLoading) {
            if (isAuthenticated && redirectWhenAuthed) {
                navigate(redirectWhenAuthed, { replace: true });
            } else if (!isAuthenticated && redirectWhenNotAuthed) {
                // Save the current location to redirect back after authentication
                navigate(redirectWhenNotAuthed, {
                    replace: true,
                    state: { from: location },
                });
            }
        }
    }, [
        isAuthenticated,
        isLoading,
        navigate,
        location,
        redirectWhenAuthed,
        redirectWhenNotAuthed,
    ]);

    return { isAuthenticated, isLoading };
};
