import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { hasRole } from "../../features/auth/utils/authUtils";

interface SidebarLinkProps {
    icon: string;
    label: string;
    active?: boolean;
    expanded?: boolean;
    onClick?: () => void;
    to?: string;
}

const SidebarLink: React.FC<SidebarLinkProps> = ({
    icon,
    label,
    active = false,
    expanded = false,
    onClick,
    to,
}) => {
    const navigate = useNavigate();

    const handleClick = () => {
        if (onClick) {
            onClick();
        }
        if (to) {
            navigate(to);
        }
    };

    return (
        <div
            className={`sidebar-link h-12 rounded-lg flex items-center cursor-pointer mb-2 menu-transition 
                ${expanded ? "w-[207px] px-3" : "w-[39px] justify-center"}
                ${
                    active
                        ? "bg-[#F8FAFC] sidebar-active"
                        : "hover:bg-[#F8FAFC] hover:bg-opacity-60"
                }
            `}
            onClick={handleClick}
        >
            <div className="flex items-center justify-center w-6 h-6 flex-shrink-0">
                <img
                    src={`/icons/${icon}.svg`}
                    alt={icon}
                    className="w-full h-full"
                />
            </div>
            <div
                className={`overflow-hidden transition-all duration-300 ${
                    expanded ? "w-auto ml-3 opacity-100" : "w-0 opacity-0"
                }`}
            >
                <span
                    className={`whitespace-nowrap text-base ${
                        active
                            ? "text-[#4318D1] font-medium"
                            : "text-[#64748B] font-normal"
                    }`}
                >
                    {label}
                </span>
            </div>
        </div>
    );
};

interface SidebarProps {
    onToggle?: (expanded: boolean) => void;
    activePage?:
        | "home"
        | "search"
        | "sessions"
        | "messages"
        | "materials"
        | "settings"
        | "availability"
        | "mentorBookings"
        | "menteeBookings";
}

const Sidebar: React.FC<SidebarProps> = ({ onToggle, activePage = "home" }) => {
    const [expanded, setExpanded] = useState(false);

    const toggleSidebar = () => {
        const newExpandedState = !expanded;
        setExpanded(newExpandedState);
        if (onToggle) {
            onToggle(newExpandedState);
        }
    };
    return (
        <div
            className={`h-full border-r border-[#E2E8F0] py-4 flex flex-col items-start sidebar-transition bg-white ${
                expanded ? "w-[240px] px-4" : "w-[72px] items-center"
            }`}
        >
            <div
                className="menu-icon w-[39px] h-12 rounded-lg flex items-center justify-center cursor-pointer mb-6"
                onClick={toggleSidebar}
            >
                <img
                    src="/icons/menu-icon.svg"
                    alt="menu"
                    className="w-6 h-6"
                />
            </div>{" "}
            {hasRole("Mentee") && (
                <SidebarLink
                    icon="home-icon"
                    label="Головна"
                    active={activePage === "home"}
                    expanded={expanded}
                    to="/dashboard"
                />
            )}
            {hasRole("Mentee") && (
                <SidebarLink
                    icon="search-icon"
                    label="Пошук менторів"
                    active={activePage === "search"}
                    expanded={expanded}
                    to="/mentors"
                />
            )}{" "}
            <SidebarLink
                icon="sessions-icon"
                label="Мої сесії"
                active={activePage === "sessions"}
                expanded={expanded}
                to="/sessions"
            />{" "}
            <SidebarLink
                icon="messages-icon"
                label="Повідомлення"
                active={activePage === "messages"}
                expanded={expanded}
                to="/messages"
            />{" "}
            <SidebarLink
                icon="materials-icon"
                label="Матеріали"
                active={activePage === "materials"}
                expanded={expanded}
                to="/materials"
            />{" "}
            <SidebarLink
                icon="settings-icon"
                label="Налаштування"
                active={activePage === "settings"}
                expanded={expanded}
            />
        </div>
    );
};

export default Sidebar;
