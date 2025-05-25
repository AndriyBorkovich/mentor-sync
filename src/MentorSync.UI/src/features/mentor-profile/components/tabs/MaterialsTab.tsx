import React from "react";
import { Mentor } from "../../../dashboard/data/mentors";

interface MaterialsTabProps {
    mentor: Mentor;
}

// Mock materials data
interface Material {
    id: string;
    title: string;
    type: "article" | "video";
    date: string;
}

const MaterialsTab: React.FC<MaterialsTabProps> = ({ mentor }) => {
    // Mock materials
    const materials: Material[] = [
        {
            id: "1",
            title: "Introduction to System Design",
            type: "article",
            date: "Грудень 2023",
        },
        {
            id: "2",
            title: "Microservices Architecture Patterns",
            type: "article",
            date: "Листопад 2023",
        },
        {
            id: "3",
            title: "Scaling Distributed Systems",
            type: "video",
            date: "Жовтень 2023",
        },
        {
            id: "4",
            title: "Cloud Architecture Best Practices",
            type: "video",
            date: "Вересень 2023",
        },
    ];

    // Handle view material
    const handleViewMaterial = (material: Material) => {
        alert(`Відкриваю матеріал: ${material.title}`);
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
                        <div>
                            <h3 className="text-base font-medium text-[#1E293B]">
                                {material.title}
                            </h3>
                            <div className="flex mt-2 text-sm text-[#64748B]">
                                <span className="mr-4">
                                    {material.type === "article"
                                        ? "Стаття"
                                        : "Відео"}
                                </span>
                                <span>{material.date}</span>
                            </div>
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
