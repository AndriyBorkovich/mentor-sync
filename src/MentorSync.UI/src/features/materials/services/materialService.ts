import { api } from "../../auth";
import { hasRole } from "../../auth";

export interface LearningMaterial {
    id: number;
    title: string;
    description: string;
    type: "Article" | "Video" | "Document"; // Mapped from MaterialType enum
    contentMarkdown?: string;
    mentorId: number;
    mentorName?: string; // Populated from API response
    createdAt: string;
    updatedAt?: string;
    attachments?: MaterialAttachment[];
    tags?: Tag[];
}

export interface MaterialAttachment {
    id: number;
    fileName: string;
    fileUrl: string;
    fileSize: number;
    contentType: string;
    uploadedAt: string;
}

export interface Tag {
    id: number;
    name: string;
}

export interface MaterialsResponse {
    items: LearningMaterial[];
    totalCount: number;
    pageSize: number;
    pageNumber: number;
}

export interface MaterialFilters {
    search?: string;
    types?: string[];
    tags?: string[];
    sortBy?: string;
    pageNumber?: number;
    pageSize?: number;
}

// Get learning materials with filters
export const getMaterials = async (
    filters?: MaterialFilters
): Promise<MaterialsResponse> => {
    try {
        let queryParams = new URLSearchParams();

        if (filters?.search) {
            queryParams.append("search", filters.search);
        }

        if (filters?.types && filters.types.length > 0) {
            filters.types.forEach((type) => {
                queryParams.append("types", type);
            });
        }

        if (filters?.tags && filters.tags.length > 0) {
            filters.tags.forEach((tag) => {
                queryParams.append("tags", tag);
            });
        }

        if (filters?.sortBy) {
            queryParams.append("sortBy", filters.sortBy);
        }

        console.log("API call with pageNumber:", filters?.pageNumber);
        queryParams.append(
            "pageNumber",
            filters?.pageNumber?.toString() || "1"
        );
        queryParams.append("pageSize", filters?.pageSize?.toString() || "10");

        const url = `/materials?${queryParams.toString()}`;
        console.log("API request URL:", url);

        const response = await api.get(url);
        return response.data;
    } catch (error) {
        console.error("Failed to fetch materials:", error);
        throw error;
    }
};

// Get a single learning material by ID
export const getMaterialById = async (
    id: number
): Promise<LearningMaterial> => {
    try {
        const response = await api.get(`/materials/${id}`);
        return response.data;
    } catch (error) {
        console.error(`Failed to fetch material with ID ${id}:`, error);
        throw error;
    }
};

// Create a new learning material (mentors only)
export const createMaterial = async (material: {
    title: string;
    description: string;
    type: "Article" | "Video" | "Document";
    contentMarkdown?: string;
    mentorId?: number; // Optional, defaults to current mentor
    tags?: string[];
}): Promise<LearningMaterial> => {
    try {
        if (!hasRole("Mentor")) {
            throw new Error("Only mentors can create materials");
        }

        const response = await api.post(`/materials`, material);
        return response.data;
    } catch (error) {
        console.error("Failed to create material:", error);
        throw error;
    }
};

// Upload an attachment for a learning material
export const uploadAttachment = async (
    materialId: number,
    file: File
): Promise<MaterialAttachment> => {
    try {
        const formData = new FormData();
        formData.append("file", file);

        const response = await api.post(
            `/materials/${materialId}/attachments`,
            formData,
            {
                headers: {
                    "Content-Type": "multipart/form-data",
                },
            }
        );
        return response.data;
    } catch (error) {
        console.error(
            `Failed to upload attachment for material ${materialId}:`,
            error
        );
        throw error;
    }
};

// Delete a material attachment
export const deleteAttachment = async (
    materialId: number,
    attachmentId: number
): Promise<void> => {
    try {
        await api.delete(
            `/materials/${materialId}/attachments/${attachmentId}`
        );
    } catch (error) {
        console.error(`Failed to delete attachment ${attachmentId}:`, error);
        throw error;
    }
};

// Get all available tags
export const getAllTags = async (): Promise<Tag[]> => {
    try {
        const response = await api.get(`/materials/tags`);
        return response.data;
    } catch (error) {
        console.error("Failed to fetch tags:", error);
        throw error;
    }
};

// Add tags to a material
export const addTagsToMaterial = async (
    materialId: number,
    tagNames: string[]
): Promise<Tag[]> => {
    try {
        const response = await api.post(`api/materials/${materialId}/tags`, {
            tagNames,
        });
        return response.data;
    } catch (error) {
        console.error(`Failed to add tags to material ${materialId}:`, error);
        throw error;
    }
};

// Map API material type to UI material type
export const mapMaterialType = (
    apiType: string
): "document" | "video" | "link" => {
    switch (apiType) {
        case "Article":
            return "document";
        case "Video":
            return "video";
        case "Document":
            return "document";
        default:
            return "document";
    }
};

// Map UI material type to API material type
export const mapToApiMaterialType = (
    uiType: string
): "Article" | "Video" | "Document" => {
    switch (uiType) {
        case "document":
            return "Document";
        case "video":
            return "Video";
        case "link":
            return "Article";
        case "presentation":
            return "Document";
        default:
            return "Document";
    }
};
