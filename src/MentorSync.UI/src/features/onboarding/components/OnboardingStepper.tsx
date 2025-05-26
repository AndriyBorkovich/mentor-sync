import React from "react";
import { useOnboarding } from "../context/OnboardingContext";

const OnboardingStepper: React.FC = () => {
    const { currentStep, goToStep, role } = useOnboarding();

    const mentorSteps = [
        { label: "Основна інформація", step: 1 },
        { label: "Професійна інформація", step: 2 },
        { label: "Навички та експертиза", step: 3 },
        { label: "Доступність", step: 4 },
        { label: "Спеціалізація", step: 5 },
    ];

    const menteeSteps = [
        { label: "Основна інформація", step: 1 },
        { label: "Професійна інформація", step: 2 },
        { label: "Навички та експертиза", step: 3 },
        { label: "Цілі навчання", step: 4 },
        { label: "Галузі інтересів", step: 5 },
    ];

    const steps = role === "mentor" ? mentorSteps : menteeSteps;

    const handleStepClick = (step: number) => {
        // Allow clicking only on completed steps
        if (step < currentStep) {
            goToStep(step as 1 | 2 | 3 | 4 | 5);
        }
    };

    return (
        <div className="flex justify-center">
            <div className="w-full max-w-3xl">
                <div className="flex items-center justify-between">
                    {steps.map((step, index) => (
                        <React.Fragment key={step.step}>
                            {/* Step Circle */}
                            <div
                                className={`flex flex-col items-center cursor-pointer ${
                                    step.step < currentStep
                                        ? "cursor-pointer"
                                        : "cursor-default"
                                }`}
                                onClick={() => handleStepClick(step.step)}
                            >
                                <div
                                    className={`w-10 h-10 rounded-full flex items-center justify-center text-white mb-2 ${
                                        step.step === currentStep
                                            ? "bg-[#4318D1]"
                                            : step.step < currentStep
                                            ? "bg-[#4318D1] opacity-70"
                                            : "bg-[#E2E8F0]"
                                    }`}
                                >
                                    {step.step < currentStep ? (
                                        <span className="material-icons">
                                            check
                                        </span>
                                    ) : (
                                        step.step
                                    )}
                                </div>
                                <span
                                    className={`text-xs text-center hidden md:block ${
                                        step.step === currentStep
                                            ? "text-[#1E293B] font-medium"
                                            : "text-[#64748B]"
                                    }`}
                                >
                                    {step.label}
                                </span>
                                <span
                                    className={`text-xs text-center block md:hidden ${
                                        step.step === currentStep
                                            ? "text-[#1E293B] font-medium"
                                            : "text-[#64748B]"
                                    }`}
                                >
                                    {step.step}
                                </span>
                            </div>

                            {/* Connector Line */}
                            {index < steps.length - 1 && (
                                <div className="flex-1 h-1 mx-2">
                                    <div
                                        className={`h-full ${
                                            step.step < currentStep
                                                ? "bg-[#4318D1]"
                                                : "bg-[#E2E8F0]"
                                        }`}
                                    ></div>
                                </div>
                            )}
                        </React.Fragment>
                    ))}
                </div>

                <div className="mt-4 text-center">
                    <div className="flex items-center justify-center">
                        <span className="text-[#4318D1] font-bold mr-2">
                            {currentStep}
                        </span>
                        <span className="text-[#64748B]">/ 5</span>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default OnboardingStepper;
