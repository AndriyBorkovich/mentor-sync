export interface PaginatedResponse<T> {
    items: T[];
    pageNumber: number;
    pageSize: number;
    totalCount: number;
    hasNextPage: boolean;
    hasPreviousPage: boolean;
    totalPages: number;
}

export interface PaginationParams {
    pageNumber?: number;
    pageSize?: number;
}
