import React, { useEffect, useState } from "react";
import { useForm, SubmitHandler } from "react-hook-form";
import { Link, useNavigate, useLocation } from "react-router-dom";
import { authService, LoginRequest } from "../services/authService";

const LoginPage: React.FC = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [errorMessage, setErrorMessage] = useState<string | null>(null);
    const [showSuccessMessage, setShowSuccessMessage] =
        useState<boolean>(false);
    const [userRole, setUserRole] = useState<string>(""); // Check if user was redirected from registration
    useEffect(() => {
        if (location.state?.registrationSuccess) {
            setShowSuccessMessage(true);
        }
        if (location.state?.role) {
            setUserRole(location.state.role);
            // Store role in localStorage for use after login
            localStorage.setItem("userRole", location.state.role);
        }
        // If userId is passed from registration page, store it
        if (location.state?.userId) {
            localStorage.setItem("userId", location.state.userId.toString());
        }
    }, [location.state]);

    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<LoginRequest>();

    const onSubmit: SubmitHandler<LoginRequest> = async (data) => {
        setIsSubmitting(true);
        setErrorMessage(null);

        try {
            const response = await authService.login(data);
            if (response?.success) {
                // Check if user needs onboarding based on API response
                if (response.needOnboarding) {
                    // User needs onboarding - use the role from either registration state or from user's data
                    const userRoleToUse =
                        userRole ||
                        localStorage.getItem("userRole") ||
                        "mentee";
                    navigate(`/onboarding/${userRoleToUse}`, { replace: true });
                } else {
                    // User doesn't need onboarding - redirect to dashboard
                    navigate("/dashboard", { replace: true });
                }
            } else {
                setErrorMessage("Помилка входу. Невірний email або пароль.");
            }
        } catch (error) {
            setErrorMessage("Помилка зєднання з сервером. Спробуйте пізніше.");
            console.error("Login error:", error);
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <div className="min-h-screen flex flex-col md:flex-row justify-center items-center">
            <div
                className="flex flex-col items-center justify-center w-full md:w-1/2 px-6 py-12 mx-auto"
                style={{ backgroundColor: "var(--color-background)" }}
            >
                <div className="text-center mb-8">
                    <Link
                        to="/"
                        className="flex items-center justify-center gap-2 mb-6"
                    >
                        <h1
                            className="text-2xl font-bold"
                            style={{ color: "var(--color-secondary)" }}
                        >
                            MentorSync
                        </h1>
                    </Link>
                    <h2
                        className="text-3xl font-bold mb-2"
                        style={{ color: "var(--color-secondary)" }}
                    >
                        Увійти до акаунту
                    </h2>
                    <p
                        className="text-base"
                        style={{ color: "var(--color-text-gray)" }}
                    >
                        Продовжіть вашу мандрівку наставництва
                    </p>
                </div>{" "}
                <div
                    className="w-full max-w-md p-8 rounded-2xl shadow-md mx-auto"
                    style={{
                        backgroundColor: "white",
                        margin: "0 auto",
                        display: "block",
                    }}
                >
                    {showSuccessMessage && (
                        <div className="mb-4 p-3 bg-green-100 text-green-800 rounded-lg">
                            Реєстрація успішна! Тепер ви можете увійти в
                            систему.
                        </div>
                    )}

                    {errorMessage && (
                        <div className="mb-4 p-3 bg-red-100 text-red-800 rounded-lg">
                            {errorMessage}
                        </div>
                    )}

                    <form
                        onSubmit={handleSubmit(onSubmit)}
                        className="space-y-6"
                    >
                        <div>
                            <label
                                htmlFor="email"
                                className="block text-sm font-medium mb-1"
                                style={{ color: "var(--color-text-dark)" }}
                            >
                                Email
                            </label>
                            <input
                                id="email"
                                type="email"
                                {...register("email", {
                                    required: "Email обов'язковий",
                                    pattern: {
                                        value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                                        message: "Невірний формат email",
                                    },
                                })}
                                className={`w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 ${
                                    errors.email
                                        ? "border-red-300 focus:ring-red-200"
                                        : "border-gray-300 focus:ring-primary/20"
                                }`}
                                style={{ borderColor: "var(--color-border)" }}
                            />
                            {errors.email && (
                                <p className="mt-1 text-sm text-red-600">
                                    {errors.email.message}
                                </p>
                            )}
                        </div>

                        <div>
                            <div className="flex justify-between">
                                <label
                                    htmlFor="password"
                                    className="block text-sm font-medium mb-1"
                                    style={{ color: "var(--color-text-dark)" }}
                                >
                                    Пароль
                                </label>
                                <Link
                                    to="/forgot-password"
                                    className="text-sm transition-colors"
                                    style={{ color: "var(--color-primary)" }}
                                >
                                    Забули пароль?
                                </Link>
                            </div>

                            <input
                                id="password"
                                type="password"
                                {...register("password", {
                                    required: "Пароль обов'язковий",
                                    minLength: {
                                        value: 6,
                                        message:
                                            "Пароль має бути не менше 6 символів",
                                    },
                                })}
                                className={`w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 ${
                                    errors.password
                                        ? "border-red-300 focus:ring-red-200"
                                        : "border-gray-300 focus:ring-primary/20"
                                }`}
                                style={{ borderColor: "var(--color-border)" }}
                            />
                            {errors.password && (
                                <p className="mt-1 text-sm text-red-600">
                                    {errors.password.message}
                                </p>
                            )}
                        </div>

                        <button
                            type="submit"
                            disabled={isSubmitting}
                            className="w-full py-2 px-4 rounded-lg flex justify-center items-center transition-colors"
                            style={{
                                backgroundColor: "var(--color-primary)",
                                color: "white",
                            }}
                        >
                            {isSubmitting ? (
                                <div className="h-5 w-5 border-2 border-white border-t-transparent rounded-full animate-spin"></div>
                            ) : (
                                "Увійти"
                            )}
                        </button>
                    </form>

                    <div className="mt-8">
                        <p
                            className="text-center text-sm"
                            style={{ color: "var(--color-text-gray)" }}
                        >
                            Або увійти через
                        </p>
                        <div className="mt-3 grid grid-cols-1 gap-2">
                            <button
                                type="button"
                                className="flex items-center justify-center gap-2 w-full py-2 px-4 border rounded-lg transition-colors"
                                style={{ borderColor: "var(--color-border)" }}
                            >
                                <img
                                    src="/google-icon.svg"
                                    alt="Google"
                                    className="w-5 h-5"
                                />
                                <span
                                    style={{ color: "var(--color-text-dark)" }}
                                >
                                    Google
                                </span>
                            </button>
                        </div>
                    </div>
                </div>{" "}
                <div className="mt-8 flex gap-2 items-center justify-center">
                    <span style={{ color: "var(--color-text-gray)" }}>
                        Ще не маєте акаунту?
                    </span>
                    <Link
                        to="/register"
                        className="font-medium transition-colors"
                        style={{ color: "var(--color-primary)" }}
                    >
                        Створити
                    </Link>
                </div>
            </div>
        </div>
    );
};

export default LoginPage;
