import {
    MentorBasicInfo,
    MentorMaterials,
    MentorReviews,
    MentorUpcomingSessions,
} from "../../../shared/types";
import api from "../../auth/services/api";

// Fetch basic info
export const getMentorBasicInfo = async (
    mentorId: number
): Promise<MentorBasicInfo> => {
    const response = await api.get<MentorBasicInfo>(
        `users/mentors/${mentorId}/basic-info`
    );
    return response.data;
};

// Fetch reviews
export const getMentorReviews = async (
    mentorId: number
): Promise<MentorReviews> => {
    const response = await api.get<MentorReviews>(
        `ratings/mentors/${mentorId}/reviews`
    );
    return response.data;
};

// Fetch upcoming sessions
export const getMentorUpcomingSessions = async (
    mentorId: number
): Promise<MentorUpcomingSessions> => {
    const response = await api.get<MentorUpcomingSessions>(
        `scheduling/mentors/${mentorId}/upcoming-sessions`
    );
    return response.data;
};

// Fetch materials
export const getMentorMaterials = async (
    mentorId: number
): Promise<MentorMaterials> => {
    const response = await api.get<MentorMaterials>(
        `materials/mentors/${mentorId}/materials`
    );
    return response.data;
};

// Store the last time each mentor profile was viewed to implement debouncing
const mentorViewTimestamps: Record<number, number> = {};
const VIEW_DEBOUNCE_TIME = 60 * 1000; // 60 seconds

/**
 * Records a mentor view event with debouncing to prevent multiple calls in a short period
 * Only sends the request if the same mentor hasn't been viewed in the last minute
 */
export const recordMentorViewEvent = async (
    mentorId: number
): Promise<boolean> => {
    try {
        // Check if we've viewed this mentor recently
        const lastViewTime = mentorViewTimestamps[mentorId] || 0;
        const currentTime = Date.now();

        // If it's been less than the debounce time, skip the request
        if (currentTime - lastViewTime < VIEW_DEBOUNCE_TIME) {
            return false;
        }

        // Update the timestamp before making the request
        mentorViewTimestamps[mentorId] = currentTime;

        // Call the endpoint
        await api.post(`recommendations/view-mentor/${mentorId}`);
        return true;
    } catch (error) {
        console.error(
            `Failed to record view event for mentor ${mentorId}:`,
            error
        );
        return false;
    }
};

/**
 * Toggles a mentor bookmark status.
 * If the mentor is already bookmarked, it removes the bookmark.
 * If the mentor is not bookmarked, it creates a bookmark.
 * @param mentorId The ID of the mentor
 * @param isCurrentlyBookmarked Whether the mentor is currently bookmarked
 * @returns A promise that resolves to true if the operation was successful, false otherwise
 */
export const toggleBookmark = async (
    mentorId: number,
    isCurrentlyBookmarked: boolean
): Promise<boolean> => {
    try {
        if (isCurrentlyBookmarked) {
            // Delete bookmark
            await api.delete(`recommendations/bookmarks/${mentorId}`);
        } else {
            // Create bookmark
            await api.post(`recommendations/bookmark/${mentorId}`);
        }

        // For development purposes, also update localStorage
        if (process.env.NODE_ENV === "development") {
            const bookmarkedMentors = JSON.parse(
                localStorage.getItem("bookmarkedMentors") || "[]"
            );
            if (isCurrentlyBookmarked) {
                // Remove the bookmark
                const index = bookmarkedMentors.indexOf(mentorId);
                if (index !== -1) {
                    bookmarkedMentors.splice(index, 1);
                }
            } else {
                // Add the bookmark
                if (!bookmarkedMentors.includes(mentorId)) {
                    bookmarkedMentors.push(mentorId);
                }
            }
            localStorage.setItem(
                "bookmarkedMentors",
                JSON.stringify(bookmarkedMentors)
            );
        }

        return true;
    } catch (error) {
        console.error(
            `Failed to toggle bookmark for mentor ${mentorId}:`,
            error
        );

        // If the API call fails in development, still update localStorage as a fallback
        if (process.env.NODE_ENV === "development") {
            const bookmarkedMentors = JSON.parse(
                localStorage.getItem("bookmarkedMentors") || "[]"
            );
            if (isCurrentlyBookmarked) {
                // Remove the bookmark
                const index = bookmarkedMentors.indexOf(mentorId);
                if (index !== -1) {
                    bookmarkedMentors.splice(index, 1);
                }
            } else {
                // Add the bookmark
                if (!bookmarkedMentors.includes(mentorId)) {
                    bookmarkedMentors.push(mentorId);
                }
            }
            localStorage.setItem(
                "bookmarkedMentors",
                JSON.stringify(bookmarkedMentors)
            );
            return true;
        }

        return false;
    }
};

/**
 * Checks if a mentor is bookmarked by the current user
 * @param mentorId The ID of the mentor to check
 * @returns A promise that resolves to true if the mentor is bookmarked, false otherwise
 */
export const checkIfMentorIsBookmarked = async (
    mentorId: number
): Promise<boolean> => {
    try {
        const response = await api.get(
            `recommendations/bookmarks/check/${mentorId}`
        );
        return response.data.isBookmarked;
    } catch (error) {
        // If the endpoint isn't available yet, we'll handle the error gracefully
        console.error(
            `Failed to check bookmark status for mentor ${mentorId}:`,
            error
        );

        // For development purposes, provide a fallback using localStorage
        // This can be removed once the backend endpoint is fully implemented
        if (process.env.NODE_ENV === "development") {
            const bookmarkedMentors = JSON.parse(
                localStorage.getItem("bookmarkedMentors") || "[]"
            );
            return bookmarkedMentors.includes(mentorId);
        }
        return false;
    }
};

/**
 * Create a new review for a mentor
 * @param mentorId The ID of the mentor to review
 * @param rating The rating to give (1-5)
 * @param reviewText The review text
 * @returns A promise that resolves to true if the review was successfully created
 */
export const createMentorReview = async (
    mentorId: number,
    rating: number,
    reviewText: string
): Promise<boolean> => {
    try {
        await api.post("ratings/reviews/mentor", {
            mentorId,
            rating,
            reviewText,
        });
        return true;
    } catch (error) {
        console.error(`Failed to create review for mentor ${mentorId}:`, error);
        return false;
    }
};

/**
 * Update an existing review for a mentor
 * @param reviewId The ID of the review to update
 * @param rating The new rating (1-5)
 * @param reviewText The new review text
 * @returns A promise that resolves to true if the review was successfully updated
 */
export const updateMentorReview = async (
    reviewId: number,
    rating: number,
    reviewText: string
): Promise<boolean> => {
    try {
        await api.put("ratings/reviews/mentor", {
            reviewId,
            rating,
            reviewText,
        });
        return true;
    } catch (error) {
        console.error(`Failed to update review ${reviewId}:`, error);
        return false;
    }
};

/**
 * Delete a review
 * @param reviewId The ID of the review to delete
 * @returns A promise that resolves to true if the review was successfully deleted
 */
export const deleteMentorReview = async (
    reviewId: number
): Promise<boolean> => {
    try {
        await api.delete(`ratings/reviews/mentor/${reviewId}`);
        return true;
    } catch (error) {
        console.error(`Failed to delete review ${reviewId}:`, error);
        return false;
    }
};

/**
 * Check if the current user has already reviewed a mentor
 * @param mentorId The ID of the mentor to check
 * @returns A promise that resolves to an object containing information about the existing review, if any
 */
export interface CheckReviewResult {
    hasReviewed: boolean;
    reviewId?: number;
    rating?: number;
    reviewText?: string;
}

export const checkMentorReview = async (
    mentorId: number
): Promise<CheckReviewResult> => {
    try {
        const response = await api.get<CheckReviewResult>(
            `ratings/reviews/mentor/${mentorId}/check`
        );
        return response.data;
    } catch (error) {
        console.error(
            `Failed to check review status for mentor ${mentorId}:`,
            error
        );
        return { hasReviewed: false };
    }
};
