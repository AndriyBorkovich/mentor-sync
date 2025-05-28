import React, { useState } from "react";
import { useAuth } from "../../auth/context/AuthContext";
import AvailabilityManagement from "../components/AvailabilityManagement";
import { getUserId } from "../../auth";
import Sidebar from "../../../components/layout/Sidebar";
import Header from "../../../components/layout/Header";

const MyAvailabilityPage: React.FC = () => {
    const { isAuthenticated } = useAuth();
    const userId = getUserId();

    const [sidebarExpanded, setSidebarExpanded] = useState(false);

    const handleSidebarToggle = () => {
        setSidebarExpanded(!sidebarExpanded);
    };

    if (!isAuthenticated) {
        return (
            <div className="container mx-auto p-4">
                <div className="bg-red-50 border-l-4 border-red-400 p-4">
                    <div className="flex">
                        <div className="flex-shrink-0">
                            <span className="material-icons text-red-400">
                                error
                            </span>
                        </div>
                        <div className="ml-3">
                            <p className="text-sm text-red-700">
                                Ви маєте бути авторизовані для доступу до цієї
                                сторінки.
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        );
    }

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
                    <h1 className="text-2xl font-bold text-[#1E293B] mb-6">
                        Мої доступні слоти часу
                    </h1>
                    <AvailabilityManagement mentorId={Number(userId)} />
                </div>
            </div>
        </div>
    );
};

export default MyAvailabilityPage;
