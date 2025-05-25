import React from "react";
import Sidebar from "../components/Sidebar";
import Header from "../components/Header";
import DashboardContent from "../components/DashboardContent";
import "../styles/logo.css";

const DashboardPage: React.FC = () => {
    // Auth context can be used later when needed
    return (
        <div className="min-h-screen flex bg-[#FFFFFF]">
            <Sidebar />
            <div className="flex flex-col flex-1">
                <Header />
                <DashboardContent />
            </div>
        </div>
    );
};

export default DashboardPage;
