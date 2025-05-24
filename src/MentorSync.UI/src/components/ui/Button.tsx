import React from "react";

interface ButtonProps {
    children: React.ReactNode;
    variant?: "primary" | "outline";
    className?: string;
    onClick?: () => void;
}

const Button: React.FC<ButtonProps> = ({
    children,
    variant = "primary",
    className = "",
    onClick,
}) => {
    const baseStyles =
        "py-4 px-6 rounded-xl text-lg font-medium transition-all";

    const variantStyles = {
        primary: "bg-primary text-white hover:bg-primary/90",
        outline: "border-2 border-primary text-primary hover:bg-primary/10",
    };

    return (
        <button
            className={`${baseStyles} ${variantStyles[variant]} ${className}`}
            onClick={onClick}
        >
            {children}
        </button>
    );
};

export default Button;
