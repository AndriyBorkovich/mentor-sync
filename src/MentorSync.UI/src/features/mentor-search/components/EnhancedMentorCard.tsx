import React from "react";
import { Link } from "react-router-dom";
import { hasRole } from "../../auth/utils/authUtils";
import { Mentor, RecommendedMentor } from "../../../shared/types";

// Enhanced MentorCard component without save/bookmark functionality
export interface MentorCardProps {
    mentor: Mentor | RecommendedMentor;
    showRecommendationScores?: boolean;
}
export const EnhancedMentorCard: React.FC<MentorCardProps> = ({
    mentor,
    showRecommendationScores = false,
}) => {
    const isMentee = hasRole("Mentee");
    const isRecommendedMentor = "collaborativeScore" in mentor;

    return (
        <div className="bg-white rounded-2xl shadow-md p-6 mb-6 relative">
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
                    className="w-16 h-16 rounded-full mr-4"
                />
                <div className="flex-1">
                    <h3 className="text-lg font-medium text-[#1E293B]">
                        {mentor.name}
                    </h3>
                    <p className="text-[#64748B] text-sm">{mentor.title}</p>
                    <div className="flex items-center mt-1">
                        <span className="text-sm font-medium text-[#1E293B] mr-1">
                            {mentor.rating.toFixed(1)}
                        </span>
                        <div className="flex">
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
                        </div>
                    </div>
                    <p className="text-[#64748B] text-sm mt-2">
                        {mentor.category}
                    </p>

                    {/* Recommendation scores - only shown for RecommendedMentor type */}
                    {showRecommendationScores && isRecommendedMentor && (
                        <div className="mt-3 border-t border-gray-100 pt-2">
                            <h4 className="text-sm font-medium text-[#1E293B] mb-1">
                                Оцінки рекомендацій:
                            </h4>
                            <div className="grid grid-cols-2 gap-2 text-xs">
                                <div>
                                    <span className="text-[#64748B]">
                                        Активність:{" "}
                                    </span>
                                    <span className="font-medium">
                                        {(
                                            mentor as RecommendedMentor
                                        ).collaborativeScore.toFixed(2)}
                                    </span>
                                </div>
                                <div>
                                    <span className="text-[#64748B]">
                                        За інтересами:{" "}
                                    </span>
                                    <span className="font-medium">
                                        {(
                                            mentor as RecommendedMentor
                                        ).contentBasedScore.toFixed(2)}
                                    </span>
                                </div>
                                <div className="col-span-2 mt-1 bg-gray-100 p-1 rounded">
                                    <span className="text-[#4318D1] font-bold">
                                        Загальна оцінка:{" "}
                                    </span>
                                    <span className="font-bold">
                                        {(
                                            mentor as RecommendedMentor
                                        ).finalScore.toFixed(2)}
                                    </span>
                                </div>
                            </div>
                        </div>
                    )}

                    <div className="flex flex-wrap gap-2 mt-4">
                        {mentor.skills.map((skill) => (
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
            {isMentee && (
                <Link to={`/mentors/${mentor.id}`} className="block w-full">
                    <button className="w-full mt-4 py-3 rounded-lg bg-[#4318D1] text-white text-sm hover:bg-[#3712A5] transition-colors">
                        Переглянути профіль
                    </button>
                </Link>
            )}
        </div>
    );
};
