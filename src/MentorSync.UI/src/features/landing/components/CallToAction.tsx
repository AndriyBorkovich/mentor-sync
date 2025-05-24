import React from "react";
import Button from "../../../components/ui/Button";

const CallToAction: React.FC = () => {
    return (
        <div
            className="rounded-3xl py-16 px-8 text-center shadow-xl relative overflow-hidden"
            style={{
                backgroundColor: "var(--color-primary)",
                color: "white",
            }}
        >
            {/* Background decorative elements */}
            <div
                className="absolute top-0 left-0 w-64 h-64 rounded-full -translate-x-1/2 -translate-y-1/2"
                style={{ backgroundColor: "white", opacity: 0.05 }}
            ></div>
            <div
                className="absolute bottom-0 right-0 w-96 h-96 rounded-full translate-x-1/3 translate-y-1/3"
                style={{ backgroundColor: "white", opacity: 0.05 }}
            ></div>{" "}
            <div className="relative z-10">
                <span
                    className="inline-flex items-center backdrop-blur-sm px-4 py-2 rounded-full mb-6"
                    style={{
                        backgroundColor: "rgba(255, 255, 255, 0.2)",
                        color: "white",
                    }}
                >
                    <span
                        className="w-2 h-2 rounded-full mr-2 animate-pulse"
                        style={{ backgroundColor: "white" }}
                    ></span>
                    Запуск платформи - Травень 2025
                </span>
                <h2 className="text-3xl md:text-4xl font-bold mb-6">
                    Готові розпочати свою ментор-подорож?
                </h2>
                <p
                    className="max-w-2xl mx-auto mb-12 text-lg leading-relaxed"
                    style={{ color: "rgba(255, 255, 255, 0.9)" }}
                >
                    Приєднуйтесь до MentorSync сьогодні та отримайте доступ до
                    мережі досвідчених професіоналів, які готові поділитися
                    своїми знаннями або станьте ментором самі та допомагайте
                    іншим.
                </p>{" "}
                <div className="flex flex-col sm:flex-row justify-center gap-6 mb-12">
                    <Button
                        className="shadow-md"
                        size="lg"
                        style={{
                            backgroundColor: "white",
                            color: "var(--color-primary)",
                        }}
                        onMouseOver={(
                            e: React.MouseEvent<HTMLButtonElement>
                        ) => {
                            e.currentTarget.style.backgroundColor = "#F9FAFB";
                        }}
                        onMouseOut={(
                            e: React.MouseEvent<HTMLButtonElement>
                        ) => {
                            e.currentTarget.style.backgroundColor = "white";
                        }}
                    >
                        Приєднатись як менті
                    </Button>
                    <Button
                        variant="outline"
                        className="shadow-md"
                        size="lg"
                        style={{
                            borderColor: "white",
                            color: "white",
                        }}
                        onMouseOver={(
                            e: React.MouseEvent<HTMLButtonElement>
                        ) => {
                            e.currentTarget.style.backgroundColor =
                                "rgba(255, 255, 255, 0.1)";
                        }}
                        onMouseOut={(
                            e: React.MouseEvent<HTMLButtonElement>
                        ) => {
                            e.currentTarget.style.backgroundColor =
                                "transparent";
                        }}
                    >
                        Стати ментором
                    </Button>
                </div>{" "}
                <div className="flex justify-center items-center flex-wrap gap-8">
                    <div className="flex items-center">
                        <svg
                            className="w-5 h-5 mr-2"
                            fill="currentColor"
                            viewBox="0 0 20 20"
                            style={{ color: "white" }}
                        >
                            <path
                                fillRule="evenodd"
                                d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z"
                                clipRule="evenodd"
                            ></path>
                        </svg>
                        <span>Безкоштовний доступ</span>
                    </div>
                    <div className="flex items-center">
                        <svg
                            className="w-5 h-5 mr-2"
                            fill="currentColor"
                            viewBox="0 0 20 20"
                            style={{ color: "white" }}
                        >
                            <path
                                fillRule="evenodd"
                                d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z"
                                clipRule="evenodd"
                            ></path>
                        </svg>
                        <span>Персоналізований підхід</span>
                    </div>
                    <div className="flex items-center">
                        <svg
                            className="w-5 h-5 mr-2"
                            fill="currentColor"
                            viewBox="0 0 20 20"
                            style={{ color: "white" }}
                        >
                            <path
                                fillRule="evenodd"
                                d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z"
                                clipRule="evenodd"
                            ></path>
                        </svg>
                        <span>Перевірені спеціалісти</span>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default CallToAction;
