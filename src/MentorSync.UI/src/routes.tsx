// Route configuration for the application
import { createBrowserRouter } from "react-router-dom";
import LandingPage from "./features/landing/pages/LandingPage";
import RegisterPage from "./features/auth/pages/RegisterPage";
import LoginPage from "./features/auth/pages/LoginPage";
import DashboardPage from "./features/dashboard/pages/DashboardPage";
import MentorSearchPage from "./features/mentor-search/pages/MentorSearchPage";
import MentorProfilePage from "./features/mentor-profile/pages/MentorProfilePage";
import SessionsPage from "./features/sessions/pages/SessionsPage";
import MessagesPage from "./features/messages/pages/MessagesPage";
import { ProtectedRoute } from "./features/auth/components/ProtectedRoute";
import { RoleBasedRoute } from "./features/auth/components/RoleBasedRoute";
import { UnauthorizedPage } from "./features/auth/components/UnauthorizedPage";

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
            // Routes that require specific roles
            {
                element: (
                    <RoleBasedRoute
                        allowedRoles={["Mentor"]}
                        redirectTo="/unauthorized"
                    />
                ),
                children: [
                    // Add mentor-specific routes here
                    // {
                    //     path: "/mentor/schedule",
                    //     element: <MentorSchedulePage />,
                    // },
                ],
            },
            {
                element: (
                    <RoleBasedRoute
                        allowedRoles={["Student"]}
                        redirectTo="/unauthorized"
                    />
                ),
                children: [
                    // Add student-specific routes here
                    // {
                    //     path: "/student/mentors",
                    //     element: <BrowseMentorsPage />,
                    // },
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
                    // Add admin-specific routes here
                    // {
                    //     path: "/admin/users",
                    //     element: <ManageUsersPage />,
                    // },
                ],
            },
        ],
    },
    // Other public routes will be added here as needed
]);

export default router;
