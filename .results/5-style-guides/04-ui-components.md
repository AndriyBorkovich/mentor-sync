# UI Components Style Guide

UI components are reusable, generic components used across multiple features (Button, Input, Modal, etc.).

## Location & Naming

-   **Location**: `src/components/ui/{ComponentName}.tsx`
-   **Naming**: Generic, not domain-specific (Button, Input, Card, Modal)
-   **Example**: `src/components/ui/Button.tsx`, `src/components/ui/Input.tsx`, `src/components/ui/Modal.tsx`

## Component Pattern

```tsx
import React, { forwardRef } from "react";

interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
	variant?: "primary" | "secondary" | "danger";
	size?: "sm" | "md" | "lg";
	isLoading?: boolean;
	fullWidth?: boolean;
}

export const Button = forwardRef<HTMLButtonElement, ButtonProps>(
	(
		{
			variant = "primary",
			size = "md",
			isLoading = false,
			fullWidth = false,
			children,
			disabled,
			className,
			...props
		},
		ref
	) => {
		const baseClasses =
			"font-semibold rounded transition-colors duration-200 flex items-center justify-center";

		const variantClasses = {
			primary:
				"bg-blue-500 text-white hover:bg-blue-600 disabled:bg-gray-300",
			secondary:
				"bg-gray-200 text-gray-900 hover:bg-gray-300 disabled:bg-gray-100",
			danger: "bg-red-500 text-white hover:bg-red-600 disabled:bg-red-300",
		}[variant];

		const sizeClasses = {
			sm: "px-3 py-1.5 text-sm",
			md: "px-4 py-2 text-base",
			lg: "px-6 py-3 text-lg",
		}[size];

		const widthClasses = fullWidth ? "w-full" : "";

		const disabledClass =
			disabled || isLoading
				? "opacity-50 cursor-not-allowed"
				: "cursor-pointer";

		return (
			<button
				ref={ref}
				disabled={disabled || isLoading}
				className={`${baseClasses} ${variantClasses} ${sizeClasses} ${widthClasses} ${disabledClass} ${
					className || ""
				}`}
				{...props}
			>
				{isLoading ? (
					<>
						<span className="animate-spin mr-2">⏳</span>
						Loading...
					</>
				) : (
					children
				)}
			</button>
		);
	}
);

Button.displayName = "Button";
```

## Requirements

-   **Extend native HTML elements**: Use `extends React.HTMLAttributes<HTMLElement>`
-   **Support forwardRef**: Allow parent components to access underlying DOM
-   **Variant pattern**: Support `variant` prop for different visual states
-   **Size prop**: Offer `sm`, `md`, `lg` size options
-   **No domain knowledge**: Generic components never know about app features
-   **Composable**: Can be combined with other UI components
-   **Accessible**: Support standard HTML attributes (disabled, aria-\*, etc.)

## Props Pattern

```tsx
interface ComponentProps extends React.HTMLAttributes<HTMLElement> {
	/** Variant determines styling */
	variant?: "primary" | "secondary";
	/** Size preset */
	size?: "sm" | "md" | "lg";
	/** Loading state with spinner */
	isLoading?: boolean;
	/** Fill container width */
	fullWidth?: boolean;
	/** Custom CSS classes (appended) */
	className?: string;
}
```

## TailwindCSS Styling

-   **Base classes**: Universal styling applied to all variants
-   **Variant classes**: Color/appearance per variant option
-   **Size classes**: Spacing/font-size per size option
-   **State classes**: Disabled, hover, focus, active states
-   **Responsive**: Use `sm:`, `md:`, `lg:` prefixes

## Composition Examples

```tsx
// Modal - used across features for dialogs
interface ModalProps {
	isOpen: boolean;
	onClose: () => void;
	title?: string;
	children: React.ReactNode;
}

export const Modal: React.FC<ModalProps> = ({
	isOpen,
	onClose,
	title,
	children,
}) => {
	if (!isOpen) return null;

	return (
		<div className="fixed inset-0 z-50 flex items-center justify-center">
			<div
				className="fixed inset-0 bg-black bg-opacity-50"
				onClick={onClose}
			/>
			<div className="relative bg-white rounded-lg shadow-xl max-w-md w-full mx-4">
				{title && (
					<h2 className="text-xl font-bold p-4 border-b">{title}</h2>
				)}
				<div className="p-4">{children}</div>
			</div>
		</div>
	);
};

// Card - generic container
interface CardProps {
	children: React.ReactNode;
	onClick?: () => void;
	hoverable?: boolean;
}

export const Card: React.FC<CardProps> = ({
	children,
	onClick,
	hoverable = false,
}) => {
	return (
		<div
			onClick={onClick}
			className={`
                bg-white rounded-lg border border-gray-200 p-4
                ${
					hoverable
						? "cursor-pointer hover:shadow-md transition-shadow"
						: ""
				}
                ${onClick ? "cursor-pointer" : ""}
            `}
		>
			{children}
		</div>
	);
};

// Input - text input with validation state
interface InputProps extends React.InputHTMLAttributes<HTMLInputElement> {
	label?: string;
	error?: string;
}

export const Input = forwardRef<HTMLInputElement, InputProps>(
	({ label, error, className, ...props }, ref) => {
		return (
			<div>
				{label && (
					<label className="block text-sm font-medium text-gray-700 mb-1">
						{label}
					</label>
				)}
				<input
					ref={ref}
					className={`
                        w-full px-3 py-2 border rounded-lg
                        ${error ? "border-red-500" : "border-gray-300"}
                        focus:outline-none focus:ring-2 focus:ring-blue-500
                        ${className || ""}
                    `}
					{...props}
				/>
				{error && <p className="text-red-500 text-sm mt-1">{error}</p>}
			</div>
		);
	}
);

Input.displayName = "Input";
```

## Export Pattern

Export all UI components from central index:

```tsx
// src/components/ui/index.ts
export { Button } from "./Button";
export { Input } from "./Input";
export { Modal } from "./Modal";
export { Card } from "./Card";

// Usage in feature components
import { Button, Input, Modal } from "@/components/ui";
```

## Accessibility Requirements

-   Support `aria-*` attributes
-   Keyboard navigation for interactive components
-   Focus states clearly visible
-   Semantic HTML (use `button` for buttons, `input` for inputs)
-   Color contrast ratios WCAG AA compliant
-   Labels associated with inputs via `htmlFor`

## Testing Snapshot Examples

```tsx
describe("Button", () => {
	it("renders primary variant", () => {
		render(<Button variant="primary">Click me</Button>);
		expect(screen.getByRole("button")).toHaveClass("bg-blue-500");
	});

	it("shows loading state", () => {
		render(<Button isLoading>Click me</Button>);
		expect(screen.getByText("Loading...")).toBeInTheDocument();
	});

	it("passes through HTML attributes", () => {
		render(<Button data-testid="custom-button">Click me</Button>);
		expect(screen.getByTestId("custom-button")).toBeInTheDocument();
	});
});
```

## Performance Notes

-   UI components are presentation-only (no hooks beyond forwardRef)
-   No external API calls or side effects
-   Minimal re-renders due to simple prop contracts
-   Composed with feature components for complex interactions
