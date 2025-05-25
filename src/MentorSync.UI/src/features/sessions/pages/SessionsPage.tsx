import React, { useState } from "react";
import Sidebar from "../../../components/layout/Sidebar";
import Header from "../../../components/layout/Header";
import SessionsContent from "../components/SessionsContent";
import "../../../components/layout/styles/logo.css";
import "../../../components/layout/styles/sidebar.css";

const SessionsPage: React.FC = () => {
    const [sidebarExpanded, setSidebarExpanded] = useState(false);

    const handleSidebarToggle = (expanded: boolean) => {
        setSidebarExpanded(expanded);
    };

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
