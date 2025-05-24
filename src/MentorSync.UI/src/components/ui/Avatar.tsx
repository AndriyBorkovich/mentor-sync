import React from "react";

interface AvatarProps {
    src?: string;
    alt?: string;
    className?: string;
    size?: "sm" | "md" | "lg";
}

const Avatar: React.FC<AvatarProps> = ({
    src,
    alt = "User avatar",
    className = "",
    size = "md",
}) => {
    const sizeClasses = {
        sm: "w-8 h-8",
        md: "w-10 h-10",
        lg: "w-12 h-12",
    };

    return (
        <div
            className={`${sizeClasses[size]} rounded-full border-2 border-white overflow-hidden ${className}`}
        >
            {src ? (
                <img
                    src={src}
                    alt={alt}
                    className="w-full h-full object-cover"
                />
            ) : (
                <div className="w-full h-full bg-gray-200 flex items-center justify-center text-gray-500">
                    <span className="text-xs">
                        {alt.charAt(0).toUpperCase()}
                    </span>
                </div>
            )}
        </div>
    );
};

export default Avatar;
