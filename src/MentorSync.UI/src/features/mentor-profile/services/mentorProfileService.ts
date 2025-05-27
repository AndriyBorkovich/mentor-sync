import api from "../../auth/services/api";

// BasicInfo types
export interface MentorProfileSkill {
    id: string;
    name: string;
}

export interface MentorBasicInfo {
    id: number;
    name: string;
    title: string;
    rating: number;
    profileImage: string;
    yearsOfExperience: number | null;
    category: string;
    bio: string;
    availability: string;
    skills: MentorProfileSkill[];
}

// Reviews types
export interface MentorProfileReview {
    id: number;
    reviewerName: string;
    reviewerImage: string;
    rating: number;
    comment: string;
    createdOn: string; // ISO date string
}

export interface MentorReviews {
    reviewCount: number;
    reviews: MentorProfileReview[];
}

// Sessions types
export interface MentorProfileSession {
    id: number;
    title: string;
    description: string;
    startTime: string; // ISO date string
    endTime: string; // ISO date string
    status: string;
    menteeName: string;
    menteeImage: string;
}

export interface MentorUpcomingSessions {
    upcomingSessions: MentorProfileSession[];
}

// Materials types
export interface MentorProfileMaterial {
    id: number;
    title: string;
    description: string;
    type: string;
    url: string;
    contentMarkdown?: string;
    createdOn: string; // ISO date string
    updatedOn?: string;
    downloadCount: number;
    tags?: string[];
    attachments?: Array<{
        id: number;
        fileName: string;
        fileUrl: string;
    }>;
}

export interface MentorMaterials {
    materials: MentorProfileMaterial[];
}

// Combined profile type
export interface MentorProfile {
    id: number;
    name: string;
    title: string;
    rating: number;
    profileImage: string;
    yearsOfExperience: number | null;
    category: string;
    skills: MentorProfileSkill[];
    bio: string;
    availability?: string;
    reviewCount: number;
    recentReviews: MentorProfileReview[];
    upcomingSessions: MentorProfileSession[];
    materials: MentorProfileMaterial[];
}

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
            console.log(
                `Skipping view event for mentor ${mentorId} - viewed recently`
            );
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
