import { useState, useEffect, useCallback } from "react";
import { toast } from "react-toastify";
import { UserShortResponse, UserFilterParams } from "../types/userTypes";
import {
    getAllUsers,
    toggleUserActiveStatus,
} from "../services/userManagementService";

export function useUserManagement() {
    const [users, setUsers] = useState<UserShortResponse[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [filters, setFilters] = useState<UserFilterParams>({});
    const [searchTerm, setSearchTerm] = useState<string>("");

    // Load users with current filters
    const loadUsers = useCallback(async () => {
        try {
            setLoading(true);
            const usersData = await getAllUsers(filters);
            setUsers(usersData);
        } catch (error) {
            toast.error("Помилка при завантаженні користувачів");
        } finally {
            setLoading(false);
        }
    }, [filters]);

    // Load users initially and when filters change
    useEffect(() => {
        loadUsers();
    }, [loadUsers]);

    // Filter users locally by search term
    const filteredUsers = useCallback(() => {
        if (!searchTerm) return users;

        const lowerSearchTerm = searchTerm.toLowerCase();
        return users.filter(
            (user) =>
                user.name.toLowerCase().includes(lowerSearchTerm) ||
                user.email.toLowerCase().includes(lowerSearchTerm)
        );
    }, [users, searchTerm]);

    // Toggle active status for a user
    const toggleUserActive = async (userId: number) => {
        try {
            await toggleUserActiveStatus(userId);

            // Update user status locally
            setUsers((prevUsers) =>
                prevUsers.map((user) =>
                    user.id === userId
                        ? { ...user, isActive: !user.isActive }
                        : user
                )
            );

            toast.success("Статус користувача змінено");
        } catch (error) {
            toast.error("Помилка при зміні статусу користувача");
        }
    };

    // Apply role filter
    const filterByRole = (role?: string) => {
        setFilters((prev) => ({ ...prev, role }));
    };

    // Apply active status filter
    const filterByStatus = (isActive?: boolean) => {
        setFilters((prev) => ({ ...prev, isActive }));
    };

    // Clear all filters
    const clearFilters = () => {
        setFilters({});
        setSearchTerm("");
    };

    return {
        users: filteredUsers(),
        loading,
        searchTerm,
        setSearchTerm,
        toggleUserActive,
        filterByRole,
        filterByStatus,
        filters,
        clearFilters,
    };
}
