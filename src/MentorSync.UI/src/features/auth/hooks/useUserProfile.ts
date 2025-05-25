import { useEffect, useState } from "react";
import { useAuth } from "../context/AuthContext";
import api from "../services/api";

export interface UserProfile {
    id: string;
    userName: string;
    email: string;
    role: string;
    // Add other user properties as needed
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

    const updateProfile = async (
        data: Partial<UserProfile>
    ): Promise<boolean> => {
        try {
            setLoading(true);
            await api.put("/users/profile", data);

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
