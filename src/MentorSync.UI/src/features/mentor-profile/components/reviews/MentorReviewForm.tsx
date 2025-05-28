import React, { useState, useEffect, useCallback } from "react";
import { useForm } from "react-hook-form";
import { toast } from "react-toastify";
import {
    createMentorReview,
    updateMentorReview,
    deleteMentorReview,
    checkMentorReview,
    CheckReviewResult,
} from "../../services/mentorProfileService";
import { hasRole } from "../../../auth/utils/authUtils";

interface MentorReviewFormProps {
    mentorId: number;
    onReviewSubmitted?: () => void;
}

interface ReviewFormData {
    rating: number;
    reviewText: string;
}

const MentorReviewForm: React.FC<MentorReviewFormProps> = ({
    mentorId,
    onReviewSubmitted,
}) => {
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [existingReview, setExistingReview] =
        useState<CheckReviewResult | null>(null);
    const [isEditing, setIsEditing] = useState(false);
    const [isLoading, setIsLoading] = useState(false);
    const isMentee = hasRole("Mentee");

    const {
        register,
        handleSubmit,
        setValue,
        watch,
        reset,
        formState: { errors },
    } = useForm<ReviewFormData>({
        defaultValues: {
            rating: 5,
            reviewText: "",
        },
    });

    const watchRating = watch("rating");

    // Memoize the checkExistingReview function to prevent recreating it on every render
    const checkExistingReview = useCallback(async () => {
        if (!mentorId || !isMentee || isLoading) return;

        setIsLoading(true);
        try {
            const result = await checkMentorReview(mentorId);
            setExistingReview(result);

            if (result.hasReviewed && result.rating && result.reviewText) {
                setValue("rating", result.rating);
                setValue("reviewText", result.reviewText);
            }
        } catch (error) {
            console.error("Error checking existing review:", error);
        } finally {
            setIsLoading(false);
        }
    }, [mentorId, isMentee, setValue]);

    // Check if the user has already reviewed this mentor only once when component mounts
    useEffect(() => {
        checkExistingReview();
    }, [checkExistingReview]);

    const handleEditClick = () => {
        setIsEditing(true);
    };

    const handleCancelEdit = () => {
        setIsEditing(false);
        // Reset form to existing review values
        if (existingReview?.rating && existingReview?.reviewText) {
            setValue("rating", existingReview.rating);
            setValue("reviewText", existingReview.reviewText);
        }
    };

    const handleDeleteReview = async () => {
        if (
            !existingReview?.reviewId ||
            !window.confirm("Ви впевнені, що хочете видалити цей відгук?")
        ) {
            return;
        }

        setIsSubmitting(true);
        try {
            const success = await deleteMentorReview(existingReview.reviewId);
            if (success) {
                toast.success("Відгук успішно видалено");
                setExistingReview(null);
                reset();
                if (onReviewSubmitted) {
                    onReviewSubmitted();
                }
            } else {
                toast.error("Не вдалося видалити відгук");
            }
        } catch (error) {
            console.error("Error deleting review:", error);
            toast.error("Виникла помилка при видаленні відгуку");
        } finally {
            setIsSubmitting(false);
        }
    };

    const onSubmit = async (data: ReviewFormData) => {
        setIsSubmitting(true);
        try {
            let success;

            if (existingReview?.hasReviewed && existingReview?.reviewId) {
                // Update existing review
                success = await updateMentorReview(
                    existingReview.reviewId,
                    data.rating,
                    data.reviewText
                );
                if (success) {
                    toast.success("Відгук успішно оновлено");
                    setIsEditing(false);
                }
            } else {
                // Create new review
                success = await createMentorReview(
                    mentorId,
                    data.rating,
                    data.reviewText
                );
                if (success) {
                    toast.success("Відгук успішно додано");
                }
            }

            if (success && onReviewSubmitted) {
                onReviewSubmitted();
                // Update local state
                setExistingReview({
                    hasReviewed: true,
                    reviewId: existingReview?.reviewId,
                    rating: data.rating,
                    reviewText: data.reviewText,
                });
            } else if (!success) {
                toast.error("Не вдалося зберегти відгук");
            }
        } catch (error) {
            console.error("Error submitting review:", error);
            toast.error("Виникла помилка при збереженні відгуку");
        } finally {
            setIsSubmitting(false);
        }
    };

    if (!isMentee) {
        return null;
    }

    if (existingReview?.hasReviewed && !isEditing) {
        return (
            <div className="bg-[#F8FAFC] p-4 rounded-lg mb-6">
                <div className="flex justify-between items-start mb-4">
                    <h3 className="font-medium text-[#1E293B]">Ваш відгук</h3>
                    <div className="flex space-x-2">
                        <button
                            onClick={handleEditClick}
                            className="text-sm text-[#4318D1] hover:text-[#3712A5]"
                            disabled={isSubmitting}
                        >
                            Редагувати
                        </button>
                        <button
                            onClick={handleDeleteReview}
                            className="text-sm text-red-600 hover:text-red-700"
                            disabled={isSubmitting}
                        >
                            Видалити
                        </button>
                    </div>
                </div>

                <div className="flex items-center mb-2">
                    {[1, 2, 3, 4, 5].map((star) => (
                        <span
                            key={star}
                            className="material-icons text-yellow-500"
                        >
                            {star <= (existingReview?.rating || 0)
                                ? "star"
                                : "star_border"}
                        </span>
                    ))}
                </div>

                <p className="text-[#64748B]">{existingReview?.reviewText}</p>
            </div>
        );
    }

    return (
        <div className="bg-[#F8FAFC] p-4 rounded-lg mb-6">
            <h3 className="font-medium text-[#1E293B] mb-4">
                {existingReview?.hasReviewed
                    ? "Редагувати відгук"
                    : "Залишити відгук"}
            </h3>

            <form onSubmit={handleSubmit(onSubmit)}>
                <div className="mb-4">
                    <label className="block text-sm font-medium text-[#1E293B] mb-1">
                        Оцінка
                    </label>
                    <div className="flex">
                        {[1, 2, 3, 4, 5].map((star) => (
                            <button
                                key={star}
                                type="button"
                                onClick={() => setValue("rating", star)}
                                className="focus:outline-none"
                            >
                                <span
                                    className={`material-icons text-2xl ${
                                        star <= watchRating
                                            ? "text-yellow-500"
                                            : "text-gray-300"
                                    }`}
                                >
                                    star
                                </span>
                            </button>
                        ))}
                    </div>
                    {errors.rating && (
                        <p className="text-red-500 text-xs mt-1">
                            {errors.rating.message}
                        </p>
                    )}
                </div>

                <div className="mb-4">
                    <label className="block text-sm font-medium text-[#1E293B] mb-1">
                        Відгук
                    </label>
                    <textarea
                        {...register("reviewText", {
                            required: "Будь ласка, напишіть відгук",
                            minLength: {
                                value: 10,
                                message:
                                    "Відгук повинен містити не менше 10 символів",
                            },
                        })}
                        className="w-full p-2 border border-gray-300 rounded-md focus:ring-[#4318D1] focus:border-[#4318D1]"
                        rows={4}
                        placeholder="Поділіться своїми враженнями від співпраці з цим ментором..."
                    />
                    {errors.reviewText && (
                        <p className="text-red-500 text-xs mt-1">
                            {errors.reviewText.message}
                        </p>
                    )}
                </div>

                <div className="flex justify-end space-x-2">
                    {existingReview?.hasReviewed && (
                        <button
                            type="button"
                            onClick={handleCancelEdit}
                            className="px-4 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-50"
                            disabled={isSubmitting}
                        >
                            Скасувати
                        </button>
                    )}

                    <button
                        type="submit"
                        className="px-4 py-2 bg-[#4318D1] text-white rounded-md hover:bg-[#3712A5] disabled:opacity-50"
                        disabled={isSubmitting}
                    >
                        {isSubmitting ? (
                            <span className="inline-block w-4 h-4 border-2 border-white border-t-transparent rounded-full animate-spin" />
                        ) : existingReview?.hasReviewed ? (
                            "Зберегти зміни"
                        ) : (
                            "Надіслати відгук"
                        )}
                    </button>
                </div>
            </form>
        </div>
    );
};

export default MentorReviewForm;
