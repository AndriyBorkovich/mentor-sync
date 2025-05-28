import React from "react";
import { MentorData, isMentorProfile } from "../../types/mentorTypes";

interface AboutTabProps {
    mentor: MentorData;
}

const AboutTab: React.FC<AboutTabProps> = ({ mentor }) => {
    // Get bio content from API data or use default text
    const getBioContent = () => {
        if (isMentorProfile(mentor) && mentor.bio) {
            return mentor.bio;
        }

        return "";
    };

    return (
        <div className="flex">
            <div className="w-2/3 pr-6">
                <h2 className="text-lg font-medium text-[#1E293B] mb-3">
                    Інформація
                </h2>
                <p className="text-[#64748B] mb-6">{getBioContent()}</p>
                <h2 className="text-lg font-medium text-[#1E293B] mb-3 mt-6">
                    Навички
                </h2>
                <div className="flex flex-wrap gap-2">
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
            <div className="w-1/3 bg-white rounded-xl shadow-sm p-6">
                {/* Availability section */}
                {isMentorProfile(mentor) && mentor.availability && (
                    <>
                        <h2 className="text-lg font-medium text-[#1E293B] mb-3">
                            Доступність
                        </h2>
                        <div className="p-3 bg-[#F8FAFC] rounded-lg mb-4">
                            <p className="text-sm text-[#64748B]">
                                {mentor.availability}
                            </p>
                        </div>
                    </>
                )}
            </div>
        </div>
    );
};

export default AboutTab;
