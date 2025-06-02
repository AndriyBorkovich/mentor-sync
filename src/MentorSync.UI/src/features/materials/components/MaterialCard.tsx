import React from "react";
import { useNavigate } from "react-router-dom";
import { Material, RecommendedMaterial } from "../../../shared/types";

interface MaterialCardProps {
    material: Material | RecommendedMaterial;
    showRecommendationScores?: boolean;
}

const MaterialCard: React.FC<MaterialCardProps> = ({
    material,
    showRecommendationScores = false,
}) => {
    const navigate = useNavigate();

    // Type guard to check if material is a RecommendedMaterial
    const isRecommendedMaterial = (
        material: Material | RecommendedMaterial
    ): material is RecommendedMaterial => {
        return (
            "collaborativeScore" in material &&
            "contentBasedScore" in material &&
            "finalScore" in material
        );
    };

    const handleClick = () => {
        navigate(`/materials/${material.id}`);
    };

    // Function to determine icon based on material type
    const getTypeIcon = (type: string) => {
        switch (type) {
            case "document":
                return "description";
            case "video":
                return "play_circle";
            case "link":
                return "link";
            case "presentation":
                return "slideshow";
            default:
                return "description";
        }
    };

    // Format score to percentages with 1 decimal place
    const formatScore = (score: number) => {
        return `${(score ?? 0).toFixed(1)}`;
    };

    return (
        <div
            className="bg-white p-5 rounded-lg shadow-sm cursor-pointer hover:shadow-md transition-shadow"
            onClick={handleClick}
        >
            <div className="flex items-center mb-4">
                <div className="w-10 h-10 rounded-lg bg-[#F1F5F9] flex items-center justify-center text-[#6C5DD3] mr-3">
                    <span className="material-icons">
                        {getTypeIcon(material.type)}
                    </span>
                </div>
                <div>
                    <h3 className="font-medium text-[#1E293B] text-base truncate">
                        {material.title}
                    </h3>
                    <p className="text-xs text-[#64748B]">
                        {material.mentorName}
                    </p>
                </div>
            </div>
            <p className="text-[#64748B] text-sm mb-4 line-clamp-2">
                {material.description}
            </p>
            <div className="flex flex-wrap gap-2 mb-4">
                {material.tags.map((tag, index) => (
                    <span
                        key={index}
                        className="text-xs py-1 px-2 bg-[#F1F5F9] text-[#64748B] rounded-md"
                    >
                        {tag}
                    </span>
                ))}
            </div>{" "}
            {showRecommendationScores && isRecommendedMaterial(material) && (
                <div className="flex flex-col gap-1 mb-2">
                    <div className="flex justify-between text-xs">
                        <span
                            className="text-[#64748B] cursor-help"
                            title="Рекомендація на основі схожості контенту: оцінює наскільки цей матеріал відповідає вашим інтересам за тематикою та змістом"
                        >
                            На основі контенту:
                        </span>
                        <span
                            className="font-medium text-[#1E293B] cursor-help"
                            title="Оцінка від 0 до 10, де 10 - найвищий рівень відповідності вашим інтересам за змістом"
                        >
                            {formatScore(material.contentBasedScore)}
                        </span>
                    </div>
                    <div className="flex justify-between text-xs">
                        <span
                            className="text-[#64748B] cursor-help"
                            title="Рекомендація на основі взаємодій: оцінює наскільки цей матеріал відповідає вашим уподобанням, ґрунтуючись на історії взаємодій схожих на вас користувачів"
                        >
                            На основі взаємодій:
                        </span>
                        <span
                            className="font-medium text-[#1E293B] cursor-help"
                            title="Оцінка від 0 до 10, де 10 - найвищий рівень відповідності вашим уподобанням на основі взаємодій інших користувачів"
                        >
                            {formatScore(material.collaborativeScore)}
                        </span>
                    </div>
                    <div className="flex justify-between text-xs">
                        <span
                            className="text-[#64748B] cursor-help"
                            title="Загальна оцінка релевантності: комбінований показник на основі змісту і взаємодій"
                        >
                            Загалом:
                        </span>
                        <span
                            className="font-medium text-[#1E293B] cursor-help"
                            title="Підсумкова оцінка від 0 до 10, яка враховує як схожість за змістом, так і взаємодії користувачів"
                        >
                            {formatScore(material.finalScore)}
                        </span>
                    </div>
                </div>
            )}
            <div className="flex justify-between items-center text-xs text-[#94A3B8]">
                <span>{material.createdAt}</span>
                {material.fileSize && <span>{material.fileSize}</span>}
            </div>
        </div>
    );
};

export default MaterialCard;
