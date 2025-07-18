// Route configuration for the application
import { createBrowserRouter } from "react-router-dom";
import LandingPage from "./features/landing/pages/LandingPage";
import RegisterPage from "./features/auth/pages/RegisterPage";
import LoginPage from "./features/auth/pages/LoginPage";
import DashboardPage from "./features/dashboard/pages/DashboardPage";
import MentorSearchPage from "./features/mentor-search/pages/MentorSearchPage";
import MentorProfilePage from "./features/mentor-profile/pages/MentorProfilePage";
import MyAvailabilityPage from "./features/mentor-profile/pages/MyAvailabilityPage";
import SessionsPage from "./features/sessions/pages/SessionsPage";
import MessagesPage from "./features/messages/pages/MessagesPage";
import MaterialsPage from "./features/materials/pages/MaterialsPage";
import MaterialViewPage from "./features/materials/pages/MaterialViewPage";
import ProfilePage from "./features/profile/pages/ProfilePage";
import MentorProfileEditPage from "./features/profile/pages/MentorProfileEditPage";
import MenteeProfileEditPage from "./features/profile/pages/MenteeProfileEditPage";
import SettingsPage from "./features/settings/pages/SettingsPage";
import { OnboardingPage } from "./features/onboarding";
import { ProtectedRoute } from "./features/auth/components/ProtectedRoute";
import { RoleBasedRoute } from "./features/auth/components/RoleBasedRoute";
import { UnauthorizedPage } from "./features/auth/components/UnauthorizedPage";
import { Navigate } from "react-router-dom";

const router = createBrowserRouter([
    {
        path: "/",
        element: <LandingPage />,
    },
    {
        path: "/register",
        element: <RegisterPage />,
    },
    {
        path: "/login",
        element: <LoginPage />,
    },
    {
        path: "/unauthorized",
        element: <UnauthorizedPage />,
    },
    {
        // Protected routes for all authenticated users
        element: <ProtectedRoute />,
        children: [
            {
                path: "/dashboard",
                element: <DashboardPage />,
            },
            {
                path: "/mentors",
                element: <MentorSearchPage />,
            },
            {
                path: "/mentors/:mentorId",
                element: <MentorProfilePage />,
            },
            {
                path: "/sessions",
                element: <SessionsPage />,
            },
            {
                path: "/messages",
                element: <MessagesPage />,
            },
            {
                path: "/materials",
                element: <MaterialsPage />,
            },
            {
                path: "/materials/:materialId",
                element: <MaterialViewPage />,
            },
            {
                path: "/profile",
                element: <ProfilePage />,
            },
            {
                element: (
                    <RoleBasedRoute
                        allowedRoles={["Mentor", "Admin"]}
                        redirectTo="/unauthorized"
                    />
                ),
                children: [
                    {
                        path: "/profile/edit/mentor",
                        element: <MentorProfileEditPage />,
                    },
                ],
            },
            {
                element: (
                    <RoleBasedRoute
                        allowedRoles={["Mentee", "Admin"]}
                        redirectTo="/unauthorized"
                    />
                ),
                children: [
                    {
                        path: "/profile/edit/mentee",
                        element: <MenteeProfileEditPage />,
                    },
                ],
            },
            {
                path: "/bookings",
                element: <Navigate to="/sessions" replace />,
            },
            // Routes that require specific roles
            {
                element: (
                    <RoleBasedRoute
                        allowedRoles={["Mentor"]}
                        redirectTo="/unauthorized"
                    />
                ),
                children: [
                    // Mentor-specific routes
                    {
                        path: "/mentor/availability",
                        element: <MyAvailabilityPage />,
                    },
                ],
            },
            {
                element: (
                    <RoleBasedRoute
                        allowedRoles={["Mentee"]}
                        redirectTo="/unauthorized"
                    />
                ),
                children: [
                    // Mentee-specific routes
                    {
                        path: "/mentee/bookings",
                        element: <Navigate to="/sessions" replace />,
                    },
                ],
            },
            {
                element: (
                    <RoleBasedRoute
                        allowedRoles={["Admin"]}
                        redirectTo="/unauthorized"
                    />
                ),
                children: [
                    // Admin-specific routes
                    {
                        path: "/settings",
                        element: <SettingsPage />,
                    },
                ],
            },
        ],
    }, // Onboarding routes
    {
        path: "/onboarding/:role",
        element: <ProtectedRoute />,
        children: [
            {
                path: "",
                element: <OnboardingPage />,
            },
        ],
    },
]);

export default router;
