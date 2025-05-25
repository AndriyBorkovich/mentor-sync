import React from "react";
import { Material } from "../data/materials";
import { useNavigate } from "react-router-dom";

interface MaterialCardProps {
    material: Material;
}

const MaterialCard: React.FC<MaterialCardProps> = ({ material }) => {
    const navigate = useNavigate();

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
            </div>

            <div className="flex justify-between items-center text-xs text-[#94A3B8]">
                <span>{material.createdAt}</span>
                {material.fileSize && <span>{material.fileSize}</span>}
            </div>
        </div>
    );
};

export default MaterialCard;
