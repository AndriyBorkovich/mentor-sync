import React from "react";
import { Mentor } from "../../../dashboard/data/mentors";

interface ReviewsTabProps {
    mentor: Mentor;
}

// Mock review data
interface Review {
    id: string;
    authorName: string;
    date: string;
    rating: number;
    content: string;
}

const ReviewsTab: React.FC<ReviewsTabProps> = () => {
    // Mock reviews data
    const reviews: Review[] = [
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

    return (
        <div>
            <div className="space-y-6">
                {reviews.map((review) => (
                    <div key={review.id}>
                        <div className="flex justify-between items-center mb-2">
                            <div className="flex items-center">
                                <h3 className="text-base font-medium text-[#1E293B] mr-3">
                                    {review.authorName}
                                </h3>
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
                            <span className="text-sm text-[#64748B]">
                                {review.date}
                            </span>
                        </div>
                        <p className="text-sm text-[#64748B]">
                            {review.content}
                        </p>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default ReviewsTab;
