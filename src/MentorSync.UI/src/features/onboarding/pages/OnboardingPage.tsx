import React, { useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useAuth } from "../../auth/context/AuthContext";
import { OnboardingProvider } from "../context/OnboardingContext";
import { onboardingService } from "../services/onboardingService";
import Step1BasicInfo from "../components/Step1BasicInfo";
import Step2ProfessionalInfo from "../components/Step2ProfessionalInfo";
import Step3SkillsExpertise from "../components/Step3SkillsExpertise";
import Step4Availability from "../components/Step4Availability";
import Step5MentorSpecific from "../components/Step5MentorSpecific";
import Step5MenteeSpecific from "../components/Step5MenteeSpecific";
import OnboardingStepper from "../components/OnboardingStepper";

const OnboardingPage: React.FC = () => {
    const navigate = useNavigate();
    const { role } = useParams<{ role: string }>();
    const { isAuthenticated, isLoading } = useAuth();

    // Redirect to login if not authenticated
    useEffect(() => {
        if (!isLoading && !isAuthenticated) {
            navigate("/login");
        }

        // Validate role parameter
        if (role !== "mentor" && role !== "mentee") {
            navigate("/dashboard");
        }
    }, [isAuthenticated, isLoading, navigate, role]);

    if (isLoading || !role || (role !== "mentor" && role !== "mentee")) {
        return (
            <div className="flex items-center justify-center min-h-screen">
                <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-primary"></div>
            </div>
        );
    }

    const userRole = role as "mentor" | "mentee";

    return (
        <OnboardingProvider initialRole={userRole}>
            <div className="min-h-screen bg-[#F8FAFC] flex flex-col">
                {/* Header */}
                <header className="bg-white py-4 px-6 shadow-sm">
                    <div className="max-w-7xl mx-auto flex items-center">
                        <div className="flex items-center gap-2">
                            <img
                                src="/logo.svg"
                                alt="MentorSync"
                                className="h-8 w-8"
                            />
                            <h1 className="text-2xl font-bold text-[#1E293B]">
                                MentorSync
                            </h1>
                        </div>
                    </div>
                </header>

                {/* Main Content */}
                <main className="flex-1 py-8">
                    <div className="max-w-3xl mx-auto px-4">
                        <OnboardingStepper />

                        <div className="mt-8 bg-white p-6 rounded-2xl shadow-sm">
                            <OnboardingContent userRole={userRole} />
                        </div>
                    </div>
                </main>

                {/* Footer */}
                <footer className="bg-white py-4 px-6 border-t border-[#E2E8F0]">
                    <div className="max-w-7xl mx-auto text-center text-[#64748B] text-sm">
                        © {new Date().getFullYear()} MentorSync. Всі права
                        захищені.
                    </div>
                </footer>
            </div>
        </OnboardingProvider>
    );
};

const OnboardingContent: React.FC<{ userRole: "mentor" | "mentee" }> = ({
    userRole,
}) => {
    const { currentStep, nextStep, prevStep, isFirstStep, isLastStep, data } =
        useOnboarding();

    // Check if all required fields for current step are filled
    const validateCurrentStep = (): boolean => {
        switch (currentStep) {
            case 1:
                return !!data.fullName && !!data.bio && !!data.location;
            case 2:
                return !!data.position && !!data.company;
            case 3:
                return data.skills.length > 0 && data.expertiseAreas.length > 0;
            case 4:
                // Validate that at least one day has time slots
                return Object.values(data.availabilityHours).some(
                    (daySlots) => daySlots.length > 0
                );
            case 5:
                if (userRole === "mentor") {
                    return (
                        !!data.mentorshipStyle &&
                        data.maxMentees > 0 &&
                        data.yearsOfExperience > 0
                    );
                } else {
                    return (
                        data.goals.length > 0 &&
                        data.desiredSkills.length > 0 &&
                        !!data.expectedSessionFrequency
                    );
                }
            default:
                return false;
        }
    };

    const handleNextStep = () => {
        if (validateCurrentStep()) {
            nextStep();
        } else {
            alert("Будь ласка, заповніть всі обов'язкові поля");
        }
    };
    const handleSubmit = async () => {
        if (!validateCurrentStep()) {
            alert("Будь ласка, заповніть всі обов'язкові поля");
            return;
        }

        try {
            // Send data to the backend based on user role
            let response;
            if (userRole === "mentor") {
                response = await onboardingService.saveMentorProfile(data);
            } else {
                response = await onboardingService.saveMenteeProfile(data);
            }

            if (response.success) {
                alert("Профіль успішно створено!");
                // Redirect to dashboard after successful submission
                window.location.href = "/dashboard";
            } else {
                throw new Error(response.message);
            }
        } catch (error) {
            console.error("Error submitting onboarding data:", error);
            alert("Виникла помилка при збереженні даних. Спробуйте ще раз.");
        }
    };

    const renderStep = () => {
        switch (currentStep) {
            case 1:
                return <Step1BasicInfo />;
            case 2:
                return <Step2ProfessionalInfo />;
            case 3:
                return <Step3SkillsExpertise />;
            case 4:
                return <Step4Availability />;
            case 5:
                return userRole === "mentor" ? (
                    <Step5MentorSpecific />
                ) : (
                    <Step5MenteeSpecific />
                );
            default:
                return <Step1BasicInfo />;
        }
    };

    return (
        <>
            <div>{renderStep()}</div>

            <div className="mt-8 flex justify-between">
                <button
                    onClick={prevStep}
                    disabled={isFirstStep}
                    className={`px-5 py-2 rounded-lg border border-[#E2E8F0] ${
                        isFirstStep
                            ? "opacity-50 cursor-not-allowed"
                            : "hover:bg-[#F1F5F9]"
                    }`}
                >
                    Назад
                </button>

                {isLastStep ? (
                    <button
                        onClick={handleSubmit}
                        className="px-5 py-2 rounded-lg bg-[#4318D1] text-white hover:bg-[#3A14B8]"
                    >
                        Завершити
                    </button>
                ) : (
                    <button
                        onClick={handleNextStep}
                        className="px-5 py-2 rounded-lg bg-[#4318D1] text-white hover:bg-[#3A14B8]"
                    >
                        Продовжити
                    </button>
                )}
            </div>
        </>
    );
};

// Import useOnboarding hook from OnboardingContext
import { useOnboarding } from "../context/OnboardingContext";

export default OnboardingPage;
