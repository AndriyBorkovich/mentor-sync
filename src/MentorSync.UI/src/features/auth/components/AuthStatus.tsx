import React from "react";
import { useAuth } from "../context/AuthContext";
import { useUserProfile } from "../hooks/useUserProfile";
import { getTokenTimeRemaining } from "../utils/authUtils";
import { Link } from "react-router-dom";

/**
 * Component that displays the current authentication status and user information
 */
export const AuthStatus: React.FC = () => {
    const { isAuthenticated, logout } = useAuth();
    const { profile } = useUserProfile();
    const tokenTime = getTokenTimeRemaining();

    if (!isAuthenticated) {
        return (
            <div className="flex items-center space-x-4">
                <Link
                    to="/login"
                    className="text-primary hover:text-primary-dark"
                >
                    Login
                </Link>
                <Link
                    to="/register"
                    className="px-4 py-2 bg-primary text-white rounded hover:bg-primary-dark"
                >
                    Register
                </Link>
            </div>
        );
    }

    return (
        <div className="relative group">
            <button className="flex items-center space-x-2">
                <div className="w-10 h-10 rounded-full bg-gray-200 flex items-center justify-center text-gray-700">
                    {profile?.userName?.charAt(0)?.toUpperCase() || "U"}
                </div>
                <span>{profile?.userName || "User"}</span>
                <svg
                    xmlns="http://www.w3.org/2000/svg"
                    className="h-5 w-5"
                    viewBox="0 0 20 20"
                    fill="currentColor"
                >
                    <path
                        fillRule="evenodd"
                        d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z"
                        clipRule="evenodd"
                    />
                </svg>
            </button>

            {/* Dropdown menu */}
            <div className="absolute right-0 mt-2 w-48 bg-white rounded-md shadow-lg py-1 z-10 hidden group-hover:block">
                <div className="px-4 py-2 text-sm text-gray-700 border-b">
                    <div className="font-medium">{profile?.email}</div>
                    <div className="text-xs text-gray-500">
                        Role: {profile?.role}
                    </div>
                    {tokenTime && (
                        <div className="text-xs text-gray-500">
                            Session expires in: {Math.floor(tokenTime / 60)} min
                        </div>
                    )}
                </div>
                <Link
                    to="/dashboard"
                    className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
                >
                    Dashboard
                </Link>
                <Link
                    to="/profile"
                    className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
                >
                    Profile
                </Link>
                <button
                    onClick={logout}
                    className="block w-full text-left px-4 py-2 text-sm text-red-600 hover:bg-gray-100"
                >
                    Sign out
                </button>
            </div>
        </div>
    );
};
