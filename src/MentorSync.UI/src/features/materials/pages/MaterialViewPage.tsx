import React, { useState, useEffect } from "react";
import { useParams, useNavigate, Link } from "react-router-dom";
import Sidebar from "../../../components/layout/Sidebar";
import Header from "../../../components/layout/Header";
import MaterialViewContent from "../components/MaterialViewContent";
import MaterialAnalytics from "../components/MaterialAnalytics";
import { getMaterialById } from "../services/materialService";
import { toast } from "react-toastify";
import "../../../components/layout/styles/logo.css";
import "../../../components/layout/styles/sidebar.css";
import { Material } from "../../../shared/types";

const MaterialViewPage: React.FC = () => {
    const { materialId } = useParams<{ materialId: string }>();
    const navigate = useNavigate();
    const [sidebarExpanded, setSidebarExpanded] = useState(false);
    const [material, setMaterial] = useState<Material | undefined>();
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [viewCount, setViewCount] = useState<number>(0);
    const [analyticsData, setAnalyticsData] = useState({
        ratingCount: 0,
        averageRating: 0,
        commentCount: 0,
    });

    useEffect(() => {
        const fetchMaterial = async () => {
            if (!materialId) {
                navigate("/materials");
                return;
            }

            setIsLoading(true);
            setError(null);

            try {
                const response = await getMaterialById(
                    parseInt(materialId, 10)
                );

                if (!response) {
                    navigate("/materials");
                    return;
                }

                // Map API response to UI Material type
                const uiMaterial: Material = {
                    id: response.id.toString(),
                    title: response.title,
                    description: response.description,
                    type: mapTypeToUiFormat(response.type) as Material["type"],
                    mentorName: response.mentorName || "Unknown Mentor",
                    createdAt: new Date(response.createdAt).toLocaleDateString(
                        "uk-UA",
                        {
                            day: "numeric",
                            month: "long",
                            year: "numeric",
                        }
                    ),
                    tags: response.tags?.map((t) => t.name) || [],
                    content: response.contentMarkdown,
                    url: response.attachments?.[0]?.fileUrl,
                    fileSize: response.attachments?.[0]
                        ? `${(
                              response.attachments[0].fileSize /
                              (1024 * 1024)
                          ).toFixed(1)} MB`
                        : undefined,
                    attachments:
                        response.attachments?.map((att) => ({
                            id: att.id,
                            fileName: att.fileName,
                            fileUrl: att.fileUrl,
                            fileSize: att.fileSize,
                            contentType: att.contentType,
                            uploadedAt: att.uploadedAt, // keep as string
                        })) || [],
                };

                setMaterial(uiMaterial);

                // Set analytics data - for now we'll use mocked values since the API doesn't provide these yet
                const mockAnalytics = {
                    viewCount: 24,
                    ratingCount: 8,
                    averageRating: 4.5,
                    commentCount: 3,
                };
                setViewCount(mockAnalytics.viewCount);
                setAnalyticsData({
                    ratingCount: mockAnalytics.ratingCount,
                    averageRating: mockAnalytics.averageRating,
                    commentCount: mockAnalytics.commentCount,
                });

                // Set document title
                document.title = `${uiMaterial.title} | MentorSync Materials`;
            } catch (err) {
                setError("Failed to load material. Please try again later.");
                toast.error("Failed to load material");
                console.error("Error fetching material:", err);
                navigate("/materials");
            } finally {
                setIsLoading(false);
            }
        };

        fetchMaterial();
        window.scrollTo(0, 0);
    }, [materialId, navigate]);

    // Helper function to map API material type to UI format
    const mapTypeToUiFormat = (type: string): string => {
        switch (type.toLowerCase()) {
            case "article":
            case "document":
                return "document";
            case "video":
                return "video";
            case "link":
                return "link";
            case "presentation":
                return "presentation";
            default:
                return "document";
        }
    };

    const handleSidebarToggle = (expanded: boolean) => {
        setSidebarExpanded(expanded);
    };

    if (!material) {
        return null; // Will redirect in useEffect
    }

    // Get material type display name
    const getTypeLabel = (type: string) => {
        switch (type) {
            case "document":
                return "Документ";
            case "video":
                return "Відео";
            case "link":
                return "Посилання";
            case "presentation":
                return "Презентація";
            default:
                return type;
        }
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
                <div className="flex flex-col flex-1 overflow-y-auto bg-[#F8FAFC]">
                    <div className="bg-white border-b border-[#E2E8F0] px-6 py-4">
                        <nav className="flex" aria-label="Breadcrumb">
                            <ol className="inline-flex items-center space-x-1 md:space-x-2">
                                <li className="inline-flex items-center">
                                    <Link
                                        to="/materials"
                                        className="text-[#64748B] hover:text-[#1E293B] inline-flex items-center"
                                    >
                                        <span className="material-icons text-sm mr-1">
                                            home
                                        </span>
                                        Матеріали
                                    </Link>
                                </li>
                                <li>
                                    <div className="flex items-center">
                                        <span className="material-icons text-[#94A3B8] text-sm mx-1">
                                            chevron_right
                                        </span>
                                        <span className="text-[#1E293B] font-medium">
                                            {getTypeLabel(material.type)}
                                        </span>
                                    </div>
                                </li>
                            </ol>
                        </nav>
                    </div>
                    <div className="flex flex-col md:flex-row gap-6">
                        <div className="flex-1 px-4 py-6 md:px-6">
                            <div className="flex items-center justify-between mb-4">
                                <h1 className="text-2xl font-bold text-[#1E293B]">
                                    {material.title}
                                </h1>
                            </div>

                            <MaterialAnalytics
                                viewCount={viewCount}
                                ratingCount={analyticsData.ratingCount}
                                averageRating={analyticsData.averageRating}
                                commentCount={analyticsData.commentCount}
                            />

                            <MaterialViewContent material={material} />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default MaterialViewPage;
