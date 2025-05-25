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
                <label
                    htmlFor="fullName"
                    className="block text-sm font-medium text-[#1E293B] mb-2"
                >
                    Повне ім'я
                </label>
                <input
                    type="text"
                    id="fullName"
                    value={data.fullName}
                    onChange={(e) => updateData({ fullName: e.target.value })}
                    className="w-full p-3 border border-[#E2E8F0] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#6C5DD3] focus:border-[#6C5DD3]"
                    placeholder="Введіть ваше повне ім'я"
                />
            </div>

            <div>
                <label
                    htmlFor="bio"
                    className="block text-sm font-medium text-[#1E293B] mb-2"
                >
                    Про себе
                </label>
                <textarea
                    id="bio"
                    value={data.bio}
                    onChange={(e) => updateData({ bio: e.target.value })}
                    rows={4}
                    className="w-full p-3 border border-[#E2E8F0] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#6C5DD3] focus:border-[#6C5DD3]"
                    placeholder="Розкажіть нам трохи про себе, ваш досвід, інтереси..."
                />
            </div>

            <div>
                <label
                    htmlFor="location"
                    className="block text-sm font-medium text-[#1E293B] mb-2"
                >
                    Місцезнаходження
                </label>
                <input
                    type="text"
                    id="location"
                    value={data.location}
                    onChange={(e) => updateData({ location: e.target.value })}
                    className="w-full p-3 border border-[#E2E8F0] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#6C5DD3] focus:border-[#6C5DD3]"
                    placeholder="Місто, Країна"
                />
            </div>

            <div>
                <label className="block text-sm font-medium text-[#1E293B] mb-2">
                    Фото профілю
                </label>
                <div className="mt-1 flex items-center">
                    <div className="w-20 h-20 rounded-full bg-[#F1F5F9] flex items-center justify-center text-[#94A3B8] overflow-hidden">
                        <span className="material-icons text-3xl">person</span>
                    </div>
                    <button
                        type="button"
                        className="ml-5 bg-white py-2 px-3 border border-[#E2E8F0] rounded-lg text-sm font-medium text-[#64748B] hover:bg-[#F8FAFC]"
                    >
                        Змінити
                    </button>
                </div>
            </div>
        </div>
    );
};

export default Step1BasicInfo;
