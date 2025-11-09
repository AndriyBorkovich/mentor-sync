# Layout Components Style Guide

Layout components (Sidebar, Header, Footer) structure page containers and are reused across multiple pages.

## Location

-   **Location**: `src/components/layout/{ComponentName}.tsx`
-   **Examples**: `Sidebar.tsx`, `Header.tsx`, `Footer.tsx`, `Navbar.tsx`

## Sidebar Pattern

```tsx
export const Sidebar: React.FC = () => {
	const [isCollapsed, setIsCollapsed] = useState(false);
	const { isAuthenticated, user, hasRole, logout } = useAuth();

	if (!isAuthenticated) return null;

	return (
		<aside
			className={`
                fixed left-0 top-0 h-screen transition-all duration-300 bg-white border-r border-gray-200
                ${isCollapsed ? "w-[72px]" : "w-[240px]"}
            `}
		>
			{/* Logo/Toggle */}
			<div className="p-4 flex justify-between items-center">
				{!isCollapsed && <h1 className="font-bold">MentorSync</h1>}
				<button onClick={() => setIsCollapsed(!isCollapsed)}>
					{isCollapsed ? "→" : "←"}
				</button>
			</div>

			{/* Navigation */}
			<nav className="mt-4 space-y-2">
				<NavLink
					to="/dashboard"
					icon="📊"
					label="Dashboard"
					collapsed={isCollapsed}
				/>
				<NavLink
					to="/materials"
					icon="📚"
					label="Materials"
					collapsed={isCollapsed}
				/>
				{hasRole("Mentor") && (
					<NavLink
						to="/sessions"
						icon="📅"
						label="Sessions"
						collapsed={isCollapsed}
					/>
				)}
				{hasRole("Admin") && (
					<NavLink
						to="/admin"
						icon="⚙️"
						label="Admin"
						collapsed={isCollapsed}
					/>
				)}
			</nav>

			{/* User Menu */}
			<div className="absolute bottom-0 left-0 right-0 p-4 border-t">
				{!isCollapsed && (
					<div className="text-sm">
						<p className="font-semibold">{user?.firstName}</p>
						<button
							onClick={logout}
							className="text-blue-500 hover:underline text-xs"
						>
							Logout
						</button>
					</div>
				)}
			</div>
		</aside>
	);
};

const NavLink: React.FC<{
	to: string;
	icon: string;
	label: string;
	collapsed: boolean;
}> = ({ to, icon, label, collapsed }) => {
	const location = useLocation();
	const isActive = location.pathname === to;

	return (
		<Link
			to={to}
			className={`
                flex items-center gap-3 px-4 py-2 rounded transition-colors
                ${
					isActive
						? "bg-blue-50 text-blue-600"
						: "text-gray-700 hover:bg-gray-50"
				}
                ${collapsed ? "justify-center" : ""}
            `}
		>
			<span>{icon}</span>
			{!collapsed && <span>{label}</span>}
		</Link>
	);
};
```

## Header Pattern

```tsx
export const Header: React.FC = () => {
	const { user } = useAuth();
	const [showNotifications, setShowNotifications] = useState(false);
	const notifications = useNotifications(); // From context

	return (
		<header className="bg-white border-b border-gray-200 px-8 py-4 flex justify-between items-center">
			<div className="flex-1">
				<h1 className="text-2xl font-bold">Dashboard</h1>
			</div>

			<div className="flex items-center gap-4">
				{/* Notifications */}
				<button
					onClick={() => setShowNotifications(!showNotifications)}
					className="relative p-2 hover:bg-gray-100 rounded"
				>
					🔔
					{notifications.length > 0 && (
						<span className="absolute top-0 right-0 bg-red-500 text-white text-xs rounded-full w-5 h-5">
							{notifications.length}
						</span>
					)}
				</button>

				{/* User Dropdown */}
				<UserDropdown user={user} />
			</div>

			{/* Notifications Panel */}
			{showNotifications && (
				<div className="absolute top-16 right-8 bg-white border rounded shadow-lg w-80">
					{notifications.length > 0 ? (
						notifications.map((n) => (
							<div key={n.id} className="p-3 border-b">
								<p className="text-sm">{n.message}</p>
							</div>
						))
					) : (
						<p className="p-3 text-gray-500">No notifications</p>
					)}
				</div>
			)}
		</header>
	);
};
```

## Footer Pattern

```tsx
export const Footer: React.FC = () => {
	return (
		<footer className="bg-gray-900 text-white mt-12">
			<div className="max-w-7xl mx-auto px-8 py-12">
				<div className="grid grid-cols-4 gap-8 mb-8">
					<div>
						<h3 className="font-bold mb-4">Product</h3>
						<ul className="space-y-2 text-sm text-gray-300">
							<li>
								<a href="#" className="hover:text-white">
									Features
								</a>
							</li>
							<li>
								<a href="#" className="hover:text-white">
									Pricing
								</a>
							</li>
						</ul>
					</div>
					{/* More columns... */}
				</div>

				<div className="border-t border-gray-700 pt-8 flex justify-between items-center">
					<p className="text-sm text-gray-400">
						&copy; 2024 MentorSync
					</p>
					<div className="flex gap-4 text-sm text-gray-400">
						<a href="#" className="hover:text-white">
							Privacy
						</a>
						<a href="#" className="hover:text-white">
							Terms
						</a>
					</div>
				</div>
			</div>
		</footer>
	);
};
```

## Main Layout Container

```tsx
interface LayoutProps {
	children: React.ReactNode;
}

export const MainLayout: React.FC<LayoutProps> = ({ children }) => {
	const { isAuthenticated } = useAuth();

	if (!isAuthenticated) return <>{children}</>;

	return (
		<div className="flex h-screen bg-gray-50">
			<Sidebar />
			<div className="flex-1 flex flex-col ml-[240px]">
				<Header />
				<main className="flex-1 overflow-auto p-8">{children}</main>
				<Footer />
			</div>
		</div>
	);
};
```

## Responsive Considerations

-   Sidebar collapses on mobile
-   Header adapts to smaller screens
-   Navigation uses hamburger menu on mobile
-   Footer stacks vertically on small screens

## Best Practices

-   ✅ Keep layout components presentation-only
-   ✅ Extract complex nested components
-   ✅ Use context for global state (auth, theme)
-   ✅ Support responsive design with TailwindCSS
-   ✅ Memoize with React.memo if no prop changes
-   ❌ Don't fetch data in layout components
-   ❌ Don't use layout-specific hooks
-   ❌ Don't mix business logic with layout
