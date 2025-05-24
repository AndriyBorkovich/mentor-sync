import React from "react";

interface PlaceholderImageProps {
    width?: string | number;
    height?: string | number;
    className?: string;
    text?: string;
    backgroundColor?: string;
    pattern?: "dots" | "lines" | "gradient" | "none";
}

const PlaceholderImage: React.FC<PlaceholderImageProps> = ({
    width = "100%",
    height = "100%",
    className = "",
    text = "Placeholder Image",
    backgroundColor = "#f0f4f8",
    pattern = "dots",
}) => {
    // Define the background pattern based on pattern prop
    const backgroundStyle = {
        width,
        height,
        backgroundColor,
        borderRadius: "0.75rem",
        position: "relative" as const,
    };

    // Different patterns
    let patternOverlay = null;
    if (pattern === "dots") {
        patternOverlay = (
            <div
                className="absolute inset-0 opacity-10"
                style={{
                    backgroundImage: `radial-gradient(#4318D1 1px, transparent 1px)`,
                    backgroundSize: "20px 20px",
                }}
            ></div>
        );
    } else if (pattern === "lines") {
        patternOverlay = (
            <div
                className="absolute inset-0 opacity-10"
                style={{
                    backgroundImage: `linear-gradient(45deg, #4318D1 25%, transparent 25%, transparent 50%, #4318D1 50%, #4318D1 75%, transparent 75%, transparent)`,
                    backgroundSize: "20px 20px",
                }}
            ></div>
        );
    } else if (pattern === "gradient") {
        patternOverlay = (
            <div
                className="absolute inset-0 opacity-50"
                style={{
                    background: `linear-gradient(135deg, ${backgroundColor} 0%, #4318D1 100%)`,
                }}
            ></div>
        );
    }

    return (
        <div
            className={`flex items-center justify-center relative overflow-hidden ${className}`}
            style={backgroundStyle}
        >
            {patternOverlay}
            <div className="relative z-10 px-4 py-2 bg-white/80 rounded-md backdrop-blur-sm">
                <span className="text-secondary font-medium">{text}</span>
            </div>
        </div>
    );
};

export default PlaceholderImage;
