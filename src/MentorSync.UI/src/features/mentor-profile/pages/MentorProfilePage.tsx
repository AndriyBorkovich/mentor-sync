import React, { useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import Sidebar from "../../../components/layout/Sidebar";
import Header from "../../../components/layout/Header";
import { recommendedMentors } from "../../dashboard/data/mentors";
import MentorProfileTabs from "../components/MentorProfileTabs";

// Define tab types for the profile
type ProfileTabType = "about" | "reviews" | "sessions" | "materials";

const MentorProfilePage: React.FC = () => {
    const { mentorId } = useParams<{ mentorId: string }>();
    const navigate = useNavigate();
    const [sidebarExpanded, setSidebarExpanded] = useState(false);
    const [activeTab, setActiveTab] = useState<ProfileTabType>("about");

    // Find the mentor by ID from mock data
    const mentor = recommendedMentors.find((m) => m.id === mentorId);

    // If mentor is not found, redirect to the mentor search page
    if (!mentor) {
        navigate("/mentors");
        return null;
    }

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
                    <MentorProfileTabs
                        mentor={mentor}
                        activeTab={activeTab}
                        onTabChange={setActiveTab}
                    />
                </div>
            </div>
        </div>
    );
};

export default MentorProfilePage;
