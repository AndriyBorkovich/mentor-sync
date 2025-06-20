import api from "../../../shared/services/api";
import { toast } from "react-toastify";

// Upload avatar
export const uploadAvatar = async (
    userId: number,
    file: File
): Promise<string> => {
    try {
        const formData = new FormData();
        formData.append("file", file);

        const response = await api.post(`/users/${userId}/avatar`, formData, {
            headers: {
                "Content-Type": "multipart/form-data",
            },
        });

        return response.data;
    } catch (error) {
        console.error("Error uploading avatar:", error);
        throw error;
    }
};

// Delete avatar
export const deleteAvatar = async (userId: number): Promise<void> => {
    try {
        await api.delete(`/users/${userId}/avatar`);
    } catch (error) {
        console.error("Error deleting avatar:", error);
        throw error;
    }
};

// Edit mentor profile
export interface UpdateMentorProfileRequest {
    id: number;
    bio: string;
    position: string;
    company: string;
    industries: number;
    skills: string[];
    programmingLanguages: string[];
    experienceYears: number;
    availability: number;
    mentorId: number;
}

export const updateMentorProfile = async (
    profileData: UpdateMentorProfileRequest
): Promise<any> => {
    try {
        const response = await api.put("/mentors/profile", profileData);
        toast.success("Профіль успішно оновлено");
        return response.data;
    } catch (error) {
        console.error("Error updating mentor profile:", error);
        toast.error("Помилка при оновленні профілю");
        throw error;
    }
};

// Edit mentee profile
export interface UpdateMenteeProfileRequest {
    id: number;
    bio: string;
    position: string;
    company: string;
    industries: number;
    skills: string[];
    programmingLanguages: string[];
    learningGoals: string[];
    menteeId: number;
}

export const updateMenteeProfile = async (
    profileData: UpdateMenteeProfileRequest
): Promise<any> => {
    try {
        const response = await api.put("/mentees/profile", profileData);
        toast.success("Профіль успішно оновлено");
        return response.data;
    } catch (error) {
        console.error("Error updating mentee profile:", error);
        toast.error("Помилка при оновленні профілю");
        throw error;
    }
};
