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
            </div>

            <div>
                <label
                    htmlFor="yearsOfExperience"
                    className="block text-sm font-medium text-[#1E293B] mb-2"
                >
                    Роки досвіду
                </label>
                <select
                    id="yearsOfExperience"
                    value={data.yearsOfExperience}
                    onChange={(e) =>
                        updateData({
                            yearsOfExperience: parseInt(e.target.value),
                        })
                    }
                    className="w-full p-3 border border-[#E2E8F0] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#6C5DD3] focus:border-[#6C5DD3]"
                >
                    <option value={0}>Менше 1 року</option>
                    <option value={1}>1 рік</option>
                    <option value={2}>2 роки</option>
                    <option value={3}>3 роки</option>
                    <option value={5}>3-5 років</option>
                    <option value={8}>5-10 років</option>
                    <option value={10}>10+ років</option>
                </select>
            </div>

            <div>
                <label
                    htmlFor="linkedin"
                    className="block text-sm font-medium text-[#1E293B] mb-2"
                >
                    LinkedIn (опціонально)
                </label>
                <div className="flex items-center border border-[#E2E8F0] rounded-lg overflow-hidden">
                    <span className="bg-[#F8FAFC] p-3 text-[#64748B] border-r border-[#E2E8F0]">
                        <span className="material-icons">link</span>
                    </span>
                    <input
                        type="text"
                        id="linkedin"
                        className="w-full p-3 focus:outline-none"
                        placeholder="https://linkedin.com/in/yourprofile"
                    />
                </div>
            </div>
        </div>
    );
};

export default Step2ProfessionalInfo;
