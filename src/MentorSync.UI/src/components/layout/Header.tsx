import React, { useState } from "react";
import { UserDropdown } from "../ui/UserDropdown";
import { NotificationsDropdown } from "../ui/NotificationsDropdown";

interface HeaderProps {
    showNotifications?: boolean;
}
const Header: React.FC<HeaderProps> = ({ showNotifications = true }) => {
    const [userDropdownOpen, setUserDropdownOpen] = useState(false);
    const [notificationsDropdownOpen, setNotificationsDropdownOpen] =
        useState(false);

    return (
        <div className="w-full h-16 border-b border-[#E2E8F0] flex items-center justify-between px-6 relative">
            <div className="flex items-center">
                <div className="flex items-center">
                    <div className="logo-placeholder">MS</div>
                    <p className="text-xl font-bold ml-3 text-[#1E293B]">
                        MentorSync
                    </p>
                </div>
            </div>
            <div className="flex items-center space-x-4">
                {showNotifications && (
                    <div
                        className="w-10 h-10 rounded-lg flex items-center justify-center cursor-pointer"
                        onClick={() =>
                            setNotificationsDropdownOpen(
                                !notificationsDropdownOpen
                            )
                        }
                    >
                        <span className="material-icons">notifications</span>
                    </div>
                )}
                <div
                    className="flex items-center cursor-pointer"
                    onClick={() => setUserDropdownOpen(!userDropdownOpen)}
                >
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
            <UserDropdown
                isOpen={userDropdownOpen}
                onClose={() => setUserDropdownOpen(false)}
            />
            <NotificationsDropdown
                isOpen={notificationsDropdownOpen}
                onClose={() => setNotificationsDropdownOpen(false)}
            />
        </div>
    );
};

export default Header;
