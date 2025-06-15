/**
 * Format role name for display
 * @param role Role string from API
 * @returns Localized role name
 */
export const formatRoleName = (role: string): string => {
    switch (role) {
        case "Admin":
            return "Адміністратор";
        case "Mentor":
            return "Ментор";
        case "Mentee":
            return "Менті";
        default:
            return role;
    }
};
