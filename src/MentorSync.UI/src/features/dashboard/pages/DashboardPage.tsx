import React, { useState } from "react";
import Sidebar from "../../../components/layout/Sidebar";
import Header from "../../../components/layout/Header";
import DashboardContent from "../components/DashboardContent";
import "../../../components/layout/styles/logo.css";
import "../../../components/layout/styles/sidebar.css";

const DashboardPage: React.FC = () => {
    // Auth context can be used later when needed
    const [sidebarExpanded, setSidebarExpanded] = useState(false);

    const handleSidebarToggle = () => {
        setSidebarExpanded(!sidebarExpanded);
    };
    return (
        <div className="min-h-screen flex bg-[#FFFFFF] overflow-hidden">
            <div
                className={`flex-shrink-0 transition-all duration-300 h-screen ${
                    sidebarExpanded ? "w-[240px]" : "w-[72px]"
                }`}
            >
                {" "}
                <Sidebar onToggle={handleSidebarToggle} activePage="home" />
            </div>
            <div className="flex flex-col flex-1">
                <Header />
                <DashboardContent />
            </div>
        </div>
    );
};

export default DashboardPage;
