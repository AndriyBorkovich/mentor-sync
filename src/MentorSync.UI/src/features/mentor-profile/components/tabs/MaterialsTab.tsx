import React from "react";
import { MentorData, isMentorProfile } from "../../types/mentorTypes";

interface MaterialsTabProps {
    mentor: MentorData;
}

// Mock materials data
interface Material {
    id: string;
    title: string;
    description?: string; // Added description
    type: string;
    date: string;
    url?: string;
    tags?: string[]; // Added tags
    contentMarkdown?: string; // Added contentMarkdown
    attachments?: Array<{ id: number; fileName: string; fileUrl: string }>; // Added attachments
}

const MaterialsTab: React.FC<MaterialsTabProps> = ({ mentor }) => {
    // Mock materials as fallback
    const mockMaterials: Material[] = [
        {
            id: "1",
            title: "Introduction to System Design",
            description: "Learn the basics of system design",
            type: "article",
            date: "Грудень 2023",
        },
        {
            id: "2",
            title: "Microservices Architecture Patterns",
            description: "Explore common microservices patterns",
            type: "article",
            date: "Листопад 2023",
        },
        {
            id: "3",
            title: "Scaling Distributed Systems",
            description: "How to scale your distributed applications",
            type: "video",
            date: "Жовтень 2023",
        },
        {
            id: "4",
            title: "Cloud Architecture Best Practices",
            description: "Best practices for cloud architecture",
            type: "video",
            date: "Вересень 2023",
        },
    ];

    // Format material date from ISO string
    const formatMaterialDate = (dateString: string): string => {
        const date = new Date(dateString);
        return date.toLocaleDateString("uk-UA", {
            month: "long",
            year: "numeric",
        });
    };

    // Get materials from API data if available, otherwise use mock data
    const getMaterials = (): Material[] => {
        if (
            isMentorProfile(mentor) &&
            mentor.materials &&
            mentor.materials.length > 0
        ) {
            return mentor.materials.map((material) => ({
                id: material.id.toString(),
                title: material.title,
                description: material.description,
                type: material.type,
                date: formatMaterialDate(material.createdOn),
                url: material.url,
                tags: material.tags,
                contentMarkdown: material.contentMarkdown,
                attachments: material.attachments,
            }));
        }
        return mockMaterials;
    };

    const materials = getMaterials(); // Handle view material
    const handleViewMaterial = (material: Material) => {
        if (material.url) {
            window.open(material.url, "_blank", "noopener,noreferrer");
        } else if (material.contentMarkdown) {
            // In a real implementation, navigate to a dedicated page with markdown rendering
            // For now, we'll use localStorage to demonstrate the concept
            localStorage.setItem(
                "viewMaterial",
                JSON.stringify({
                    title: material.title,
                    content: material.contentMarkdown,
                    type: material.type,
                    description: material.description,
                    attachments: material.attachments || [],
                })
            );
            window.open(
                `/materials/view/${material.id}`,
                "_blank",
                "noopener,noreferrer"
            );
        } else {
            alert(`Відкриваю матеріал: ${material.title}`);
        }
    };

    // Translate material type to Ukrainian
    const translateMaterialType = (type: string): string => {
        const typeMap: Record<string, string> = {
            article: "Стаття",
            video: "Відео",
            pdf: "PDF документ",
            link: "Посилання",
        };

        return typeMap[type.toLowerCase()] || type;
    };

    return (
        <div>
            <h2 className="text-lg font-medium text-[#1E293B] mb-4">
                Навчальні матеріали
            </h2>

            <div className="space-y-4">
                {materials.map((material) => (
                    <div
                        key={material.id}
                        className="border border-[#E2E8F0] p-4 rounded-lg flex justify-between items-center"
                    >
                        <div className="flex-1">
                            <h3 className="text-base font-medium text-[#1E293B]">
                                {material.title}
                            </h3>
                            {material.description && (
                                <p className="text-sm text-[#64748B] mt-1 line-clamp-2">
                                    {material.description}
                                </p>
                            )}
                            <div className="flex mt-2 text-sm text-[#64748B]">
                                <span className="mr-4">
                                    {translateMaterialType(material.type)}
                                </span>
                                <span>{material.date}</span>
                            </div>
                            {material.tags && material.tags.length > 0 && (
                                <div className="flex flex-wrap gap-1 mt-2">
                                    {material.tags.map((tag, index) => (
                                        <span
                                            key={index}
                                            className="px-2 py-0.5 bg-[#F8FAFC] text-xs rounded-md text-[#64748B]"
                                        >
                                            {tag}
                                        </span>
                                    ))}
                                </div>
                            )}
                            {material.attachments &&
                                material.attachments.length > 0 && (
                                    <div className="mt-2 text-xs text-blue-600">
                                        {material.attachments.length}{" "}
                                        {material.attachments.length === 1
                                            ? "додаток"
                                            : "додатків"}
                                    </div>
                                )}
                        </div>
                        <button
                            className="px-4 py-2 border border-[#E2E8F0] rounded-lg text-[#1E293B] hover:bg-[#F8FAFC] transition-colors"
                            onClick={() => handleViewMaterial(material)}
                        >
                            Переглянути
                        </button>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default MaterialsTab;
