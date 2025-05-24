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
        <div className="bg-white rounded-2xl shadow-md p-6">
            <div className="bg-primary w-12 h-12 rounded-full flex items-center justify-center mb-6">
                <div className="text-white">{icon}</div>
            </div>

            <h3 className="text-xl font-bold text-secondary mb-4">{title}</h3>
            <p className="text-textGray">{description}</p>
        </div>
    );
};

export default FeatureCard;
