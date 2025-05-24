import React from "react";

interface PlaceholderImageProps {
    width?: string | number;
    height?: string | number;
    className?: string;
    text?: string;
    backgroundColor?: string;
}

const PlaceholderImage: React.FC<PlaceholderImageProps> = ({
    width = "100%",
    height = "100%",
    className = "",
    text = "Placeholder Image",
    backgroundColor = "#f3f4f6",
}) => {
    return (
        <div
            className={`flex items-center justify-center ${className}`}
            style={{
                width,
                height,
                backgroundColor,
                borderRadius: "0.5rem",
            }}
        >
            <span className="text-gray-400 text-sm font-medium">{text}</span>
        </div>
    );
};

export default PlaceholderImage;
