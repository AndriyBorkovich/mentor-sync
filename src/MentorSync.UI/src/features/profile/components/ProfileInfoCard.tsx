import React from "react";

interface ProfileInfoItem {
    label: string;
    value: string;
}

interface ProfileInfoCardProps {
    title: string;
    items: ProfileInfoItem[];
}

const ProfileInfoCard: React.FC<ProfileInfoCardProps> = ({ title, items }) => {
    return (
        <div className="bg-white rounded-lg shadow p-6">
            <h2 className="text-xl font-semibold text-gray-800 mb-4">
                {title}
            </h2>
            <div className="space-y-3">
                {items.map((item, index) => (
                    <div key={index} className="flex flex-col">
                        <span className="text-sm text-gray-500 break-words">
                            {item.label}
                        </span>
                        <span className="text-gray-700 break-words">
                            {item.value}
                        </span>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default ProfileInfoCard;
