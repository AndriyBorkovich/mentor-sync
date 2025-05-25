import React, { useState } from "react";
import { Mentor } from "../data/mentors";
import { Link } from "react-router-dom";

interface MentorCardProps {
    mentor: Mentor;
}

const MentorCard: React.FC<MentorCardProps> = ({ mentor }) => {
    const [isSaved, setIsSaved] = useState(false);

    const toggleSave = (e: React.MouseEvent) => {
        e.stopPropagation();
        setIsSaved(!isSaved);
    };

    return (
        <div className="bg-white rounded-2xl shadow-md p-6 mb-6 relative">
            <div
                className="absolute top-4 right-4 cursor-pointer"
                onClick={toggleSave}
            >
                <span className="material-icons text-[#4318D1]">
                    {isSaved ? "bookmark" : "bookmark_border"}
                </span>
            </div>
            <div className="flex items-start">
                <img
                    src={mentor.profileImage}
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
                            {mentor.rating}
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
            <Link to={`/mentors/${mentor.id}`} className="block w-full">
                <button className="w-full mt-4 py-3 rounded-lg bg-[#4318D1] text-white text-sm hover:bg-[#3712A5] transition-colors">
                    Переглянути профіль
                </button>
            </Link>
        </div>
    );
};

export default MentorCard;
