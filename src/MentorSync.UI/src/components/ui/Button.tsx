import React from "react";

interface ButtonProps {
    children: React.ReactNode;
    variant?: "primary" | "outline";
    className?: string;
    onClick?: () => void;
    size?: "sm" | "md" | "lg";
    style?: React.CSSProperties;
    onMouseOver?: (e: React.MouseEvent<HTMLButtonElement>) => void;
    onMouseOut?: (e: React.MouseEvent<HTMLButtonElement>) => void;
}

const Button: React.FC<ButtonProps> = ({
    children,
    variant = "primary",
    className = "",
    onClick,
    size = "md",
    style = {},
    onMouseOver,
    onMouseOut,
}) => {
    const sizeStyles = {
        sm: "py-2 px-4 text-sm",
        md: "py-3 px-5 text-base",
        lg: "py-4 px-6 text-lg",
    };

    const baseStyles = `${sizeStyles[size]} rounded-xl font-medium transition-all shadow-sm`;
    const variantStyles = {
        primary: "text-white focus:outline-none",
        outline: "border-2 focus:outline-none",
    };

    // Use inline style with CSS variables for colors
    const defaultStyle =
        variant === "primary"
            ? {
                  backgroundColor: "var(--color-primary)",
              }
            : {
                  borderColor: "var(--color-primary)",
                  color: "var(--color-primary)",
              };

    // Merge default styles with any custom styles passed in
    const buttonStyle = { ...defaultStyle, ...style };
    return (
        <button
            className={`${baseStyles} ${variantStyles[variant]} ${className}`}
            onClick={onClick}
            style={buttonStyle}
            onMouseOver={(e) => {
                if (onMouseOver) {
                    onMouseOver(e);
                } else {
                    if (variant === "primary") {
                        e.currentTarget.style.backgroundColor =
                            "var(--color-primary-dark)";
                    } else {
                        e.currentTarget.style.backgroundColor =
                            "var(--color-primary-light)";
                    }
                }
            }}
            onMouseOut={(e) => {
                if (onMouseOut) {
                    onMouseOut(e);
                } else {
                    if (variant === "primary") {
                        e.currentTarget.style.backgroundColor =
                            "var(--color-primary)";
                    } else {
                        e.currentTarget.style.backgroundColor = "";
                    }
                }
            }}
        >
            {children}
        </button>
    );
};

export default Button;
