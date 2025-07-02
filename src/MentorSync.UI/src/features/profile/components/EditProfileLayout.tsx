import React, { useState } from "react";
import Sidebar from "../../../components/layout/Sidebar";
import Header from "../../../components/layout/Header";

interface EditProfileLayoutProps {
    children: React.ReactNode;
    title: string;
    isSubmitting: boolean;
    onCancel: () => void;
    onSubmit: () => void;
}

const EditProfileLayout: React.FC<EditProfileLayoutProps> = ({
    children,
    title,
    isSubmitting,
    onCancel,
    onSubmit,
}) => {
    const [sidebarExpanded, setSidebarExpanded] = useState(false);

    const handleSidebarToggle = (expanded: boolean) => {
        setSidebarExpanded(expanded);
    };

    return (
        <div className="min-h-screen flex bg-[#FFFFFF] overflow-hidden">
            <div
                className={`flex-shrink-0 transition-all duration-300 h-screen ${
                    sidebarExpanded ? "w-[240px]" : "w-[72px]"
                }`}
            >
                <Sidebar onToggle={handleSidebarToggle} />
            </div>
            <div className="flex-1 flex flex-col">
                <Header showNotifications={true} />

                <main className="flex-1 p-4 md:p-8 bg-slate-50">
                    <div className="max-w-4xl mx-auto">
                        <div className="bg-white rounded-lg shadow p-6 mb-6">
                            <div className="flex justify-between items-center mb-6">
                                <h1 className="text-2xl font-bold text-gray-800">
                                    {title}
                                </h1>
                            </div>

                            {children}

                            <div className="flex justify-end mt-6 space-x-4">
                                <button
                                    type="button"
                                    onClick={onCancel}
                                    className="px-4 py-2 border border-gray-300 rounded-lg text-gray-600 hover:bg-gray-50"
                                >
                                    Скасувати
                                </button>
                                <button
                                    type="button"
                                    onClick={onSubmit}
                                    disabled={isSubmitting}
                                    title="Зберегти зміни"
                                    className={`px-4 py-2 bg-primary text-black rounded-lg hover:bg-primary-dark ${
                                        isSubmitting
                                            ? "opacity-70 cursor-not-allowed"
                                            : ""
                                    }`}
                                >
                                    {isSubmitting ? (
                                        <div className="flex items-center">
                                            <svg
                                                className="animate-spin -ml-1 mr-2 h-4 w-4 text-white"
                                                xmlns="http://www.w3.org/2000/svg"
                                                fill="none"
                                                viewBox="0 0 24 24"
                                            >
                                                <circle
                                                    className="opacity-25"
                                                    cx="12"
                                                    cy="12"
                                                    r="10"
                                                    stroke="currentColor"
                                                    strokeWidth="4"
                                                ></circle>
                                                <path
                                                    className="opacity-75"
                                                    fill="currentColor"
                                                    d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                                                ></path>
                                            </svg>
                                            Збереження...
                                        </div>
                                    ) : (
                                        "Зберегти зміни"
                                    )}
                                </button>
                            </div>
                        </div>
                    </div>
                </main>
            </div>
        </div>
    );
};

export default EditProfileLayout;
