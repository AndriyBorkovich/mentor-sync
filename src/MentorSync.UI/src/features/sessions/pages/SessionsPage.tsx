import React, { useState } from "react";
import Sidebar from "../../../components/layout/Sidebar";
import Header from "../../../components/layout/Header";
import SessionsContent from "../components/SessionsContent";
import "../../../components/layout/styles/logo.css";
import "../../../components/layout/styles/sidebar.css";
import { useAuth } from "../../auth/context/AuthContext";
import { Navigate } from "react-router-dom";

const SessionsPage: React.FC = () => {
    const [sidebarExpanded, setSidebarExpanded] = useState(false);
    const { isAuthenticated, isLoading } = useAuth();

    const handleSidebarToggle = (expanded: boolean) => {
        setSidebarExpanded(expanded);
    };

    // Wait for authentication check to complete
    if (isLoading) {
        return (
            <div className="flex items-center justify-center min-h-screen">
                <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-[#4318D1]"></div>
            </div>
        );
    }

    // Redirect to login if not authenticated
    if (!isAuthenticated) {
        return <Navigate to="/login" replace />;
    }

    return (
        <div className="min-h-screen flex bg-[#FFFFFF] overflow-hidden">
            <div
                className={`flex-shrink-0 transition-all duration-300 h-screen ${
                    sidebarExpanded ? "w-[240px]" : "w-[72px]"
                }`}
            >
                <Sidebar onToggle={handleSidebarToggle} activePage="sessions" />
            </div>
            <div className="flex flex-col flex-1">
                <Header />
                <SessionsContent />
            </div>
        </div>
    );
};

export default SessionsPage;
