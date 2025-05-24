import React from "react";

interface TestimonialCardProps {
    quote: string;
    author: string;
    role: string;
    avatarUrl?: string;
    rating?: number;
}

const TestimonialCard: React.FC<TestimonialCardProps> = ({
    quote,
    author,
    role,
    avatarUrl,
    rating = 5,
}) => {
    return (
        <div
            className="p-8 rounded-2xl shadow-custom border transition-all"
            style={{
                backgroundColor: "white",
                borderColor: "#E2E8F0",
            }}
            onMouseOver={(e) => {
                e.currentTarget.style.boxShadow =
                    "0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05)";
            }}
            onMouseOut={(e) => {
                e.currentTarget.style.boxShadow =
                    "0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06)";
            }}
        >
            <div className="mb-4">
                {/* Quote icon */}
                <svg
                    className="w-10 h-10 opacity-20"
                    fill="currentColor"
                    viewBox="0 0 24 24"
                    style={{ color: "var(--color-primary)" }}
                >
                    <path d="M14.017 21v-7.391c0-5.704 3.731-9.57 8.983-10.609l.995 2.151c-2.432.917-3.995 3.638-3.995 5.849h4v10h-9.983zm-14.017 0v-7.391c0-5.704 3.748-9.57 9-10.609l.996 2.151c-2.433.917-3.996 3.638-3.996 5.849h3.983v10h-9.983z" />
                </svg>
            </div>
            <p
                className="text-lg mb-8 leading-relaxed italic"
                style={{ color: "var(--color-secondary)" }}
            >
                {quote}
            </p>{" "}
            {/* Star rating */}
            <div className="flex items-center mb-6">
                {[...Array(5)].map((_, i) => (
                    <svg
                        key={i}
                        className="w-5 h-5"
                        fill="currentColor"
                        viewBox="0 0 20 20"
                        style={{
                            color: i < rating ? "#F59E0B" : "#D1D5DB",
                        }}
                    >
                        <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z" />
                    </svg>
                ))}
            </div>{" "}
            <div className="flex items-center">
                <div
                    className="w-12 h-12 rounded-full overflow-hidden flex-shrink-0 shadow-sm"
                    style={{
                        backgroundColor: "#E5E7EB",
                        borderWidth: "2px",
                        borderStyle: "solid",
                        borderColor: "var(--color-primary)",
                    }}
                >
                    {avatarUrl ? (
                        <img
                            src={avatarUrl}
                            alt={author}
                            className="w-full h-full object-cover"
                        />
                    ) : (
                        <div
                            className="w-full h-full flex items-center justify-center font-bold"
                            style={{ color: "var(--color-primary)" }}
                        >
                            {author.charAt(0)}
                        </div>
                    )}
                </div>
                <div className="ml-4">
                    <h4
                        className="font-bold"
                        style={{ color: "var(--color-secondary)" }}
                    >
                        {author}
                    </h4>
                    <p
                        className="text-sm"
                        style={{ color: "var(--color-text-gray)" }}
                    >
                        {role}
                    </p>
                </div>
            </div>
        </div>
    );
};

const TestimonialsSection: React.FC = () => {
    const testimonials = [
        {
            quote: "Завдяки MentorSync я знайшов ментора, який допоміг мені покращити мої навички розробки та отримати кращу роботу вже через 3 місяці.",
            author: "Олександр К.",
            role: "Frontend Developer",
        },
        {
            quote: "Як ментор, я можу гнучко планувати свій час і допомагати іншим. Це дуже зручна платформа для обміну знаннями.",
            author: "Наталія С.",
            role: "Senior Software Engineer",
        },
        {
            quote: "Відмінна платформа для професійного зростання. Отримую персоналізовані поради та практичний досвід від експертів.",
            author: "Дмитро В.",
            role: "Junior Developer",
        },
    ];

    return (
        <div className="py-16">
            <div className="text-center mb-16">
                <h2
                    className="text-3xl md:text-4xl font-bold mb-4"
                    style={{ color: "var(--color-secondary)" }}
                >
                    Історії успіху
                </h2>
                <p
                    className="text-lg mb-12 max-w-2xl mx-auto"
                    style={{ color: "var(--color-text-gray)" }}
                >
                    Ось що говорять користувачі про свій досвід з MentorSync
                </p>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
                {testimonials.map((testimonial, index) => (
                    <TestimonialCard
                        key={index}
                        quote={testimonial.quote}
                        author={testimonial.author}
                        role={testimonial.role}
                    />
                ))}
            </div>
        </div>
    );
};

export default TestimonialsSection;
