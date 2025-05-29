import React from "react";
import { MentorData, isMentorProfile } from "../types/mentorTypes";
import AboutTab from "./tabs/AboutTab";
import ReviewsTab from "./tabs/ReviewsTab";
import SessionsTab from "./tabs/SessionsTab";
import MaterialsTab from "./tabs/MaterialsTab";
import { hasRole } from "../../auth/utils/authUtils";

// Define tab types
export type ProfileTabType = "about" | "reviews" | "sessions" | "materials";

interface MentorProfileTabsProps {
    mentor: MentorData;
    activeTab: ProfileTabType;
    onTabChange: (tab: ProfileTabType) => void;
    isBookmarked?: boolean;
    onToggleBookmark?: () => void;
    bookmarkLoading?: boolean;
    onRefreshReviews?: () => void;
}

const MentorProfileTabs: React.FC<MentorProfileTabsProps> = ({
    mentor,
    activeTab,
    onTabChange,
    isBookmarked = false,
    onToggleBookmark,
    bookmarkLoading = false,
    onRefreshReviews,
}) => {
    const isMentee = hasRole("Mentee");

    function getReviewWord(count: number): string {
        const n = Math.abs(count) % 100;
        const lastDigit = n % 10;

        if (n > 10 && n < 20) {
            return "відгуків";
        }
        if (lastDigit === 1) {
            return "відгук";
        }
        if (lastDigit >= 2 && lastDigit <= 4) {
            return "відгуки";
        }
        return "відгуків";
    }

    function getYearsWord(count: number): string {
        const n = Math.abs(count) % 100;
        const lastDigit = n % 10;

        if (n > 10 && n < 20) {
            return "років";
        }
        if (lastDigit === 1) {
            return "рік";
        }
        if (lastDigit >= 2 && lastDigit <= 4) {
            return "роки";
        }
        return "років";
    }

    return (
        <div className="bg-white rounded-xl shadow-md overflow-hidden">
            {/* Mentor header information */}
            <div className="p-6 pb-0">
                <div className="flex items-start justify-between">
                    <div className="flex items-start">
                        <img
                            src={
                                mentor.profileImage
                                    ? mentor.profileImage
                                    : "https://ui-avatars.com/api/?name=" +
                                      encodeURIComponent(mentor.name) +
                                      "&background=F3F4F6&color=1E293B&size=64"
                            }
                            alt={mentor.name}
                            className="w-[120px] h-[120px] rounded-full mr-6 border border-[#E2E8F0]"
                        />
                        <div>
                            <div className="flex items-center">
                                <h1 className="text-2xl font-medium text-[#1E293B] mr-4">
                                    {mentor.name}
                                </h1>
                                <span className="bg-[#4318D1] text-white px-4 py-1 rounded-2xl text-sm">
                                    Ментор
                                </span>
                            </div>
                            <div className="flex items-center mt-4">
                                <span className="text-base font-medium text-[#1E293B] mr-1">
                                    {mentor.rating}
                                </span>
                                <div className="flex mr-2">
                                    {[...Array(5)].map((_, i) => (
                                        <span
                                            key={i}
                                            className="material-icons text-yellow-500 text-base"
                                        >
                                            {i < Math.floor(mentor.rating)
                                                ? "star"
                                                : "star_border"}
                                        </span>
                                    ))}
                                </div>{" "}
                                <span className="text-sm text-[#64748B]">
                                    {isMentorProfile(mentor) &&
                                        `(${mentor.reviewCount} ${getReviewWord(
                                            mentor.reviewCount
                                        )})`}
                                </span>
                                <div className="flex items-center ml-8">
                                    <span className="text-sm text-[#64748B]">
                                        {mentor.yearsOfExperience ?? 0}{" "}
                                        {getYearsWord(
                                            mentor.yearsOfExperience ?? 0
                                        )}{" "}
                                        досвіду
                                    </span>
                                </div>
                            </div>

                            <div className="flex gap-2 mt-4">
                                {mentor.skills.slice(0, 3).map((skill) => (
                                    <div
                                        key={skill.id}
                                        className="px-3 py-1 bg-[#F8FAFC] rounded-2xl"
                                    >
                                        <span className="text-xs text-[#1E293B]">
                                            {skill.name}
                                        </span>
                                    </div>
                                ))}
                            </div>
                        </div>
                    </div>
                    {isMentee && onToggleBookmark && (
                        <button
                            onClick={onToggleBookmark}
                            disabled={bookmarkLoading}
                            className="flex items-center justify-center w-10 h-10 rounded-full hover:bg-gray-100 focus:outline-none"
                            title={
                                isBookmarked
                                    ? "Видалити з обраного"
                                    : "Додати до обраного"
                            }
                        >
                            <span
                                className={`material-icons text-2xl ${
                                    isBookmarked
                                        ? "text-yellow-500"
                                        : "text-gray-400"
                                }`}
                            >
                                {isBookmarked ? "bookmark" : "bookmark_outline"}
                            </span>
                        </button>
                    )}
                </div>
            </div>

            {/* Tab navigation */}
            <div className="border-b border-[#E2E8F0] mt-6">
                <div className="flex px-6">
                    <button
                        className={`pb-4 px-3 font-medium text-base transition-colors ${
                            activeTab === "about"
                                ? "text-[#1E293B] border-b-2 border-black"
                                : "text-[#64748B] hover:text-[#1E293B]"
                        }`}
                        onClick={() => onTabChange("about")}
                    >
                        Про
                    </button>
                    <button
                        className={`pb-4 px-3 font-medium text-base transition-colors ${
                            activeTab === "reviews"
                                ? "text-[#1E293B] border-b-2 border-black"
                                : "text-[#64748B] hover:text-[#1E293B]"
                        }`}
                        onClick={() => onTabChange("reviews")}
                    >
                        Відгуки
                    </button>
                    <button
                        className={`pb-4 px-3 font-medium text-base transition-colors ${
                            activeTab === "sessions"
                                ? "text-[#1E293B] border-b-2 border-black"
                                : "text-[#64748B] hover:text-[#1E293B]"
                        }`}
                        onClick={() => onTabChange("sessions")}
                    >
                        Сесії
                    </button>
                    <button
                        className={`pb-4 px-3 font-medium text-base transition-colors ${
                            activeTab === "materials"
                                ? "text-[#1E293B] border-b-2 border-black"
                                : "text-[#64748B] hover:text-[#1E293B]"
                        }`}
                        onClick={() => onTabChange("materials")}
                    >
                        Матеріали
                    </button>
                </div>
            </div>

            {/* Tab content */}
            <div className="p-6">
                {activeTab === "about" && <AboutTab mentor={mentor} />}{" "}
                {activeTab === "reviews" && (
                    <ReviewsTab
                        mentor={mentor}
                        onRefreshReviews={onRefreshReviews}
                    />
                )}
                {activeTab === "sessions" && <SessionsTab mentor={mentor} />}
                {activeTab === "materials" && <MaterialsTab mentor={mentor} />}
            </div>
        </div>
    );
};

export default MentorProfileTabs;
