import api from "../../../shared/services/api";
import { UserShortResponse, UserFilterParams } from "../types/userTypes";

/**
 * Get all users with optional filtering
 */
export const getAllUsers = async (
    filters?: UserFilterParams
): Promise<UserShortResponse[]> => {
    try {
        // Construct query string based on filters
        let queryParams = new URLSearchParams();

        if (filters?.role) {
            queryParams.append("role", filters.role);
        }

        if (filters?.isActive !== undefined) {
            queryParams.append("isActive", filters.isActive.toString());
        }

        const queryString = queryParams.toString();
        const endpoint = queryString ? `/users?${queryString}` : "/users";

        const response = await api.get<UserShortResponse[]>(endpoint);
        return response.data;
    } catch (error) {
        console.error("Error fetching users:", error);
        throw error;
    }
};

/**
 * Toggle user's active status
 */
export const toggleUserActiveStatus = async (userId: number): Promise<void> => {
    try {
        await api.post(`/users/${userId}/active`);
    } catch (error) {
        console.error(
            `Error toggling active status for user ${userId}:`,
            error
        );
        throw error;
    }
};
