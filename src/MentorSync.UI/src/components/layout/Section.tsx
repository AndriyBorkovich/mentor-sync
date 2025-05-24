import React from "react";

interface SectionProps {
    children: React.ReactNode;
    className?: string;
}

const Section: React.FC<SectionProps> = ({ children, className = "" }) => {
    return <section className={`px-16 py-12 ${className}`}>{children}</section>;
};

export default Section;
