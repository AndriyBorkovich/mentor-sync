import React, { useState } from "react";

interface SidebarLinkProps {
    icon: string;
    label: string;
    active?: boolean;
    expanded?: boolean;
    onClick?: () => void;
}

const SidebarLink: React.FC<SidebarLinkProps> = ({
    icon,
    label,
    active = false,
    expanded = false,
    onClick,
}) => {
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
            onClick={onClick}
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
}

const Sidebar: React.FC<SidebarProps> = ({ onToggle }) => {
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
            </div>
            <SidebarLink
                icon="home-icon"
                label="Головна"
                active={true}
                expanded={expanded}
            />
            <SidebarLink
                icon="search-icon"
                label="Пошук менторів"
                expanded={expanded}
            />
            <SidebarLink
                icon="sessions-icon"
                label="Мої сесії"
                expanded={expanded}
            />
            <SidebarLink
                icon="messages-icon"
                label="Повідомлення"
                expanded={expanded}
            />
            <SidebarLink
                icon="materials-icon"
                label="Матеріали"
                expanded={expanded}
            />
            <SidebarLink
                icon="settings-icon"
                label="Налаштування"
                expanded={expanded}
            />
        </div>
    );
};

export default Sidebar;
