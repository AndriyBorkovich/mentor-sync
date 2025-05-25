import React, { useState } from "react";

interface MaterialUploadModalProps {
    isOpen: boolean;
    onClose: () => void;
    onUpload: (material: any) => void;
}

const MaterialUploadModal: React.FC<MaterialUploadModalProps> = ({
    isOpen,
    onClose,
    onUpload,
}) => {
    const [materialType, setMaterialType] = useState<
        "document" | "video" | "link" | "presentation"
    >("document");
    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");
    const [tags, setTags] = useState("");
    const [content, setContent] = useState("");
    const [url, setUrl] = useState("");
    const [file, setFile] = useState<File | null>(null);
    const [isSubmitting, setIsSubmitting] = useState(false);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        setIsSubmitting(true);

        const tagsArray = tags
            .split(",")
            .map((tag) => tag.trim())
            .filter((tag) => tag);

        // Create new material object
        const newMaterial = {
            id: `new-${Date.now()}`,
            title,
            description,
            type: materialType,
            mentorName: "You", // In a real app, this would be the current user's name
            createdAt: new Date().toLocaleDateString("uk-UA", {
                day: "numeric",
                month: "long",
                year: "numeric",
            }),
            tags: tagsArray,
            ...(materialType === "document" && { content }),
            ...(materialType === "link" && { url }),
            ...(materialType === "video" && { url }),
            ...(file && {
                fileSize: `${(file.size / (1024 * 1024)).toFixed(1)} MB`,
            }),
        };

        // Simulate API call delay
        setTimeout(() => {
            onUpload(newMaterial);
            setIsSubmitting(false);
            resetForm();
            onClose();
        }, 1000);
    };

    const resetForm = () => {
        setMaterialType("document");
        setTitle("");
        setDescription("");
        setTags("");
        setContent("");
        setUrl("");
        setFile(null);
    };

    const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files[0]) {
            setFile(e.target.files[0]);
        }
    };

    if (!isOpen) return null;

    return (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
            <div className="bg-white rounded-lg w-full max-w-2xl p-6 relative max-h-[90vh] overflow-y-auto">
                <button
                    onClick={onClose}
                    className="absolute top-4 right-4 text-[#64748B] hover:text-[#1E293B]"
                >
                    <span className="material-icons">close</span>
                </button>

                <h2 className="text-xl font-bold text-[#1E293B] mb-6">
                    Додати новий матеріал
                </h2>

                <form onSubmit={handleSubmit}>
                    <div className="mb-4">
                        <label className="block text-sm font-medium text-[#1E293B] mb-2">
                            Тип матеріалу
                        </label>
                        <div className="flex flex-wrap gap-3">
                            {[
                                { id: "document", label: "Документ" },
                                { id: "video", label: "Відео" },
                                { id: "link", label: "Посилання" },
                                { id: "presentation", label: "Презентація" },
                            ].map((type) => (
                                <button
                                    key={type.id}
                                    type="button"
                                    onClick={() =>
                                        setMaterialType(type.id as any)
                                    }
                                    className={`px-4 py-2 rounded-lg ${
                                        materialType === type.id
                                            ? "bg-[#6C5DD3] text-white"
                                            : "bg-[#F1F5F9] text-[#64748B]"
                                    }`}
                                >
                                    {type.label}
                                </button>
                            ))}
                        </div>
                    </div>

                    <div className="mb-4">
                        <label
                            htmlFor="title"
                            className="block text-sm font-medium text-[#1E293B] mb-2"
                        >
                            Назва
                        </label>
                        <input
                            type="text"
                            id="title"
                            value={title}
                            onChange={(e) => setTitle(e.target.value)}
                            className="w-full p-3 border border-[#E2E8F0] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#6C5DD3] focus:border-[#6C5DD3]"
                            placeholder="Введіть назву матеріалу"
                            required
                        />
                    </div>

                    <div className="mb-4">
                        <label
                            htmlFor="description"
                            className="block text-sm font-medium text-[#1E293B] mb-2"
                        >
                            Опис
                        </label>
                        <textarea
                            id="description"
                            value={description}
                            onChange={(e) => setDescription(e.target.value)}
                            className="w-full p-3 border border-[#E2E8F0] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#6C5DD3] focus:border-[#6C5DD3]"
                            placeholder="Опишіть цей матеріал"
                            rows={3}
                            required
                        />
                    </div>

                    <div className="mb-4">
                        <label
                            htmlFor="tags"
                            className="block text-sm font-medium text-[#1E293B] mb-2"
                        >
                            Теги (розділені комами)
                        </label>
                        <input
                            type="text"
                            id="tags"
                            value={tags}
                            onChange={(e) => setTags(e.target.value)}
                            className="w-full p-3 border border-[#E2E8F0] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#6C5DD3] focus:border-[#6C5DD3]"
                            placeholder="React, JavaScript, Frontend"
                            required
                        />
                    </div>

                    {materialType === "document" && (
                        <div className="mb-4">
                            <label
                                htmlFor="content"
                                className="block text-sm font-medium text-[#1E293B] mb-2"
                            >
                                Вміст (Markdown)
                            </label>
                            <textarea
                                id="content"
                                value={content}
                                onChange={(e) => setContent(e.target.value)}
                                className="w-full p-3 border border-[#E2E8F0] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#6C5DD3] focus:border-[#6C5DD3] font-mono"
                                placeholder="# Заголовок\nВміст в форматі Markdown"
                                rows={8}
                                required
                            />
                        </div>
                    )}

                    {(materialType === "video" || materialType === "link") && (
                        <div className="mb-4">
                            <label
                                htmlFor="url"
                                className="block text-sm font-medium text-[#1E293B] mb-2"
                            >
                                URL
                            </label>
                            <input
                                type="url"
                                id="url"
                                value={url}
                                onChange={(e) => setUrl(e.target.value)}
                                className="w-full p-3 border border-[#E2E8F0] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#6C5DD3] focus:border-[#6C5DD3]"
                                placeholder="https://..."
                                required
                            />
                        </div>
                    )}

                    {(materialType === "presentation" ||
                        materialType === "document") && (
                        <div className="mb-4">
                            <label
                                htmlFor="file"
                                className="block text-sm font-medium text-[#1E293B] mb-2"
                            >
                                Файл
                            </label>
                            <div className="border border-dashed border-[#E2E8F0] rounded-lg p-4 text-center">
                                <input
                                    type="file"
                                    id="file"
                                    onChange={handleFileChange}
                                    className="hidden"
                                />
                                <label
                                    htmlFor="file"
                                    className="cursor-pointer"
                                >
                                    <div className="flex flex-col items-center">
                                        <span className="material-icons text-3xl text-[#94A3B8] mb-2">
                                            upload_file
                                        </span>
                                        {file ? (
                                            <span className="text-[#1E293B]">
                                                {file.name}
                                            </span>
                                        ) : (
                                            <span className="text-[#64748B]">
                                                Натисніть для завантаження файлу
                                            </span>
                                        )}
                                    </div>
                                </label>
                            </div>
                        </div>
                    )}

                    <div className="flex justify-end mt-6">
                        <button
                            type="button"
                            onClick={onClose}
                            className="px-4 py-2 text-[#64748B] mr-2"
                            disabled={isSubmitting}
                        >
                            Скасувати
                        </button>
                        <button
                            type="submit"
                            className="px-4 py-2 bg-[#6C5DD3] text-white rounded-lg hover:bg-[#5B4DC4] disabled:opacity-50"
                            disabled={isSubmitting}
                        >
                            {isSubmitting ? (
                                <span className="flex items-center">
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
                                    Завантаження...
                                </span>
                            ) : (
                                "Опублікувати матеріал"
                            )}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default MaterialUploadModal;
