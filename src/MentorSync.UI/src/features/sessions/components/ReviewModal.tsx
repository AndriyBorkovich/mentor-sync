import React from "react";
import { PastSession } from "../data/pastSessions";
import { useForm, Controller } from "react-hook-form";

interface ReviewModalProps {
    session: PastSession;
    onClose: () => void;
    onSubmit: (sessionId: string, rating: number, comment: string) => void;
}

interface ReviewFormData {
    rating: number;
    comment: string;
}

const ReviewModal: React.FC<ReviewModalProps> = ({
    session,
    onClose,
    onSubmit,
}) => {
    const {
        control,
        register,
        handleSubmit,
        watch,
        formState: { isSubmitting, errors },
    } = useForm<ReviewFormData>({
        defaultValues: {
            rating: 0,
            comment: "",
        },
    });

    const rating = watch("rating");

    const onSubmitForm = (data: ReviewFormData) => {
        // Simulate API call
        return new Promise<void>((resolve) => {
            setTimeout(() => {
                onSubmit(session.id, data.rating, data.comment);
                onClose();
                resolve();
            }, 500);
        });
    };

    return (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
            <div className="bg-white rounded-lg max-w-lg w-full mx-4">
                <div className="p-6">
                    <div className="flex justify-between items-center mb-6">
                        <h2 className="text-xl font-bold text-[#1E293B]">
                            Залишити відгук
                        </h2>
                        <button
                            className="text-[#64748B] hover:text-[#1E293B]"
                            onClick={onClose}
                        >
                            <span className="material-icons">close</span>
                        </button>
                    </div>

                    <div className="mb-6">
                        <div className="flex items-center mb-2">
                            <img
                                src="/placeholder-avatar.jpg"
                                alt={session.mentorName}
                                className="w-12 h-12 rounded-full bg-gray-200 mr-4"
                            />
                            <div>
                                <h3 className="font-medium text-[#1E293B]">
                                    {session.mentorName}
                                </h3>
                                <p className="text-sm text-[#64748B]">
                                    {session.title}
                                </p>
                            </div>
                        </div>

                        <p className="text-sm text-[#64748B] mt-2">
                            {session.date} о {session.time}
                        </p>
                    </div>

                    <form onSubmit={handleSubmit(onSubmitForm)}>
                        <div className="mb-6">
                            <label className="block text-sm font-medium text-[#1E293B] mb-2">
                                Оцінка
                            </label>
                            <Controller
                                name="rating"
                                control={control}
                                rules={{ required: "Оцінка є обов'язковою" }}
                                render={({ field }) => (
                                    <div className="flex">
                                        {[1, 2, 3, 4, 5].map((star) => (
                                            <button
                                                key={star}
                                                type="button"
                                                className="text-2xl focus:outline-none"
                                                onClick={() =>
                                                    field.onChange(star)
                                                }
                                            >
                                                <span className="material-icons text-yellow-500">
                                                    {field.value >= star
                                                        ? "star"
                                                        : "star_border"}
                                                </span>
                                            </button>
                                        ))}
                                    </div>
                                )}
                            />
                            {errors.rating && (
                                <p className="text-red-500 text-sm mt-1">
                                    {errors.rating.message}
                                </p>
                            )}
                        </div>

                        <div className="mb-6">
                            <label className="block text-sm font-medium text-[#1E293B] mb-2">
                                Коментар
                            </label>
                            <textarea
                                {...register("comment", {
                                    required: "Будь ласка, напишіть коментар",
                                })}
                                className="w-full p-3 border border-[#E2E8F0] rounded-lg h-32"
                                placeholder="Поділіться вашою думкою про сесію..."
                            />
                            {errors.comment && (
                                <p className="text-red-500 text-sm mt-1">
                                    {errors.comment.message}
                                </p>
                            )}
                        </div>

                        <div className="flex justify-end">
                            <button
                                type="button"
                                className="mr-4 px-6 py-3 border border-[#E2E8F0] rounded-lg text-[#64748B]"
                                onClick={onClose}
                                disabled={isSubmitting}
                            >
                                Скасувати
                            </button>
                            <button
                                type="submit"
                                className="px-6 py-3 bg-[#6C5DD3] text-white rounded-lg hover:bg-[#5B4DC4]"
                                disabled={isSubmitting || rating === 0}
                            >
                                {isSubmitting
                                    ? "Відправляємо..."
                                    : "Відправити відгук"}
                            </button>{" "}
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
};

export default ReviewModal;
