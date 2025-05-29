import React, { useState } from "react";
import { useOnboarding } from "../context/OnboardingContext";
import { programmingLanguages } from "../../../shared/constants/programmingLanguages";

const Step3SkillsExpertise: React.FC = () => {
    const { data, updateData } = useOnboarding();
    const [skillInput, setSkillInput] = useState("");
    const [languageInput, setLanguageInput] = useState("");

    // Common programming languages for suggestions
    const commonLanguages = programmingLanguages;

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

    const handleAddLanguage = () => {
        if (
            languageInput.trim() !== "" &&
            !data.programmingLanguages.includes(languageInput.trim())
        ) {
            const updatedLanguages = [
                ...data.programmingLanguages,
                languageInput.trim(),
            ];
            updateData({ programmingLanguages: updatedLanguages });
            setLanguageInput("");
        }
    };

    const handleRemoveLanguage = (language: string) => {
        const updatedLanguages = data.programmingLanguages.filter(
            (lang) => lang !== language
        );
        updateData({ programmingLanguages: updatedLanguages });
    };

    const selectLanguage = (language: string) => {
        if (!data.programmingLanguages.includes(language)) {
            const updatedLanguages = [...data.programmingLanguages, language];
            updateData({ programmingLanguages: updatedLanguages });
        }
    };

    return (
        <div className="space-y-6">
            <h2 className="text-2xl font-bold text-[#1E293B]">Навички</h2>
            <p className="text-[#64748B]">
                Вкажіть ваші ключові навички та мови програмування
            </p>

            <div className="space-y-6">
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
                            onKeyDown={(e) =>
                                e.key === "Enter" && handleAddSkill()
                            }
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
                        Мови програмування
                    </label>
                    <div className="flex gap-2 items-center">
                        <input
                            type="text"
                            value={languageInput}
                            onChange={(e) => setLanguageInput(e.target.value)}
                            className="flex-1 p-3 border border-[#E2E8F0] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#6C5DD3] focus:border-[#6C5DD3]"
                            placeholder="Наприклад: JavaScript"
                            onKeyDown={(e) =>
                                e.key === "Enter" && handleAddLanguage()
                            }
                        />
                        <button
                            type="button"
                            onClick={handleAddLanguage}
                            className="p-3 bg-[#6C5DD3] text-white rounded-lg hover:bg-[#5B4DC4]"
                        >
                            <span className="material-icons">add</span>
                        </button>
                    </div>

                    <div className="mt-3 flex flex-wrap gap-2">
                        {commonLanguages.map((lang) => (
                            <button
                                key={lang}
                                type="button"
                                onClick={() => selectLanguage(lang)}
                                className="px-3 py-1 border border-[#E2E8F0] rounded-full text-sm text-[#64748B] hover:bg-[#F1F5F9]"
                            >
                                {lang}
                            </button>
                        ))}
                    </div>

                    <div className="mt-3 flex flex-wrap gap-2">
                        {data.programmingLanguages.map((language, index) => (
                            <div
                                key={index}
                                className="flex items-center bg-[#F1F5F9] text-[#64748B] px-3 py-1 rounded-full"
                            >
                                <span>{language}</span>
                                <button
                                    type="button"
                                    onClick={() =>
                                        handleRemoveLanguage(language)
                                    }
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

export default Step3SkillsExpertise;
