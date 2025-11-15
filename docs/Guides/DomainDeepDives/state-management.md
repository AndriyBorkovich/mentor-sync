# State Management Domain Deep Dive

## Overview

State management uses React Context API for global auth and onboarding state, combined with custom hooks for domain-specific logic and useState for local component state.

## Context API Architecture

### AuthContext (Global Auth State)

```tsx
// src/MentorSync.UI/src/features/auth/context/AuthContext.tsx
interface AuthContextType {
	isAuthenticated: boolean;
	isLoading: boolean;
	needsOnboarding: boolean;
	logout: () => void;
	loginUser: (response: any) => void;
	refreshAuthToken: () => Promise<boolean>;
	verifyAuthStatus: () => Promise<void>;
	user: User | null;
}

const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
	const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
	const [isLoading, setIsLoading] = useState<boolean>(true);
	const [needsOnboarding, setNeedsOnboarding] = useState<boolean>(false);
	const [user, setUser] = useState<User | null>(null);

	// Verify auth on component mount
	useEffect(() => {
		verifyAuth();
	}, []);

	return (
		<AuthContext.Provider
			value={{
				isAuthenticated,
				isLoading,
				needsOnboarding,
				logout,
				loginUser,
				verifyAuthStatus,
				refreshAuthToken,
				user,
			}}
		>
			{children}
		</AuthContext.Provider>
	);
};

export const useAuth = () => {
	const context = useContext(AuthContext);
	if (!context) {
		throw new Error("useAuth must be used within an AuthProvider");
	}
	return context;
};
```

### OnboardingContext (Multi-Step Form State)

```tsx
// src/MentorSync.UI/src/features/onboarding/context/OnboardingContext.tsx
interface OnboardingContextType {
	data: OnboardingData;
	currentStep: OnboardingStep;
	updateData: (newData: Partial<OnboardingData>) => void;
	nextStep: () => void;
	prevStep: () => void;
	goToStep: (step: OnboardingStep) => void;
	isLastStep: boolean;
	isFirstStep: boolean;
	role: "mentor" | "mentee";
}

export const OnboardingProvider: React.FC<OnboardingProviderProps> = ({
	children,
	initialRole,
}) => {
	const [data, setData] = useState<OnboardingData>(initialOnboardingData);
	const [currentStep, setCurrentStep] = useState<OnboardingStep>(1);
	const [role] = useState<"mentor" | "mentee">(initialRole);

	const updateData = (newData: Partial<OnboardingData>) => {
		setData((prevData) => ({
			...prevData,
			...newData,
		}));
	};

	const nextStep = () => {
		if (currentStep < 5) {
			setCurrentStep((prevStep) => (prevStep + 1) as OnboardingStep);
		}
	};

	const prevStep = () => {
		if (currentStep > 1) {
			setCurrentStep((prevStep) => (prevStep - 1) as OnboardingStep);
		}
	};

	const goToStep = (step: OnboardingStep) => {
		setCurrentStep(step);
	};

	const isLastStep = currentStep === 5;
	const isFirstStep = currentStep === 1;

	return (
		<OnboardingContext.Provider
			value={{
				data,
				currentStep,
				updateData,
				nextStep,
				prevStep,
				goToStep,
				isLastStep,
				isFirstStep,
				role,
			}}
		>
			{children}
		</OnboardingContext.Provider>
	);
};

export const useOnboarding = () => {
	const context = useContext(OnboardingContext);
	if (!context) {
		throw new Error(
			"useOnboarding must be used within an OnboardingProvider"
		);
	}
	return context;
};
```

## Custom Hooks for Domain Logic

### useUserProfile Hook

```ts
// src/MentorSync.UI/src/features/auth/hooks/useUserProfile.ts
export const useUserProfile = () => {
	const { isAuthenticated } = useAuth();
	const [profile, setProfile] = useState<UserProfile | null>(null);
	const [loading, setLoading] = useState<boolean>(true);

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
			} catch (err) {
				console.error("Failed to fetch user profile:", err);
				setProfile(null);
			} finally {
				setLoading(false);
			}
		};

		fetchUserProfile();
	}, [isAuthenticated]);

	return { profile, loading };
};
```

### useMentorProfile Hook

```ts
// src/MentorSync.UI/src/features/mentor-profile/hooks/useMentorProfile.ts
export function useMentorProfile(mentorId: string | undefined) {
	const [mentor, setMentor] = useState<MentorData>(mockMentor);
	const [loading, setLoading] = useState<boolean>(true);
	const [error, setError] = useState<string | null>(null);

	useEffect(() => {
		const fetchMentorProfile = async () => {
			if (!mentorId) {
				setError("Mentor ID not provided");
				return;
			}

			try {
				setLoading(true);
				const basicInfo = await getMentorBasicInfo(
					parseInt(mentorId, 10)
				);
				const reviews = await getMentorReviews(parseInt(mentorId, 10));
				const materials = await getMentorMaterials(
					parseInt(mentorId, 10)
				);

				setMentor({
					...basicInfo,
					reviews,
					materials,
				});
			} catch (err) {
				setError("Failed to load mentor profile");
				console.error(err);
			} finally {
				setLoading(false);
			}
		};

		fetchMentorProfile();
	}, [mentorId]);

	return { mentor, loading, error };
}
```

### useMaterials Hook

```ts
// src/MentorSync.UI/src/features/materials/hooks/useMaterials.ts
export function useMaterials(filters?: MaterialFilters) {
	const [materials, setMaterials] = useState<Material[]>([]);
	const [loading, setLoading] = useState<boolean>(false);
	const [error, setError] = useState<string | null>(null);
	const [totalCount, setTotalCount] = useState<number>(0);

	useEffect(() => {
		const fetchMaterials = async () => {
			try {
				setLoading(true);
				const response = await getMaterials(filters);
				setMaterials(response.materials);
				setTotalCount(response.totalCount);
			} catch (err) {
				setError("Failed to fetch materials");
			} finally {
				setLoading(false);
			}
		};

		fetchMaterials();
	}, [filters]);

	return { materials, loading, error, totalCount };
}
```

### useUserManagement Hook (Admin)

```ts
// src/MentorSync.UI/src/features/settings/hooks/useUserManagement.ts
export function useUserManagement() {
	const [users, setUsers] = useState<UserShortResponse[]>([]);
	const [loading, setLoading] = useState<boolean>(true);
	const [filters, setFilters] = useState<UserFilterParams>({});

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

	return { users, loading, filters, setFilters, loadUsers };
}
```

## Local Component State Pattern

### Materials Page State Management

```tsx
// src/MentorSync.UI/src/features/materials/pages/MaterialsPage.tsx
const MaterialsPage: React.FC = () => {
	// Separate concerns: filters vs pagination
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

	const [materials, setMaterials] = useState<Material[]>([]);
	const [isLoading, setIsLoading] = useState<boolean>(true);

	// Memoize combined filters to prevent unnecessary re-renders
	const combinedFilters = useMemo(
		() => ({
			...filters,
			...pagination,
		}),
		[filters, pagination]
	);

	// useCallback to distinguish filter changes from pagination changes
	const handleFilterChange = useCallback((newFilter) => {
		setFilters(newFilter);
		setPagination({ pageNumber: 1, pageSize: 12 }); // Reset to page 1
	}, []);

	return (
		<MaterialsContent
			filters={combinedFilters}
			onFilterChange={handleFilterChange}
		/>
	);
};
```

### Sessions Page State Management

```tsx
// src/MentorSync.UI/src/features/sessions/components/SessionsContent.tsx
const SessionsContent: React.FC = () => {
	const [bookings, setBookings] = useState<BookingSession[]>([]);
	const [loading, setLoading] = useState<boolean>(true);
	const [filterType, setFilterType] = useState<FilterType>("all");
	const [showFilterDropdown, setShowFilterDropdown] = useState(false);
	const [confirmingId, setConfirmingId] = useState<number | null>(null);

	useEffect(() => {
		const loadBookings = async () => {
			try {
				setLoading(true);
				const data = hasRole("Mentor")
					? await getMentorBookings()
					: await getMenteeBookings();
				setBookings(filterBookings(data, filterType));
			} finally {
				setLoading(false);
			}
		};

		loadBookings();
	}, [filterType]);

	return <div>{/* Filter dropdown with state management */}</div>;
};
```

## Provider Setup

### App.tsx

```tsx
// src/MentorSync.UI/src/App.tsx
import { RouterProvider } from "react-router-dom";
import { AuthProvider } from "./features/auth/context/AuthContext";
import { ToastContainer } from "react-toastify";

function App() {
	return (
		<AuthProvider>
			<RouterProvider router={router} />
			<ToastContainer
				position="bottom-right"
				autoClose={3000}
				theme="light"
			/>
		</AuthProvider>
	);
}
```

### Onboarding Provider (Feature-Scoped)

```tsx
// src/MentorSync.UI/src/features/onboarding/pages/OnboardingPage.tsx
const OnboardingPage: React.FC = () => {
	const { role } = useParams<{ role: string }>();

	return (
		<OnboardingProvider initialRole={role as "mentor" | "mentee"}>
			<OnboardingStepper />
			<OnboardingContent userRole={role as "mentor" | "mentee"} />
		</OnboardingProvider>
	);
};
```

## Key Patterns

1. **Context for Global State**: Only `AuthContext` and `OnboardingContext` are global
2. **Custom Hooks for Domain Logic**: Extract domain concerns into hooks (useUserProfile, useMaterials, etc.)
3. **Local State for UI**: useState for component-scoped UI state (dropdowns, modals, loading states)
4. **Memoization**: Use useMemo/useCallback to prevent unnecessary re-renders in complex components
5. **State Separation**: Separate concerns (filters vs pagination) to isolate side effects
6. **Error Handling**: Try-catch in hooks with toast notifications for user feedback
7. **Loading States**: Always have loading, error, and data states in data-fetching hooks

## File Organization

```
src/
├── features/
│   ├── auth/
│   │   └── context/
│   │       └── AuthContext.tsx      (Global auth state)
│   ├── onboarding/
│   │   └── context/
│   │       └── OnboardingContext.tsx (Feature-scoped onboarding state)
│   └── {domain}/
│       └── hooks/
│           └── use{Domain}.ts        (Domain-specific logic)
└── App.tsx                           (Provider hierarchy)
```
