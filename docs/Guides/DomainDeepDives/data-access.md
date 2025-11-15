# Data Layer & API Integration Domain Deep Dive

## Overview

Data layer uses Axios-based API client with typed services for each domain, pagination support, and centralized error handling. Backend returns Result<T> wrapped responses.

## Centralized API Client

### Base Axios Configuration

```ts
// src/MentorSync.UI/src/shared/services/api.ts
import axios, {
	AxiosError,
	AxiosResponse,
	InternalAxiosRequestConfig,
} from "axios";

const api = axios.create({
	baseURL: import.meta.env.VITE_API_URL || "api",
	headers: {
		"Content-Type": "application/json",
	},
});

// Request interceptor: Add Bearer token
api.interceptors.request.use((config: InternalAxiosRequestConfig) => {
	const authTokens = getAuthTokens();
	if (authTokens?.token) {
		config.headers.Authorization = `Bearer ${authTokens.token}`;
	}
	return config;
});

// Response interceptor: Handle errors
api.interceptors.response.use(
	(response: AxiosResponse) => response,
	(error: AxiosError) => {
		if (error.response?.status === 401) {
			removeAuthTokens();
			toast.error("Сесія закінчилася. Будь ласка, увійдіть знову.");
		} else if (error.response?.status === 403) {
			toast.error("У вас немає доступу до цього ресурсу.");
		} else if (error.response?.status >= 500) {
			toast.error("Помилка сервера. Будь ласка, спробуйте пізніше.");
		}
		return Promise.reject(error);
	}
);

export default api;
```

## Domain-Specific Services

### Authentication Service

```ts
// src/MentorSync.UI/src/features/auth/services/authService.ts
export interface LoginRequest {
	email: string;
	password: string;
}

export interface RegisterRequest {
	email: string;
	userName: string;
	role: string;
	password: string;
	confirmPassword: string;
}

export interface AuthResponse {
	token?: string;
	refreshToken?: string;
	expiration?: string;
	needOnboarding?: boolean;
	success: boolean;
	message?: string;
	userId?: number;
}

export const authService = {
	register: async (data: RegisterRequest): Promise<AuthResponse> => {
		try {
			const response = await api.post("/users/register", data);
			return { ...response.data, success: true };
		} catch (error) {
			return { success: false, message: "Помилка реєстрації" };
		}
	},

	login: async (data: LoginRequest): Promise<AuthResponse> => {
		try {
			const response = await api.post("/users/login", data);
			if (response.data.token) {
				saveAuthTokens({
					token: response.data.token,
					refreshToken: response.data.refreshToken,
					expiration: response.data.expiration,
					needOnboarding: response.data.needOnboarding,
					userId: response.data.userId,
				});
			}
			return { ...response.data, success: true };
		} catch (error) {
			return { success: false, message: "Помилка входу" };
		}
	},

	refreshToken: async (): Promise<AuthResponse> => {
		try {
			const tokens = getAuthTokens();
			const response = await api.post("/users/refresh-token", {
				accessToken: tokens?.token,
				refreshToken: tokens?.refreshToken,
			});
			if (response.data.token) {
				saveAuthTokens(response.data);
			}
			return { ...response.data, success: true };
		} catch (error) {
			removeAuthTokens();
			return { success: false, message: "Помилка оновлення токену" };
		}
	},

	logout: (): void => {
		removeAuthTokens();
		localStorage.removeItem("needOnboarding");
	},
};
```

### Materials Service

```ts
// src/MentorSync.UI/src/features/materials/services/materialService.ts
export interface MaterialFilters {
	search?: string;
	types?: string[];
	tags?: string[];
	sortBy?: string;
	pageNumber?: number;
	pageSize?: number;
}

export interface Material {
	id: string;
	title: string;
	description: string;
	type: "document" | "video" | "link";
	mentorName: string;
	mentorId: number;
	createdAt: string;
	tags: string[];
	content: string;
}

export interface MaterialsResponse {
	materials: Material[];
	totalCount: number;
}

export const getMaterials = async (
	filters?: MaterialFilters
): Promise<MaterialsResponse> => {
	try {
		let queryParams = new URLSearchParams();

		if (filters?.search) queryParams.append("search", filters.search);
		if (filters?.types?.length) {
			filters.types.forEach((type) => queryParams.append("types", type));
		}
		if (filters?.tags?.length) {
			filters.tags.forEach((tag) => queryParams.append("tags", tag));
		}
		if (filters?.sortBy) queryParams.append("sortBy", filters.sortBy);

		queryParams.append(
			"pageNumber",
			filters?.pageNumber?.toString() || "1"
		);
		queryParams.append("pageSize", filters?.pageSize?.toString() || "10");

		const response = await api.get(`/materials?${queryParams.toString()}`);
		return response.data;
	} catch (error) {
		console.error("Failed to fetch materials:", error);
		throw error;
	}
};

export const getMaterialById = async (id: number): Promise<Material> => {
	const response = await api.get(`/materials/${id}`);
	return response.data;
};

export const createMaterial = async (material: {
	title: string;
	description: string;
	type: string;
}): Promise<Material> => {
	const response = await api.post("/materials", material);
	return response.data;
};
```

### Recommended Materials Service

```ts
// src/MentorSync.UI/src/features/materials/services/recommendedMaterialService.ts
export interface RecommendedMaterial extends Material {
	collaborativeScore: number;
	contentBasedScore: number;
	finalScore: number;
}

export const getRecommendedMaterials = async (
	filters?: MaterialFilters
): Promise<{ materials: RecommendedMaterial[]; totalCount: number }> => {
	try {
		let queryParams = new URLSearchParams();

		if (filters?.search) queryParams.append("searchTerm", filters.search);
		if (filters?.types?.length) {
			filters.types.forEach((type) => queryParams.append("type", type));
		}

		queryParams.append(
			"pageNumber",
			filters?.pageNumber?.toString() || "1"
		);
		queryParams.append("pageSize", filters?.pageSize?.toString() || "10");

		const response = await api.get(
			`/recommendations/materials?${queryParams.toString()}`
		);
		return response.data;
	} catch (error) {
		console.error("Failed to fetch recommended materials:", error);
		throw error;
	}
};
```

### User Management Service (Admin)

```ts
// src/MentorSync.UI/src/features/settings/services/userManagementService.ts
export interface UserFilterParams {
	role?: string;
	status?: "active" | "inactive";
	search?: string;
}

export interface UserShortResponse {
	id: number;
	userName: string;
	email: string;
	role: string;
	isActive: boolean;
}

export const getAllUsers = async (
	filters: UserFilterParams
): Promise<UserShortResponse[]> => {
	try {
		let queryParams = new URLSearchParams();
		if (filters.role) queryParams.append("role", filters.role);
		if (filters.search) queryParams.append("search", filters.search);

		const response = await api.get(`/users?${queryParams.toString()}`);
		return response.data;
	} catch (error) {
		console.error("Failed to fetch users:", error);
		throw error;
	}
};

export const toggleUserActiveStatus = async (
	userId: number,
	isActive: boolean
): Promise<void> => {
	await api.patch(`/users/${userId}/toggle-active`, { isActive });
};
```

## Pagination Pattern

### Pagination Response Type

```ts
// src/MentorSync.UI/src/shared/types/pagination.ts
export interface PaginationMetadata {
	pageNumber: number;
	pageSize: number;
	totalCount: number;
	totalPages: number;
	hasNextPage: boolean;
	hasPreviousPage: boolean;
}

export interface PaginatedResponse<T> {
	items: T[];
	metadata: PaginationMetadata;
}
```

### Pagination Usage

```tsx
// src/MentorSync.UI/src/features/materials/pages/MaterialsPage.tsx
const [pagination, setPagination] = useState({
	pageNumber: 1,
	pageSize: 12,
});

const combinedFilters = useMemo(
	() => ({
		...filters,
		...pagination,
	}),
	[filters, pagination]
);

// Fetch materials with pagination
const response = await getMaterials(combinedFilters);
setMaterials(response.materials);
setTotalCount(response.totalCount);

// Handle page change
const handlePageChange = (newPage: number) => {
	setPagination({ ...pagination, pageNumber: newPage });
};
```

## Error Handling

### API Error Response Type

```ts
// src/MentorSync.UI/src/features/auth/services/authStorage.ts
export interface ApiErrorResponse {
	type: string;
	title: string;
	status: number;
	detail: string;
	instance: string;
	errors?: { [key: string]: string[] };
}
```

### Error Handling Pattern

```ts
// Handle in service
try {
	const response = await api.get("/resource");
	return response.data;
} catch (error) {
	const apiError = error.response?.data as ApiErrorResponse;
	const errorMessage = apiError?.detail || "Помилка завантаження";
	toast.error(errorMessage);
	throw error;
}

// Handle in component
useEffect(() => {
	const loadData = async () => {
		try {
			const data = await fetchResource();
			setData(data);
		} catch (error) {
			setError("Failed to load resource");
		}
	};
	loadData();
}, []);
```

## Typed API Contracts

### Response Interfaces

```ts
// src/MentorSync.UI/src/shared/types/user.ts
export interface UserProfile {
	id: number;
	userName: string;
	email: string;
	role: "Mentor" | "Mentee" | "Admin";
	profileImageUrl?: string;
	createdAt: string;
	mentorProfile?: MentorProfile;
	menteeProfile?: MenteeProfile;
}

export interface MentorProfile {
	id: number;
	bio: string;
	yearsOfExperience: number;
	specializations: string[];
	hourlyRate: number;
}

export interface MenteeProfile {
	id: number;
	learningGoals: string;
	preferredSchedule: string;
}
```

## Key Patterns

1. **Service Functions Not Classes**: All services export functions, not class instances
2. **Typed Responses**: All API responses have TypeScript interfaces
3. **Error Transformation**: API errors transformed to user-friendly messages
4. **Query Parameter Building**: Use URLSearchParams for complex query params
5. **Bearer Token Auto-Injection**: Handled by request interceptor
6. **Pagination Metadata**: Include totalPages, hasNextPage, hasPreviousPage
7. **Centralized API Client**: All HTTP requests go through single api instance

## Configuration

-   Base URL: `VITE_API_URL` environment variable (defaults to `/api`)
-   Content Type: Always `application/json`
-   Authorization: `Bearer {token}` scheme
-   Error Format: RFC 7807 Problem Details

## File Organization

```ts
src/shared/
├── services/
│   └── api.ts                    (Central axios instance)
└── types/
    ├── pagination.ts             (Pagination interfaces)
    └── user.ts                   (User-related types)

src/features/{domain}/
└── services/
    └── {domain}Service.ts        (Domain-specific service)
```
