import React, { useState } from "react";
import Sidebar from "../../../components/layout/Sidebar";
import Header from "../../../components/layout/Header";
import MentorProfileContainerOptimized from "../components/MentorProfileContainerOptimized";

const MentorProfilePage: React.FC = () => {
    const [sidebarExpanded, setSidebarExpanded] = useState(false);

    const handleSidebarToggle = () => {
        setSidebarExpanded(!sidebarExpanded);
    };

    return (
        <div className="flex h-screen bg-[#F8FAFC]">
            <div
                className={`flex-shrink-0 transition-all duration-300 h-screen ${
                    sidebarExpanded ? "w-[240px]" : "w-[72px]"
                }`}
            >
                <Sidebar onToggle={handleSidebarToggle} activePage="search" />
            </div>
            <div className="flex flex-col flex-1 h-screen overflow-hidden">
                <Header />
                <div className="flex-1 overflow-y-auto p-6">
                    <MentorProfileContainerOptimized />
                </div>
            </div>
        </div>
    );
};

export default MentorProfilePage;
