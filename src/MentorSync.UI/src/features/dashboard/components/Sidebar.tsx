import React from "react";

interface SidebarLinkProps {
    icon: string;
    active?: boolean;
    onClick?: () => void;
}

const SidebarLink: React.FC<SidebarLinkProps> = ({
    icon,
    active = false,
    onClick,
}) => {
    return (
        <div
            className={`w-[39px] h-12 rounded-lg flex items-center justify-center cursor-pointer mb-2 ${
                active
                    ? "bg-[#F8FAFC]"
                    : "hover:bg-[#F8FAFC] hover:bg-opacity-60"
            }`}
            onClick={onClick}
        >
            <img src={`/icons/${icon}.svg`} alt={icon} className="w-6 h-6" />
        </div>
    );
};

const Sidebar: React.FC = () => {
    return (
        <div className="h-full w-[72px] border-r border-[#E2E8F0] py-4 flex flex-col items-center">
            <SidebarLink icon="dashboard-icon" active />
            <SidebarLink icon="profile-icon" />
            <SidebarLink icon="settings-icon" />
        </div>
    );
};

export default Sidebar;
