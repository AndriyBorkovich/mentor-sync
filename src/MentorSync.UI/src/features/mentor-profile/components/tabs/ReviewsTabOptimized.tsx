import React, { useState, useEffect } from "react";
import { MentorData, isMentorProfile } from "../../types/mentorTypes";
import { hasRole } from "../../../auth/utils/authUtils";
import MentorReviewFormOptimized from "../reviews/MentorReviewFormOptimized";

interface ReviewsTabProps {
    mentor: MentorData;
}

// Mock review data
interface Review {
    id: string;
    authorName: string;
    authorImage?: string;
    date: string;
    rating: number;
    content: string;
}

const ReviewsTabOptimized: React.FC<ReviewsTabProps> = ({ mentor }) => {
    // Use a ref instead of state for refresh trigger to avoid unnecessary renders
    const [shouldRefreshReviews, setShouldRefreshReviews] = useState(false);
    const isMentee = hasRole("Mentee");
    const mentorId = isMentorProfile(mentor) ? mentor.id : parseInt(mentor.id);

    // Format the date from ISO string with time since
    const formatReviewDate = (dateString: string): string => {
        const date = new Date(dateString);
        const today = new Date();
        const diffTime = Math.abs(today.getTime() - date.getTime());
        const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

        if (diffDays < 7) {
            if (diffDays === 0) return "Сьогодні";
            if (diffDays === 1) return "Вчора";
            return `${diffDays} днів тому`;
        }

        return date.toLocaleDateString("uk-UA", {
            day: "numeric",
            month: "long",
            year: "numeric",
        });
    };

    // Default mock reviews data
    const mockReviews: Review[] = [
        {
            id: "1",
            authorName: "Девід Чен",
            date: "15 січня, 2024",
            rating: 5,
            content:
                "Винятковий наставник. Дійсно допоміг мені зрозуміти дизайн системи.",
        },
        {
            id: "2",
            authorName: "Сара Міллер",
            date: "10 січня, 2024",
            rating: 5,
            content: "Чудово пояснює складні поняття простими словами.",
        },
        {
            id: "3",
            authorName: "Майкл Браун",
            date: "5 Січня, 2024",
            rating: 4,
            content:
                "Дуже обізнаний про AWS та хмарну архітектуру. Рекомендую.",
        },
        {
            id: "4",
            authorName: "Емілі Ванг",
            date: "30 грудня, 2023",
            rating: 5,
            content:
                "Оксана - дивовижний наставник. Вона допомогла мені підготуватися до інтерв'ю.",
        },
    ];

    // Get reviews from API data if available, otherwise use mock data
    // Memoize reviews to avoid recreating on every render
    const reviews = React.useMemo(() => {
        if (
            isMentorProfile(mentor) &&
            mentor.recentReviews &&
            mentor.recentReviews.length > 0
        ) {
            return mentor.recentReviews.map((review) => ({
                id: review.id.toString(),
                authorName: review.reviewerName,
                authorImage: review.reviewerImage,
                date: formatReviewDate(review.createdOn),
                rating: review.rating,
                content: review.comment,
            }));
        }
        return [];
    }, [mentor, shouldRefreshReviews]); // Only recalculate if mentor data changes or refresh is triggered

    const totalRating = isMentorProfile(mentor) ? mentor.rating : mentor.rating;
    const totalReviews = isMentorProfile(mentor) ? mentor.reviewCount : 0;

    const handleReviewSubmitted = () => {
        // Toggle the refresh flag to trigger a re-fetch of reviews
        setShouldRefreshReviews((prev) => !prev);
    };

    return (
        <div>
            {/* Review summary at the top */}
            <div className="bg-[#F8FAFC] p-4 rounded-lg mb-6">
                <div className="flex items-center justify-between">
                    <div className="flex items-center">
                        <span className="text-3xl font-bold text-[#1E293B] mr-3">
                            {totalRating.toFixed(1)}
                        </span>
                        <div className="flex flex-col">
                            <div className="flex">
                                {[...Array(5)].map((_, i) => (
                                    <span
                                        key={i}
                                        className="material-icons text-yellow-500"
                                    >
                                        {i < Math.floor(totalRating)
                                            ? "star"
                                            : "star_border"}
                                    </span>
                                ))}
                            </div>
                            <span className="text-sm text-[#64748B]">
                                Всього відгуків: {totalReviews}
                            </span>
                        </div>
                    </div>
                </div>
            </div>

            {/* Add review form for mentees */}
            {isMentee && (
                <MentorReviewFormOptimized
                    mentorId={mentorId}
                    onReviewSubmitted={handleReviewSubmitted}
                />
            )}

            <h3 className="text-lg font-medium text-[#1E293B] mb-4">
                Останні відгуки
            </h3>

            <div className="space-y-6">
                {reviews && reviews.length > 0 ? (
                    reviews.map((review) => (
                        <div
                            key={review.id}
                            className="border-b border-[#E2E8F0] pb-4 mb-4 last:border-b-0"
                        >
                            <div className="flex justify-between items-center mb-2">
                                <div className="flex items-center">
                                    {review.authorImage ? (
                                        <img
                                            src={review.authorImage}
                                            alt={review.authorName}
                                            className="w-10 h-10 rounded-full mr-3"
                                        />
                                    ) : (
                                        <div className="w-10 h-10 rounded-full bg-gray-200 mr-3 flex items-center justify-center">
                                            <span className="material-icons text-gray-500">
                                                person
                                            </span>
                                        </div>
                                    )}
                                    <div>
                                        <p className="font-medium text-[#1E293B]">
                                            {review.authorName}
                                        </p>
                                        <p className="text-xs text-[#64748B]">
                                            {review.date}
                                        </p>
                                    </div>
                                </div>
                                <div className="flex">
                                    {[...Array(5)].map((_, i) => (
                                        <span
                                            key={i}
                                            className="material-icons text-yellow-500 text-sm"
                                        >
                                            {i < review.rating
                                                ? "star"
                                                : "star_border"}
                                        </span>
                                    ))}
                                </div>
                            </div>
                            <p className="text-sm text-[#64748B]">
                                {review.content}
                            </p>
                        </div>
                    ))
                ) : (
                    <div className="text-center py-6 text-[#64748B]">
                        Немає відгуків для цього ментора
                    </div>
                )}
            </div>
        </div>
    );
};

export default ReviewsTabOptimized;
