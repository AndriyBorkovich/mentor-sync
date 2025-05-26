import React from "react";
import { useOnboarding } from "../context/OnboardingContext";
import { Industry } from "../data/OnboardingTypes";

interface Step5MentorSpecificProps {
    displayYearsExperience?: boolean;
}

const Step5MentorSpecific: React.FC<Step5MentorSpecificProps> = ({
    displayYearsExperience = true,
}) => {
    const { data, updateData } = useOnboarding();

    const industries = [
        { value: Industry.WebDevelopment, label: "Веб-розробка" },
        { value: Industry.DataScience, label: "Data Science" },
        { value: Industry.CyberSecurity, label: "Кібербезпека" },
        { value: Industry.CloudComputing, label: "Хмарні обчислення" },
        { value: Industry.DevOps, label: "DevOps" },
        { value: Industry.GameDevelopment, label: "Розробка ігор" },
        { value: Industry.ItSupport, label: "ІТ-підтримка" },
        { value: Industry.ArtificialIntelligence, label: "Штучний інтелект" },
        { value: Industry.Blockchain, label: "Блокчейн" },
        { value: Industry.Networking, label: "Мережі" },
        { value: Industry.UxUiDesign, label: "UX/UI дизайн" },
        { value: Industry.EmbeddedSystems, label: "Вбудовані системи" },
        { value: Industry.ItConsulting, label: "ІТ-консалтинг" },
        {
            value: Industry.DatabaseAdministration,
            label: "Адміністрування баз даних",
        },
    ];

    const handleExperienceChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = parseInt(e.target.value);
        if (!isNaN(value) && value >= 0) {
            updateData({ yearsOfExperience: value });
        }
    };

    const toggleIndustry = (industryValue: Industry) => {
        const currentValue = data.industryFlag || 0;
        const newValue =
            currentValue & industryValue
                ? currentValue & ~industryValue // Remove flag if already selected
                : currentValue | industryValue; // Add flag if not selected
        updateData({ industryFlag: newValue });
    };

    const isIndustrySelected = (industryValue: Industry): boolean => {
        return (data.industryFlag & industryValue) !== 0;
    };
    return (
        <div className="space-y-6">
            <h2 className="text-2xl font-bold text-[#1E293B]">
                Галузі спеціалізації
            </h2>
            <p className="text-[#64748B]">
                {displayYearsExperience
                    ? "Розкажіть про вашу досвідченість та галузі спеціалізації"
                    : "Виберіть галузі, які вас цікавлять для навчання"}
            </p>
            <div className="space-y-6">
                {displayYearsExperience && (
                    <div>
                        <label
                            htmlFor="years"
                            className="block text-sm font-medium text-[#1E293B] mb-2"
                        >
                            Роки досвіду
                        </label>
                        <input
                            type="number"
                            id="years"
                            min="1"
                            value={data.yearsOfExperience}
                            onChange={handleExperienceChange}
                            className="w-full p-3 border border-[#E2E8F0] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#6C5DD3] focus:border-[#6C5DD3]"
                            placeholder="Наприклад: 5"
                        />
                    </div>
                )}

                <div>
                    {" "}
                    <label className="block text-sm font-medium text-[#1E293B] mb-2">
                        {displayYearsExperience
                            ? "Галузі спеціалізації"
                            : "Галузі інтересів"}
                    </label>
                    <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
                        {industries.map((industry) => (
                            <div
                                key={industry.value}
                                className={`border rounded-lg p-3 cursor-pointer ${
                                    isIndustrySelected(industry.value)
                                        ? "border-[#6C5DD3] bg-[#6C5DD3]/10"
                                        : "border-[#E2E8F0]"
                                }`}
                                onClick={() => toggleIndustry(industry.value)}
                            >
                                <div className="flex items-center">
                                    <input
                                        type="checkbox"
                                        checked={isIndustrySelected(
                                            industry.value
                                        )}
                                        onChange={() => {}}
                                        className="h-4 w-4 text-[#6C5DD3] rounded mr-3"
                                    />
                                    <span className="text-[#1E293B]">
                                        {industry.label}
                                    </span>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Step5MentorSpecific;
