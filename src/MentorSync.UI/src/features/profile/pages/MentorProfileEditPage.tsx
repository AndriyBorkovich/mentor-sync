import React, { useState, useEffect } from "react";
import { useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

import EditProfileLayout from "../components/EditProfileLayout";
import AvatarSection from "../components/AvatarSection";
import { useUserProfile } from "../../auth/hooks/useUserProfile";
import {
    UpdateMentorProfileRequest,
    updateMentorProfile,
} from "../services/profileService";
import { programmingLanguages } from "../../../shared/constants/programmingLanguages";
import {
    timeOfDayOptions,
    Availability,
} from "../../../shared/enums/availability";
import { Industry, industriesMapping } from "../../../shared/enums/industry";

const MentorProfileEditPage: React.FC = () => {
    const navigate = useNavigate();
    const { profile, loading, error, updateProfile } = useUserProfile();
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [skillInput, setSkillInput] = useState("");
    const [languageInput, setLanguageInput] = useState("");
    const [avatarUrl, setAvatarUrl] = useState<string | undefined>(undefined);

    const {
        register,
        handleSubmit,
        control,
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
            experienceYears: 0,
            availability: Availability.Morning,
        },
    });

    const watchSkills = watch("skills");
    const watchProgrammingLanguages = watch("programmingLanguages");

    useEffect(() => {
        // Initialize form when profile is loaded
        if (profile?.mentorProfile && !loading) {
            setValue("bio", profile.mentorProfile?.bio || "");
            setValue("position", profile.mentorProfile?.position || "");
            setValue("company", profile.mentorProfile?.company || "");
            setValue(
                "industry",
                profile.mentorProfile?.category
                    ? industriesMapping.find(
                          (opt) =>
                              opt.label === profile.mentorProfile?.category ||
                              ""
                      )?.value || Industry.WebDevelopment
                    : Industry.WebDevelopment
            );
            setValue("skills", profile.mentorProfile?.skills || []);
            setValue(
                "programmingLanguages",
                profile.mentorProfile?.programmingLanguages || []
            );
            setValue(
                "experienceYears",
                profile.mentorProfile?.experienceYears || 0
            );
            setValue("availability", Availability.Morning); // Default value since we don't have it in the profile
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

    const onCancel = () => {
        navigate("/profile");
    };

    const onSubmit = async (data: any) => {
        if (!profile || !profile.mentorProfile) {
            toast.error("Профіль не знайдено");
            return;
        }

        setIsSubmitting(true);

        try {
            const updateData: UpdateMentorProfileRequest = {
                id: profile.mentorProfile.id,
                bio: data.bio,
                position: data.position,
                company: data.company,
                industries: data.industry,
                skills: data.skills,
                programmingLanguages: data.programmingLanguages,
                experienceYears: parseInt(data.experienceYears, 10),
                availability: data.availability,
                mentorId: profile.id,
            };

            await updateMentorProfile(updateData);
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
            title="Редагування профілю ментора"
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
                                placeholder="Наприклад: Senior Developer"
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

                        <div>
                            <label className="block text-sm font-medium text-gray-700 mb-1">
                                Досвід (років)
                            </label>
                            <input
                                type="number"
                                {...register("experienceYears", {
                                    required: "Досвід є обов'язковим",
                                    min: {
                                        value: 0,
                                        message:
                                            "Досвід не може бути від'ємним",
                                    },
                                    max: {
                                        value: 50,
                                        message:
                                            "Досвід не може перевищувати 50 років",
                                    },
                                })}
                                className={`w-full px-3 py-2 border rounded-lg ${
                                    errors.experienceYears
                                        ? "border-red-500"
                                        : "border-gray-300"
                                }`}
                                min="0"
                                max="50"
                            />
                            {errors.experienceYears && (
                                <p className="text-red-500 text-xs mt-1">
                                    {errors.experienceYears.message as string}
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
                            placeholder="Розкажіть про себе, свій досвід та сферу експертизи..."
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
                                placeholder="Наприклад: Project Management"
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
                        Доступність
                    </h2>{" "}
                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                            Коли ви надаєте перевагу менторству?
                        </label>
                        <div className="grid grid-cols-1 md:grid-cols-2 gap-3 mt-1">
                            {timeOfDayOptions.map((option) => (
                                <div
                                    key={option.value}
                                    className="flex items-center p-2 border border-gray-200 rounded-lg"
                                >
                                    <input
                                        type="radio"
                                        id={`availability-${option.value}`}
                                        value={option.value}
                                        {...register("availability")}
                                        className="mr-2"
                                    />
                                    <label
                                        htmlFor={`availability-${option.value}`}
                                        className="text-sm"
                                    >
                                        {option.label}{" "}
                                        <span className="text-gray-500 text-xs">
                                            ({option.desc})
                                        </span>
                                    </label>
                                </div>
                            ))}
                        </div>
                    </div>
                </div>
            </form>
        </EditProfileLayout>
    );
};

export default MentorProfileEditPage;
