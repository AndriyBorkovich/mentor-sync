import React, { useRef, useEffect } from "react";

interface NotificationDropdownProps {
    isOpen: boolean;
    onClose: () => void;
}

export const NotificationsDropdown: React.FC<NotificationDropdownProps> = ({
    isOpen,
    onClose,
}) => {
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
