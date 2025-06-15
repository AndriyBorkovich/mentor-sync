export interface UserShortResponse {
    id: number;
    name: string;
    email: string;
    role: string;
    avatarUrl?: string;
    isActive: boolean;
    isEmailConfirmed: boolean;
}

export interface UserFilterParams {
    role?: string;
    isActive?: boolean;
    searchQuery?: string;
}
