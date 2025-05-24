import React from "react";

interface FeatureCardProps {
    icon: React.ReactNode;
    title: string;
    description: string;
}

const FeatureCard: React.FC<FeatureCardProps> = ({
    icon,
    title,
    description,
}) => {
    return (
        <div
            className="bg-white rounded-2xl shadow-md p-8 hover:shadow-lg transition-shadow border border-gray-100"
            style={{
                backgroundColor: "white",
                borderRadius: "1rem",
                boxShadow: "0 4px 6px -1px rgba(0, 0, 0, 0.1)",
                padding: "2rem",
                transition: "box-shadow 0.3s ease",
                border: "1px solid #f3f4f6",
            }}
            onMouseOver={(e) => {
                e.currentTarget.style.boxShadow =
                    "0 10px 15px -3px rgba(0, 0, 0, 0.1)";
            }}
            onMouseOut={(e) => {
                e.currentTarget.style.boxShadow =
                    "0 4px 6px -1px rgba(0, 0, 0, 0.1)";
            }}
        >
            <div
                className="w-14 h-14 rounded-full flex items-center justify-center mb-8"
                style={{
                    backgroundColor: "var(--color-primary)",
                    width: "3.5rem",
                    height: "3.5rem",
                    borderRadius: "9999px",
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
                    marginBottom: "2rem",
                }}
            >
                <div style={{ color: "white" }}>{icon}</div>
            </div>

            <h3
                className="text-xl font-bold mb-4"
                style={{
                    fontSize: "1.25rem",
                    fontWeight: "bold",
                    marginBottom: "1rem",
                    color: "var(--color-secondary)",
                }}
            >
                {title}
            </h3>
            <p
                className="leading-relaxed"
                style={{
                    lineHeight: "1.625",
                    color: "var(--color-text-gray)",
                }}
            >
                {description}
            </p>
        </div>
    );
};

export default FeatureCard;
