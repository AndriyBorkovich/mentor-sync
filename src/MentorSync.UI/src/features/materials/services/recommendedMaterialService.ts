import { api } from "../../auth";

export interface RecommendedMaterial {
    id: string;
    title: string;
    description: string;
    type: string;
    mentorId: number;
    mentorName: string;
    createdAt: string;
    tags: string[];
    url?: string;
    fileSize?: string;
    collaborativeScore: number;
    contentBasedScore: number;
    finalScore: number;
}

export interface RecommendedMaterialsResponse {
    items: RecommendedMaterial[];
    totalCount: number;
    pageSize: number;
    pageNumber: number;
}

export interface MaterialFilters {
    searchTerm?: string;
    types?: string[];
    tags?: string[];
    pageNumber?: number;
    pageSize?: number;
}

export const getRecommendedMaterials = async (
    filters?: MaterialFilters
): Promise<RecommendedMaterialsResponse> => {
    try {
        let queryParams = new URLSearchParams();

        if (filters?.searchTerm) {
            queryParams.append("searchTerm", filters.searchTerm);
        }

        if (filters?.types && filters.types.length > 0) {
            filters.types.forEach((type) => {
                queryParams.append("type", type);
            });
        }

        if (filters?.tags && filters.tags.length > 0) {
            filters.tags.forEach((tag) => {
                queryParams.append("tags", tag);
            });
        }

        queryParams.append(
            "pageNumber",
            filters?.pageNumber?.toString() || "1"
        );
        queryParams.append("pageSize", filters?.pageSize?.toString() || "10");

        const response = await api.get(
            `/recommendations/materials?${queryParams.toString()}`
        );
        return response.data;
    } catch (error) {
        console.error("Failed to fetch recommended materials:", error);
        throw error;
    }
};
