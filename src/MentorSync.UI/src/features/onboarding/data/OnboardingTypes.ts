// Onboarding data types
export interface OnboardingData {
    // Step 1: Basic Info
    fullName: string;
    bio: string;
    location: string;

    // Step 2: Professional Info
    position: string;
    company: string;
    yearsOfExperience: number;

    // Step 3: Skills & Expertise
    skills: string[];
    expertiseAreas: string[];

    // Step 4: Availability
    availabilityHours: AvailabilityHours;
    preferredMeetingFormat: "online" | "inPerson" | "both";

    // Step 5: Specific to role
    // For Mentees
    goals: string[];
    desiredSkills: string[];
    expectedSessionFrequency: string;

    // For Mentors
    mentorshipStyle: string;
    maxMentees: number;
    hourlyRate?: number; // Optional, if mentorship is paid
    preferredMenteeLevel: string[];
}

export interface AvailabilityHours {
    monday: TimeSlot[];
    tuesday: TimeSlot[];
    wednesday: TimeSlot[];
    thursday: TimeSlot[];
    friday: TimeSlot[];
    saturday: TimeSlot[];
    sunday: TimeSlot[];
}

export interface TimeSlot {
    start: string; // Format: "HH:MM"
    end: string; // Format: "HH:MM"
}

// Default empty onboarding data
export const initialOnboardingData: OnboardingData = {
    // Step 1: Basic Info
    fullName: "",
    bio: "",
    location: "",

    // Step 2: Professional Info
    position: "",
    company: "",
    yearsOfExperience: 0,

    // Step 3: Skills & Expertise
    skills: [],
    expertiseAreas: [],

    // Step 4: Availability
    availabilityHours: {
        monday: [],
        tuesday: [],
        wednesday: [],
        thursday: [],
        friday: [],
        saturday: [],
        sunday: [],
    },
    preferredMeetingFormat: "both",

    // Step 5: Specific to role
    goals: [],
    desiredSkills: [],
    expectedSessionFrequency: "",
    mentorshipStyle: "",
    maxMentees: 5,
    preferredMenteeLevel: [],
};

// OnboardingStep type to track progress
export type OnboardingStep = 1 | 2 | 3 | 4 | 5;
