import { useEffect, useState } from "react";
import { useAuth } from "../context/AuthContext";
import api from "../../../shared/services/api";

interface UserProfile {
    id: string;
    userName: string;
    email: string;
    role: string;
    // Add other user properties as needed
}

export const useUser = () => {
    const { isAuthenticated } = useAuth();
    const [user, setUser] = useState<UserProfile | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchUserProfile = async () => {
            if (!isAuthenticated) {
                setUser(null);
                setLoading(false);
                return;
            }

            try {
                setLoading(true);
                const response = await api.get("/users/profile");
                setUser(response.data);
                setError(null);
            } catch (err) {
                console.error("Failed to fetch user profile:", err);
                setError("Failed to load user profile");
                setUser(null);
            } finally {
                setLoading(false);
            }
        };

        fetchUserProfile();
    }, [isAuthenticated]);

    return { user, loading, error };
};
