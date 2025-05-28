import React, { useState } from "react";
import Sidebar from "../../../components/layout/Sidebar";
import Header from "../../../components/layout/Header";
import { Outlet } from "react-router-dom";
import { hasRole } from "../../auth/utils/authUtils";

const BookingsLayoutPage: React.FC = () => {
    const [sidebarExpanded, setSidebarExpanded] = useState(false);
    const isMentee = hasRole("Mentee");
    const isMentor = hasRole("Mentor");

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
                <Sidebar
                    onToggle={handleSidebarToggle}
                    activePage={
                        isMentee
                            ? "menteeBookings"
                            : isMentor
                            ? "mentorBookings"
                            : "home"
                    }
                />
            </div>
            <div className="flex flex-col flex-1 h-screen overflow-hidden">
                <Header />
                <div className="flex-1 overflow-y-auto p-6">
                    <Outlet />
                </div>
            </div>
        </div>
    );
};

export default BookingsLayoutPage;
