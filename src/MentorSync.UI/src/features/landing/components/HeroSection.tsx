import React from "react";
import Button from "../../../components/ui/Button";
import PlaceholderImage from "../../../components/ui/PlaceholderImage";

const HeroSection: React.FC = () => {
    return (
        <div
            className="grid grid-cols-1 lg:grid-cols-2 gap-12 items-center relative"
            style={{
                display: "grid",
                gridTemplateColumns: "1fr",
                gap: "3rem",
                alignItems: "center",
                position: "relative",
            }}
        >
            {/* Background decorative element */}
            <div
                style={{
                    position: "absolute",
                    top: "-6rem",
                    right: "-6rem",
                    width: "16rem",
                    height: "16rem",
                    backgroundColor: "var(--color-primary-light)",
                    borderRadius: "9999px",
                    opacity: "0.2",
                    filter: "blur(64px)",
                }}
            ></div>
            <div
                style={{
                    position: "absolute",
                    bottom: "-6rem",
                    left: "-6rem",
                    width: "12rem",
                    height: "12rem",
                    backgroundColor: "var(--color-primary-light)",
                    borderRadius: "9999px",
                    opacity: "0.2",
                    filter: "blur(64px)",
                }}
            ></div>{" "}
            <div
                className="py-8 md:py-12 relative z-10"
                style={{
                    position: "relative",
                    zIndex: 10,
                }}
            >
                {" "}
                <h1
                    className="text-4xl md:text-5xl font-bold leading-tight mb-8"
                    style={{
                        color: "var(--color-secondary)",
                    }}
                >
                    Прискоріть
                    <br />
                    свою кар'єру
                    <br />
                    за допомогою
                    <br />
                    наставництва
                </h1>{" "}
                <p
                    className="text-lg md:text-xl mb-10 leading-relaxed"
                    style={{
                        color: "var(--color-text-gray)",
                    }}
                >
                    Зв'яжіться з досвідченими технічними професіоналами або
                    поділіться своєю досвідом. Приєднуйтесь до нашої спільноти
                    учнів та лідерів.
                </p>{" "}
                <div
                    className="flex flex-col sm:flex-row gap-4"
                    style={{
                        display: "flex",
                        flexDirection: "column",
                        gap: "1rem",
                    }}
                >
                    <Button variant="primary">Приєднуйтесь як менті</Button>
                    <Button variant="outline">Приєднуйтесь як ментор</Button>
                </div>
                <div
                    style={{
                        marginTop: "2.5rem",
                        display: "inline-flex",
                        alignItems: "center",
                        color: "var(--color-text-gray)",
                    }}
                >
                    <svg
                        style={{
                            width: "1.25rem",
                            height: "1.25rem",
                            marginRight: "0.5rem",
                            color: "#10B981", // green-500
                        }}
                        fill="currentColor"
                        viewBox="0 0 20 20"
                    >
                        <path
                            fillRule="evenodd"
                            d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z"
                            clipRule="evenodd"
                        ></path>
                    </svg>
                    <span>Безкоштовна реєстрація, почніть відразу</span>
                </div>
            </div>
            <div className="hidden lg:flex items-center justify-center relative z-10">
                {/* Placeholder for hero image */}
                <PlaceholderImage
                    height="500px"
                    width="100%"
                    text="Hero Image - Professional mentorship illustration"
                    backgroundColor="#f0f4f8"
                    className="rounded-2xl shadow-xl"
                    pattern="gradient"
                />
                {/* Stats badge positioned over the image */}{" "}
                <div
                    style={{
                        position: "absolute",
                        bottom: "-1.25rem",
                        left: "-1.25rem",
                        backgroundColor: "white",
                        padding: "1rem",
                        borderRadius: "0.75rem",
                        boxShadow: "0 10px 15px -3px rgba(0, 0, 0, 0.1)",
                        display: "flex",
                        alignItems: "center",
                    }}
                >
                    <div
                        style={{
                            backgroundColor: "rgba(67, 24, 209, 0.1)", // primary with 10% opacity
                            padding: "0.5rem",
                            borderRadius: "0.5rem",
                            marginRight: "0.75rem",
                        }}
                    >
                        <svg
                            style={{
                                width: "1.5rem",
                                height: "1.5rem",
                                color: "var(--color-primary)",
                            }}
                            fill="none"
                            stroke="currentColor"
                            viewBox="0 0 24 24"
                        >
                            <path
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                strokeWidth="2"
                                d="M13 7h8m0 0v8m0-8l-8 8-4-4-6 6"
                            ></path>
                        </svg>
                    </div>
                    <div>
                        <p
                            style={{
                                fontSize: "0.875rem",
                                fontWeight: "500",
                                color: "var(--color-text-gray)",
                            }}
                        >
                            Запуск платформи
                        </p>
                        <p
                            style={{
                                fontSize: "1.125rem",
                                fontWeight: "bold",
                                color: "var(--color-secondary)",
                            }}
                        >
                            Травень 2025
                        </p>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default HeroSection;
