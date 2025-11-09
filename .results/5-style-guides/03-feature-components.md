# Feature Components Style Guide

Feature components are domain-specific components used only within a particular feature. They are not reusable across other features.

## File Location & Naming

-   **Location**: `src/features/{domain}/components/{ComponentName}.tsx`
-   **Naming**: PascalCase matching the component name exactly
-   **Example**: `src/features/materials/components/MaterialCard.tsx`, `src/features/scheduling/components/SessionBookingForm.tsx`

## Component Structure

All feature components must follow this structure:

```tsx
// Imports: React first, then third-party, then internal
import React, { useCallback, useMemo } from "react";
import { useNavigate } from "react-router-dom";
import { Material } from "../types/Material";
import { useMaterials } from "../hooks/useMaterials";

// Interface definition: Props interface required
interface MaterialCardProps {
	material: Material;
	onSelect: (id: string) => void;
	isSelected?: boolean;
	loading?: boolean;
}

// Component definition: React.FC with explicit typing
export const MaterialCard: React.FC<MaterialCardProps> = ({
	material,
	onSelect,
	isSelected = false,
	loading = false,
}) => {
	// Hooks
	const navigate = useNavigate();

	// State
	const [isExpanded, setIsExpanded] = React.useState(false);

	// Computations with useMemo
	const displayTitle = useMemo(() => {
		return material.title.length > 50
			? `${material.title.substring(0, 50)}...`
			: material.title;
	}, [material.title]);

	// Handlers with useCallback
	const handleClick = useCallback(() => {
		onSelect(material.id);
	}, [material.id, onSelect]);

	const handleExpand = useCallback(() => {
		setIsExpanded(!isExpanded);
	}, [isExpanded]);

	// Render
	return (
		<div
			onClick={handleClick}
			className={`
                border rounded-lg p-4 cursor-pointer transition-all
                ${
					isSelected
						? "bg-blue-50 border-blue-500"
						: "bg-white border-gray-200"
				}
                ${
					loading
						? "opacity-50 pointer-events-none"
						: "hover:shadow-md"
				}
            `}
		>
			<div className="flex justify-between items-start">
				<h3 className="font-bold text-lg">{displayTitle}</h3>
				<button
					onClick={(e) => {
						e.stopPropagation();
						handleExpand();
					}}
					className="text-gray-400 hover:text-gray-600"
				>
					{isExpanded ? "−" : "+"}
				</button>
			</div>

			<p className="text-gray-600 text-sm mt-2">{material.description}</p>

			{isExpanded && (
				<div className="mt-4 pt-4 border-t border-gray-200">
					<p className="text-sm text-gray-700">
						Category: {material.category}
					</p>
					<p className="text-sm text-gray-700">
						Level: {material.level}
					</p>
				</div>
			)}
		</div>
	);
};
```

## Props Interface Requirements

-   **Required** for all feature components
-   **Named**: `{ComponentName}Props`
-   **Exported** (not inline)
-   **Include JSDoc** for complex props:
    ```tsx
    interface MaterialFilterProps {
    	/** Current filter criteria */
    	filters: MaterialFilter;
    	/** Callback when filters change */
    	onFilterChange: (filters: MaterialFilter) => void;
    	/** Whether filter panel is disabled */
    	disabled?: boolean;
    }
    ```

## Styling Rules

-   **TailwindCSS only**: All styling via `className`
-   **No CSS files**: Never create `.css` or `.module.css` files
-   **Dynamic classes**: Use template literals for conditional classes:
    ```tsx
    className={`
        base-classes
        ${condition ? 'active-classes' : 'inactive-classes'}
        ${state === 'loading' ? 'animate-pulse' : 'animate-none'}
    `}
    ```
-   **Responsive**: Mobile-first with `sm:`, `md:`, `lg:` prefixes
-   **Consistency**: Reference `tailwind.config.ts` for custom colors/spacing

## State Management

-   **Local state only**: Feature components use `useState` for UI concerns only
-   **Never access feature hooks**: Don't call custom domain hooks from components
-   **Pass callbacks as props**: All data updates via `onXXX` callbacks
-   **Lift state**: Complex state belongs in pages/container components

Example of wrong approach:

```tsx
// ❌ WRONG: Component directly calls hook
export const MaterialList: React.FC = () => {
	const { data } = useMaterials(); // Should be passed as prop
	return (
		<div>
			{data.map((m) => (
				<MaterialCard key={m.id} material={m} />
			))}
		</div>
	);
};

// ✅ CORRECT: Data passed from parent
interface MaterialListProps {
	materials: Material[];
	onMaterialSelect: (id: string) => void;
}
export const MaterialList: React.FC<MaterialListProps> = ({
	materials,
	onMaterialSelect,
}) => {
	return (
		<div>
			{materials.map((m) => (
				<MaterialCard
					key={m.id}
					material={m}
					onSelect={onMaterialSelect}
				/>
			))}
		</div>
	);
};
```

## Event Handlers

-   **useCallback required** for all handlers passed as props:
    ```tsx
    const handleClick = useCallback(() => {
    	onSelect(material.id);
    }, [material.id, onSelect]);
    ```
-   **Dependency arrays strict**: Include all dependencies to avoid stale closures
-   **Event propagation**: Stop propagation when needed: `e.stopPropagation()`
-   **Keyboard support**: Include `onKeyDown` handlers for accessibility when appropriate

## Composition Pattern

Break large components into smaller sub-components:

```tsx
// Parent component
export const MaterialDetailPanel: React.FC<MaterialDetailPanelProps> = ({
	material,
	onEdit,
}) => {
	return (
		<div className="p-4 border rounded">
			<MaterialHeader material={material} />
			<MaterialContent material={material} />
			<MaterialActions material={material} onEdit={onEdit} />
		</div>
	);
};

// Child components (same file, not exported unless needed elsewhere)
const MaterialHeader: React.FC<{ material: Material }> = ({ material }) => (
	<h2 className="font-bold text-2xl mb-2">{material.title}</h2>
);

const MaterialContent: React.FC<{ material: Material }> = ({ material }) => (
	<p className="text-gray-700">{material.description}</p>
);

const MaterialActions: React.FC<{
	material: Material;
	onEdit: (id: string) => void;
}> = ({ material, onEdit }) => (
	<button
		onClick={() => onEdit(material.id)}
		className="mt-4 px-4 py-2 bg-blue-500 text-white rounded"
	>
		Edit
	</button>
);
```

## Error Handling

-   **Error state as prop**: Components receive error state from parent
-   **Graceful degradation**: Show fallback UI if required data missing
-   **User-friendly messages**: Never show technical error details

```tsx
interface ItemListProps {
	items: Item[];
	loading: boolean;
	error: string | null;
}

export const ItemList: React.FC<ItemListProps> = ({
	items,
	loading,
	error,
}) => {
	if (loading) return <LoadingSpinner />;
	if (error) return <ErrorAlert message={error} />;
	if (items.length === 0) return <EmptyState message="No items found" />;

	return (
		<div>
			{items.map((item) => (
				<ItemCard key={item.id} item={item} />
			))}
		</div>
	);
};
```

## Testing Considerations

-   **Minimize dependencies**: Easier to test when props are simple/serializable
-   **Callbacks testable**: Mock `onXXX` callbacks in tests
-   **No side effects**: Don't fetch data or subscribe to effects in feature components

Example test:

```tsx
describe("MaterialCard", () => {
	it("calls onSelect when clicked", () => {
		const mockOnSelect = jest.fn();
		const material: Material = {
			id: "1",
			title: "Test",
			description: "Test",
			category: "General",
			level: 1,
		};

		render(<MaterialCard material={material} onSelect={mockOnSelect} />);

		fireEvent.click(screen.getByText("Test"));
		expect(mockOnSelect).toHaveBeenCalledWith("1");
	});
});
```

## Performance Checklist

-   ☑️ All event handlers wrapped with `useCallback`
-   ☑️ Expensive computations wrapped with `useMemo`
-   ☑️ Props interface properly typed
-   ☑️ No unnecessary re-renders (check React DevTools Profiler)
-   ☑️ No dynamic object/array literals in JSX (pass as props)
-   ☑️ Dependencies arrays complete and accurate
