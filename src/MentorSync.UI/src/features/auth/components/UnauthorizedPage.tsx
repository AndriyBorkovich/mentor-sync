import React from "react";
import { Link } from "react-router-dom";

/**
 * Component displayed when a user tries to access a resource they don't have permission for
 */
export const UnauthorizedPage: React.FC = () => {
    return (
        <div className="flex flex-col items-center justify-center min-h-screen px-4">
            <div className="text-center">
                <h1 className="text-6xl font-bold text-red-500 mb-4">403</h1>
                <h2 className="text-3xl mb-8">Unauthorized Access</h2>
                <p className="text-lg text-gray-600 mb-6">
                    You don't have permission to access this page.
                </p>
                <Link
                    to="/dashboard"
                    className="inline-block py-3 px-6 bg-primary text-white rounded-lg hover:bg-primary-dark transition-colors"
                >
                    Return to Dashboard
                </Link>
            </div>
        </div>
    );
};
