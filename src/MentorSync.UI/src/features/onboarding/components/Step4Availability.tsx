import React from "react";
import { useOnboarding } from "../context/OnboardingContext";
import {
    Availability,
    timeOfDayOptions,
} from "../../../shared/enums/availability";

const Step4Availability: React.FC = () => {
    const { data, updateData } = useOnboarding();

    const toggleTimeOfDay = (value: Availability) => {
        const currentValue = data.availabilityFlag || 0;
        const newValue =
            currentValue & value
                ? currentValue & ~value // Remove flag if already selected
                : currentValue | value; // Add flag if not selected
        updateData({ availabilityFlag: newValue });
    };

    const isTimeOfDaySelected = (value: Availability): boolean => {
        return (data.availabilityFlag & value) !== 0;
    };

    return (
        <div className="space-y-6">
            <h2 className="text-2xl font-bold text-[#1E293B]">Доступність</h2>
            <p className="text-[#64748B]">
                Вкажіть коли ви доступні для менторських сесій
            </p>

            <div className="space-y-6">
                <div>
                    <label className="block text-sm font-medium text-[#1E293B] mb-2">
                        Доступний час
                    </label>
                    <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-2 gap-4">
                        {timeOfDayOptions.map((option) => (
                            <div
                                key={option.value}
                                className={`border rounded-lg p-4 cursor-pointer transition-all ${
                                    isTimeOfDaySelected(option.value)
                                        ? "border-[#6C5DD3] bg-[#6C5DD3]/10"
                                        : "border-[#E2E8F0] hover:border-[#6C5DD3]/50"
                                }`}
                                onClick={() => toggleTimeOfDay(option.value)}
                            >
                                <div className="flex items-center">
                                    <div className="flex-shrink-0">
                                        <input
                                            type="checkbox"
                                            checked={isTimeOfDaySelected(
                                                option.value
                                            )}
                                            onChange={() => {}}
                                            className="h-4 w-4 text-[#6C5DD3] rounded"
                                        />
                                    </div>
                                    <div className="ml-3">
                                        <div className="text-[#1E293B] font-medium">
                                            {option.label}
                                        </div>
                                        <div className="text-[#64748B] text-sm">
                                            {option.desc}
                                        </div>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Step4Availability;
