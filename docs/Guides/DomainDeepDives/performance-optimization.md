# Performance Domain Deep Dive

## Overview

Performance optimizations include React memoization (useMemo, useCallback), lazy loading routes, pagination for large lists, API response caching, and efficient component rendering patterns.

## React Memoization Strategies

### State Separation Pattern

```tsx
// src/MentorSync.UI/src/features/materials/pages/MaterialsPage.tsx
// ✅ GOOD: Separate concerns to avoid unnecessary re-renders
const [filters, setFilters] = useState({
	search: "",
	types: [] as string[],
	tags: [] as string[],
	sortBy: "newest",
});

const [pagination, setPagination] = useState({
	pageNumber: 1,
	pageSize: 12,
});

// Memoize combined object for prop passing
const combinedFilters = useMemo(
	() => ({
		...filters,
		...pagination,
	}),
	[filters, pagination]
);

// useCallback ensures function reference is stable
const handleFilterChange = useCallback((newFilter) => {
	setFilters(newFilter);
	setPagination({ pageNumber: 1, pageSize: 12 }); // Reset pagination
}, []);

// ❌ AVOID: Combining unrelated state
// const [filterState, setFilterState] = useState({
//     filters: { search: "", types: [] },
//     pagination: { pageNumber: 1 }
// });
// // This causes re-renders when pagination changes, triggering expensive filters logic
```

### Expensive Computation Memoization

```tsx
// src/MentorSync.UI/src/features/materials/pages/MaterialsPage.tsx
// Memoize mapper functions to prevent recreating on every render
const mapAPIToUIMaterials = useCallback(
	(item: any) => ({
		id: item.id.toString(),
		title: item.title,
		description: item.description,
		type: mapMaterialType(item.type),
		mentorName: item.mentorName || "Unknown Mentor",
		mentorId: item.mentorId,
		createdAt: new Date(item.createdAt).toLocaleDateString("uk-UA", {
			day: "numeric",
			month: "long",
			year: "numeric",
		}),
		tags: item.tags?.map((tag: any) => tag.name) || [],
		content: item.contentMarkdown,
		fileSize: item.attachments?.[0]?.fileSize
			? `${(item.attachments[0].fileSize / (1024 * 1024)).toFixed(1)} MB`
			: "Unknown",
	}),
	[]
);

// Use useMemo for expensive derivations
const mappedMaterials = useMemo(
	() => materials.map(mapAPIToUIMaterials),
	[materials, mapAPIToUIMaterials]
);
```

### Dependency Array Optimization

```tsx
// src/MentorSync.UI/src/features/settings/hooks/useUserManagement.ts
// GOOD: Memoized callback with correct dependencies
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
}, [filters]); // Only depends on filters

// Load users initially and when filters change
useEffect(() => {
	loadUsers();
}, [loadUsers]); // Depends on loadUsers to avoid stale closures
```

## Lazy Loading Routes

### Code Splitting with React.lazy

```tsx
// src/MentorSync.UI/src/routes.tsx
import { lazy, Suspense } from "react";

const LandingPage = lazy(() => import("./features/landing/pages/LandingPage"));
const LoginPage = lazy(() => import("./features/auth/pages/LoginPage"));
const RegisterPage = lazy(() => import("./features/auth/pages/RegisterPage"));
const DashboardPage = lazy(
	() => import("./features/dashboard/pages/DashboardPage")
);
const MentorSearchPage = lazy(
	() => import("./features/mentor-search/pages/MentorSearchPage")
);

const router = createBrowserRouter([
	{
		path: "/",
		element: (
			<Suspense fallback={<LoadingSpinner />}>
				<LandingPage />
			</Suspense>
		),
	},
	{
		path: "/login",
		element: (
			<Suspense fallback={<LoadingSpinner />}>
				<LoginPage />
			</Suspense>
		),
	},
	// More routes...
]);
```

### Route-Based Code Splitting

```tsx
// Pages are only loaded when user navigates to them
// Landing page: Users see this first, loads immediately
// Login page: Loaded when user clicks login
// Dashboard: Loaded after authentication
// This reduces initial bundle size significantly
```

## Pagination Performance

### Pagination Implementation

```tsx
// src/MentorSync.UI/src/features/materials/pages/MaterialsPage.tsx
const [pagination, setPagination] = useState({
	pageNumber: 1,
	pageSize: 12, // Load only 12 items per page
});

// Fetch only requested page
const handlePageChange = useCallback((newPage: number) => {
	setPagination((prev) => ({
		...prev,
		pageNumber: newPage,
	}));
}, []);

// Server-side pagination via API
const response = await getMaterials({
	...filters,
	pageNumber: pagination.pageNumber,
	pageSize: pagination.pageSize,
});

// Only load page data, not entire dataset
setMaterials(response.materials); // 12 items
setTotalCount(response.totalCount); // Total count for pagination UI
```

### Pagination Response Type

```ts
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

// Server returns only necessary page data
// Client calculates derived values from metadata
```

## API Response Caching

### Cache Implementation Pattern

```ts
// src/MentorSync.UI/src/features/materials/hooks/useMaterials.ts
const materialsCacheRef = useRef<Map<string, CachedResponse>>(new Map());

export function useMaterials(filters?: MaterialFilters) {
	const [materials, setMaterials] = useState<Material[]>([]);
	const [loading, setLoading] = useState<boolean>(false);

	// Create cache key from filters
	const cacheKey = JSON.stringify(filters);

	useEffect(() => {
		const fetchMaterials = async () => {
			// Check cache first
			if (materialsCacheRef.current.has(cacheKey)) {
				const cached = materialsCacheRef.current.get(cacheKey);
				if (Date.now() - cached.timestamp < 5 * 60 * 1000) {
					// 5 min cache
					setMaterials(cached.data);
					return;
				}
			}

			try {
				setLoading(true);
				const response = await getMaterials(filters);

				// Cache the response
				materialsCacheRef.current.set(cacheKey, {
					data: response.materials,
					timestamp: Date.now(),
				});

				setMaterials(response.materials);
			} finally {
				setLoading(false);
			}
		};

		fetchMaterials();
	}, [cacheKey]);

	return { materials, loading };
}
```

## Component Memoization

### React.memo for Props-Based Optimization

```tsx
// src/MentorSync.UI/src/features/materials/components/MaterialCard.tsx
interface MaterialCardProps {
	material: Material | RecommendedMaterial;
	showRecommendationScores?: boolean;
}

// Memoize component to prevent re-renders when props don't change
const MaterialCard: React.FC<MaterialCardProps> = ({
	material,
	showRecommendationScores = false,
}) => {
	const navigate = useNavigate();

	const handleClick = useCallback(() => {
		navigate(`/materials/${material.id}`);
	}, [material.id, navigate]);

	return <div onClick={handleClick}>{/* Card content */}</div>;
};

export default React.memo(MaterialCard);
```

## Performance Profiling

### React DevTools Profiler

```tsx
// Identify expensive renders
// 1. Open React DevTools in Chrome
// 2. Go to Profiler tab
// 3. Record interactions
// 4. Look for "Unnecessary" renders where props didn't change
// 5. Add useMemo/useCallback or React.memo to optimize

// Example: MaterialsPage took 850ms to render
// - MaterialsContent: 400ms (expensive filter logic)
// - MaterialCard (×12): 50ms each (re-renders even when props same)
// → Solution: Memoize filters with useMemo, wrap MaterialCard with React.memo
```

## Rendering Optimization

### Conditional Rendering Pattern

```tsx
// src/MentorSync.UI/src/features/sessions/components/SessionsContent.tsx
const SessionsContent: React.FC = () => {
	const [bookings, setBookings] = useState<BookingSession[]>([]);
	const [loading, setLoading] = useState<boolean>(true);
	const [filterType, setFilterType] = useState<FilterType>("all");

	if (loading) {
		return <div className="animate-pulse">Loading...</div>;
	}

	// Filter data on client-side only after loading
	const filteredBookings = useMemo(
		() => bookings.filter((booking) => applyFilter(booking, filterType)),
		[bookings, filterType]
	);

	return (
		<div>
			{filteredBookings.length === 0 ? (
				<p>No bookings found</p>
			) : (
				filteredBookings.map((booking) => (
					<BookingCard key={booking.id} booking={booking} />
				))
			)}
		</div>
	);
};
```

## Performance Budgets

### Load Time Targets

-   Initial page load: < 3 seconds
-   Route navigation: < 1 second
-   Component interaction response: < 100ms
-   API response time: < 500ms

### Bundle Size Targets

-   Main bundle: < 200KB (gzipped)
-   Lazy-loaded route: < 50KB each
-   Total JS: < 500KB (gzipped)

## Key Patterns

1. **Separate State Concerns**: Don't combine unrelated state
2. **Memoize Expensive Operations**: Use useMemo for derivations
3. **Stable Function References**: Use useCallback for callbacks
4. **Pagination Over Full Lists**: Always paginate large datasets
5. **Lazy Load Routes**: Split code by route
6. **React.memo Components**: Wrap components that receive stable props
7. **Cache API Responses**: Cache rarely-changing data
8. **Profile Before Optimizing**: Use React DevTools Profiler

## Tools

-   React DevTools Profiler
-   Chrome DevTools Performance tab
-   Lighthouse audit
-   Bundle analyzer (webpack-bundle-analyzer)
