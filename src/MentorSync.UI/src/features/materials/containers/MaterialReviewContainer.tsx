import React, { useEffect, useState } from "react";
import { toast } from "react-toastify";
import MaterialReviewForm from "../components/MaterialReviewForm";
import MaterialReviews from "../components/MaterialReviews";
import {
    MaterialReviewsResponse,
    UserReview,
    createMaterialReview,
    deleteMaterialReview,
    getMaterialReviews,
    getUserMaterialReview,
    updateMaterialReview,
} from "../services/materialReviewService";
import { getUserId } from "../../auth";

interface MaterialReviewContainerProps {
    materialId: number;
    mentorId: number; // To check if current user is the material author
}

const MaterialReviewContainer: React.FC<MaterialReviewContainerProps> = ({
    materialId,
    mentorId,
}) => {
    const userId = getUserId();
    const [isLoading, setIsLoading] = useState(true);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [reviewsData, setReviewsData] =
        useState<MaterialReviewsResponse | null>(null);
    const [userReview, setUserReview] = useState<UserReview | null>(null); // Check if current user is the author of the material
    const isAuthor = userId === mentorId;

    // Fetch initial data
    useEffect(() => {
        const fetchData = async () => {
            setIsLoading(true);
            try {
                // Get all reviews for this material
                const reviewsResponse = await getMaterialReviews(materialId);
                setReviewsData(reviewsResponse); // If user is logged in and not the author, check if they've reviewed this material
                if (userId && !isAuthor) {
                    try {
                        const userReviewResponse = await getUserMaterialReview(
                            materialId,
                            userId
                        );
                        setUserReview(userReviewResponse);
                    } catch (error) {
                        // User hasn't reviewed yet - this is OK
                        setUserReview(null);
                    }
                }
            } catch (error) {
                toast.error("Не вдалося завантажити відгуки");
                console.error("Error fetching reviews:", error);
            } finally {
                setIsLoading(false);
            }
        };

        fetchData();
    }, [materialId, isAuthor]);

    const handleSubmitReview = async (data: {
        rating: number;
        comment: string;
    }) => {
        if (!userId) {
            toast.error("Ви маєте бути авторизовані, щоб залишити відгук");
            return;
        }

        setIsSubmitting(true);
        try {
            if (userReview) {
                // Update existing review
                await updateMaterialReview(
                    userReview.reviewId,
                    userId,
                    data.rating,
                    data.comment
                );
                toast.success("Відгук успішно оновлено");

                // Update local state
                setUserReview({
                    ...userReview,
                    rating: data.rating,
                    reviewText: data.comment,
                    updatedAt: new Date().toISOString(),
                });
            } else {
                const reviewId = await createMaterialReview(
                    materialId,
                    userId,
                    data.rating,
                    data.comment
                );
                toast.success("Відгук успішно додано");

                // Update local state
                setUserReview({
                    reviewId,
                    rating: data.rating,
                    reviewText: data.comment,
                    createdAt: new Date().toISOString(),
                    updatedAt: null,
                });
            }

            // Refresh reviews list with full data to update ratings and review count
            const updatedReviews = await getMaterialReviews(materialId);
            setReviewsData(updatedReviews);
        } catch (error) {
            toast.error("Не вдалося відправити відгук");
            console.error("Error submitting review:", error);
        } finally {
            setIsSubmitting(false);
        }
    };

    const handleDeleteReview = async () => {
        if (!userId || !userReview) return;

        if (window.confirm("Ви впевнені, що хочете видалити свій відгук?")) {
            setIsSubmitting(true);
            try {
                await deleteMaterialReview(userReview.reviewId, userId);
                toast.success("Відгук успішно видалено"); // Update local state
                setUserReview(null);

                // Refresh reviews list with full data to update ratings and review count
                const updatedReviews = await getMaterialReviews(materialId);
                setReviewsData(updatedReviews);
            } catch (error) {
                toast.error("Не вдалося видалити відгук");
                console.error("Error deleting review:", error);
            } finally {
                setIsSubmitting(false);
            }
        }
    };

    if (isLoading) {
        return <div className="py-4 text-center">Завантаження відгуків...</div>;
    }

    return (
        <div className="mt-8">
            {!isAuthor && userId && (
                <div className="mb-6">
                    {userReview ? (
                        <>
                            <MaterialReviewForm
                                initialRating={userReview.rating}
                                initialComment={userReview.reviewText}
                                isSubmitting={isSubmitting}
                                onSubmit={handleSubmitReview}
                            />
                            <div className="text-right">
                                <button
                                    onClick={handleDeleteReview}
                                    disabled={isSubmitting}
                                    className="text-red-600 text-sm hover:text-red-800"
                                >
                                    Видалити мій відгук
                                </button>
                            </div>
                        </>
                    ) : (
                        <MaterialReviewForm
                            isSubmitting={isSubmitting}
                            onSubmit={handleSubmitReview}
                        />
                    )}
                </div>
            )}

            {reviewsData && (
                <MaterialReviews
                    reviews={reviewsData.reviews}
                    reviewCount={reviewsData.reviewCount}
                    averageRating={reviewsData.averageRating}
                />
            )}
        </div>
    );
};

export default MaterialReviewContainer;
