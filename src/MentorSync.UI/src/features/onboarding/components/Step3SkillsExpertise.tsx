import React, { useState } from "react";
import { useOnboarding } from "../context/OnboardingContext";

const Step3SkillsExpertise: React.FC = () => {
    const { data, updateData } = useOnboarding();
    const [skillInput, setSkillInput] = useState("");

    // Pre-defined categories for expertise areas
    const expertiseCategories = [
        "Frontend Development",
        "Backend Development",
        "Mobile Development",
        "DevOps",
        "Data Science",
        "UI/UX Design",
        "Project Management",
        "Machine Learning",
        "Cybersecurity",
        "Cloud Computing",
    ];

    const handleAddSkill = () => {
        if (
            skillInput.trim() !== "" &&
            !data.skills.includes(skillInput.trim())
        ) {
            const updatedSkills = [...data.skills, skillInput.trim()];
            updateData({ skills: updatedSkills });
            setSkillInput("");
        }
    };

    const handleRemoveSkill = (skill: string) => {
        const updatedSkills = data.skills.filter((s) => s !== skill);
        updateData({ skills: updatedSkills });
    };

    const handleExpertiseToggle = (expertise: string) => {
        const updatedExpertise = data.expertiseAreas.includes(expertise)
            ? data.expertiseAreas.filter((e) => e !== expertise)
            : [...data.expertiseAreas, expertise];

        updateData({ expertiseAreas: updatedExpertise });
    };

    return (
        <div className="space-y-6">
            <h2 className="text-2xl font-bold text-[#1E293B]">
                Навички та експертиза
            </h2>
            <p className="text-[#64748B]">
                Виберіть ваші ключові навички та області експертизи
            </p>

            <div>
                <label className="block text-sm font-medium text-[#1E293B] mb-2">
                    Навички
                </label>
                <div className="flex gap-2 items-center">
                    <input
                        type="text"
                        value={skillInput}
                        onChange={(e) => setSkillInput(e.target.value)}
                        className="flex-1 p-3 border border-[#E2E8F0] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#6C5DD3] focus:border-[#6C5DD3]"
                        placeholder="Наприклад: React.js"
                        onKeyDown={(e) => e.key === "Enter" && handleAddSkill()}
                    />
                    <button
                        type="button"
                        onClick={handleAddSkill}
                        className="p-3 bg-[#6C5DD3] text-white rounded-lg hover:bg-[#5B4DC4]"
                    >
                        <span className="material-icons">add</span>
                    </button>
                </div>
                <div className="mt-2 flex flex-wrap gap-2">
                    {data.skills.map((skill, index) => (
                        <div
                            key={index}
                            className="flex items-center bg-[#F1F5F9] text-[#64748B] px-3 py-1 rounded-full"
                        >
                            <span>{skill}</span>
                            <button
                                type="button"
                                onClick={() => handleRemoveSkill(skill)}
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

            <div>
                <label className="block text-sm font-medium text-[#1E293B] mb-2">
                    Області експертизи
                </label>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
                    {expertiseCategories.map((category) => (
                        <div
                            key={category}
                            className={`border rounded-lg p-3 cursor-pointer ${
                                data.expertiseAreas.includes(category)
                                    ? "border-[#6C5DD3] bg-[#6C5DD3]/10"
                                    : "border-[#E2E8F0]"
                            }`}
                            onClick={() => handleExpertiseToggle(category)}
                        >
                            <div className="flex items-center">
                                <input
                                    type="checkbox"
                                    checked={data.expertiseAreas.includes(
                                        category
                                    )}
                                    onChange={() => {}}
                                    className="h-4 w-4 text-[#6C5DD3] rounded mr-3"
                                />
                                <span className="text-[#1E293B]">
                                    {category}
                                </span>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
};

export default Step3SkillsExpertise;
