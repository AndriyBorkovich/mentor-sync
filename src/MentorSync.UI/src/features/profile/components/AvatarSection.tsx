import React, { useState } from "react";
import FileUpload from "../../../components/ui/FileUpload";
import { deleteAvatar, uploadAvatar } from "../services/profileService";

interface AvatarSectionProps {
    userId: number;
    currentAvatarUrl?: string;
    onAvatarUpdate: (newAvatarUrl: string | null) => void;
}

const AvatarSection: React.FC<AvatarSectionProps> = ({
    userId,
    currentAvatarUrl,
    onAvatarUpdate,
}) => {
    const [isUploading, setIsUploading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const handleFileSelected = async (file: File) => {
        try {
            setIsUploading(true);
            setError(null);
            const newAvatarUrl = await uploadAvatar(userId, file);
            onAvatarUpdate(newAvatarUrl);
        } catch (error) {
            console.error("Error uploading avatar:", error);
            setError("Помилка при завантаженні зображення");
        } finally {
            setIsUploading(false);
        }
    };

    const handleDeleteAvatar = async () => {
        if (!currentAvatarUrl) return;

        try {
            setIsUploading(true);
            setError(null);
            await deleteAvatar(userId);
            onAvatarUpdate(null);
        } catch (error) {
            console.error("Error deleting avatar:", error);
            setError("Помилка при видаленні зображення");
        } finally {
            setIsUploading(false);
        }
    };

    return (
        <div className="mb-6 border-b pb-6">
            <h2 className="text-lg font-medium text-gray-800 mb-4">
                Фотографія профілю
            </h2>

            <div className="flex flex-col md:flex-row md:items-end gap-6">
                <div className="w-full max-w-xs">
                    <FileUpload
                        onFileSelected={handleFileSelected}
                        currentImageUrl={currentAvatarUrl}
                        isLoading={isUploading}
                        error={error || undefined}
                        label="Фотографія профілю"
                        accept="image/jpeg,image/png,image/gif"
                    />
                </div>

                {currentAvatarUrl && (
                    <div className="mt-4 md:mt-0">
                        <button
                            type="button"
                            onClick={handleDeleteAvatar}
                            disabled={isUploading}
                            className="text-red-600 hover:text-red-800 flex items-center"
                        >
                            <span className="material-icons mr-1 text-sm">
                                delete
                            </span>
                            Видалити фото
                        </button>
                    </div>
                )}
            </div>

            <p className="text-sm text-gray-500 mt-2">
                Рекомендований розмір: 400×400 пікселів. Максимальний розмір
                файлу: 5 МБ.
            </p>
        </div>
    );
};

export default AvatarSection;
