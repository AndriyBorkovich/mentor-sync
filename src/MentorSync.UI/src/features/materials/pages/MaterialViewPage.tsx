import React, { useState, useEffect } from "react";
import { useParams, useNavigate, Link } from "react-router-dom";
import Sidebar from "../../../components/layout/Sidebar";
import Header from "../../../components/layout/Header";
import MaterialViewContent from "../components/MaterialViewContent";
import RelatedMaterials from "../components/RelatedMaterials";
import MaterialAnalytics from "../components/MaterialAnalytics";
import { materials, Material } from "../data/materials";
import "../../../components/layout/styles/logo.css";
import "../../../components/layout/styles/sidebar.css";

const MaterialViewPage: React.FC = () => {
    const { materialId } = useParams<{ materialId: string }>();
    const navigate = useNavigate();
    const [sidebarExpanded, setSidebarExpanded] = useState(false);
    const [material, setMaterial] = useState<Material | undefined>(
        materials.find((m) => m.id === materialId)
    );
    const [viewCount, setViewCount] = useState<number>(0);
    const [analyticsData, setAnalyticsData] = useState({
        downloadCount: 0,
        ratingCount: 0,
        averageRating: 0,
        commentCount: 0,
    });

    useEffect(() => {
        // Update material when URL param changes
        const foundMaterial = materials.find((m) => m.id === materialId);
        setMaterial(foundMaterial);

        if (foundMaterial) {
            // Simulated analytics data
            setViewCount(Math.floor(Math.random() * 50) + 10);
            setAnalyticsData({
                downloadCount: Math.floor(Math.random() * 20),
                ratingCount: Math.floor(Math.random() * 15),
                averageRating: 3.5 + Math.random() * 1.5,
                commentCount: 2, // We have 2 mock comments
            });

            // Set document title
            document.title = `${foundMaterial.title} | MentorSync Materials`;
        } else {
            // Material not found, redirect to materials page
            navigate("/materials");
        }

        // Scroll to top when material changes
        window.scrollTo(0, 0);
    }, [materialId, navigate]);

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
                                downloadCount={analyticsData.downloadCount}
                                ratingCount={analyticsData.ratingCount}
                                averageRating={analyticsData.averageRating}
                                commentCount={analyticsData.commentCount}
                            />

                            <MaterialViewContent material={material} />
                        </div>
                        <div className="md:w-80 flex-shrink-0">
                            <div className="sticky top-0 pt-6 px-4">
                                <RelatedMaterials
                                    currentMaterial={material}
                                    materials={materials}
                                />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default MaterialViewPage;
