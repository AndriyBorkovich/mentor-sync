import React from "react";

interface TestimonialCardProps {
    quote: string;
    author: string;
    role: string;
    avatarUrl?: string;
}

const TestimonialCard: React.FC<TestimonialCardProps> = ({
    quote,
    author,
    role,
    avatarUrl,
}) => {
    return (
        <div className="bg-white p-6 rounded-xl shadow-md">
            <div className="mb-4">
                {/* Quote icon */}
                <svg
                    className="w-8 h-8 text-primary opacity-30"
                    fill="currentColor"
                    viewBox="0 0 24 24"
                >
                    <path d="M14.017 21v-7.391c0-5.704 3.731-9.57 8.983-10.609l.995 2.151c-2.432.917-3.995 3.638-3.995 5.849h4v10h-9.983zm-14.017 0v-7.391c0-5.704 3.748-9.57 9-10.609l.996 2.151c-2.433.917-3.996 3.638-3.996 5.849h3.983v10h-9.983z" />
                </svg>
            </div>

            <p className="text-secondary mb-6">{quote}</p>

            <div className="flex items-center">
                <div className="w-12 h-12 rounded-full overflow-hidden flex-shrink-0 bg-gray-200 border border-primary">
                    {avatarUrl ? (
                        <img
                            src={avatarUrl}
                            alt={author}
                            className="w-full h-full object-cover"
                        />
                    ) : (
                        <div className="w-full h-full flex items-center justify-center text-primary font-bold">
                            {author.charAt(0)}
                        </div>
                    )}
                </div>
                <div className="ml-4">
                    <h4 className="font-bold text-secondary">{author}</h4>
                    <p className="text-textGray text-sm">{role}</p>
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
        <div className="py-12">
            <h2 className="text-3xl font-bold text-secondary text-center mb-4">
                Історії успіху
            </h2>
            <p className="text-textGray text-center mb-12 max-w-2xl mx-auto">
                Ось що говорять користувачі про свій досвід з MentorSync
            </p>

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
