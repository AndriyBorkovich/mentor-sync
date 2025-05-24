import React from "react";

interface AvatarProps {
    src?: string;
    alt?: string;
    className?: string;
    size?: "xs" | "sm" | "md" | "lg" | "xl";
    border?: boolean;
    style?: React.CSSProperties;
}

const Avatar: React.FC<AvatarProps> = ({
    src,
    alt = "User avatar",
    className = "",
    size = "md",
    border = true,
    style = {},
}) => {
    const sizeClasses = {
        xs: "w-6 h-6 text-[0.5rem]",
        sm: "w-8 h-8 text-xs",
        md: "w-10 h-10 text-sm",
        lg: "w-12 h-12 text-base",
        xl: "w-16 h-16 text-lg",
    };

    const borderClass = border ? "border-2 border-white" : "";

    return (
        <div
            className={`${sizeClasses[size]} ${borderClass} rounded-full overflow-hidden shadow-sm ${className}`}
            style={style}
        >
            {src ? (
                <img
                    src={src}
                    alt={alt}
                    className="w-full h-full object-cover"
                />
            ) : (
                <div className="w-full h-full bg-gray-100 flex items-center justify-center text-primary font-medium">
                    <span>{alt.charAt(0).toUpperCase()}</span>
                </div>
            )}
        </div>
    );
};

export default Avatar;
