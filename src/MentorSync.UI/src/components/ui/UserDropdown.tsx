import React, { useRef, useEffect } from "react";

interface UserDropdownProps {
    isOpen: boolean;
    onClose: () => void;
}
export const UserDropdown: React.FC<UserDropdownProps> = ({
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
