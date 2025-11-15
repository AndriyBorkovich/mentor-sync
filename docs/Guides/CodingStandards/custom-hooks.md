# Hooks Style Guide

## MentorSync-Specific Hook Patterns

### Data-Fetching Hooks Pattern

All data-fetching hooks in MentorSync follow consistent pattern:

```ts
export function useDomain() {
	const [data, setData] = useState<DataType[]>([]);
	const [loading, setLoading] = useState<boolean>(true);
	const [error, setError] = useState<string | null>(null);

	useEffect(() => {
		const fetchData = async () => {
			try {
				setLoading(true);
				const response = await fetchFromAPI();
				setData(response);
				setError(null);
			} catch (err) {
				console.error("Failed to fetch:", err);
				setError("Failed to load data");
				setData([]);
			} finally {
				setLoading(false);
			}
		};

		fetchData();
	}, [dependencies]);

	return { data, loading, error };
}
```

### useUserProfile Hook

```ts
// src/features/auth/hooks/useUserProfile.ts
export const useUserProfile = () => {
	const { isAuthenticated } = useAuth();
	const [profile, setProfile] = useState<UserProfile | null>(null);
	const [loading, setLoading] = useState<boolean>(true);

	useEffect(() => {
		if (!isAuthenticated) {
			setProfile(null);
			setLoading(false);
			return;
		}

		const fetch = async () => {
			try {
				setLoading(true);
				const response = await api.get("/users/profile");
				setProfile(response.data);
			} catch (err) {
				console.error("Failed to fetch profile:", err);
				setProfile(null);
			} finally {
				setLoading(false);
			}
		};

		fetch();
	}, [isAuthenticated]);

	return { profile, loading };
};
```

### useUserManagement Hook (Admin)

```ts
// src/features/settings/hooks/useUserManagement.ts
export function useUserManagement() {
	const [users, setUsers] = useState<UserShortResponse[]>([]);
	const [loading, setLoading] = useState<boolean>(true);
	const [filters, setFilters] = useState<UserFilterParams>({});

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

	useEffect(() => {
		loadUsers();
	}, [loadUsers]);

	return { users, loading, filters, setFilters, loadUsers };
}
```

## Key Characteristics

1. **Naming Convention**: Always prefix with `use` (useUser, useMaterials, useNotifications)
2. **State Pattern**: [data, loading, error, ...operations]
3. **Error Handling**: Try-catch with user-friendly error messages
4. **Dependency Arrays**: Always explicitly list dependencies to prevent stale closures
5. **useCallback**: Use for memoized callbacks that refetch data
6. **Early Returns**: Check auth status and return early if not authenticated
7. **Finally Block**: Always clean up loading state in finally

## File Count: 7 total hooks

useUserProfile, usePersistentAuth, useMentorProfile, useMaterials, useChat, useUserManagement, useOnboarding
