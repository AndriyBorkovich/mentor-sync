import React from "react";
import { useOnboarding } from "../context/OnboardingContext";

const Step2ProfessionalInfo: React.FC = () => {
    const { data, updateData } = useOnboarding();

    return (
        <div className="space-y-6">
            <h2 className="text-2xl font-bold text-[#1E293B]">
                Професійна інформація
            </h2>
            <p className="text-[#64748B]">
                Розкажіть про свій професійний досвід
            </p>
            <div>
                <label
                    htmlFor="position"
                    className="block text-sm font-medium text-[#1E293B] mb-2"
                >
                    Посада
                </label>
                <input
                    type="text"
                    id="position"
                    value={data.position}
                    onChange={(e) => updateData({ position: e.target.value })}
                    className="w-full p-3 border border-[#E2E8F0] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#6C5DD3] focus:border-[#6C5DD3]"
                    placeholder="Наприклад: Senior Frontend Developer"
                />
            </div>
            <div>
                <label
                    htmlFor="company"
                    className="block text-sm font-medium text-[#1E293B] mb-2"
                >
                    Компанія
                </label>
                <input
                    type="text"
                    id="company"
                    value={data.company}
                    onChange={(e) => updateData({ company: e.target.value })}
                    className="w-full p-3 border border-[#E2E8F0] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#6C5DD3] focus:border-[#6C5DD3]"
                    placeholder="Наприклад: Google"
                />
            </div>{" "}
        </div>
    );
};

export default Step2ProfessionalInfo;
