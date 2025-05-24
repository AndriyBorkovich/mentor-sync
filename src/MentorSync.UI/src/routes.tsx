// Route configuration for the application
import { createBrowserRouter } from "react-router-dom";
import LandingPage from "./features/landing/pages/LandingPage";

const router = createBrowserRouter([
    {
        path: "/",
        element: <LandingPage />,
    },
    // Other routes will be added here as needed
]);

export default router;
