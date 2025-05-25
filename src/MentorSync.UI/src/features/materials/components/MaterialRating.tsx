import React, { useState } from "react";

interface MaterialRatingProps {
    initialRating?: number;
    totalRatings?: number;
    onRatingSubmit?: (rating: number) => void;
}

const MaterialRating: React.FC<MaterialRatingProps> = ({
    initialRating = 0,
    totalRatings = 0,
    onRatingSubmit,
}) => {
    const [rating, setRating] = useState<number>(initialRating);
    const [hoveredRating, setHoveredRating] = useState<number | null>(null);
    const [hasRated, setHasRated] = useState<boolean>(initialRating > 0);
    const [isSubmitting, setIsSubmitting] = useState<boolean>(false);

    const handleRatingClick = (newRating: number) => {
        if (hasRated || !onRatingSubmit) return;

        setRating(newRating);
        setIsSubmitting(true);

        // Simulate API call
        setTimeout(() => {
            onRatingSubmit(newRating);
            setHasRated(true);
            setIsSubmitting(false);
        }, 500);
    };

    const displayRating = hoveredRating !== null ? hoveredRating : rating;

    return (
        <div className="bg-white p-6 rounded-lg shadow-sm mb-6">
            <h2 className="text-xl font-medium text-[#1E293B] mb-4">
                Оцінка матеріалу
            </h2>

            <div className="flex items-center">
                <div className="flex">
                    {[1, 2, 3, 4, 5].map((star) => (
                        <button
                            key={star}
                            type="button"
                            disabled={hasRated || isSubmitting}
                            onClick={() => handleRatingClick(star)}
                            onMouseEnter={() => setHoveredRating(star)}
                            onMouseLeave={() => setHoveredRating(null)}
                            className={`text-2xl focus:outline-none ${
                                hasRated ? "cursor-default" : "cursor-pointer"
                            }`}
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

                <div className="ml-4">
                    <span className="font-medium text-[#1E293B]">
                        {rating.toFixed(1)}
                    </span>
                    <span className="text-[#64748B] text-sm ml-1">
                        ({totalRatings} оцінок)
                    </span>
                </div>
            </div>

            {isSubmitting && (
                <p className="text-sm text-[#6C5DD3] mt-2">
                    Збереження вашої оцінки...
                </p>
            )}

            {hasRated && !isSubmitting && (
                <p className="text-sm text-green-600 mt-2">
                    Дякуємо за вашу оцінку!
                </p>
            )}

            {!hasRated && !isSubmitting && (
                <p className="text-sm text-[#64748B] mt-2">
                    Клікніть на зірку, щоб оцінити цей матеріал
                </p>
            )}
        </div>
    );
};

export default MaterialRating;
