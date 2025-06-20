import React, { useState, useEffect } from "react";
import { useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

import EditProfileLayout from "../components/EditProfileLayout";
import AvatarSection from "../components/AvatarSection";
import { useUserProfile } from "../../auth/hooks/useUserProfile";
import {
    UpdateMenteeProfileRequest,
    updateMenteeProfile,
} from "../services/profileService";
import { programmingLanguages } from "../../../shared/constants/programmingLanguages";
import { Industry, industriesMapping } from "../../../shared/enums/industry";

const MenteeProfileEditPage: React.FC = () => {
    const navigate = useNavigate();
    const { profile, loading, error, updateProfile } = useUserProfile();
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [skillInput, setSkillInput] = useState("");
    const [languageInput, setLanguageInput] = useState("");
    const [goalInput, setGoalInput] = useState("");
    const [avatarUrl, setAvatarUrl] = useState<string | undefined>(undefined);

    const {
        register,
        handleSubmit,
        setValue,
        watch,
        formState: { errors },
    } = useForm<any>({
        defaultValues: {
            bio: "",
            position: "",
            company: "",
            industry: Industry.WebDevelopment,
            skills: [],
            programmingLanguages: [],
            learningGoals: [],
        },
    });

    const watchSkills = watch("skills");
    const watchProgrammingLanguages = watch("programmingLanguages");
    const watchLearningGoals = watch("learningGoals");

    useEffect(() => {
        // Initialize form when profile is loaded
        if (profile?.menteeProfile && !loading) {
            setValue("bio", profile.menteeProfile?.bio || "");
            setValue("position", profile.menteeProfile?.position || "");
            setValue("company", profile.menteeProfile?.company || "");
            setValue(
                "industry",
                profile.menteeProfile?.category
                    ? industriesMapping.find(
                          (opt) =>
                              opt.label === profile.menteeProfile?.category ||
                              ""
                      )?.value || Industry.WebDevelopment
                    : Industry.WebDevelopment
            );
            setValue("skills", profile.menteeProfile?.skills || []);
            setValue(
                "programmingLanguages",
                profile.menteeProfile?.programmingLanguages || []
            );
            setValue(
                "learningGoals",
                profile.menteeProfile?.learningGoals || []
            );
            // Set avatar URL from profile only if it hasn't been modified by user actions
            if (avatarUrl === undefined) {
                setAvatarUrl(profile.profileImageUrl);
            }
        }
    }, [profile, loading, setValue]);

    const handleAddSkill = () => {
        if (skillInput.trim() && !watchSkills.includes(skillInput.trim())) {
            const updatedSkills = [...watchSkills, skillInput.trim()];
            setValue("skills", updatedSkills);
            setSkillInput("");
        }
    };

    const handleRemoveSkill = (skill: string) => {
        setValue(
            "skills",
            watchSkills.filter((s: string) => s !== skill)
        );
    };

    const handleAddLanguage = () => {
        if (
            languageInput.trim() &&
            !watchProgrammingLanguages.includes(languageInput.trim())
        ) {
            const updatedLanguages = [
                ...watchProgrammingLanguages,
                languageInput.trim(),
            ];
            setValue("programmingLanguages", updatedLanguages);
            setLanguageInput("");
        }
    };

    const handleRemoveLanguage = (language: string) => {
        setValue(
            "programmingLanguages",
            watchProgrammingLanguages.filter((l: string) => l !== language)
        );
    };

    const handleAddGoal = () => {
        if (
            goalInput.trim() &&
            !watchLearningGoals.includes(goalInput.trim())
        ) {
            const updatedGoals = [...watchLearningGoals, goalInput.trim()];
            setValue("learningGoals", updatedGoals);
            setGoalInput("");
        }
    };

    const handleRemoveGoal = (goal: string) => {
        setValue(
            "learningGoals",
            watchLearningGoals.filter((g: string) => g !== goal)
        );
    };

    const onCancel = () => {
        navigate("/profile");
    };

    const onSubmit = async (data: any) => {
        if (!profile || !profile.menteeProfile) {
            toast.error("Профіль не знайдено");
            return;
        }

        setIsSubmitting(true);

        try {
            const updateData: UpdateMenteeProfileRequest = {
                id: profile.menteeProfile.id,
                bio: data.bio,
                position: data.position,
                company: data.company,
                industries: data.industry,
                skills: data.skills,
                programmingLanguages: data.programmingLanguages,
                learningGoals: data.learningGoals,
                menteeId: profile.id,
            };

            await updateMenteeProfile(updateData);
            await updateProfile?.();
            navigate("/profile");
        } catch (error) {
            console.error("Error updating profile:", error);
        } finally {
            setIsSubmitting(false);
        }
    };
    const handleAvatarUpdate = (newAvatarUrl: string | null) => {
        setAvatarUrl(newAvatarUrl || undefined);
        if (profile && updateProfile) {
            updateProfile().catch((error) => {
                console.error("Error updating profile with new avatar:", error);
            });
        }
    };

    if (loading) {
        return (
            <div className="flex justify-center items-center min-h-screen">
                <div className="w-12 h-12 border-4 border-primary border-t-transparent rounded-full animate-spin"></div>
            </div>
        );
    }

    if (error || !profile) {
        return (
            <div className="text-center p-8">
                <h2 className="text-xl text-red-600">
                    Помилка при завантаженні профілю
                </h2>
                <p className="mt-2 text-gray-600">
                    {error || "Профіль не знайдено"}
                </p>
                <button
                    className="mt-4 px-4 py-2 bg-primary text-white rounded-lg"
                    onClick={() => navigate("/profile")}
                >
                    Повернутися до профілю
                </button>
            </div>
        );
    }

    return (
        <EditProfileLayout
            title="Редагування профілю менті"
            isSubmitting={isSubmitting}
            onCancel={onCancel}
            onSubmit={handleSubmit(onSubmit)}
        >
            <form onSubmit={(e) => e.preventDefault()}>
                {profile && (
                    <AvatarSection
                        userId={profile.id}
                        currentAvatarUrl={avatarUrl}
                        onAvatarUpdate={handleAvatarUpdate}
                    />
                )}

                <div className="mb-6">
                    <h2 className="text-lg font-medium text-gray-800 mb-4">
                        Основна інформація
                    </h2>

                    <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                        <div>
                            <label className="block text-sm font-medium text-gray-700 mb-1">
                                Посада
                            </label>
                            <input
                                type="text"
                                {...register("position", {
                                    required: "Посада є обов'язковою",
                                })}
                                className={`w-full px-3 py-2 border rounded-lg ${
                                    errors.position
                                        ? "border-red-500"
                                        : "border-gray-300"
                                }`}
                                placeholder="Наприклад: Junior Developer"
                            />
                            {errors.position && (
                                <p className="text-red-500 text-xs mt-1">
                                    {errors.position.message as string}
                                </p>
                            )}
                        </div>

                        <div>
                            <label className="block text-sm font-medium text-gray-700 mb-1">
                                Компанія
                            </label>
                            <input
                                type="text"
                                {...register("company", {
                                    required: "Компанія є обов'язковою",
                                })}
                                className={`w-full px-3 py-2 border rounded-lg ${
                                    errors.company
                                        ? "border-red-500"
                                        : "border-gray-300"
                                }`}
                                placeholder="Наприклад: Tech Solutions Inc."
                            />
                            {errors.company && (
                                <p className="text-red-500 text-xs mt-1">
                                    {errors.company.message as string}
                                </p>
                            )}
                        </div>

                        <div>
                            <label className="block text-sm font-medium text-gray-700 mb-1">
                                Категорія
                            </label>
                            <select
                                {...register("industry", {
                                    required: "Категорія є обов'язковою",
                                })}
                                className={`w-full px-3 py-2 border rounded-lg ${
                                    errors.industry
                                        ? "border-red-500"
                                        : "border-gray-300"
                                }`}
                            >
                                {industriesMapping.map((option) => (
                                    <option
                                        key={option.value}
                                        value={option.value}
                                    >
                                        {option.label}
                                    </option>
                                ))}
                            </select>
                            {errors.industry && (
                                <p className="text-red-500 text-xs mt-1">
                                    {errors.industry.message as string}
                                </p>
                            )}
                        </div>
                    </div>

                    <div className="mt-6">
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                            Біографія
                        </label>
                        <textarea
                            {...register("bio", {
                                required: "Біографія є обов'язковою",
                            })}
                            rows={5}
                            className={`w-full px-3 py-2 border rounded-lg ${
                                errors.bio
                                    ? "border-red-500"
                                    : "border-gray-300"
                            }`}
                            placeholder="Розкажіть про себе, свій досвід та сферу інтересів..."
                        ></textarea>
                        {errors.bio && (
                            <p className="text-red-500 text-xs mt-1">
                                {errors.bio.message as string}
                            </p>
                        )}
                    </div>
                </div>

                <div className="mb-6">
                    <h2 className="text-lg font-medium text-gray-800 mb-4">
                        Навички та технології
                    </h2>

                    <div className="mb-6">
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                            Навички
                        </label>
                        <div className="flex">
                            <input
                                type="text"
                                value={skillInput}
                                onChange={(e) => setSkillInput(e.target.value)}
                                className="flex-grow px-3 py-2 border border-gray-300 rounded-l-lg"
                                placeholder="Наприклад: UI Design"
                                onKeyPress={(e) => {
                                    if (e.key === "Enter") {
                                        e.preventDefault();
                                        handleAddSkill();
                                    }
                                }}
                            />
                            <button
                                type="button"
                                onClick={handleAddSkill}
                                className="px-4 py-2 bg-primary text-white rounded-r-lg"
                            >
                                Додати
                            </button>
                        </div>

                        <div className="mt-2 flex flex-wrap gap-2">
                            {watchSkills.map((skill: string, index: number) => (
                                <div
                                    key={index}
                                    className="bg-gray-100 text-gray-700 px-3 py-1 rounded-full flex items-center"
                                >
                                    <span>{skill}</span>
                                    <button
                                        type="button"
                                        onClick={() => handleRemoveSkill(skill)}
                                        className="ml-2 text-gray-500 hover:text-red-500"
                                    >
                                        ✕
                                    </button>
                                </div>
                            ))}
                            {watchSkills.length === 0 && (
                                <p className="text-gray-500 text-sm italic">
                                    Навички не додано
                                </p>
                            )}
                        </div>
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                            Мови програмування
                        </label>
                        <div className="flex">
                            <input
                                type="text"
                                value={languageInput}
                                onChange={(e) =>
                                    setLanguageInput(e.target.value)
                                }
                                className="flex-grow px-3 py-2 border border-gray-300 rounded-l-lg"
                                placeholder="Наприклад: JavaScript"
                                list="programming-languages"
                                onKeyPress={(e) => {
                                    if (e.key === "Enter") {
                                        e.preventDefault();
                                        handleAddLanguage();
                                    }
                                }}
                            />
                            <datalist id="programming-languages">
                                {programmingLanguages.map((lang, index) => (
                                    <option key={index} value={lang} />
                                ))}
                            </datalist>
                            <button
                                type="button"
                                onClick={handleAddLanguage}
                                className="px-4 py-2 bg-primary text-white rounded-r-lg"
                            >
                                Додати
                            </button>
                        </div>

                        <div className="mt-2 flex flex-wrap gap-2">
                            {watchProgrammingLanguages.map(
                                (lang: string, index: number) => (
                                    <div
                                        key={index}
                                        className="bg-gray-100 text-gray-700 px-3 py-1 rounded-full flex items-center"
                                    >
                                        <span>{lang}</span>
                                        <button
                                            type="button"
                                            onClick={() =>
                                                handleRemoveLanguage(lang)
                                            }
                                            className="ml-2 text-gray-500 hover:text-red-500"
                                        >
                                            ✕
                                        </button>
                                    </div>
                                )
                            )}
                            {watchProgrammingLanguages.length === 0 && (
                                <p className="text-gray-500 text-sm italic">
                                    Мови програмування не додано
                                </p>
                            )}
                        </div>
                    </div>
                </div>

                <div className="mb-6">
                    <h2 className="text-lg font-medium text-gray-800 mb-4">
                        Цілі навчання
                    </h2>

                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                            Ваші цілі навчання
                        </label>
                        <div className="flex">
                            <input
                                type="text"
                                value={goalInput}
                                onChange={(e) => setGoalInput(e.target.value)}
                                className="flex-grow px-3 py-2 border border-gray-300 rounded-l-lg"
                                placeholder="Наприклад: Вивчити React"
                                onKeyPress={(e) => {
                                    if (e.key === "Enter") {
                                        e.preventDefault();
                                        handleAddGoal();
                                    }
                                }}
                            />
                            <button
                                type="button"
                                onClick={handleAddGoal}
                                className="px-4 py-2 bg-primary text-white rounded-r-lg"
                            >
                                Додати
                            </button>
                        </div>

                        <div className="mt-2 flex flex-wrap gap-2">
                            {watchLearningGoals.map(
                                (goal: string, index: number) => (
                                    <div
                                        key={index}
                                        className="bg-gray-100 text-gray-700 px-3 py-1 rounded-full flex items-center"
                                    >
                                        <span>{goal}</span>
                                        <button
                                            type="button"
                                            onClick={() =>
                                                handleRemoveGoal(goal)
                                            }
                                            className="ml-2 text-gray-500 hover:text-red-500"
                                        >
                                            ✕
                                        </button>
                                    </div>
                                )
                            )}
                            {watchLearningGoals.length === 0 && (
                                <p className="text-gray-500 text-sm italic">
                                    Цілі не додано
                                </p>
                            )}
                        </div>
                    </div>
                </div>
            </form>
        </EditProfileLayout>
    );
};

export default MenteeProfileEditPage;
