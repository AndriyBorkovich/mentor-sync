# UI Domain Deep Dive

## Overview

The UI domain covers all React component development patterns, including functional components, hooks usage, styling with TailwindCSS, and component composition patterns across the MentorSync frontend.

## Component Architecture

### Functional Component Pattern

All components use `React.FC<Props>` pattern with TypeScript interfaces:

```tsx
// src/MentorSync.UI/src/components/layout/Navbar.tsx
interface NavbarProps {
	// Props definition
}

const Navbar: React.FC<NavbarProps> = () => {
	return <nav>{/* Component JSX */}</nav>;
};

export default Navbar;
```

### Hooks for Local State Management

**useState Pattern:**

```tsx
// src/MentorSync.UI/src/features/sessions/components/SessionsContent.tsx
const [bookings, setBookings] = useState<BookingSession[]>([]);
const [loading, setLoading] = useState<boolean>(true);
const [filterType, setFilterType] = useState<FilterType>("all");
const [showFilterDropdown, setShowFilterDropdown] = useState(false);
```

**useCallback for Function Memoization:**

```tsx
// src/MentorSync.UI/src/features/settings/hooks/useUserManagement.ts
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
```

**useMemo for Expensive Computations:**

```tsx
// src/MentorSync.UI/src/features/materials/pages/MaterialsPage.tsx
const combinedFilters = useMemo(
	() => ({
		...filters,
		...pagination,
	}),
	[filters, pagination]
);

const mapAPIToUIMaterials = useCallback(
	(item: any) => ({
		id: item.id.toString(),
		title: item.title,
		description: item.description,
		type: mapMaterialType(item.type),
		mentorName: item.mentorName || "Unknown Mentor",
		// ... more mappings
	}),
	[]
);
```

### Layout Composition

**Standard Page Layout:**

```tsx
// src/MentorSync.UI/src/features/sessions/pages/SessionsPage.tsx
const SessionsPage: React.FC = () => {
	const [sidebarExpanded, setSidebarExpanded] = useState(false);

	return (
		<div className="min-h-screen flex bg-[#FFFFFF] overflow-hidden">
			<div
				className={`flex-shrink-0 transition-all duration-300 h-screen ${
					sidebarExpanded ? "w-[240px]" : "w-[72px]"
				}`}
			>
				<Sidebar onToggle={handleSidebarToggle} activePage="sessions" />
			</div>
			<div className="flex-1 flex flex-col">
				<Header showNotifications={true} />
				<SessionsContent />
			</div>
		</div>
	);
};
```

**Landing Page Layout:**

```tsx
// src/MentorSync.UI/src/features/landing/pages/LandingPage.tsx
const LandingPage: React.FC = () => {
	return (
		<div className="bg-background min-h-screen flex flex-col">
			<Navbar />
			<main>
				<Section spacing="xl">
					<HeroSection />
				</Section>
				<Section className="bg-white" spacing="md">
					<SocialProofSection />
				</Section>
				{/* More sections */}
			</main>
			<Footer />
		</div>
	);
};
```

## TailwindCSS Styling

### Utility-First Approach

All styling done through Tailwind utility classes:

```tsx
<nav
    className="flex justify-between items-center px-8 md:px-16 py-6 sticky top-0 backdrop-blur-sm z-50 shadow-sm"
    style={{
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
        padding: "1.5rem 2rem",
        position: "sticky",
        top: 0,
        backgroundColor: "rgba(248, 250, 252, 0.95)",
        backdropFilter: "blur(8px)",
        zIndex: 50,
        boxShadow: "0 1px 3px rgba(0, 0, 0, 0.05)",
    }}
>
```

### Design Token Usage

-   Primary color: `#4318D1` (Purple)
-   Background color: `#F8FAFC` (Light)
-   Text color: `#1E293B` (Dark)
-   Spacing: Multiples of 4px (e.g., `p-4`, `mb-2`)

## Component Categories

### Pages (16 total)

-   LandingPage, LoginPage, RegisterPage
-   DashboardPage, ProfilePage
-   MentorSearchPage, MentorProfilePage, MyAvailabilityPage
-   SessionsPage, MessagesPage, MaterialsPage, MaterialViewPage
-   OnboardingPage, SettingsPage

### Feature Components (30+ total)

Located in `src/features/{domain}/components/`:

-   Mentor profile components (MentorProfileContainer, MentorProfileTabs, etc.)
-   Materials (MaterialsContent, MaterialCard, MaterialViewContent, MaterialFilter)
-   Scheduling (AvailabilityManagement, CalendarView, TimePickerField)
-   Onboarding (OnboardingContent, OnboardingStepper, Step1-5 components)

### UI Components (8 total)

Located in `src/components/ui/`:

-   Button, Input, Modal dialogs
-   UserDropdown, NotificationsDropdown
-   Cards and form elements

### Layout Components (5 total)

Located in `src/components/layout/`:

-   Sidebar, Header, Footer
-   Navbar, Section wrapper

## Performance Optimizations

### Memoization Strategy

```tsx
// Separate state for filters and pagination to avoid unnecessary re-renders
const [filters, setFilters] = useState({
	search: "",
	types: [],
	tags: [],
	sortBy: "newest",
});
const [pagination, setPagination] = useState({ pageNumber: 1, pageSize: 12 });

// Memoize combined object for prop passing
const combinedFilters = useMemo(
	() => ({ ...filters, ...pagination }),
	[filters, pagination]
);

// useCallback distinguishes filter changes from pagination changes
const handleFilterChange = useCallback((newFilter) => {
	setFilters(newFilter);
	setPagination({ pageNumber: 1, pageSize: 12 }); // Reset pagination on filter change
}, []);
```

### Lazy Loading Routes

```tsx
// src/MentorSync.UI/src/routes.tsx
import { lazy, Suspense } from "react";

const routes = [
	// Lazy-loaded pages with Suspense fallback
];
```

## Key Patterns

1. **State Isolation**: Separate concerns (filters vs pagination) into different state objects
2. **Type Safety**: All components have TypeScript Props interfaces
3. **Error Boundaries**: Graceful error handling with user-friendly toast notifications
4. **Controlled Components**: Form inputs are controlled with state
5. **Accessibility**: ARIA labels and semantic HTML where applicable

## File Structure

```
src/MentorSync.UI/src/
├── components/
│   ├── ui/              (8 UI components)
│   └── layout/          (5 layout components)
├── features/
│   └── {domain}/
│       └── components/  (Feature-specific components)
├── pages/               (15+ page components)
└── shared/
    └── services/        (API layer)
```
