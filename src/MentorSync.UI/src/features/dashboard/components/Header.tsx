import React from "react";

const Header: React.FC = () => {
    return (
        <div className="w-full h-16 border-b border-[#E2E8F0] flex items-center justify-between px-6">
            <div className="flex items-center">
                <div className="w-8 h-8 flex items-center justify-center cursor-pointer">
                    <span className="material-icons">menu</span>
                </div>{" "}
                <div className="ml-5 flex items-center">
                    <div className="logo-placeholder">MS</div>
                    <h1 className="text-xl font-bold ml-3 text-[#1E293B]">
                        MentorSync
                    </h1>
                </div>
            </div>
            <div className="flex items-center space-x-4">
                <div className="w-10 h-10 rounded-lg flex items-center justify-center cursor-pointer">
                    <span className="material-icons">notifications</span>
                </div>
                <div className="flex items-center">
                    <img
                        src="https://randomuser.me/api/portraits/women/44.jpg"
                        alt="User"
                        className="w-8 h-8 rounded-full"
                    />
                    <span className="material-icons ml-1 text-gray-600">
                        expand_more
                    </span>
                </div>
            </div>
        </div>
    );
};

export default Header;
