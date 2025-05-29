import React, { useState } from "react";
import { Material } from "../../../shared/types";

interface ShareMaterialProps {
    material: Material;
}

const ShareMaterial: React.FC<ShareMaterialProps> = ({ material }) => {
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [copySuccess, setCopySuccess] = useState("");

    const materialUrl = `${window.location.origin}/materials/${material.id}`;

    const handleCopyLink = async () => {
        try {
            await navigator.clipboard.writeText(materialUrl);
            setCopySuccess("Посилання скопійовано!");
            setTimeout(() => setCopySuccess(""), 3000);
        } catch (err) {
            setCopySuccess("Помилка при копіюванні");
        }
    };

    const shareOptions = [
        { name: "Email", icon: "email", color: "#EA4335" },
        { name: "Telegram", icon: "send", color: "#0088cc" },
        { name: "LinkedIn", icon: "share", color: "#0A66C2" },
    ];

    return (
        <>
            <button
                onClick={() => setIsModalOpen(true)}
                className="inline-flex items-center bg-white border border-[#E2E8F0] text-[#64748B] px-4 py-2 rounded-lg hover:bg-[#F8FAFC]"
            >
                <span className="material-icons mr-2">share</span>
                Поділитися
            </button>

            {isModalOpen && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
                    <div className="bg-white rounded-lg w-full max-w-md p-6 relative">
                        <button
                            onClick={() => setIsModalOpen(false)}
                            className="absolute top-4 right-4 text-[#64748B] hover:text-[#1E293B]"
                        >
                            <span className="material-icons">close</span>
                        </button>

                        <h2 className="text-xl font-bold text-[#1E293B] mb-4">
                            Поділитися матеріалом
                        </h2>

                        <p className="text-[#64748B] mb-4">{material.title}</p>

                        <div className="flex justify-between mb-6">
                            {shareOptions.map((option) => (
                                <button
                                    key={option.name}
                                    className="flex flex-col items-center p-3"
                                >
                                    <div
                                        className="w-12 h-12 rounded-full flex items-center justify-center mb-2"
                                        style={{
                                            backgroundColor: option.color,
                                        }}
                                    >
                                        <span className="material-icons text-white">
                                            {option.icon}
                                        </span>
                                    </div>
                                    <span className="text-sm text-[#64748B]">
                                        {option.name}
                                    </span>
                                </button>
                            ))}
                        </div>

                        <div className="mt-6">
                            <label className="block text-sm font-medium text-[#1E293B] mb-2">
                                Посилання на матеріал
                            </label>
                            <div className="flex">
                                <input
                                    type="text"
                                    readOnly
                                    value={materialUrl}
                                    className="flex-1 p-2 border border-[#E2E8F0] rounded-l-lg focus:outline-none bg-[#F8FAFC]"
                                />
                                <button
                                    onClick={handleCopyLink}
                                    className="bg-[#6C5DD3] text-white px-4 py-2 rounded-r-lg hover:bg-[#5B4DC4]"
                                >
                                    <span className="material-icons">
                                        content_copy
                                    </span>
                                </button>
                            </div>
                            {copySuccess && (
                                <p className="text-green-500 text-sm mt-1">
                                    {copySuccess}
                                </p>
                            )}
                        </div>
                    </div>
                </div>
            )}
        </>
    );
};

export default ShareMaterial;
