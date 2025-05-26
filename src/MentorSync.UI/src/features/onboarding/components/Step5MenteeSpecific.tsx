import React, { useState } from "react";
import { useOnboarding } from "../context/OnboardingContext";

const Step5MenteeSpecific: React.FC = () => {
    const { data, updateData } = useOnboarding();
    const [goalInput, setGoalInput] = useState<string>("");

    const handleAddGoal = () => {
        if (goalInput.trim() !== "" && !data.goals.includes(goalInput.trim())) {
            const updatedGoals = [...data.goals, goalInput.trim()];
            updateData({ goals: updatedGoals });
            setGoalInput("");
        }
    };

    const handleRemoveGoal = (goal: string) => {
        const updatedGoals = data.goals.filter((g) => g !== goal);
        updateData({ goals: updatedGoals });
    };

    return (
        <div className="space-y-6">
            <h2 className="text-2xl font-bold text-[#1E293B]">
                Ваші цілі навчання
            </h2>
            <p className="text-[#64748B]">
                Розкажіть, які у вас цілі та яких навичок ви хочете набути
            </p>

            <div className="space-y-6">
                <div>
                    <label className="block text-sm font-medium text-[#1E293B] mb-2">
                        Цілі навчання
                    </label>
                    <p className="text-sm text-[#64748B] mb-2">
                        Які конкретні цілі ви хочете досягти з ментором?
                    </p>
                    <div className="flex gap-2 items-center">
                        <input
                            type="text"
                            value={goalInput}
                            onChange={(e) => setGoalInput(e.target.value)}
                            className="flex-1 p-3 border border-[#E2E8F0] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#6C5DD3] focus:border-[#6C5DD3]"
                            placeholder="Наприклад: Освоїти фронтенд-розробку"
                            onKeyPress={(e) =>
                                e.key === "Enter" && handleAddGoal()
                            }
                        />
                        <button
                            type="button"
                            onClick={handleAddGoal}
                            className="p-3 bg-[#6C5DD3] text-white rounded-lg hover:bg-[#5B4DC4]"
                        >
                            <span className="material-icons">add</span>
                        </button>
                    </div>
                    <div className="mt-2 flex flex-wrap gap-2">
                        {data.goals.map((goal, index) => (
                            <div
                                key={index}
                                className="flex items-center bg-[#F1F5F9] text-[#64748B] px-3 py-1 rounded-full"
                            >
                                <span>{goal}</span>
                                <button
                                    type="button"
                                    onClick={() => handleRemoveGoal(goal)}
                                    className="ml-2 focus:outline-none"
                                >
                                    <span className="material-icons text-sm">
                                        close
                                    </span>
                                </button>
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Step5MenteeSpecific;
