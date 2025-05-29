import { toast } from "react-toastify";
import { useOnboarding } from "../context/OnboardingContext";
import { onboardingService } from "../services/onboardingService";
import Step1BasicInfo from "./Step1BasicInfo";
import Step2ProfessionalInfo from "./Step2ProfessionalInfo";
import Step3SkillsExpertise from "./Step3SkillsExpertise";
import Step4Availability from "./Step4Availability";
import Step5MenteeSpecific from "./Step5MenteeSpecific";
import Step5MentorSpecific from "./Step5MentorSpecific";
import { useNavigate } from "react-router-dom";

export const OnboardingContent: React.FC<{ userRole: "mentor" | "mentee" }> = ({
    userRole,
}) => {
    const { currentStep, nextStep, prevStep, isFirstStep, isLastStep, data } =
        useOnboarding(); // Check if all required fields for current step are filled
    const navigate = useNavigate();
    const validateCurrentStep = (): boolean => {
        switch (currentStep) {
            case 1:
                return !!data.bio;
            case 2:
                return !!data.position;
            case 3:
                return data.skills.length > 0;
            case 4:
                if (userRole === "mentor") {
                    // Mentor requires availability
                    return data.availabilityFlag > 0;
                } else {
                    // Mentee requires learning goals
                    return data.goals.length > 0;
                }
            case 5:
                if (userRole === "mentor") {
                    return data.industryFlag > 0 && data.yearsOfExperience > 0;
                } else {
                    return data.industryFlag > 0; // Mentees also need to select industries
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
        } // Additional validation for API requirements
        // Common validation for both roles
        if (data.bio.trim() === "") {
            alert("Будь ласка, заповніть поле 'Про себе'");
            return;
        }
        if (data.position.trim() === "") {
            alert("Будь ласка, вкажіть вашу посаду");
            return;
        }
        if (data.skills.length === 0) {
            alert("Будь ласка, вкажіть ваші навички");
            return;
        }

        if (data.industryFlag === 0) {
            alert("Будь ласка, виберіть хоча б одну галузь");
            return;
        }

        // Role-specific validations
        if (userRole === "mentor") {
            if (data.availabilityFlag === 0) {
                alert("Будь ласка, виберіть доступний час");
                return;
            }
            if (data.yearsOfExperience <= 0) {
                alert("Будь ласка, вкажіть кількість років досвіду");
                return;
            }
        } else {
            if (data.goals.length === 0) {
                alert("Будь ласка, вкажіть ваші цілі навчання");
                return;
            }
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
                toast.success("Профіль успішно створено!");
                // Redirect to dashboard after successful submission
                userRole == "mentee"
                    ? navigate("/dashboard")
                    : navigate("/sessions");
            } else {
                throw new Error(
                    response.message || "Помилка збереження профілю"
                );
            }
        } catch (error: any) {
            console.error("Error submitting onboarding data:", error);
            alert(
                `Виникла помилка при збереженні даних: ${
                    error.message || "Невідома помилка"
                }. Спробуйте ще раз.`
            );
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
                return userRole === "mentor" ? (
                    <Step4Availability />
                ) : (
                    <Step5MenteeSpecific />
                );
            case 5:
                return userRole === "mentor" ? (
                    <Step5MentorSpecific />
                ) : (
                    // For mentees, the last step is Industry selection
                    <Step5MentorSpecific displayYearsExperience={false} />
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
