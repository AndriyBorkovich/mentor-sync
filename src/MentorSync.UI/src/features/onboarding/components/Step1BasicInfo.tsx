import React from "react";
import { useOnboarding } from "../context/OnboardingContext";

const Step1BasicInfo: React.FC = () => {
    const { data, updateData } = useOnboarding();

    return (
        <div className="space-y-6">
            <h2 className="text-2xl font-bold text-[#1E293B]">
                Розкажіть нам про себе
            </h2>
            <p className="text-[#64748B]">
                Ця інформація буде видима у вашому профілі
            </p>
            <div>
                <textarea
                    id="bio"
                    value={data.bio}
                    onChange={(e) => updateData({ bio: e.target.value })}
                    rows={4}
                    className="w-full p-3 border border-[#E2E8F0] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#6C5DD3] focus:border-[#6C5DD3]"
                    placeholder="Розкажіть нам трохи про себе, ваш досвід, інтереси..."
                />
            </div>
        </div>
    );
};

export default Step1BasicInfo;
