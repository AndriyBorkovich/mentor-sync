import React from "react";

interface ProfileHeaderProps {
    userName: string;
    profileImageUrl?: string;
}

const ProfileHeader: React.FC<ProfileHeaderProps> = ({
    userName,
    profileImageUrl,
}) => {
    return (
        <div className="bg-white rounded-lg shadow p-6">
            <div className="flex flex-col md:flex-row items-center">
                <div className="w-24 h-24 rounded-full overflow-hidden bg-gray-200 flex items-center justify-center">
                    {profileImageUrl ? (
                        <img
                            src={profileImageUrl}
                            alt={userName}
                            className="w-full h-full object-cover"
                        />
                    ) : (
                        <div className="text-3xl font-semibold text-gray-600">
                            {userName?.charAt(0)?.toUpperCase() || "U"}
                        </div>
                    )}
                </div>
                <div className="md:ml-6 mt-4 md:mt-0 text-center md:text-left">
                    <h1 className="text-2xl font-bold text-gray-800">
                        {userName}
                    </h1>
                </div>
                <div className="flex-1 flex justify-end mt-4 md:mt-0">
                    <button className="bg-white border border-primary text-primary hover:bg-primary hover:text-white transition-colors px-4 py-2 rounded-lg">
                        Редагувати профіль
                    </button>
                </div>
            </div>
        </div>
    );
};

export default ProfileHeader;
