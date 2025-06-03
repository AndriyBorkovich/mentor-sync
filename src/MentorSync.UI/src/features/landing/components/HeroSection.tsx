import React, { useState, useEffect } from "react";
import Button from "../../../components/ui/Button";

const HeroSection: React.FC = () => {
    const [isMobile, setIsMobile] = useState(false); // Default to desktop view for SSR

    useEffect(() => {
        // Set initial state
        setIsMobile(window.innerWidth < 1024);

        const handleResize = () => {
            setIsMobile(window.innerWidth < 1024);
        };

        window.addEventListener("resize", handleResize);
        return () => {
            window.removeEventListener("resize", handleResize);
        };
    }, []);

    return (
        <div
            className="grid grid-cols-1 lg:grid-cols-2 gap-12 items-center relative"
            style={{
                display: "grid",
                gridTemplateColumns: isMobile ? "1fr" : "1fr 1fr",
                gap: "3rem",
                alignItems: "center",
                position: "relative",
                maxWidth: "1200px",
                margin: "0 auto",
                padding: "0 1.5rem",
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
            ></div>
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
                    наставництва{" "}
                </h1>
                <p
                    className="text-lg md:text-xl mb-10 leading-relaxed"
                    style={{
                        color: "var(--color-text-gray)",
                    }}
                >
                    Зв'яжіться з досвідченими технічними професіоналами або
                    поділіться своєю досвідом. Приєднуйтесь до нашої спільноти
                    учнів та лідерів.
                </p>
                <div
                    className="flex flex-col sm:flex-row gap-4"
                    style={{
                        display: "flex",
                        flexDirection: isMobile ? "column" : "row",
                        gap: "1rem",
                    }}
                >
                    <Button
                        variant="primary"
                        style={{ minWidth: "200px" }}
                        onClick={() =>
                            (window.location.href = "/register?role=mentee")
                        }
                    >
                        Приєднуйтесь як менті
                    </Button>
                    <Button
                        variant="outline"
                        style={{ minWidth: "200px" }}
                        onClick={() =>
                            (window.location.href = "/register?role=mentor")
                        }
                    >
                        Приєднуйтесь як ментор
                    </Button>
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
            <div
                className={`${
                    isMobile ? "hidden" : "flex"
                } items-center justify-center relative z-10`}
            >
                {/* Placeholder for hero image               
                <PlaceholderImage
                    height="500px"
                    width="100%"
                    text="MentorSync - Прискорити свою кар'єру за допомогою наставництва"
                    backgroundColor="#f0f4f8"
                    className="rounded-2xl shadow-xl"
                    pattern="gradient"
                /> */}
                <img alt="mentor-sync" src="/main-logo.svg"></img>
            </div>
        </div>
    );
};

export default HeroSection;
