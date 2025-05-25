import React, { useState, useRef, useEffect } from "react";

interface UserDropdownProps {
    isOpen: boolean;
    onClose: () => void;
}

const UserDropdown: React.FC<UserDropdownProps> = ({ isOpen, onClose }) => {
    const dropdownRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        const handleClickOutside = (event: MouseEvent) => {
            if (
                dropdownRef.current &&
                !dropdownRef.current.contains(event.target as Node)
            ) {
                onClose();
            }
        };

        if (isOpen) {
            document.addEventListener("mousedown", handleClickOutside);
        }

        return () => {
            document.removeEventListener("mousedown", handleClickOutside);
        };
    }, [isOpen, onClose]);

    if (!isOpen) return null;

    return (
        <div
            ref={dropdownRef}
            className="absolute right-6 top-16 w-[200px] bg-white border border-[#E2E8F0] rounded-lg shadow-lg z-10"
        >
            <div className="border-b border-[#E2E8F0] p-4">
                <h3 className="text-base font-medium text-[#1E293B]">
                    Іван Іванов
                </h3>
                <p className="text-sm text-[#64748B]">john@example.com</p>
            </div>
            <div className="p-4">
                <div className="py-2 cursor-pointer text-[#1E293B]">
                    Профіль
                </div>
                <div className="py-2 cursor-pointer text-[#1E293B]">
                    Налаштування
                </div>
                <div className="py-2 cursor-pointer text-[#DC2626]">Вийти</div>
            </div>
        </div>
    );
};

const NotificationsDropdown: React.FC<{
    isOpen: boolean;
    onClose: () => void;
}> = ({ isOpen, onClose }) => {
    const dropdownRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        const handleClickOutside = (event: MouseEvent) => {
            if (
                dropdownRef.current &&
                !dropdownRef.current.contains(event.target as Node)
            ) {
                onClose();
            }
        };

        if (isOpen) {
            document.addEventListener("mousedown", handleClickOutside);
        }

        return () => {
            document.removeEventListener("mousedown", handleClickOutside);
        };
    }, [isOpen, onClose]);

    if (!isOpen) return null;

    const mockNotifications = [
        {
            id: 1,
            message: "Нове повідомлення від Оксани Лень",
            time: "5 хвилин тому",
        },
        {
            id: 2,
            message: "Нагадування майбутнього сеансу",
            time: "1 годину тому",
        },
    ];

    return (
        <div
            ref={dropdownRef}
            className="absolute right-16 top-16 w-[300px] bg-white border border-[#E2E8F0] rounded-lg shadow-lg z-10"
        >
            <div className="border-b border-[#E2E8F0] p-4">
                <h3 className="text-base font-medium text-[#1E293B]">
                    Сповіщення
                </h3>
            </div>
            <div className="p-4">
                {mockNotifications.map((notification) => (
                    <div
                        key={notification.id}
                        className="py-2 px-3 mb-2 bg-[#F8FAFC] rounded-lg cursor-pointer hover:bg-[#E2E8F0]"
                    >
                        <p className="text-sm text-[#1E293B]">
                            {notification.message}
                        </p>
                        <p className="text-xs text-[#64748B]">
                            {notification.time}
                        </p>
                    </div>
                ))}
            </div>
        </div>
    );
};

const Header: React.FC = () => {
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
                <div
                    className="w-10 h-10 rounded-lg flex items-center justify-center cursor-pointer"
                    onClick={() =>
                        setNotificationsDropdownOpen(!notificationsDropdownOpen)
                    }
                >
                    <span className="material-icons">notifications</span>
                </div>
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
