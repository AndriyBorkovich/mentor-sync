# Authentication Domain Deep Dive

## Overview

Authentication covers JWT-based token management, role-based access control, OAuth2 provider integration, and session management using React Context API for global auth state.

## JWT Token Management

### Token Storage

```ts
// src/MentorSync.UI/src/features/auth/services/authStorage.ts
export interface AuthTokens {
	token: string;
	refreshToken?: string;
	expiration?: string;
	userId?: number;
	needOnboarding?: boolean;
}

export const AUTH_STORAGE_KEY = "mentorsync_auth";

export const getAuthTokens = (): AuthTokens | null => {
	try {
		const tokens = localStorage.getItem(AUTH_STORAGE_KEY);
		if (!tokens) return null;
		return JSON.parse(tokens);
	} catch (error) {
		console.error("Failed to parse auth tokens from localStorage", error);
		return null;
	}
};

export const saveAuthTokens = (tokens: AuthTokens): void => {
	localStorage.setItem(AUTH_STORAGE_KEY, JSON.stringify(tokens));
};

export const removeAuthTokens = (): void => {
	localStorage.removeItem(AUTH_STORAGE_KEY);
};
```

### Backend JWT Generation

```cs
// src/Modules/Users/MentorSync.Users/Infrastructure/JwtTokenService.cs
public sealed class JwtTokenService(
    IOptions<JwtOptions> jwtOptions,
    UserManager<AppUser> userManager)
    : IJwtTokenService
{
    public async Task<TokenResult> GenerateToken(AppUser user)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        var refreshToken = GenerateRefreshToken();
        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return new TokenResult(
            accessToken,
            refreshToken,
            tokenOptions.ValidTo);
    }

    private async Task<List<Claim>> GetClaims(AppUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Name, user.UserName!),
        };

        var roles = await userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
```

### Token Validation

```ts
// src/MentorSync.UI/src/features/auth/utils/authUtils.ts
export const isTokenValid = (): boolean => {
	try {
		const tokens = getAuthTokens();
		if (!tokens?.token) return false;

		const decodedToken = jwtDecode<JwtPayload>(tokens.token);
		if (!decodedToken.exp) return false;

		// Add 5-minute buffer to prevent edge cases
		const bufferMs = 5 * 60 * 1000;
		return decodedToken.exp * 1000 > Date.now() + bufferMs;
	} catch (error) {
		console.error("Error validating token:", error);
		return false;
	}
};

export const getTokenTimeRemaining = (): number | null => {
	try {
		const tokens = getAuthTokens();
		if (!tokens?.token) return null;

		const decodedToken = jwtDecode<JwtPayload>(tokens.token);
		if (!decodedToken.exp) return null;

		const remaining = decodedToken.exp * 1000 - Date.now();
		return remaining > 0 ? Math.floor(remaining / 1000) : 0;
	} catch (error) {
		console.error("Error calculating token time:", error);
		return null;
	}
};
```

## API Request Authentication

### Bearer Token Injection

```ts
// src/MentorSync.UI/src/shared/services/api.ts
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
		config.headers = config.headers || {};
		config.headers.Authorization = `Bearer ${authTokens.token}`;
	}

	return config;
});
```

### Error Handling

```ts
// Handle authentication errors (401)
if (error.response.status === 401) {
	console.warn("Authentication error: Token may have expired");
	removeAuthTokens();
	toast.error("Сесія закінчилася. Будь ласка, увійдіть знову.");
} else if (error.response.status === 403) {
	toast.error("У вас немає доступу до цього ресурсу.");
}
```

## Role-Based Access Control

### Role Checking Utility

```ts
// src/MentorSync.UI/src/features/auth/utils/authUtils.ts
export const hasRole = (role: string | string[]): boolean => {
	try {
		const user = getUserFromToken();
		const tokenRole = user?.[Claims.UserRole];
		if (!tokenRole) return false;

		const roles = Array.isArray(role) ? role : [role];
		return typeof tokenRole === "string" && roles.includes(tokenRole);
	} catch (error) {
		console.error("Error checking user role:", error);
		return false;
	}
};

export const getUserRole = (): string => {
	const user = getUserFromToken();
	const tokenRole = user?.[Claims.UserRole];
	return typeof tokenRole === "string"
		? tokenRole
		: localStorage.getItem("userRole") || "";
};
```

### Route Protection

```tsx
// src/MentorSync.UI/src/features/auth/components/ProtectedRoute.tsx
export const ProtectedRoute: React.FC = () => {
	const { isAuthenticated, isLoading } = useAuth();

	if (isLoading) {
		return <LoadingSpinner />;
	}

	if (!isAuthenticated) {
		return <Navigate to="/login" replace />;
	}

	return <Outlet />;
};

// src/MentorSync.UI/src/features/auth/components/RoleBasedRoute.tsx
export const RoleBasedRoute: React.FC<RoleBasedRouteProps> = ({
	allowedRoles,
	redirectTo = "/dashboard",
}) => {
	const { isAuthenticated, isLoading } = useAuth();
	const { profile, loading } = useUserProfile();

	if (isLoading || loading) {
		return <LoadingSpinner />;
	}

	if (!isAuthenticated) {
		return <Navigate to="/login" replace />;
	}

	if (profile && !allowedRoles.includes(profile.role)) {
		return <Navigate to={redirectTo} replace />;
	}

	return <Outlet />;
};
```

### Route Configuration

```tsx
// src/MentorSync.UI/src/routes.tsx
const router = createBrowserRouter([
	{
		path: "/mentor/availability",
		element: (
			<RoleBasedRoute
				allowedRoles={["Mentor"]}
				redirectTo="/unauthorized"
			/>
		),
		children: [{ path: "", element: <MyAvailabilityPage /> }],
	},
	{
		path: "/settings",
		element: (
			<RoleBasedRoute
				allowedRoles={["Admin"]}
				redirectTo="/unauthorized"
			/>
		),
		children: [{ path: "", element: <SettingsPage /> }],
	},
]);
```

## AuthContext for Global State

### Context Definition

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

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
	const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
	const [isLoading, setIsLoading] = useState<boolean>(true);
	const [needsOnboarding, setNeedsOnboarding] = useState<boolean>(false);
	const [user, setUser] = useState<User | null>(null);

	// Token validation on mount and token changes
	useEffect(() => {
		verifyAuth();
	}, []);

	return (
		<AuthContext.Provider value={{ isAuthenticated, isLoading, ...rest }}>
			{children}
		</AuthContext.Provider>
	);
};
```

### Token Refresh Flow

```tsx
const refreshAuthToken = useCallback(async (): Promise<boolean> => {
	try {
		const result = await authService.refreshToken();
		if (result.success && result.token) {
			setIsAuthenticated(true);
			if (result.needOnboarding !== undefined) {
				setNeedsOnboarding(result.needOnboarding);
			}
			return true;
		}
		setIsAuthenticated(false);
		return false;
	} catch (error) {
		console.error("Failed to refresh token:", error);
		setIsAuthenticated(false);
		return false;
	}
}, []);
```

## Login/Logout Flow

### Login Process

```tsx
// src/MentorSync.UI/src/features/auth/pages/LoginPage.tsx
const onSubmit: SubmitHandler<LoginRequest> = async (data) => {
	try {
		const response = await authService.login(data);

		if (response?.success) {
			loginUser(response);
			await verifyAuthStatus();

			if (response.needOnboarding) {
				navigate(`/onboarding/${userRole}`);
			} else {
				navigate(userRole === "mentor" ? "/sessions" : "/dashboard");
			}
		}
	} catch (error) {
		setErrorMessage(error.message);
	}
};
```

### Backend Login Handler

```cs
// src/Modules/Users/MentorSync.Users/Features/Login/LoginCommandHandler.cs
public async Task<Result<AuthResponse>> Handle(LoginCommand command, CancellationToken cancellationToken = default)
{
    var user = await userManager.FindByEmailAsync(command.Email);
    if (user is null) return Result.NotFound("User not found");

    var tokenResult = await jwtTokenService.GenerateToken(user);
    user.RefreshToken = tokenResult.RefreshToken;
    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(jwtOptions.Value.RefreshTokenExpirationInDays);
    await userManager.UpdateAsync(user);

    var needsOnboarding = /* check profile existence */;

    return Result.Success(new AuthResponse(
        tokenResult.AccessToken,
        tokenResult.RefreshToken,
        tokenResult.Expiration,
        needsOnboarding));
}
```

### Logout

```ts
export const authService = {
	logout: (): void => {
		removeAuthTokens();
		localStorage.removeItem("needOnboarding");
		localStorage.removeItem("userId");
		localStorage.removeItem("userRole");
	},
};
```

## Key Patterns

1. **Token Storage**: Tokens stored in localStorage with `mentorsync_auth` key
2. **Bearer Scheme**: Authorization header format: `Bearer {token}`
3. **Token Validation**: 5-minute buffer before token expiration
4. **Refresh Strategy**: Automatic refresh on 401 or manually via `refreshToken` method
5. **Role Check**: Always check role via `hasRole()` before conditionally rendering features
6. **Protected Routes**: All protected routes use `ProtectedRoute` or `RoleBasedRoute` wrappers

## Configuration

-   JWT Issuer/Audience: Defined in `appsettings.json`
-   Token Expiration: Configured in `JwtOptions`
-   Refresh Token Validity: Usually 7-30 days
