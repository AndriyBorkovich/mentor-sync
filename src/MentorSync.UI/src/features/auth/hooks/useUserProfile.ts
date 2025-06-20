import { useEffect, useState } from "react";
import { useAuth } from "../context/AuthContext";
import api from "../../../shared/services/api";

export interface MenteeProfileInfo {
    id: number;
    bio: string;
    position: string;
    company: string;
    category: string;
    skills: string[];
    programmingLanguages: string[];
    learningGoals: string[];
}

export interface MentorProfileInfo {
    id: number;
    bio: string;
    position: string;
    company: string;
    category: string;
    skills: string[];
    programmingLanguages: string[];
    experienceYears: number;
}

export interface UserProfile {
    id: number;
    userName: string;
    email: string;
    role: string;
    profileImageUrl?: string;
    isActive: boolean;
    menteeProfile?: MenteeProfileInfo;
    mentorProfile?: MentorProfileInfo;
}

/**
 * Hook for fetching and managing user profile data
 */
export const useUserProfile = () => {
    const { isAuthenticated } = useAuth();
    const [profile, setProfile] = useState<UserProfile | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchUserProfile = async () => {
            if (!isAuthenticated) {
                setProfile(null);
                setLoading(false);
                return;
            }

            try {
                setLoading(true);
                const response = await api.get("/users/profile");
                setProfile(response.data);
                setError(null);
            } catch (err) {
                console.error("Failed to fetch user profile:", err);
                setError("Failed to load user profile");
                setProfile(null);
            } finally {
                setLoading(false);
            }
        };

        fetchUserProfile();
    }, [isAuthenticated]);

    const updateProfile = async (): Promise<boolean> => {
        try {
            setLoading(true);

            // Refresh profile data
            const response = await api.get("/users/profile");
            setProfile(response.data);
            setError(null);
            return true;
        } catch (err) {
            console.error("Failed to update user profile:", err);
            setError("Failed to update user profile");
            return false;
        } finally {
            setLoading(false);
        }
    };

    return { profile, loading, error, updateProfile };
};
