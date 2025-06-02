import api from "../../../shared/services/api";
import { OnboardingData } from "../data/OnboardingTypes";
import { getUserId } from "../../auth/utils/authUtils";

// Function to get the user ID from the JWT token or fallback to localStorage

export const onboardingService = {
    /**
     * Save onboarding data for a mentor
     * @param data The onboarding data
     * @returns Response with success status and message
     */
    saveMentorProfile: async (data: OnboardingData) => {
        try {
            const mentorProfileData = {
                bio: data.bio,
                position: data.position,
                company: data.company,
                industries: data.industryFlag, // Industry enum flags
                skills: data.skills,
                programmingLanguages: data.programmingLanguages,
                experienceYears: data.yearsOfExperience,
                availability: data.availabilityFlag, // Availability enum flags
                mentorId: getUserId(),
            };

            // Make API call to save mentor profile
            const response = await api.post(
                "/mentors/profile",
                mentorProfileData
            );
            return {
                success: true,
                data: response.data,
            };
        } catch (error) {
            console.error("Error saving mentor profile:", error);
            return {
                success: false,
                message: "Failed to save mentor profile",
            };
        }
    },
    /**
     * Save onboarding data for a mentee
     * @param data The onboarding data
     * @returns Response with success status and message
     */ saveMenteeProfile: async (data: OnboardingData) => {
        try {
            const menteeProfileData = {
                bio: data.bio,
                position: data.position,
                company: data.company,
                industries: data.industryFlag, // Industry enum flags
                skills: data.skills,
                programmingLanguages: data.programmingLanguages,
                learningGoals: data.goals,
                menteeId: getUserId(),
            };

            // Make API call to save mentee profile
            const response = await api.post(
                "/mentees/profile",
                menteeProfileData
            );
            return {
                success: true,
                data: response.data,
            };
        } catch (error) {
            console.error("Error saving mentee profile:", error);
            return {
                success: false,
                message: "Failed to save mentee profile",
            };
        }
    },
};
