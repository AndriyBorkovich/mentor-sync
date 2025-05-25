import api from "../../auth/services/api";
import { OnboardingData } from "../data/OnboardingTypes";

export const onboardingService = {
    /**
     * Save onboarding data for a mentor
     * @param data The onboarding data
     * @returns Response with success status and message
     */
    saveMentorProfile: async (data: OnboardingData) => {
        try {
            // Format data according to API expectations
            const mentorProfileData = {
                fullName: data.fullName,
                bio: data.bio,
                location: data.location,
                position: data.position,
                company: data.company,
                yearsOfExperience: data.yearsOfExperience,
                skills: data.skills,
                expertiseAreas: data.expertiseAreas,
                availabilityHours: data.availabilityHours,
                preferredMeetingFormat: data.preferredMeetingFormat,
                mentorshipStyle: data.mentorshipStyle,
                maxMentees: data.maxMentees,
                hourlyRate: data.hourlyRate,
                preferredMenteeLevel: data.preferredMenteeLevel,
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
     */
    saveMenteeProfile: async (data: OnboardingData) => {
        try {
            // Format data according to API expectations
            const menteeProfileData = {
                fullName: data.fullName,
                bio: data.bio,
                location: data.location,
                position: data.position,
                company: data.company,
                yearsOfExperience: data.yearsOfExperience,
                skills: data.skills,
                expertiseAreas: data.expertiseAreas,
                availabilityHours: data.availabilityHours,
                preferredMeetingFormat: data.preferredMeetingFormat,
                goals: data.goals,
                desiredSkills: data.desiredSkills,
                expectedSessionFrequency: data.expectedSessionFrequency,
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
