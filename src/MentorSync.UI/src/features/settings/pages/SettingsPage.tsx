import React, { useState } from "react";
import Sidebar from "../../../components/layout/Sidebar";
import Header from "../../../components/layout/Header";
import UserManagement from "../components/UserManagement";
import { hasRole } from "../../auth/utils/authUtils";
import { Navigate } from "react-router-dom";

const SettingsPage: React.FC = () => {
    const [sidebarExpanded, setSidebarExpanded] = useState(false);
    const isAdmin = hasRole("Admin");

    const handleSidebarToggle = (expanded: boolean) => {
        setSidebarExpanded(expanded);
    };

    // Redirect non-admin users
    if (!isAdmin) {
        return <Navigate to="/dashboard" />;
    }

    return (
        <div className="min-h-screen flex bg-[#FFFFFF] overflow-hidden">
            <div
                className={`flex-shrink-0 transition-all duration-300 h-screen ${
                    sidebarExpanded ? "w-[240px]" : "w-[72px]"
                }`}
            >
                <Sidebar onToggle={handleSidebarToggle} activePage="settings" />
            </div>
            <div className="flex flex-col flex-1 overflow-hidden">
                <Header showNotifications={false} />
                <div className="p-6 overflow-y-auto">
                    <h1 className="text-2xl font-medium text-[#1E293B] mb-6">
                        Налаштування
                    </h1>
                    <UserManagement />
                </div>
            </div>
        </div>
    );
};

export default SettingsPage;
