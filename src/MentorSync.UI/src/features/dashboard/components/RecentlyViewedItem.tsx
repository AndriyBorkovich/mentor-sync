import React from "react";
import { Mentor } from "../data/mentors";

interface RecentlyViewedItemProps {
    mentor: Mentor;
}

const RecentlyViewedItem: React.FC<RecentlyViewedItemProps> = ({ mentor }) => {
    return (
        <div className="bg-[#F8FAFC] rounded-lg p-3 flex items-center mb-4">
            <img
                src={mentor.profileImage}
                alt={mentor.name}
                className="w-12 h-12 rounded-full mr-3"
            />
            <div>
                <h3 className="text-base font-medium text-[#1E293B]">
                    {mentor.name}
                </h3>
                <p className="text-sm text-[#64748B]">{mentor.title}</p>
            </div>
        </div>
    );
};

export default RecentlyViewedItem;
