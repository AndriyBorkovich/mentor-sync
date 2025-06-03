export interface Skill {
    id: string;
    name: string;
}

export interface Mentor {
    id: string;
    name: string;
    title: string;
    rating: number;
    skills: Skill[];
    profileImage: string;
    yearsOfExperience?: number; // minimum years of experience
    category?: string; // mentor direction
}

export interface RecommendedMentor extends Mentor {
    collaborativeScore: number;
    contentBasedScore: number;
    finalScore: number;
}

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
    reviewsCount: number;
    profileImage: string;
    yearsOfExperience: number | null;
    category: string;
    bio: string;
    availability: string;
    skills: MentorProfileSkill[];
    programmingLanguages: string[];
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
    programmingLanguages: string[];
    bio: string;
    availability?: string;
    reviewCount: number;
    recentReviews: MentorProfileReview[];
    upcomingSessions: MentorProfileSession[];
    materials: MentorProfileMaterial[];
}

export interface Material {
    id: string;
    title: string;
    description: string;
    type: "document" | "video" | "link" | "presentation";
    sessionId?: string;
    mentorId: number;
    mentorName: string;
    createdAt: string;
    tags: string[];
    thumbnail?: string;
    content?: string;
    url?: string;
    fileSize?: string;
    attachments?: MaterialAttachment[];
}

export interface RecommendedMaterial extends Material {
    collaborativeScore: number;
    contentBasedScore: number;
    finalScore: number;
}

export interface MaterialAttachment {
    id: number;
    fileName: string;
    fileUrl: string;
    fileSize: number;
    contentType: string;
    uploadedAt: string;
}
