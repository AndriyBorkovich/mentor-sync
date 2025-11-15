# Pages Style Guide

## MentorSync-Specific Page Patterns

### Page Structure

All pages in MentorSync follow a consistent three-part layout:

```tsx
const PageName: React.FC = () => {
	const [sidebarExpanded, setSidebarExpanded] = useState(false);

	return (
		<div className="min-h-screen flex bg-[#FFFFFF] overflow-hidden">
			{/* Sidebar Container */}
			<div
				className={`flex-shrink-0 transition-all duration-300 h-screen ${
					sidebarExpanded ? "w-[240px]" : "w-[72px]"
				}`}
			>
				<Sidebar onToggle={handleSidebarToggle} activePage="pageName" />
			</div>

			{/* Main Content Area */}
			<div className="flex-1 flex flex-col">
				<Header showNotifications={true} />
				<main className="flex-1 overflow-auto">
					{/* Page-specific content */}
				</main>
			</div>
		</div>
	);
};
```

### Authentication-Required Pages

All pages except Landing, Login, Register require authentication:

```tsx
// SessionsPage, MaterialsPage, ProfilePage, DashboardPage, etc.
const Page: React.FC = () => {
	const { isAuthenticated, isLoading } = useAuth();

	if (isLoading) {
		return <LoadingSpinner />;
	}

	if (!isAuthenticated) {
		return <Navigate to="/login" replace />;
	}

	// Page content
};
```

### Role-Based Page Routing

Pages requiring specific roles use RoleBasedRoute wrapper in routes.tsx:

```tsx
// Mentor-only pages
{
    path: "/mentor/availability",
    element: (
        <RoleBasedRoute allowedRoles={["Mentor"]} redirectTo="/unauthorized" />
    ),
    children: [{ path: "", element: <MyAvailabilityPage /> }],
}

// Admin-only pages
{
    path: "/settings",
    element: (
        <RoleBasedRoute allowedRoles={["Admin"]} redirectTo="/unauthorized" />
    ),
    children: [{ path: "", element: <SettingsPage /> }],
}
```

### Multi-Step Pages (Onboarding)

Onboarding wraps page with OnboardingProvider:

```tsx
const OnboardingPage: React.FC = () => {
	const { role } = useParams<{ role: string }>();

	return (
		<OnboardingProvider initialRole={role as "mentor" | "mentee"}>
			<div className="min-h-screen">
				<OnboardingStepper />
				<OnboardingContent userRole={role} />
			</div>
		</OnboardingProvider>
	);
};
```

## Key Characteristics

1. **Responsive Sidebar Toggle**: Sidebar transitions between collapsed (72px) and expanded (240px)
2. **Consistent Header**: All pages include Header with notifications
3. **Loading States**: Show spinner while auth/data is loading
4. **Error Redirects**: Unauthenticated users redirect to `/login`
5. **Tab-Based Navigation**: Some pages use tab switchers (materials, sessions)
6. **Modal Dialogs**: Use modals for uploads, confirmations, edits

## File Count: 16 total pages

Landing, Login, Register, Dashboard, Profile, MentorSearch, MentorProfile, MyAvailability, Sessions, Messages, Materials, MaterialView, MentorProfileEdit, MenteeProfileEdit, Settings, Onboarding
