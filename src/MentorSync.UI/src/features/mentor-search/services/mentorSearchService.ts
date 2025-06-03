import api from "../../../shared/services/api";
import { Industry } from "../../../shared/enums/industry";
import { Mentor, RecommendedMentor } from "../../../shared/types";
import {
    PaginatedResponse,
    PaginationParams,
} from "../../../shared/types/pagination";

interface SearchMentorsParams extends PaginationParams {
    searchTerm?: string;
    programmingLanguages?: string[];
    industry?: Industry;
    minExperienceYears?: number;
    minRating?: number;
    maxRating?: number;
}

export const mentorSearchService = {
    /**
     * Search for mentors with filters
     * @param params Search parameters
     * @returns List of mentors matching the search criteria with pagination
     */
    searchMentors: async (
        params: SearchMentorsParams
    ): Promise<{
        success: boolean;
        data?: PaginatedResponse<Mentor>;
        error?: string;
    }> => {
        try {
            // Build query parameters
            const queryParams = new URLSearchParams();

            if (params.searchTerm) {
                queryParams.append("searchTerm", params.searchTerm);
            }

            if (params.programmingLanguages?.length) {
                params.programmingLanguages.forEach((lang) => {
                    queryParams.append("programmingLanguages", lang);
                });
            }
            if (params.industry) {
                queryParams.append("industry", params.industry.toString());
            }
            if (params.minExperienceYears) {
                queryParams.append(
                    "minExperienceYears",
                    params.minExperienceYears.toString()
                );
            }

            if (params.minRating !== undefined) {
                queryParams.append("minRating", params.minRating.toString());
            }

            if (params.maxRating !== undefined) {
                queryParams.append("maxRating", params.maxRating.toString());
            }

            if (params.pageNumber) {
                queryParams.append("pageNumber", params.pageNumber.toString());
            }

            if (params.pageSize) {
                queryParams.append("pageSize", params.pageSize.toString());
            }

            // Make API call to search mentors
            const response = await api.get(
                `/users/mentors/search?${queryParams.toString()}`
            );

            return {
                success: true,
                data: response.data,
            };
        } catch (error) {
            console.error("Error searching mentors:", error);
            return {
                success: false,
                error: "Failed to search mentors. Please try again later.",
            };
        }
    },

    /**
     * Get recommended mentors with filters
     * @param params Search parameters
     * @returns List of recommended mentors with scores
     */ getRecommendedMentors: async (
        params: SearchMentorsParams
    ): Promise<{
        success: boolean;
        data?: PaginatedResponse<RecommendedMentor>;
        error?: string;
    }> => {
        try {
            // Build query parameters
            const queryParams = new URLSearchParams();

            if (params.searchTerm) {
                queryParams.append("searchTerm", params.searchTerm);
            }

            if (params.programmingLanguages?.length) {
                params.programmingLanguages.forEach((lang) => {
                    queryParams.append("programmingLanguages", lang);
                });
            }

            if (params.industry) {
                queryParams.append("industry", params.industry.toString());
            }
            if (params.minExperienceYears) {
                queryParams.append(
                    "minExperienceYears",
                    params.minExperienceYears.toString()
                );
            }

            if (params.minRating !== undefined) {
                queryParams.append("minRating", params.minRating.toString());
            }

            if (params.maxRating !== undefined) {
                queryParams.append("maxRating", params.maxRating.toString());
            }

            if (params.pageNumber) {
                queryParams.append("pageNumber", params.pageNumber.toString());
            }

            if (params.pageSize) {
                queryParams.append("pageSize", params.pageSize.toString());
            }

            // Make API call to get recommended mentors
            const response = await api.get(
                `/recommendations/mentors?${queryParams.toString()}`
            );

            return {
                success: true,
                data: response.data,
            };
        } catch (error) {
            console.error("Error fetching recommended mentors:", error);
            return {
                success: false,
                error: "Failed to fetch recommended mentors. Please try again later.",
            };
        }
    },
};
