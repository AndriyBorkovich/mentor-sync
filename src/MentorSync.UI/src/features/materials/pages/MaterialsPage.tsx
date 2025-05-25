import React, { useState } from "react";
import Sidebar from "../../../components/layout/Sidebar";
import Header from "../../../components/layout/Header";
import MaterialsContent from "../components/MaterialsContent";
import MaterialUploadModal from "../components/MaterialUploadModal";
import { Material, materials } from "../data/materials";
import "../../../components/layout/styles/logo.css";
import "../../../components/layout/styles/sidebar.css";

const MaterialsPage: React.FC = () => {
    const [sidebarExpanded, setSidebarExpanded] = useState(false);
    const [isUploadModalOpen, setIsUploadModalOpen] = useState(false);
    const [localMaterials, setLocalMaterials] = useState<Material[]>(materials);

    const handleSidebarToggle = (expanded: boolean) => {
        setSidebarExpanded(expanded);
    };

    const handleMaterialUpload = (newMaterial: Material) => {
        // In a real app, this would send data to an API
        // For now, we just update local state
        setLocalMaterials([newMaterial, ...localMaterials]);
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
                    <div className="container mx-auto px-4 py-4 flex justify-end">
                        <button
                            onClick={() => setIsUploadModalOpen(true)}
                            className="px-4 py-2 bg-[#6C5DD3] text-white rounded-lg hover:bg-[#5B4DC4] flex items-center"
                        >
                            <span className="material-icons mr-2">add</span>
                            Додати матеріал
                        </button>
                    </div>
                    <MaterialsContent materials={localMaterials} />
                </div>
            </div>

            <MaterialUploadModal
                isOpen={isUploadModalOpen}
                onClose={() => setIsUploadModalOpen(false)}
                onUpload={handleMaterialUpload}
            />
        </div>
    );
};

export default MaterialsPage;
