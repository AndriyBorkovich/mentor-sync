import React from "react";
import { Material } from "../data/materials";
import { useNavigate } from "react-router-dom";

interface RelatedMaterialItemProps {
    material: Material;
}

const RelatedMaterialItem: React.FC<RelatedMaterialItemProps> = ({
    material,
}) => {
    const navigate = useNavigate();

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
            onClick={() => navigate(`/materials/${material.id}`)}
            className="flex items-center p-3 hover:bg-[#F1F5F9] rounded-lg cursor-pointer"
        >
            <div className="w-8 h-8 rounded-lg bg-[#F1F5F9] flex items-center justify-center text-[#6C5DD3] mr-3">
                <span className="material-icons text-sm">
                    {getTypeIcon(material.type)}
                </span>
            </div>
            <div className="flex-1 min-w-0">
                <h4 className="font-medium text-[#1E293B] text-sm truncate">
                    {material.title}
                </h4>
                <p className="text-xs text-[#64748B] truncate">
                    {material.mentorName}
                </p>
            </div>
        </div>
    );
};

interface RelatedMaterialsProps {
    currentMaterial: Material;
    materials: Material[];
    limit?: number;
}

const RelatedMaterials: React.FC<RelatedMaterialsProps> = ({
    currentMaterial,
    materials,
    limit = 3,
}) => {
    // Filter out the current material and find materials with matching tags
    const relatedMaterials = materials
        .filter((m) => m.id !== currentMaterial.id)
        .filter((m) => m.tags.some((tag) => currentMaterial.tags.includes(tag)))
        .slice(0, limit);

    if (relatedMaterials.length === 0) {
        return null;
    }

    return (
        <div className="bg-white rounded-lg p-6 shadow-sm">
            <h2 className="text-xl font-medium text-[#1E293B] mb-4">
                Схожі матеріали
            </h2>
            <div className="space-y-2">
                {relatedMaterials.map((material) => (
                    <RelatedMaterialItem
                        key={material.id}
                        material={material}
                    />
                ))}
            </div>
        </div>
    );
};

export default RelatedMaterials;
