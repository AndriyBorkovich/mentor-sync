import { api } from "../../auth";

export interface MaterialReview {
    id: number;
    rating: number;
    comment: string;
    createdOn: string;
    reviewerName: string;
    reviewerImage: string;
    isReviewByMentor: boolean;
}

export interface MaterialReviewsResponse {
    reviewCount: number;
    averageRating: number;
    reviews: MaterialReview[];
}

export interface UserReview {
    reviewId: number;
    rating: number;
    reviewText: string;
    createdAt: string;
    updatedAt: string | null;
}

// Get reviews for a material
export const getMaterialReviews = async (
    materialId: number
): Promise<MaterialReviewsResponse> => {
    try {
        const response = await api.get<MaterialReviewsResponse>(
            `/ratings/materials/${materialId}/reviews`
        );
        return response.data;
    } catch (error) {
        console.error("Error fetching material reviews:", error);
        throw error;
    }
};

// Get a user's review for a material
export const getUserMaterialReview = async (
    materialId: number,
    userId: number
): Promise<UserReview | null> => {
    const response = await api.get<UserReview | null>(
        `/ratings/materials/${materialId}/user/${userId}/review`
    );

    if (response.status === 204) {
        return null;
    }

    return response.data;
};

// Create a new review
export const createMaterialReview = async (
    materialId: number,
    reviewerId: number,
    rating: number,
    reviewText: string
): Promise<number> => {
    try {
        const response = await api.post(
            `/ratings/materials/${materialId}/reviews`,
            {
                reviewerId,
                rating,
                reviewText,
            }
        );
        return response.data.reviewId;
    } catch (error) {
        console.error("Error creating material review:", error);
        throw error;
    }
};

// Update an existing review
export const updateMaterialReview = async (
    reviewId: number,
    reviewerId: number,
    rating: number,
    reviewText: string
): Promise<void> => {
    try {
        // Make sure the review ID is defined before making the request
        if (!reviewId || isNaN(reviewId)) {
            throw new Error("Review ID is required for updating a review");
        }

        await api.put(`/ratings/reviews/material`, {
            reviewId,
            reviewerId,
            rating,
            reviewText,
        });
    } catch (error) {
        console.error("Error updating material review:", error);
        throw error;
    }
};

// Delete a review
export const deleteMaterialReview = async (
    reviewId: number,
    userId: number
): Promise<void> => {
    try {
        await api.delete(
            `/ratings/materials/reviews/${reviewId}?userId=${userId}`
        );
    } catch (error) {
        console.error("Error deleting material review:", error);
        throw error;
    }
};
