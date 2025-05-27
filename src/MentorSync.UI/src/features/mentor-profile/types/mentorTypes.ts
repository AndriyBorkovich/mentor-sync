import { Mentor } from "../../dashboard/data/mentors";
import { MentorProfile } from "../services/mentorProfileService";

// Union type that accepts either the old Mentor type or the new MentorProfile type
export type MentorData = Mentor | MentorProfile;

// Helper function to check if a mentor object is of type MentorProfile
export function isMentorProfile(mentor: MentorData): mentor is MentorProfile {
    return (
        (mentor as MentorProfile).bio !== undefined &&
        (mentor as MentorProfile).reviewCount !== undefined
    );
}

// Convert a number ID to string to maintain compatibility with components
export function ensureStringId(id: string | number): string {
    return typeof id === "number" ? id.toString() : id;
}
