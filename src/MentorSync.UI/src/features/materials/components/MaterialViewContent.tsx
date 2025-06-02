import React, { useState } from "react";
import { marked } from "marked";
import ShareMaterial from "./ShareMaterial";
import { Material, MaterialAttachment } from "../../../shared/types";
import MaterialReviewContainer from "../containers/MaterialReviewContainer";

interface DocumentViewProps {
    material: Material;
}

const DocumentView: React.FC<DocumentViewProps> = ({ material }) => {
    const renderMarkdown = () => {
        if (!material.content) return <div>No content available</div>;

        const html = marked(material.content);
        return (
            <div
                dangerouslySetInnerHTML={{ __html: html }}
                className="prose max-w-none"
            />
        );
    };

    return (
        <div className="bg-white rounded-lg p-6 shadow-sm">
            {renderMarkdown()}
        </div>
    );
};

const VideoView: React.FC<DocumentViewProps> = ({ material }) => {
    if (!material.url) return <div>Video URL not available</div>;

    // Simplified - in real app, parse the URL and use the appropriate embed code
    return (
        <div className="bg-white rounded-lg p-6 shadow-sm">
            <div className="aspect-w-16 aspect-h-9 mb-4">
                <iframe
                    src={material.url.replace("watch?v=", "embed/")}
                    className="w-full h-[500px]"
                    title={material.title}
                    frameBorder="0"
                    allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                    allowFullScreen
                ></iframe>
            </div>
            <h2 className="text-xl font-medium text-[#1E293B] mb-2">
                {material.title}
            </h2>
            <p className="text-[#64748B]">{material.description}</p>
        </div>
    );
};

const LinkView: React.FC<DocumentViewProps> = ({ material }) => {
    if (!material.url) return <div>Link URL not available</div>;

    return (
        <div className="bg-white rounded-lg p-6 shadow-sm">
            <h2 className="text-xl font-medium text-[#1E293B] mb-3">
                {material.title}
            </h2>
            <p className="text-[#64748B] mb-4">{material.description}</p>

            <div className="border border-[#E2E8F0] rounded-lg p-4 mb-4">
                <div className="flex items-center">
                    <span className="material-icons text-[#6C5DD3] mr-3">
                        link
                    </span>
                    <a
                        href={material.url}
                        target="_blank"
                        rel="noopener noreferrer"
                        className="text-[#6C5DD3] underline break-all"
                    >
                        {material.url}
                    </a>
                </div>
            </div>

            <a
                href={material.url}
                target="_blank"
                rel="noopener noreferrer"
                className="inline-flex items-center bg-[#6C5DD3] text-white px-4 py-2 rounded-lg hover:bg-[#5B4DC4]"
            >
                <span className="material-icons mr-2">open_in_new</span>
                Відкрити посилання
            </a>
        </div>
    );
};

const PresentationView: React.FC<DocumentViewProps> = ({ material }) => {
    // In a real app, this would be integrated with an actual presentation viewer
    return (
        <div className="bg-white rounded-lg p-6 shadow-sm">
            <div
                className="bg-[#F1F5F9] rounded-lg p-12 flex items-center justify-center mb-4"
                style={{ height: "500px" }}
            >
                <div className="text-center">
                    <span className="material-icons text-[#94A3B8] text-6xl mb-4">
                        slideshow
                    </span>
                    <p className="text-[#64748B]">
                        Презентація "{material.title}"
                    </p>
                    <p className="text-[#94A3B8] text-sm mt-2">
                        {material.fileSize}
                    </p>
                </div>
            </div>

            <div className="flex justify-between">
                <div>
                    <h2 className="text-xl font-medium text-[#1E293B] mb-2">
                        {material.title}
                    </h2>
                    <p className="text-[#64748B]">{material.description}</p>
                </div>
                <button className="bg-[#6C5DD3] text-white px-4 py-2 rounded-lg h-10 flex items-center hover:bg-[#5B4DC4]">
                    <span className="material-icons mr-2">download</span>
                    Завантажити
                </button>
            </div>
        </div>
    );
};

const AttachmentsList: React.FC<{
    attachments: MaterialAttachment[];
}> = ({ attachments }) => {
    console.log("Rendering attachments list", attachments);
    if (!attachments?.length) return null;

    const getFileIcon = (contentType: string): string => {
        if (contentType.startsWith("image/")) return "image";
        if (contentType === "application/pdf") return "picture_as_pdf";
        if (contentType.includes("word")) return "description";
        if (
            contentType.includes("excel") ||
            contentType.includes("spreadsheet")
        )
            return "table_view";
        if (
            contentType.includes("powerpoint") ||
            contentType.includes("presentation")
        )
            return "slideshow";
        return "attach_file";
    };

    const formatFileSize = (bytes: number): string => {
        if (bytes === 0) return "0 Bytes";
        const k = 1024;
        const sizes = ["Bytes", "KB", "MB", "GB"];
        const i = Math.floor(Math.log(bytes) / Math.log(k));
        return `${parseFloat((bytes / Math.pow(k, i)).toFixed(1))} ${sizes[i]}`;
    };

    return (
        <div className="mt-8 bg-white rounded-lg p-6 shadow-sm">
            <h3 className="text-lg font-medium text-[#1E293B] mb-4">
                Додаткові вкладення
            </h3>
            <div className="space-y-3">
                {attachments.map((attachment) => (
                    <div
                        key={attachment.id}
                        className="flex items-center justify-between p-3 bg-[#F8FAFC] rounded-lg hover:bg-[#F1F5F9] transition-colors"
                    >
                        <div className="flex items-center flex-1 min-w-0">
                            <span className="material-icons text-[#64748B] mr-3">
                                {getFileIcon(attachment.contentType)}
                            </span>
                            <div className="flex-1 min-w-0">
                                <p className="text-[#1E293B] font-medium truncate">
                                    {attachment.fileName}
                                </p>
                                <div className="flex text-sm text-[#64748B] gap-2">
                                    <span>
                                        {formatFileSize(attachment.fileSize)}
                                    </span>
                                    <span>•</span>
                                    <span>
                                        {new Date(
                                            attachment.uploadedAt
                                        ).toLocaleDateString()}
                                    </span>
                                </div>
                            </div>
                        </div>
                        <a
                            href={attachment.fileUrl}
                            download={attachment.fileName}
                            className="ml-4 p-2 text-[#6C5DD3] hover:bg-[#F1F5F9] rounded-lg transition-colors"
                            title="Завантажити файл"
                        >
                            <span className="material-icons">download</span>
                        </a>
                    </div>
                ))}
            </div>
        </div>
    );
};

interface MaterialViewContentProps {
    material: Material;
}

const MaterialViewContent: React.FC<MaterialViewContentProps> = ({
    material,
}) => {
    const [activeTab, setActiveTab] = useState<"content" | "comments">(
        "content"
    );

    const renderMaterialContent = () => {
        switch (material.type) {
            case "document":
                console.log("Rendering document view", material);
                return (
                    <>
                        <DocumentView material={material} />
                        <AttachmentsList
                            attachments={material.attachments ?? []}
                        />
                    </>
                );
            case "video":
                return (
                    <>
                        <VideoView material={material} />
                        <AttachmentsList
                            attachments={material.attachments ?? []}
                        />
                    </>
                );
            case "link":
                return (
                    <>
                        <LinkView material={material} />
                        <AttachmentsList
                            attachments={material.attachments ?? []}
                        />
                    </>
                );
            case "presentation":
                return (
                    <>
                        <PresentationView material={material} />
                        <AttachmentsList
                            attachments={material.attachments ?? []}
                        />
                    </>
                );
            default:
                return <div>Unsupported material type</div>;
        }
    };

    return (
        <div className="container mx-auto px-4 py-6">
            <div className="mb-6">
                <div className="flex items-center mb-4">
                    <div className="w-12 h-12 rounded-full bg-gray-200 mr-4"></div>
                    <div className="flex-1">
                        <h3 className="font-medium text-[#1E293B]">
                            {material.mentorName}
                        </h3>
                        <p className="text-sm text-[#64748B]">
                            {material.createdAt}
                        </p>
                    </div>
                    <div>
                        <ShareMaterial material={material} />
                    </div>
                </div>

                <div className="flex flex-wrap gap-2 mb-4">
                    {material.tags.map((tag, index) => (
                        <span
                            key={index}
                            className="text-xs py-1 px-2 bg-[#F1F5F9] text-[#64748B] rounded-md"
                        >
                            {tag}
                        </span>
                    ))}
                </div>
            </div>
            {/* Tabs for Content/Comments */}
            <div className="border-b border-[#E2E8F0] mb-6">
                <div className="flex">
                    <button
                        onClick={() => setActiveTab("content")}
                        className={`py-3 px-6 ${
                            activeTab === "content"
                                ? "border-b-2 border-[#6C5DD3] text-[#1E293B] font-medium"
                                : "text-[#64748B]"
                        }`}
                    >
                        Вміст
                    </button>
                    <button
                        onClick={() => setActiveTab("comments")}
                        className={`py-3 px-6 flex items-center ${
                            activeTab === "comments"
                                ? "border-b-2 border-[#6C5DD3] text-[#1E293B] font-medium"
                                : "text-[#64748B]"
                        }`}
                    >
                        Коментарі
                    </button>
                </div>{" "}
            </div>{" "}
            {activeTab === "content" ? (
                <>{renderMaterialContent()}</>
            ) : (
                <div className="bg-white rounded-lg p-6 shadow-sm">
                    <h2 className="text-xl font-medium text-[#1E293B] mb-6">
                        Коментарі
                    </h2>
                    {/* Display reviews in a separate section */}
                    <div className="mt-8">
                        <h2 className="text-xl font-bold text-[#1E293B] mb-4">
                            Відгуки про матеріал
                        </h2>
                        <MaterialReviewContainer
                            materialId={parseInt(material.id, 10)}
                            mentorId={material.mentorId}
                        />
                    </div>
                </div>
            )}
        </div>
    );
};

export default MaterialViewContent;
