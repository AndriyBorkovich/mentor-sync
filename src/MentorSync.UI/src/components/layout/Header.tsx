import React, { useState } from "react";
import { UserDropdown } from "../ui/UserDropdown";
import { NotificationsDropdown } from "../ui/NotificationsDropdown";
import { useUserProfile } from "../../features/auth/hooks/useUserProfile";

interface HeaderProps {
    showNotifications?: boolean;
}
const Header: React.FC<HeaderProps> = ({ showNotifications = true }) => {
    const [userDropdownOpen, setUserDropdownOpen] = useState(false);
    const [notificationsDropdownOpen, setNotificationsDropdownOpen] =
        useState(false);
    const { profile, loading } = useUserProfile();

    // Use profile.profileImage if available, otherwise fallback to avatar URL
    const profileImage = profile?.profileImageUrl
        ? profile.profileImageUrl
        : profile?.userName
        ? `https://ui-avatars.com/api/?name=${encodeURIComponent(
              profile.userName
          )}&background=F3F4F6&color=1E293B&size=64`
        : "https://ui-avatars.com/api/?name=User&background=F3F4F6&color=1E293B&size=64";

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
                        <img
                            src={`/icons/notification-icon.svg`}
                            alt={"notifications"}
                        />
                    </div>
                )}
                <div
                    className="flex items-center cursor-pointer"
                    onClick={() => setUserDropdownOpen(!userDropdownOpen)}
                >
                    <img
                        src={profileImage}
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
                profile={profile}
                loading={loading}
            />
            <NotificationsDropdown
                isOpen={notificationsDropdownOpen}
                onClose={() => setNotificationsDropdownOpen(false)}
            />
        </div>
    );
};

export default Header;
