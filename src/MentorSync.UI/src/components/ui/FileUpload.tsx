import React, { useState, useRef, ChangeEvent } from "react";

interface FileUploadProps {
    onFileSelected: (file: File) => void;
    accept?: string;
    label?: string;
    currentImageUrl?: string;
    isLoading?: boolean;
    error?: string;
}

const FileUpload: React.FC<FileUploadProps> = ({
    onFileSelected,
    accept = "image/*",
    label = "Завантажити файл",
    currentImageUrl,
    isLoading = false,
    error,
}) => {
    const [previewUrl, setPreviewUrl] = useState<string | null>(
        currentImageUrl || null
    );
    const fileInputRef = useRef<HTMLInputElement>(null);

    // Update preview if currentImageUrl changes externally
    React.useEffect(() => {
        if (currentImageUrl !== undefined) {
            setPreviewUrl(currentImageUrl || null);
        }
    }, [currentImageUrl]);

    const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files.length > 0) {
            const file = e.target.files[0];
            onFileSelected(file);

            // Create a preview
            const reader = new FileReader();
            reader.onload = () => {
                setPreviewUrl(reader.result as string);
            };
            reader.readAsDataURL(file);
        }
    };

    const handleClick = () => {
        if (fileInputRef.current) {
            fileInputRef.current.click();
        }
    };

    return (
        <div className="w-full">
            <label className="block text-sm font-medium text-gray-700 mb-2">
                {label}
            </label>

            <div
                className={`border-2 border-dashed rounded-lg p-6 flex flex-col items-center justify-center cursor-pointer hover:bg-gray-50 transition-colors ${
                    error ? "border-red-400" : "border-gray-300"
                }`}
                onClick={handleClick}
            >
                <input
                    type="file"
                    ref={fileInputRef}
                    className="hidden"
                    accept={accept}
                    onChange={handleFileChange}
                    disabled={isLoading}
                />

                {previewUrl ? (
                    <div className="w-40 h-40 mb-4 relative">
                        <img
                            src={previewUrl}
                            alt="Preview"
                            className="w-full h-full object-cover rounded-lg"
                        />
                        <div className="absolute inset-0 flex items-center justify-center opacity-0 hover:opacity-80 bg-black bg-opacity-40 transition-opacity rounded-lg">
                            <span className="material-icons text-white text-3xl">
                                edit
                            </span>
                        </div>
                    </div>
                ) : (
                    <div className="text-center">
                        <span className="material-icons text-4xl text-gray-400 mb-2">
                            cloud_upload
                        </span>
                        <p className="text-sm text-gray-500">
                            Натисніть для завантаження зображення
                        </p>
                        <p className="text-xs text-gray-400 mt-1">
                            JPG, PNG або GIF, до 5 МБ
                        </p>
                    </div>
                )}
            </div>

            {error && <div className="text-red-500 text-sm mt-1">{error}</div>}

            {isLoading && (
                <div className="flex items-center justify-center mt-2">
                    <div className="w-5 h-5 border-t-2 border-b-2 border-primary rounded-full animate-spin"></div>
                    <span className="ml-2 text-sm text-gray-600">
                        Завантаження...
                    </span>
                </div>
            )}
        </div>
    );
};

export default FileUpload;
