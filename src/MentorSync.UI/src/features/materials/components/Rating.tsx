import React, { useState } from "react";

interface RatingProps {
    value: number;
    onChange: (value: number) => void;
    disabled?: boolean;
    size?: "small" | "medium" | "large";
}

export const Rating: React.FC<RatingProps> = ({
    value,
    onChange,
    disabled = false,
    size = "medium",
}) => {
    const [hoveredRating, setHoveredRating] = useState<number | null>(null);

    const displayRating = hoveredRating !== null ? hoveredRating : value;

    const sizeClasses = {
        small: "text-lg",
        medium: "text-2xl",
        large: "text-3xl",
    };

    return (
        <div className="flex">
            {[1, 2, 3, 4, 5].map((star) => (
                <button
                    key={star}
                    type="button"
                    disabled={disabled}
                    onClick={() => onChange(star)}
                    onMouseEnter={() => setHoveredRating(star)}
                    onMouseLeave={() => setHoveredRating(null)}
                    className={`${sizeClasses[size]} focus:outline-none ${
                        disabled ? "cursor-default" : "cursor-pointer"
                    } mr-1`}
                >
                    <span
                        className={`material-icons ${
                            displayRating >= star
                                ? "text-yellow-400"
                                : "text-gray-300"
                        }`}
                    >
                        star
                    </span>
                </button>
            ))}
        </div>
    );
};
