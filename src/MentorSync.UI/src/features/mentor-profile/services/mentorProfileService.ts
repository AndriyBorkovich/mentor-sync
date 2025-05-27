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
