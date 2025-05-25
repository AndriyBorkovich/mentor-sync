import React, { useState } from "react";

interface CommentFormProps {
    onSubmit: (content: string) => void;
    isLoading: boolean;
}

const CommentForm: React.FC<CommentFormProps> = ({ onSubmit, isLoading }) => {
    const [comment, setComment] = useState("");

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (comment.trim()) {
            onSubmit(comment);
            setComment("");
        }
    };

    return (
        <form onSubmit={handleSubmit} className="mt-6">
            <div className="mb-4">
                <label
                    htmlFor="comment"
                    className="block text-sm font-medium text-[#1E293B] mb-2"
                >
                    Додати коментар
                </label>
                <textarea
                    id="comment"
                    rows={3}
                    value={comment}
                    onChange={(e) => setComment(e.target.value)}
                    className="w-full p-3 border border-[#E2E8F0] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#6C5DD3] focus:border-[#6C5DD3]"
                    placeholder="Напишіть свій коментар тут..."
                    disabled={isLoading}
                />
            </div>
            <div className="flex justify-end">
                <button
                    type="submit"
                    className="px-4 py-2 bg-[#6C5DD3] text-white rounded-lg hover:bg-[#5B4DC4] disabled:opacity-50 disabled:cursor-not-allowed"
                    disabled={isLoading || !comment.trim()}
                >
                    {isLoading ? (
                        <span className="flex items-center">
                            <svg
                                className="animate-spin -ml-1 mr-2 h-4 w-4 text-white"
                                xmlns="http://www.w3.org/2000/svg"
                                fill="none"
                                viewBox="0 0 24 24"
                            >
                                <circle
                                    className="opacity-25"
                                    cx="12"
                                    cy="12"
                                    r="10"
                                    stroke="currentColor"
                                    strokeWidth="4"
                                ></circle>
                                <path
                                    className="opacity-75"
                                    fill="currentColor"
                                    d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                                ></path>
                            </svg>
                            Відправка...
                        </span>
                    ) : (
                        "Відправити коментар"
                    )}
                </button>
            </div>
        </form>
    );
};

export interface Comment {
    id: string;
    author: string;
    authorAvatar?: string;
    content: string;
    timestamp: string;
}

interface CommentsListProps {
    comments: Comment[];
}

const CommentsList: React.FC<CommentsListProps> = ({ comments }) => {
    if (comments.length === 0) {
        return (
            <div className="text-center py-8 text-[#64748B]">
                <p>Коментарі відсутні. Будьте першим, хто залишить коментар.</p>
            </div>
        );
    }

    return (
        <div className="space-y-6">
            {comments.map((comment) => (
                <div key={comment.id} className="flex">
                    <div className="h-10 w-10 rounded-full bg-gray-200 mr-4 flex-shrink-0">
                        {comment.authorAvatar && (
                            <img
                                src={comment.authorAvatar}
                                alt={comment.author}
                                className="h-full w-full rounded-full object-cover"
                            />
                        )}
                    </div>
                    <div className="flex-1">
                        <div className="flex justify-between items-center mb-1">
                            <h4 className="font-medium text-[#1E293B]">
                                {comment.author}
                            </h4>
                            <span className="text-xs text-[#94A3B8]">
                                {comment.timestamp}
                            </span>
                        </div>
                        <p className="text-[#64748B]">{comment.content}</p>
                    </div>
                </div>
            ))}
        </div>
    );
};

export { CommentForm, CommentsList };
