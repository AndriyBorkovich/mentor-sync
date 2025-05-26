import React, { useRef, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../features/auth/context/AuthContext";
import { useUserProfile } from "../../features/auth/hooks/useUserProfile";

interface UserDropdownProps {
    isOpen: boolean;
    onClose: () => void;
}
export const UserDropdown: React.FC<UserDropdownProps> = ({
    isOpen,
    onClose,
}) => {
    const dropdownRef = useRef<HTMLDivElement>(null);
    const { logout } = useAuth();
    const { profile, loading } = useUserProfile();
    const navigate = useNavigate();

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
            {" "}
            <div className="border-b border-[#E2E8F0] p-4">
                <h3 className="text-base font-medium text-[#1E293B]">
                    {loading ? "Loading..." : profile?.userName || "User"}
                </h3>
                <p className="text-sm text-[#64748B]">
                    {profile?.email || "Loading email..."}
                </p>
                {profile?.role && (
                    <span className="text-xs bg-[#4318D1]/10 text-[#4318D1] px-2 py-0.5 rounded-full">
                        {profile.role}
                    </span>
                )}
            </div>
            <div className="p-4">
                <div
                    className="py-2 cursor-pointer text-[#1E293B] hover:text-[#4318D1]"
                    onClick={() => {
                        navigate("/profile");
                        onClose();
                    }}
                >
                    Профіль
                </div>
                <div
                    className="py-2 cursor-pointer text-[#1E293B] hover:text-[#4318D1]"
                    onClick={() => {
                        navigate("/settings");
                        onClose();
                    }}
                >
                    Налаштування
                </div>{" "}
                <div
                    className="py-2 cursor-pointer text-[#DC2626] hover:text-[#B91C1C]"
                    onClick={() => {
                        logout();
                        navigate("/login", { replace: true });
                        onClose();
                    }}
                >
                    Вийти
                </div>
            </div>
        </div>
    );
};
