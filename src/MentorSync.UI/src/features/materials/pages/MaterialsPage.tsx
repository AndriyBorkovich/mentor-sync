import React, { useState, useEffect } from "react";
import Sidebar from "../../../components/layout/Sidebar";
import Header from "../../../components/layout/Header";
import MaterialsContent from "../components/MaterialsContent";
import MaterialUploadModal from "../components/MaterialUploadModal";
import {
    getMaterials,
    createMaterial,
    uploadAttachment,
    mapMaterialType,
} from "../services/materialService";
import { getRecommendedMaterials } from "../services/recommendedMaterialService";
import "../../../components/layout/styles/logo.css";
import "../../../components/layout/styles/sidebar.css";
import { toast } from "react-toastify";
import { hasRole } from "../../auth";
import { Material, RecommendedMaterial } from "../../../shared/types";

// Define tab types
type TabType = "allMaterials" | "recommendedMaterials";

const MaterialsPage: React.FC = () => {
    const [sidebarExpanded, setSidebarExpanded] = useState(false);
    const [isUploadModalOpen, setIsUploadModalOpen] = useState(false);
    const [materials, setMaterials] = useState<Material[]>([]);
    const [recommendedMaterials, setRecommendedMaterials] = useState<
        RecommendedMaterial[]
    >([]);
    const [totalCount, setTotalCount] = useState<number>(0);
    const [recommendedTotalCount, setRecommendedTotalCount] =
        useState<number>(0);
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [activeTab, setActiveTab] = useState<TabType>("allMaterials");
    const [filters, setFilters] = useState({
        search: "",
        types: [] as string[],
        tags: [] as string[],
        sortBy: "newest",
        pageNumber: 1,
        pageSize: 12,
    });

    const isMentor = hasRole("Mentor");
    const isMentee = hasRole("Mentee"); // Fetch materials when component mounts or filters change
    useEffect(() => {
        const fetchMaterials = async () => {
            setIsLoading(true);
            try {
                if (activeTab === "allMaterials") {
                    const response = await getMaterials({
                        search: filters.search,
                        types: filters.types,
                        tags: filters.tags,
                        sortBy: filters.sortBy,
                        pageNumber: filters.pageNumber,
                        pageSize: filters.pageSize,
                    });

                    // Map API materials to UI format
                    const uiMaterials = response.items.map((item) => ({
                        id: item.id.toString(),
                        title: item.title,
                        description: item.description,
                        type: mapMaterialType(item.type),
                        mentorName: item.mentorName || "Unknown Mentor",
                        mentorId: item.mentorId,
                        createdAt: new Date(item.createdAt).toLocaleDateString(
                            "uk-UA",
                            {
                                day: "numeric",
                                month: "long",
                                year: "numeric",
                            }
                        ),
                        tags: item.tags?.map((tag) => tag.name) || [],
                        content: item.contentMarkdown,
                        fileSize:
                            item.attachments && item.attachments.length > 0
                                ? `${(
                                      item.attachments[0].fileSize /
                                      (1024 * 1024)
                                  ).toFixed(1)} MB`
                                : undefined,
                        url:
                            item.attachments && item.attachments.length > 0
                                ? item.attachments[0].fileUrl
                                : undefined,
                    }));
                    setMaterials(uiMaterials);
                    setTotalCount(response.totalCount || uiMaterials.length);
                    setError(null);
                } else {
                    // Fetch recommended materials
                    const response = await getRecommendedMaterials({
                        searchTerm: filters.search,
                        types: filters.types,
                        tags: filters.tags,
                        pageNumber: filters.pageNumber,
                        pageSize: filters.pageSize,
                    });
                    const uiRecommendedMaterials = response.items.map(
                        (item) => ({
                            id: item.id,
                            title: item.title,
                            description: item.description,
                            type: mapMaterialType(item.type) as
                                | "document"
                                | "video"
                                | "link"
                                | "presentation",
                            mentorName: item.mentorName,
                            mentorId: item.mentorId,
                            createdAt: new Date(
                                item.createdAt
                            ).toLocaleDateString("uk-UA", {
                                day: "numeric",
                                month: "long",
                                year: "numeric",
                            }),
                            tags: item.tags || [],
                            url: item.url,
                            fileSize: item.fileSize,
                            // Add recommendation score fields
                            collaborativeScore: item.collaborativeScore,
                            contentBasedScore: item.contentBasedScore,
                            finalScore: item.finalScore,
                        })
                    );

                    setRecommendedMaterials(uiRecommendedMaterials);
                    setRecommendedTotalCount(
                        response.totalCount || uiRecommendedMaterials.length
                    );
                    setError(null);
                }
            } catch (err) {
                console.error("Error fetching materials:", err);
                setError("Failed to load materials. Please try again later.");
                toast.error("Не вдалося завантажити матеріали");
            } finally {
                setIsLoading(false);
            }
        };

        fetchMaterials();
    }, [filters, activeTab]);

    const handleSidebarToggle = (expanded: boolean) => {
        setSidebarExpanded(expanded);
    };

    const handleMaterialUpload = async (materialData: any) => {
        try {
            // Create the material first
            const newMaterial = await createMaterial({
                title: materialData.title,
                description: materialData.description,
                type: materialData.type,
                contentMarkdown: materialData.contentMarkdown,
                mentorId: materialData.mentorId || 1, // Default to ID 1 for testing
                tags: materialData.tags,
            });

            // If there's a file, upload it as an attachment
            if (materialData.file) {
                await uploadAttachment(newMaterial.id, materialData.file);
            }

            // Refresh the materials list
            const updatedFilters = { ...filters, pageNumber: 1 };
            setFilters(updatedFilters);

            toast.success("Матеріал успішно створено");
        } catch (err) {
            console.error("Error uploading material:", err);
            toast.error("Не вдалося створити матеріал");
        }
    };

    const handleFilterChange = (newFilters: any) => {
        setFilters({
            ...filters,
            ...newFilters,
            pageNumber: 1, // Reset to first page when filters change
        });
    };
    return (
        <div className="min-h-screen flex bg-[#FFFFFF] overflow-hidden">
            <div
                className={`flex-shrink-0 transition-all duration-300 h-screen ${
                    sidebarExpanded ? "w-[240px]" : "w-[72px]"
                }`}
            >
                <Sidebar
                    onToggle={handleSidebarToggle}
                    activePage="materials"
                />
            </div>
            <div className="flex flex-col flex-1 overflow-hidden">
                <Header />
                <div className="flex-1 overflow-y-auto bg-[#F8FAFC]">
                    {error && (
                        <div className="bg-red-50 border-l-4 border-red-400 p-4 mb-4">
                            <div className="flex">
                                <div className="flex-shrink-0">
                                    <span className="material-icons text-red-400">
                                        error
                                    </span>
                                </div>
                                <div className="ml-3">
                                    <p className="text-sm text-red-700">
                                        {error}
                                    </p>
                                </div>
                            </div>
                        </div>
                    )}
                    <div className="container mx-auto px-4 py-4 flex justify-between items-center">
                        <h1 className="text-2xl font-semibold text-[#1E293B]">
                            Навчальні матеріали
                        </h1>
                        {isMentor && (
                            <button
                                onClick={() => setIsUploadModalOpen(true)}
                                className="px-4 py-2 bg-[#6C5DD3] text-white rounded-lg hover:bg-[#5B4DC4] flex items-center"
                            >
                                <span className="material-icons mr-2">add</span>
                                Додати матеріал
                            </button>
                        )}
                    </div>
                    {/* Tab navigation */}
                    <div className="container mx-auto px-4 border-b border-[#E2E8F0]">
                        <div className="flex space-x-8">                            <button
                                onClick={() => {
                                    setActiveTab("allMaterials");
                                    setFilters({...filters, pageNumber: 1}); // Reset to first page when changing tabs
                                }}
                                className={`py-4 px-1 font-medium text-sm border-b-2 transition-colors ${
                                    activeTab === "allMaterials"
                                        ? "border-[#6C5DD3] text-[#6C5DD3]"
                                        : "border-transparent text-[#64748B] hover:text-[#334155]"
                                }`}
                            >
                                Всі матеріали
                            </button>
                            {isMentee && (
                                <button
                                    onClick={() => {
                                        setActiveTab("recommendedMaterials");
                                        setFilters({...filters, pageNumber: 1}); // Reset to first page when changing tabs
                                    }}
                                    className={`py-4 px-1 font-medium text-sm border-b-2 transition-colors ${
                                        activeTab === "recommendedMaterials"
                                            ? "border-[#6C5DD3] text-[#6C5DD3]"
                                            : "border-transparent text-[#64748B] hover:text-[#334155]"
                                    }`}
                                >
                                    Рекомендовані матеріали
                                </button>
                            )}
                        </div>
                    </div>{" "}
                    {isLoading ? (
                        <div className="flex justify-center items-center h-64">
                            <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-[#6C5DD3]"></div>
                        </div>
                    ) : (
                        <MaterialsContent
                            materials={
                                activeTab === "allMaterials"
                                    ? materials
                                    : recommendedMaterials
                            }
                            onFilterChange={handleFilterChange}
                            currentFilters={filters}
                            totalCount={
                                activeTab === "allMaterials"
                                    ? totalCount
                                    : recommendedTotalCount
                            }
                            isRecommended={activeTab === "recommendedMaterials"}
                        />
                    )}
                </div>
            </div>

            {isMentor && (
                <MaterialUploadModal
                    isOpen={isUploadModalOpen}
                    onClose={() => setIsUploadModalOpen(false)}
                    onUpload={handleMaterialUpload}
                />
            )}
        </div>
    );
};

export default MaterialsPage;
