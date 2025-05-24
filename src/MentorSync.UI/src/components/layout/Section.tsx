import React from "react";

interface SectionProps {
    children: React.ReactNode;
    className?: string;
    fullWidth?: boolean;
    spacing?: "sm" | "md" | "lg" | "xl" | "none";
    backgroundColor?: string;
}

const Section: React.FC<SectionProps> = ({
    children,
    className = "",
    fullWidth = false,
    spacing = "lg",
    backgroundColor = "",
}) => {
    const spacingClasses = {
        none: "py-0",
        sm: "py-8",
        md: "py-12",
        lg: "py-16",
        xl: "py-24",
    };

    return (
        <section
            className={`${className}`}
            style={{ backgroundColor: backgroundColor || "" }}
        >
            <div
                className={`${
                    fullWidth ? "" : "container mx-auto"
                } px-4 sm:px-8 md:px-16 ${spacingClasses[spacing]}`}
            >
                {children}
            </div>
        </section>
    );
};

export default Section;
