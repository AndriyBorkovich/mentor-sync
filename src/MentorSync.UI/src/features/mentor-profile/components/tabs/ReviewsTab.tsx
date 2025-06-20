import React, { useMemo } from "react";
import { MentorData, isMentorProfile } from "../../types/mentorTypes";
import { hasRole } from "../../../auth/utils/authUtils";
import MentorReviewForm from "../reviews/MentorReviewForm";

interface ReviewsTabProps {
    mentor: MentorData;
    onRefreshReviews?: () => void;
}

const ReviewsTab: React.FC<ReviewsTabProps> = ({
    mentor,
    onRefreshReviews,
}) => {
    const isMentee = hasRole("Mentee");
    const mentorId = isMentorProfile(mentor) ? mentor.id : parseInt(mentor.id);

    // Calculated values
    const totalRating = mentor.rating;
    const totalReviews = isMentorProfile(mentor) ? mentor.reviewCount : 0; // Format the date from ISO string with time since
    const formatReviewDate = (dateString: string): string => {
        const date = new Date(dateString);
        const today = new Date();

        // Check if the date is today by comparing year, month, and day
        const isToday =
            date.getFullYear() === today.getFullYear() &&
            date.getMonth() === today.getMonth() &&
            date.getDate() === today.getDate();

        // Check if the date is yesterday
        const yesterday = new Date();
        yesterday.setDate(yesterday.getDate() - 1);
        const isYesterday =
            date.getFullYear() === yesterday.getFullYear() &&
            date.getMonth() === yesterday.getMonth() &&
            date.getDate() === yesterday.getDate();

        // For older dates, calculate the difference in days
        const diffTime = today.getTime() - date.getTime();
        const diffDays = Math.floor(diffTime / (1000 * 60 * 60 * 24));

        if (diffDays < 7) {
            if (isToday) return "Сьогодні";
            if (isYesterday) return "Вчора";
            return `${diffDays} днів тому`;
        }

        return date.toLocaleDateString("uk-UA", {
            day: "numeric",
            month: "long",
            year: "numeric",
        });
    }; // Get reviews from API data if available, otherwise use mock data
    // Memoize reviews to avoid recreating on every render
    const reviews = useMemo(() => {
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
    }, [mentor, formatReviewDate]); // Only recalculate if mentor data changes

    const handleReviewSubmitted = () => {
        if (onRefreshReviews) {
            onRefreshReviews();
        }
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
            </div>{" "}
            {/* Add review form for mentees */}
            {isMentee && (
                <MentorReviewForm
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
                                            src={
                                                review.authorImage
                                                    ? review.authorImage
                                                    : "https://ui-avatars.com/api/?name=" +
                                                      encodeURIComponent(
                                                          review.authorName
                                                      ) +
                                                      "&background=F3F4F6&color=1E293B&size=64"
                                            }
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

export default ReviewsTab;
