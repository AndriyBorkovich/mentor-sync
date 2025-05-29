import { Availability } from "../../../shared/enums/availability";
import { Industry } from "../../../shared/enums/industry";

// Onboarding data types
export interface OnboardingData {
    // Step 1: Bio
    bio: string;

    // Step 2: Professional Info
    position: string;
    company: string;

    // Step 3: Skills & Expertise
    skills: string[];
    programmingLanguages: string[];
    industryFlag: number; // Maps to Industry enum

    // Step 4: Availability

    // Step 5: Specific to role
    // For Mentees
    goals: string[];

    // For Mentors - YOE
    yearsOfExperience: number;
    availabilityFlag: number; // Maps to Availability enum
}

// Default empty onboarding data
export const initialOnboardingData: OnboardingData = {
    // Step 1: Bio
    bio: "",

    // Step 2: Professional Info
    position: "",
    company: "",
    yearsOfExperience: 0,

    // Step 3: Skills & Expertise
    skills: [],
    programmingLanguages: [],

    // Step 4: Availability
    availabilityFlag: Availability.None,

    // Step 5: Specific to role
    goals: [],
    industryFlag: Industry.None,
};

// OnboardingStep type to track progress
export type OnboardingStep = 1 | 2 | 3 | 4 | 5;
