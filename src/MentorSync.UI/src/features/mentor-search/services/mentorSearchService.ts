import api from "../../auth/services/api";
import { Industry } from "../../../shared/enums/industry";
import { Mentor, RecommendedMentor } from "../../../shared/types";

interface SearchMentorsParams {
    searchTerm?: string;
    programmingLanguages?: string[];
    industry?: Industry;
    minExperienceYears?: number;
}

export const mentorSearchService = {
    /**
     * Search for mentors with filters
     * @param params Search parameters
     * @returns List of mentors matching the search criteria
     */
    searchMentors: async (
        params: SearchMentorsParams
    ): Promise<{ success: boolean; data?: Mentor[]; error?: string }> => {
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
     */
    getRecommendedMentors: async (
        params: SearchMentorsParams
    ): Promise<{
        success: boolean;
        data?: RecommendedMentor[];
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

            // default maxResults to 10 if not provided
            queryParams.append("maxResults", "12");

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
