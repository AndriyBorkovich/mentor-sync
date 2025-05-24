import React, { useState } from "react";
import { Link } from "react-router-dom";

const Navbar: React.FC = () => {
    const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);

    return (
        <nav
            className="flex justify-between items-center px-8 md:px-16 py-6 sticky top-0 backdrop-blur-sm z-50 shadow-sm"
            style={{
                display: "flex",
                justifyContent: "space-between",
                alignItems: "center",
                padding: "1.5rem 2rem",
                paddingLeft: "2rem",
                paddingRight: "2rem",
                position: "sticky",
                top: 0,
                backgroundColor: "rgba(248, 250, 252, 0.95)",
                backdropFilter: "blur(8px)",
                zIndex: 50,
                boxShadow: "0 1px 3px rgba(0, 0, 0, 0.05)",
            }}
        >
            <div
                className="font-bold text-2xl"
                style={{
                    fontWeight: "bold",
                    fontSize: "1.5rem",
                    color: "var(--color-secondary)",
                }}
            >
                MentorSync
            </div>
            {/* Mobile menu button */}
            <button
                className="md:hidden"
                onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
                aria-label="Toggle menu"
                style={{
                    display: "block",
                }}
            >
                <svg
                    style={{
                        width: "1.5rem",
                        height: "1.5rem",
                        color: "var(--color-secondary)",
                    }}
                    fill="none"
                    stroke="currentColor"
                    viewBox="0 0 24 24"
                >
                    {isMobileMenuOpen ? (
                        <path
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            strokeWidth="2"
                            d="M6 18L18 6M6 6l12 12"
                        />
                    ) : (
                        <path
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            strokeWidth="2"
                            d="M4 6h16M4 12h16M4 18h16"
                        />
                    )}
                </svg>
            </button>{" "}
            {/* Desktop menu */}{" "}
            <div
                className="hidden md:flex items-center space-x-6"
                style={{
                    alignItems: "center",
                    gap: "1.5rem",
                }}
            >
                <Link
                    to="/login"
                    style={{
                        color: "var(--color-text-gray)",
                        fontWeight: 500,
                        transition: "color 0.15s ease-in-out",
                    }}
                    onMouseOver={(e) => {
                        e.currentTarget.style.color = "var(--color-secondary)";
                    }}
                    onMouseOut={(e) => {
                        e.currentTarget.style.color = "var(--color-text-gray)";
                    }}
                >
                    Увійти
                </Link>
                <Link
                    to="/register"
                    style={{
                        backgroundColor: "var(--color-primary)",
                        color: "white",
                        padding: "0.5rem 1.25rem",
                        borderRadius: "0.75rem",
                        boxShadow: "0 1px 2px rgba(0, 0, 0, 0.05)",
                        transition: "all 0.2s ease",
                    }}
                    onMouseOver={(e) => {
                        e.currentTarget.style.backgroundColor =
                            "var(--color-primary-dark)";
                    }}
                    onMouseOut={(e) => {
                        e.currentTarget.style.backgroundColor =
                            "var(--color-primary)";
                    }}
                >
                    Зареєструватися
                </Link>
            </div>
            {/* Mobile menu dropdown */}
            {isMobileMenuOpen && (
                <div
                    className="md:hidden"
                    style={{
                        position: "absolute",
                        top: "100%",
                        left: 0,
                        right: 0,
                        backgroundColor: "white",
                        boxShadow: "0 10px 15px -3px rgba(0, 0, 0, 0.1)",
                        borderRadius: "0 0 0.75rem 0.75rem",
                        padding: "1rem",
                        borderTop: "1px solid #f3f4f6",
                    }}
                >
                    <div
                        style={{
                            display: "flex",
                            flexDirection: "column",
                            gap: "1rem",
                        }}
                    >
                        <Link
                            to="/login"
                            style={{
                                color: "var(--color-text-gray)",
                                fontWeight: 500,
                                padding: "0.5rem 0",
                                transition: "color 0.15s ease-in-out",
                            }}
                            onClick={() => setIsMobileMenuOpen(false)}
                            onMouseOver={(e) => {
                                e.currentTarget.style.color =
                                    "var(--color-secondary)";
                            }}
                            onMouseOut={(e) => {
                                e.currentTarget.style.color =
                                    "var(--color-text-gray)";
                            }}
                        >
                            Увійти
                        </Link>
                        <Link
                            to="/register"
                            style={{
                                backgroundColor: "var(--color-primary)",
                                color: "white",
                                padding: "0.75rem 1.25rem",
                                borderRadius: "0.75rem",
                                boxShadow: "0 1px 2px rgba(0, 0, 0, 0.05)",
                                textAlign: "center",
                                transition: "all 0.2s ease",
                            }}
                            onClick={() => setIsMobileMenuOpen(false)}
                            onMouseOver={(e) => {
                                e.currentTarget.style.backgroundColor =
                                    "var(--color-primary-dark)";
                            }}
                            onMouseOut={(e) => {
                                e.currentTarget.style.backgroundColor =
                                    "var(--color-primary)";
                            }}
                        >
                            Зареєструватися
                        </Link>
                    </div>
                </div>
            )}
        </nav>
    );
};

export default Navbar;
