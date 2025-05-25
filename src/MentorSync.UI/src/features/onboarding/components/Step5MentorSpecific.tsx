import React, { useState } from "react";
import { useOnboarding } from "../context/OnboardingContext";

const Step5MentorSpecific: React.FC = () => {
    const { data, updateData } = useOnboarding();
    const [rateEnabled, setRateEnabled] = useState<boolean>(!!data.hourlyRate);

    const mentorshipStyles = [
        "Директивний",
        "Недирективний",
        "Коучинг",
        "Наставництво",
        "Консультування",
    ];

    const experienceLevels = [
        "Початківець",
        "Середній",
        "Досвідчений",
        "Експерт",
        "Будь-який рівень",
    ];

    const handleMentorshipStyleChange = (style: string) => {
        updateData({ mentorshipStyle: style });
    };

    const handleMaxMenteesChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = parseInt(e.target.value);
        if (!isNaN(value) && value >= 1) {
            updateData({ maxMentees: value });
        }
    };

    const handleHourlyRateChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = parseFloat(e.target.value);
        updateData({ hourlyRate: isNaN(value) ? undefined : value });
    };

    const togglePreferredLevel = (level: string) => {
        let updatedLevels = [...data.preferredMenteeLevel];
        if (updatedLevels.includes(level)) {
            updatedLevels = updatedLevels.filter((l) => l !== level);
        } else {
            updatedLevels.push(level);
        }
        updateData({ preferredMenteeLevel: updatedLevels });
    };

    return (
        <div className="space-y-6">
            <h2 className="text-2xl font-bold text-[#1E293B]">
                Менторські налаштування
            </h2>
            <p className="text-[#64748B]">
                Розкажіть про свій стиль менторства та переваги
            </p>

            <div className="space-y-6">
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
                        onChange={(e) => {
                            const value = parseInt(e.target.value);
                            if (!isNaN(value) && value >= 0) {
                                updateData({ yearsOfExperience: value });
                            }
                        }}
                        className="w-full p-3 border border-[#E2E8F0] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#6C5DD3] focus:border-[#6C5DD3]"
                        placeholder="Наприклад: 5"
                    />
                </div>

                <div>
                    <label className="block text-sm font-medium text-[#1E293B] mb-2">
                        Стиль менторства
                    </label>
                    <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-3">
                        {mentorshipStyles.map((style) => (
                            <div
                                key={style}
                                className={`border rounded-lg p-3 cursor-pointer ${
                                    data.mentorshipStyle === style
                                        ? "border-[#6C5DD3] bg-[#6C5DD3]/10"
                                        : "border-[#E2E8F0]"
                                }`}
                                onClick={() =>
                                    handleMentorshipStyleChange(style)
                                }
                            >
                                <div className="flex items-center">
                                    <input
                                        type="radio"
                                        checked={data.mentorshipStyle === style}
                                        onChange={() => {}}
                                        className="h-4 w-4 text-[#6C5DD3] rounded mr-3"
                                    />
                                    <span className="text-[#1E293B]">
                                        {style}
                                    </span>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>

                <div>
                    <label
                        htmlFor="maxMentees"
                        className="block text-sm font-medium text-[#1E293B] mb-2"
                    >
                        Максимальна кількість менті
                    </label>
                    <input
                        type="number"
                        id="maxMentees"
                        min="1"
                        value={data.maxMentees}
                        onChange={handleMaxMenteesChange}
                        className="w-full p-3 border border-[#E2E8F0] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#6C5DD3] focus:border-[#6C5DD3]"
                    />
                </div>

                <div>
                    <div className="flex items-center justify-between mb-2">
                        <label className="text-sm font-medium text-[#1E293B]">
                            Платне менторство
                        </label>
                        <label className="inline-flex items-center cursor-pointer">
                            <input
                                type="checkbox"
                                className="sr-only peer"
                                checked={rateEnabled}
                                onChange={() => setRateEnabled(!rateEnabled)}
                            />
                            <div className="relative w-11 h-6 bg-gray-200 peer-focus:outline-none rounded-full peer peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-[#6C5DD3]"></div>
                        </label>
                    </div>
                    {rateEnabled && (
                        <div className="mt-2">
                            <div className="flex items-center">
                                <span className="text-[#64748B] mr-2">$</span>
                                <input
                                    type="number"
                                    value={data.hourlyRate || ""}
                                    onChange={handleHourlyRateChange}
                                    className="w-full p-3 border border-[#E2E8F0] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#6C5DD3] focus:border-[#6C5DD3]"
                                    placeholder="Почасова ставка"
                                    min="0"
                                    step="1"
                                />
                                <span className="text-[#64748B] ml-2">
                                    за годину
                                </span>
                            </div>
                        </div>
                    )}
                </div>

                <div>
                    <label className="block text-sm font-medium text-[#1E293B] mb-2">
                        Переважний рівень менті
                    </label>
                    <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-3">
                        {experienceLevels.map((level) => (
                            <div
                                key={level}
                                className={`border rounded-lg p-3 cursor-pointer ${
                                    data.preferredMenteeLevel.includes(level)
                                        ? "border-[#6C5DD3] bg-[#6C5DD3]/10"
                                        : "border-[#E2E8F0]"
                                }`}
                                onClick={() => togglePreferredLevel(level)}
                            >
                                <div className="flex items-center">
                                    <input
                                        type="checkbox"
                                        checked={data.preferredMenteeLevel.includes(
                                            level
                                        )}
                                        onChange={() => {}}
                                        className="h-4 w-4 text-[#6C5DD3] rounded mr-3"
                                    />
                                    <span className="text-[#1E293B]">
                                        {level}
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
