import React, { createContext, useContext, useState, ReactNode } from "react";
import {
    OnboardingData,
    OnboardingStep,
    initialOnboardingData,
} from "../data/OnboardingTypes";

interface OnboardingContextType {
    data: OnboardingData;
    currentStep: OnboardingStep;
    updateData: (newData: Partial<OnboardingData>) => void;
    nextStep: () => void;
    prevStep: () => void;
    goToStep: (step: OnboardingStep) => void;
    isLastStep: boolean;
    isFirstStep: boolean;
    role: "mentor" | "mentee";
}

const OnboardingContext = createContext<OnboardingContextType | undefined>(
    undefined
);

export const useOnboarding = () => {
    const context = useContext(OnboardingContext);
    if (!context) {
        throw new Error(
            "useOnboarding must be used within an OnboardingProvider"
        );
    }
    return context;
};

interface OnboardingProviderProps {
    children: ReactNode;
    initialRole: "mentor" | "mentee";
}

export const OnboardingProvider: React.FC<OnboardingProviderProps> = ({
    children,
    initialRole,
}) => {
    const [data, setData] = useState<OnboardingData>(initialOnboardingData);
    const [currentStep, setCurrentStep] = useState<OnboardingStep>(1);
    const [role] = useState<"mentor" | "mentee">(initialRole);

    const updateData = (newData: Partial<OnboardingData>) => {
        setData((prevData) => ({
            ...prevData,
            ...newData,
        }));
    };

    const nextStep = () => {
        if (currentStep < 5) {
            setCurrentStep((prevStep) => (prevStep + 1) as OnboardingStep);
        }
    };

    const prevStep = () => {
        if (currentStep > 1) {
            setCurrentStep((prevStep) => (prevStep - 1) as OnboardingStep);
        }
    };

    const goToStep = (step: OnboardingStep) => {
        setCurrentStep(step);
    };

    const isLastStep = currentStep === 5;
    const isFirstStep = currentStep === 1;

    const value = {
        data,
        currentStep,
        updateData,
        nextStep,
        prevStep,
        goToStep,
        isLastStep,
        isFirstStep,
        role,
    };

    return (
        <OnboardingContext.Provider value={value}>
            {children}
        </OnboardingContext.Provider>
    );
};
