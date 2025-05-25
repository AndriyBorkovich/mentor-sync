import React from "react";
import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { useUserProfile } from "../hooks/useUserProfile";

interface RoleBasedRouteProps {
    allowedRoles: string[];
    redirectTo?: string;
}

/**
 * A component that checks if the authenticated user has the required role before rendering children.
 * If the user doesn't have the required role, they are redirected.
 */
export const RoleBasedRoute: React.FC<RoleBasedRouteProps> = ({
    allowedRoles,
    redirectTo = "/dashboard",
}) => {
    const { isAuthenticated, isLoading } = useAuth();
    const { profile, loading } = useUserProfile();

    // Show loading state while checking authentication or profile
    if (isLoading || loading) {
        return (
            <div className="flex items-center justify-center min-h-screen">
                <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-primary"></div>
            </div>
        );
    }

    // If not authenticated, redirect to login
    if (!isAuthenticated) {
        return <Navigate to="/login" replace />;
    }

    // If user's role is not in the allowed roles, redirect
    if (profile && !allowedRoles.includes(profile.role)) {
        return <Navigate to={redirectTo} replace />;
    }

    // If authenticated and role is allowed, render child routes
    return <Outlet />;
};
