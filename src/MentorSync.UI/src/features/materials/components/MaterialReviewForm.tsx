import React from "react";
import { useForm, Controller } from "react-hook-form";
import { Rating } from "./Rating";

interface MaterialReviewFormProps {
    initialRating?: number;
    initialComment?: string;
    isSubmitting?: boolean;
    onSubmit: (data: { rating: number; comment: string }) => void;
}

interface FormValues {
    rating: number;
    comment: string;
}

const MaterialReviewForm: React.FC<MaterialReviewFormProps> = ({
    initialRating = 0,
    initialComment = "",
    isSubmitting = false,
    onSubmit,
}) => {
    const {
        control,
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<FormValues>({
        defaultValues: {
            rating: initialRating,
            comment: initialComment,
        },
    });

    const submitHandler = (data: FormValues) => {
        onSubmit(data);
    };

    return (
        <div className="bg-white p-6 rounded-lg shadow-sm mb-6">
            <h2 className="text-xl font-medium text-[#1E293B] mb-4">
                Залиште відгук про матеріал
            </h2>

            <form onSubmit={handleSubmit(submitHandler)}>
                <div className="mb-4">
                    <label className="block text-sm font-medium text-gray-700 mb-1">
                        Оцінка
                    </label>
                    <Controller
                        name="rating"
                        control={control}
                        rules={{ required: "Оцінка обов'язкова" }}
                        render={({ field }) => (
                            <Rating
                                value={field.value}
                                onChange={field.onChange}
                                disabled={isSubmitting}
                            />
                        )}
                    />
                    {errors.rating && (
                        <p className="text-red-500 text-sm mt-1">
                            {errors.rating.message}
                        </p>
                    )}
                </div>

                <div className="mb-4">
                    <label
                        htmlFor="comment"
                        className="block text-sm font-medium text-gray-700 mb-1"
                    >
                        Коментар
                    </label>
                    <textarea
                        id="comment"
                        rows={4}
                        className={`w-full px-3 py-2 border ${
                            errors.comment
                                ? "border-red-500"
                                : "border-gray-300"
                        } rounded-md focus:outline-none focus:ring-2 focus:ring-[#6C5DD3] focus:border-transparent`}
                        placeholder="Розкажіть про ваші враження від матеріалу..."
                        disabled={isSubmitting}
                        {...register("comment", {
                            required: "Коментар обов'язковий",
                            minLength: {
                                value: 10,
                                message:
                                    "Коментар повинен містити принаймні 10 символів",
                            },
                        })}
                    />
                    {errors.comment && (
                        <p className="text-red-500 text-sm mt-1">
                            {errors.comment.message}
                        </p>
                    )}
                </div>

                <button
                    type="submit"
                    disabled={isSubmitting}
                    className={`w-full py-2 px-4 rounded-md font-medium text-white ${
                        isSubmitting
                            ? "bg-[#9A8EE0] cursor-not-allowed"
                            : "bg-[#6C5DD3] hover:bg-[#5A4BBF] focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-[#6C5DD3]"
                    }`}
                >
                    {isSubmitting ? "Відправляємо..." : "Відправити відгук"}
                </button>
            </form>
        </div>
    );
};

export default MaterialReviewForm;
