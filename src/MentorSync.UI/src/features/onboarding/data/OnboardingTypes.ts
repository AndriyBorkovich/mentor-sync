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

// Enums matching the backend
export enum Industry {
    None = 0,
    WebDevelopment = 1 << 0, // 1
    DataScience = 1 << 1, // 2
    CyberSecurity = 1 << 2, // 4
    CloudComputing = 1 << 3, // 8
    DevOps = 1 << 4, // 16
    GameDevelopment = 1 << 5, // 32
    ItSupport = 1 << 6, // 64
    ArtificialIntelligence = 1 << 7, // 128
    Blockchain = 1 << 8, // 256
    Networking = 1 << 9, // 512
    UxUiDesign = 1 << 10, // 1024
    EmbeddedSystems = 1 << 11, // 2048
    ItConsulting = 1 << 12, // 4096
    DatabaseAdministration = 1 << 13, // 8192
}

export enum Availability {
    None = 0,
    Morning = 1 << 0, // 1
    Afternoon = 1 << 1, // 2
    Evening = 1 << 2, // 4
    Night = 1 << 3, // 8
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
    desiredSkills: [],
    industryFlag: Industry.None,
};

// OnboardingStep type to track progress
export type OnboardingStep = 1 | 2 | 3 | 4 | 5;
