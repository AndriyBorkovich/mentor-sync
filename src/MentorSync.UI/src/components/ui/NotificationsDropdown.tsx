import React, { useRef, useEffect, useState } from "react";
import { useNotifications } from "../../features/notifications/useNotifications";

interface NotificationDropdownProps {
    isOpen: boolean;
    onClose: () => void;
}

interface Notification {
    id: string;
    title?: string;
    message: string;
    time?: string;
}

export const NotificationsDropdown: React.FC<NotificationDropdownProps> = ({
    isOpen,
    onClose,
}) => {
    const dropdownRef = useRef<HTMLDivElement>(null);
    const [notifications, setNotifications] = useState<Notification[]>([]);

    useNotifications((data) => {
        console.log("Notification received:", data);
        setNotifications((prev) => [
            {
                id: crypto.randomUUID(),
                title: data?.Title,
                message: data?.Message || "Booking status updated",
                time: new Date().toLocaleTimeString([], {
                    hour: "2-digit",
                    minute: "2-digit",
                }),
            },
            ...prev,
        ]);
    });

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
            className="absolute right-16 top-16 w-[300px] bg-white border border-[#E2E8F0] rounded-lg shadow-lg z-10"
        >
            <div className="border-b border-[#E2E8F0] p-4">
                <h3 className="text-base font-medium text-[#1E293B]">
                    Сповіщення
                </h3>
            </div>
            <div className="p-4">
                {notifications.length === 0 ? (
                    <div className="text-sm text-[#64748B]">
                        Немає нових сповіщень
                    </div>
                ) : (
                    notifications.map((notification) => (
                        <div
                            key={notification.id}
                            className="py-2 px-3 mb-2 bg-[#F8FAFC] rounded-lg cursor-pointer hover:bg-[#E2E8F0]"
                        >
                            <p className="text-sm text-[#1E293B] font-semibold">
                                {notification.title
                                    ? `${notification.title}: `
                                    : ""}
                                <span className="font-normal">
                                    {notification.message}
                                </span>
                            </p>
                            <p className="text-xs text-[#64748B]">
                                {notification.time}
                            </p>
                        </div>
                    ))
                )}
            </div>
        </div>
    );
};
