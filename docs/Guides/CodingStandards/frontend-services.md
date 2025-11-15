# Frontend Services Style Guide

Services handle all HTTP communication with the backend API.

## Location & Naming

-   **Location**: `src/features/{domain}/services/{domain}Service.ts`
-   **Naming**: `{domain}Service.ts` (e.g., `materialService.ts`, `userService.ts`)
-   **Export**: Named exports for each service function

## Service Pattern

```typescript
import { apiClient } from "@/shared/services/api";

// Request/Response types
export interface MaterialFilters {
	searchTerm?: string;
	category?: string;
	pageNumber?: number;
	pageSize?: number;
}

export interface MaterialResponse {
	id: string;
	title: string;
	description: string;
	category: string;
	level: number;
}

// Service object with functions
export const materialService = {
	// List with pagination
	list: async (filters?: MaterialFilters) => {
		const response = await apiClient.get<{
			items: MaterialResponse[];
			totalCount: number;
		}>("/materials", { params: filters });
		return response.data;
	},

	// Get single item
	getById: async (id: string) => {
		const response = await apiClient.get<MaterialResponse>(
			`/materials/${id}`
		);
		return response.data;
	},

	// Create item
	create: async (data: CreateMaterialRequest) => {
		const response = await apiClient.post<MaterialResponse>(
			"/materials",
			data
		);
		return response.data;
	},

	// Update item
	update: async (id: string, data: UpdateMaterialRequest) => {
		const response = await apiClient.put<MaterialResponse>(
			`/materials/${id}`,
			data
		);
		return response.data;
	},

	// Delete item
	delete: async (id: string) => {
		await apiClient.delete(`/materials/${id}`);
	},
};
```

## Principles

1. **All async**: Every service function returns Promise
2. **Type-safe**: Always provide request/response types
3. **Simple functions**: No classes, just exported functions
4. **Consistent error handling**: Errors thrown by apiClient interceptor
5. **No side effects**: Don't manipulate state from services
6. **Stateless**: No instance data, pure functions only

## Centralized API Client

```typescript
// src/shared/services/api.ts
import axios from "axios";
import { toast } from "react-toastify";

export const apiClient = axios.create({
	baseURL: process.env.REACT_APP_API_URL || "http://localhost:5001/api",
	timeout: 30000,
	headers: {
		"Content-Type": "application/json",
	},
});

// Request interceptor: Auto-inject Bearer token
apiClient.interceptors.request.use((config) => {
	const token = localStorage.getItem("accessToken");
	if (token) {
		config.headers.Authorization = `Bearer ${token}`;
	}
	return config;
});

// Response interceptor: Handle errors globally
apiClient.interceptors.response.use(
	(response) => response,
	(error) => {
		if (error.response?.status === 401) {
			// Token expired, logout
			localStorage.removeItem("accessToken");
			localStorage.removeItem("refreshToken");
			window.location.href = "/login";
		} else if (error.response?.status === 403) {
			toast.error("Access denied");
		} else if (error.response?.status >= 500) {
			toast.error("Server error. Please try again later.");
		} else {
			toast.error(error.response?.data?.detail || "An error occurred");
		}
		return Promise.reject(error);
	}
);
```

## Complex Service Examples

```typescript
// Auth service with token management
export const authService = {
	login: async (email: string, password: string) => {
		const response = await apiClient.post<AuthResponse>("/users/login", {
			email,
			password,
		});
		return response.data;
	},

	register: async (data: RegisterRequest) => {
		const response = await apiClient.post<AuthResponse>(
			"/users/register",
			data
		);
		return response.data;
	},

	refreshToken: async () => {
		const refreshToken = localStorage.getItem("refreshToken");
		if (!refreshToken) {
			throw new Error("No refresh token available");
		}

		const response = await apiClient.post<AuthResponse>(
			"/users/refresh-token",
			{
				refreshToken,
			}
		);
		return response.data;
	},

	getCurrentUser: async () => {
		const response = await apiClient.get<UserProfileResponse>(
			"/users/profile"
		);
		return response.data;
	},
};

// Mentor service with search
export const mentorService = {
	search: async (filters: MentorFilters) => {
		const response = await apiClient.get<PagedResponse<MentorResponse>>(
			"/mentors",
			{ params: filters }
		);
		return response.data;
	},

	getById: async (id: string) => {
		const response = await apiClient.get<MentorDetailResponse>(
			`/mentors/${id}`
		);
		return response.data;
	},

	getRatings: async (mentorId: string, page: number = 1) => {
		const response = await apiClient.get<PagedResponse<RatingResponse>>(
			`/mentors/${mentorId}/ratings`,
			{ params: { pageNumber: page, pageSize: 10 } }
		);
		return response.data;
	},
};
```

## Error Handling in Services

Errors are caught by API interceptor, but service can enhance:

```typescript
export const sessionService = {
	book: async (sessionData: BookSessionRequest) => {
		try {
			const response = await apiClient.post<SessionResponse>(
				"/sessions/book",
				sessionData
			);
			toast.success("Session booked successfully!");
			return response.data;
		} catch (error) {
			// Interceptor already showed error toast
			throw error;
		}
	},

	cancel: async (sessionId: string) => {
		try {
			await apiClient.post(`/sessions/${sessionId}/cancel`);
			toast.success("Session cancelled");
		} catch (error) {
			if ((error as AxiosError).response?.status === 409) {
				toast.error("Cannot cancel session at this time");
			}
			throw error;
		}
	},
};
```

## File Upload Service

```typescript
export const fileService = {
	uploadAvatar: async (file: File) => {
		const formData = new FormData();
		formData.append("file", file);

		const response = await apiClient.post<{ url: string }>(
			"/files/upload/avatar",
			formData,
			{
				headers: { "Content-Type": "multipart/form-data" },
			}
		);
		return response.data;
	},
};
```

## Pagination Service Response

```typescript
export interface PagedResponse<T> {
	items: T[];
	totalCount: number;
	pageNumber: number;
	pageSize: number;
	totalPages: number;
	hasNextPage: boolean;
	hasPreviousPage: boolean;
}

// Usage in service
export const materialService = {
	list: async (filters: MaterialFilters & PaginationParams) => {
		const response = await apiClient.get<PagedResponse<MaterialResponse>>(
			"/materials",
			{ params: filters }
		);
		return response.data; // Includes pagination metadata
	},
};
```

## Testing Services

```typescript
describe('materialService', () => {
    it('fetches materials with filters', async () => {
        const mockData = { items: [...], totalCount: 10 };
        jest.spyOn(apiClient, 'get').mockResolvedValue({ data: mockData });

        const result = await materialService.list({ category: 'Programming', pageNumber: 1 });

        expect(result).toEqual(mockData);
        expect(apiClient.get).toHaveBeenCalledWith('/materials', {
            params: expect.objectContaining({ category: 'Programming' }),
        });
    });
});
```

## Best Practices

-   ✅ Use object export pattern for grouping related functions
-   ✅ Type all requests and responses
-   ✅ Handle pagination parameters consistently
-   ✅ Use centralized apiClient for consistent interceptors
-   ✅ Keep functions simple and focused
-   ✅ Let interceptors handle global error states
-   ✅ Support cancellation via AbortController if needed
-   ❌ Don't import services directly in components
-   ❌ Don't use service functions in services (keep flat)
-   ❌ Don't mix unrelated concerns (auth + materials)
-   ❌ Don't create service classes (use functions)
