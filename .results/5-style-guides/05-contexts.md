# Contexts Style Guide

React Context is used only for truly global application state (authentication, onboarding, notifications).

## Location & Naming

-   **Location**: `src/features/{domain}/contexts/{ContextName}Context.tsx` (feature-scoped) OR `src/shared/contexts/{ContextName}Context.tsx` (global)
-   **Naming**: `{Name}Context` (e.g., `AuthContext`, `OnboardingContext`)
-   **Example**: `src/features/auth/contexts/AuthContext.tsx`

## Global Contexts Only

Only create contexts for:

-   Authentication (user, tokens, roles)
-   Onboarding (progress, steps)
-   Notifications (global toast/alert system)

For all other data, use custom hooks instead.

## Context Pattern

```tsx
import React, { createContext, useContext, useState, useEffect } from "react";

// 1. Define value type
interface AuthContextValue {
	user: User | null;
	isAuthenticated: boolean;
	isLoading: boolean;
	login: (email: string, password: string) => Promise<void>;
	logout: () => void;
	hasRole: (role: string) => boolean;
}

// 2. Create context (internal - not exported directly)
const AuthContext = createContext<AuthContextValue | undefined>(undefined);

// 3. Provider component
interface AuthProviderProps {
	children: React.ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
	const [user, setUser] = useState<User | null>(null);
	const [isLoading, setIsLoading] = useState(true);

	// Persistent auth on mount
	useEffect(() => {
		const initializeAuth = async () => {
			try {
				const token = localStorage.getItem("accessToken");
				if (token) {
					const userData = await authService.getCurrentUser();
					setUser(userData);
				}
			} catch (error) {
				console.error("Auth initialization failed:", error);
			} finally {
				setIsLoading(false);
			}
		};

		initializeAuth();
	}, []);

	const login = async (email: string, password: string) => {
		setIsLoading(true);
		try {
			const response = await authService.login(email, password);
			localStorage.setItem("accessToken", response.accessToken);
			localStorage.setItem("refreshToken", response.refreshToken);
			setUser(response.user);
		} finally {
			setIsLoading(false);
		}
	};

	const logout = () => {
		localStorage.removeItem("accessToken");
		localStorage.removeItem("refreshToken");
		setUser(null);
	};

	const hasRole = (role: string): boolean => {
		return user?.roles?.includes(role) ?? false;
	};

	const value: AuthContextValue = {
		user,
		isAuthenticated: user !== null,
		isLoading,
		login,
		logout,
		hasRole,
	};

	return (
		<AuthContext.Provider value={value}>{children}</AuthContext.Provider>
	);
};

// 4. Custom hook for consumption
export const useAuth = (): AuthContextValue => {
	const context = useContext(AuthContext);
	if (context === undefined) {
		throw new Error("useAuth must be used within an AuthProvider");
	}
	return context;
};
```

## Provider Registration

Register in `src/App.tsx`:

```tsx
export const App: React.FC = () => {
	return (
		<AuthProvider>
			<OnboardingProvider>
				<NotificationsProvider>
					<Router>
						<Routes>{/* Routes */}</Routes>
					</Router>
				</NotificationsProvider>
			</OnboardingProvider>
		</AuthProvider>
	);
};
```

## Type Safety

-   **Define interface** for context value
-   **Type context creation** explicitly: `createContext<ValueType | undefined>(undefined)`
-   **Error in hook** if context not provided: `throw new Error('Hook must be used within Provider')`
-   **Never return undefined** from hook; throw error instead

## Feature-Scoped Context Example

Use for complex feature state if absolutely necessary (prefer custom hooks):

```tsx
// src/features/onboarding/contexts/OnboardingContext.tsx

interface OnboardingStep {
	id: string;
	title: string;
	completed: boolean;
}

interface OnboardingContextValue {
	steps: OnboardingStep[];
	currentStepIndex: number;
	completeStep: (stepId: string) => void;
	goToStep: (stepIndex: number) => void;
	isComplete: boolean;
}

const OnboardingContext = createContext<OnboardingContextValue | undefined>(
	undefined
);

export const OnboardingProvider: React.FC<{ children: React.ReactNode }> = ({
	children,
}) => {
	const [steps, setSteps] = useState<OnboardingStep[]>([
		{ id: "profile", title: "Profile Setup", completed: false },
		{ id: "expertise", title: "Expertise Selection", completed: false },
		{ id: "availability", title: "Availability Setup", completed: false },
	]);
	const [currentStepIndex, setCurrentStepIndex] = useState(0);

	const completeStep = (stepId: string) => {
		setSteps(
			steps.map((step) =>
				step.id === stepId ? { ...step, completed: true } : step
			)
		);
	};

	const goToStep = (index: number) => {
		if (index >= 0 && index < steps.length) {
			setCurrentStepIndex(index);
		}
	};

	const isComplete = steps.every((step) => step.completed);

	return (
		<OnboardingContext.Provider
			value={{
				steps,
				currentStepIndex,
				completeStep,
				goToStep,
				isComplete,
			}}
		>
			{children}
		</OnboardingContext.Provider>
	);
};

export const useOnboarding = (): OnboardingContextValue => {
	const context = useContext(OnboardingContext);
	if (context === undefined) {
		throw new Error("useOnboarding must be used within OnboardingProvider");
	}
	return context;
};
```

## Notifications Context Pattern

```tsx
interface Notification {
	id: string;
	message: string;
	type: "success" | "error" | "warning" | "info";
}

interface NotificationsContextValue {
	notifications: Notification[];
	addNotification: (message: string, type: Notification["type"]) => void;
	removeNotification: (id: string) => void;
}

export const NotificationsProvider: React.FC<{ children: React.ReactNode }> = ({
	children,
}) => {
	const [notifications, setNotifications] = useState<Notification[]>([]);

	const addNotification = (message: string, type: Notification["type"]) => {
		const id = Math.random().toString(36).substr(2, 9);
		setNotifications((prev) => [...prev, { id, message, type }]);

		// Auto-remove after 3 seconds
		setTimeout(() => {
			removeNotification(id);
		}, 3000);
	};

	const removeNotification = (id: string) => {
		setNotifications((prev) => prev.filter((n) => n.id !== id));
	};

	return (
		<NotificationsContext.Provider
			value={{ notifications, addNotification, removeNotification }}
		>
			{children}
		</NotificationsContext.Provider>
	);
};
```

## Performance Considerations

-   **Separate contexts** for separate concerns (don't combine auth + onboarding + notifications)
-   **Update small values** when only that value changes
-   **Memoize context value**: Use `useMemo` to prevent provider re-renders:

    ```tsx
    const value = useMemo(
    	() => ({
    		user,
    		isAuthenticated,
    		login,
    		logout,
    		hasRole,
    	}),
    	[user, isAuthenticated, login, logout, hasRole]
    );

    return (
    	<AuthContext.Provider value={value}>{children}</AuthContext.Provider>
    );
    ```

## Testing

```tsx
// Test file: AuthContext.test.tsx
import { renderHook, act } from "@testing-library/react";
import { AuthProvider, useAuth } from "./AuthContext";

describe("AuthContext", () => {
	it("provides auth methods", () => {
		const wrapper = ({ children }: { children: React.ReactNode }) => (
			<AuthProvider>{children}</AuthProvider>
		);

		const { result } = renderHook(() => useAuth(), { wrapper });

		expect(result.current.isAuthenticated).toBe(false);
		expect(result.current.hasRole).toBeDefined();
		expect(result.current.login).toBeDefined();
	});
});
```

## Anti-Patterns

-   ❌ Creating context for feature-specific data (use custom hooks)
-   ❌ Combining unrelated concerns (auth + onboarding together)
-   ❌ Not memoizing context value (causes unnecessary re-renders)
-   ❌ Making consumers responsible for null-checking (throw error in hook)
-   ❌ Storing API responses in context (cache in hooks instead)
