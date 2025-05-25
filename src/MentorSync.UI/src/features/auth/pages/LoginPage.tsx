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
    const [redirectToOnboarding, setRedirectToOnboarding] =
        useState<boolean>(false);
    const [userRole, setUserRole] = useState<string>("");

    // Check if user was redirected from registration
    useEffect(() => {
        if (location.state?.registrationSuccess) {
            setShowSuccessMessage(true);
        }
        if (location.state?.redirectToOnboarding) {
            setRedirectToOnboarding(true);
            setUserRole(location.state.role);
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
                // Successful login - redirect to onboarding if coming from registration, otherwise to dashboard
                if (redirectToOnboarding && userRole) {
                    navigate(`/onboarding/${userRole}`, { replace: true });
                } else {
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
                        <div
                            className="w-8 h-8 bg-cover bg-center"
                            style={{ backgroundImage: "url('/logo.svg')" }}
                        ></div>
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
                        <div
                            className="mb-6 p-3 rounded-lg text-center"
                            style={{
                                backgroundColor: "#DCFCE7",
                                color: "#166534",
                            }}
                        >
                            Реєстрація успішна! Будь ласка, підтвердіть акаунт
                            на пошті і увійдіть до вашого акаунту.
                        </div>
                    )}

                    {errorMessage && (
                        <div
                            className="mb-6 p-3 rounded-lg text-center"
                            style={{
                                backgroundColor: "#FEE2E2",
                                color: "#B91C1C",
                            }}
                        >
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
                                className="block mb-2 font-medium text-sm"
                                style={{ color: "var(--color-text-gray)" }}
                            >
                                Електронна пошта
                            </label>
                            <input
                                id="email"
                                type="email"
                                {...register("email", {
                                    required: "Електронна пошта обов'язкова",
                                    pattern: {
                                        value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                                        message:
                                            "Неправильний формат електронної пошти",
                                    },
                                })}
                                className={`w-full p-3 border rounded-lg ${
                                    errors.email ? "border-red-500" : ""
                                }`}
                                style={{
                                    borderColor: errors.email
                                        ? "#EF4444"
                                        : "#E2E8F0",
                                }}
                                placeholder="Введіть вашу електронну пошту"
                            />
                            {errors.email && (
                                <p className="mt-1 text-sm text-red-500">
                                    {errors.email.message}
                                </p>
                            )}
                        </div>

                        <div>
                            <label
                                htmlFor="password"
                                className="block mb-2 font-medium text-sm"
                                style={{ color: "var(--color-text-gray)" }}
                            >
                                Пароль
                            </label>
                            <div className="relative">
                                <input
                                    id="password"
                                    type="password"
                                    {...register("password", {
                                        required: "Пароль обов'язковий",
                                    })}
                                    className={`w-full p-3 border rounded-lg ${
                                        errors.password ? "border-red-500" : ""
                                    }`}
                                    style={{
                                        borderColor: errors.password
                                            ? "#EF4444"
                                            : "#E2E8F0",
                                    }}
                                    placeholder="Введіть ваш пароль"
                                />
                                <button
                                    type="button"
                                    className="absolute right-3 top-1/2 transform -translate-y-1/2"
                                    onClick={() => {
                                        const passwordInput =
                                            document.getElementById(
                                                "password"
                                            ) as HTMLInputElement;
                                        if (passwordInput) {
                                            passwordInput.type =
                                                passwordInput.type ===
                                                "password"
                                                    ? "text"
                                                    : "password";
                                        }
                                    }}
                                >
                                    <svg
                                        width="20"
                                        height="20"
                                        viewBox="0 0 20 20"
                                        fill="none"
                                        xmlns="http://www.w3.org/2000/svg"
                                    >
                                        <path
                                            d="M2.5 10C2.5 10 5 5 10 5C15 5 17.5 10 17.5 10C17.5 10 15 15 10 15C5 15 2.5 10 2.5 10Z"
                                            stroke="currentColor"
                                            strokeWidth="1.5"
                                            strokeLinecap="round"
                                            strokeLinejoin="round"
                                        />
                                        <path
                                            d="M10 12.5C11.3807 12.5 12.5 11.3807 12.5 10C12.5 8.61929 11.3807 7.5 10 7.5C8.61929 7.5 7.5 8.61929 7.5 10C7.5 11.3807 8.61929 12.5 10 12.5Z"
                                            stroke="currentColor"
                                            strokeWidth="1.5"
                                            strokeLinecap="round"
                                            strokeLinejoin="round"
                                        />
                                    </svg>
                                </button>
                            </div>
                            {errors.password && (
                                <p className="mt-1 text-sm text-red-500">
                                    {errors.password.message}
                                </p>
                            )}
                            <div className="flex justify-end mt-1">
                                <Link
                                    to="/forgot-password"
                                    className="text-sm transition-colors"
                                    style={{ color: "var(--color-primary)" }}
                                >
                                    Забули пароль?
                                </Link>
                            </div>
                        </div>

                        <button
                            type="submit"
                            disabled={isSubmitting}
                            className="w-full py-3 rounded-lg transition-colors font-medium text-white"
                            style={{
                                backgroundColor: isSubmitting
                                    ? "var(--color-primary-light)"
                                    : "var(--color-primary)",
                                cursor: isSubmitting
                                    ? "not-allowed"
                                    : "pointer",
                            }}
                        >
                            {isSubmitting ? "Вхід..." : "Увійти"}
                        </button>
                    </form>

                    <div className="mt-8">
                        <div className="relative">
                            <div className="absolute inset-0 flex items-center">
                                <div
                                    className="w-full border-t"
                                    style={{ borderColor: "#E2E8F0" }}
                                ></div>
                            </div>
                            <div className="relative flex justify-center">
                                <span
                                    className="px-4 text-sm bg-white"
                                    style={{ color: "var(--color-text-gray)" }}
                                >
                                    Або продовжити з
                                </span>
                            </div>
                        </div>{" "}
                        <div className="mt-6 flex flex-col md:flex-row items-center justify-center gap-3">
                            <button
                                className="flex items-center justify-center gap-2 w-full md:w-auto px-4 py-2 border rounded-lg transition-colors"
                                style={{
                                    borderColor: "#E2E8F0",
                                    color: "var(--color-secondary)",
                                }}
                            >
                                <img
                                    src="/google-logo.svg"
                                    alt="Google"
                                    className="w-5 h-5"
                                />
                                Google
                            </button>
                            <button
                                className="flex items-center justify-center gap-2 w-full md:w-auto px-4 py-2 border rounded-lg transition-colors"
                                style={{
                                    borderColor: "#E2E8F0",
                                    color: "var(--color-secondary)",
                                }}
                            >
                                <img
                                    src="/github-logo.svg"
                                    alt="GitHub"
                                    className="w-5 h-5"
                                />
                                GitHub
                            </button>
                            <button
                                className="flex items-center justify-center gap-2 w-full md:w-auto px-4 py-2 border rounded-lg transition-colors"
                                style={{
                                    borderColor: "#E2E8F0",
                                    color: "var(--color-secondary)",
                                }}
                            >
                                <img
                                    src="/linkedin-logo.svg"
                                    alt="LinkedIn"
                                    className="w-5 h-5"
                                />
                                LinkedIn
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
