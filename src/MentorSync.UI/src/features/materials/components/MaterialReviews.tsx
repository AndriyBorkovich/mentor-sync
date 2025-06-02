import React from "react";
import { Rating } from "./Rating";

interface Review {
    id: number;
    rating: number;
    comment: string;
    createdOn: string;
    reviewerName: string;
    reviewerImage: string;
    isReviewByMentor: boolean;
}

interface MaterialReviewsProps {
    reviews: Review[];
    reviewCount: number;
    averageRating: number;
}

const MaterialReviews: React.FC<MaterialReviewsProps> = ({
    reviews,
    reviewCount,
    averageRating,
}) => {
    const formatDate = (date: string): string => {
        return new Date(date).toLocaleDateString("uk-UA", {
            day: "numeric",
            month: "long",
            year: "numeric",
        });
    };
    return (
        <div className="bg-white p-6 rounded-lg shadow-sm">
            <div className="flex items-center justify-between mb-6">
                <h2 className="text-xl font-medium text-[#1E293B]">
                    Відгуки про матеріал
                </h2>
                <div className="flex items-center">
                    <Rating
                        value={averageRating}
                        onChange={() => {}}
                        disabled={true}
                    />
                    <span className="ml-2 text-sm text-gray-500">
                        {reviewCount} відгуків
                    </span>
                </div>
            </div>

            {reviews.length > 0 ? (
                <div className="space-y-6">
                    {reviews.map((review) => (
                        <div
                            key={review.id}
                            className="border-b pb-4 last:border-b-0 last:pb-0"
                        >
                            <div className="flex items-center mb-2">
                                <img
                                    src={
                                        review.reviewerImage !=
                                        "/assets/avatars/default.jpg"
                                            ? review.reviewerImage
                                            : "https://ui-avatars.com/api/?name=" +
                                              encodeURIComponent(
                                                  review.reviewerName
                                              ) +
                                              "&background=F3F4F6&color=1E293B&size=64"
                                    }
                                    alt={review.reviewerName}
                                    className="w-10 h-10 rounded-full mr-3"
                                />
                                <div>
                                    <div className="flex items-center">
                                        <span className="font-medium text-[#1E293B] mr-2">
                                            {review.reviewerName}
                                        </span>
                                        {review.isReviewByMentor && (
                                            <span className="bg-blue-100 text-blue-600 text-xs px-2 py-0.5 rounded-full">
                                                Ментор
                                            </span>
                                        )}
                                    </div>
                                    <div className="flex items-center text-sm text-gray-500">
                                        <span className="flex items-center">
                                            <Rating
                                                value={review.rating}
                                                onChange={() => {}}
                                                disabled={true}
                                                size="small"
                                            />
                                        </span>
                                        <span className="mx-2">•</span>
                                        <span>
                                            {formatDate(review.createdOn)}
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <p className="text-gray-700">{review.comment}</p>
                        </div>
                    ))}
                </div>
            ) : (
                <p className="text-gray-500 text-center py-8">
                    Поки що немає відгуків для цього матеріалу.
                </p>
            )}
        </div>
    );
};

export default MaterialReviews;
