import React, { useState, useEffect } from "react";
import { EnhancedMentorCard } from "./EnhancedMentorCard";
import { mentorSearchService } from "../services/mentorSearchService";
import { Industry, industriesMapping } from "../../../shared/enums/industry";
import { programmingLanguages } from "../../../shared/constants/programmingLanguages";
import { Mentor, RecommendedMentor } from "../../../shared/types";

// Tabs for mentors and recommended mentors
type TabType = "mentors" | "recommendedMentors";

const MentorSearchContent: React.FC = () => {
    const [activeTab, setActiveTab] = useState<TabType>("mentors");
    const [searchTerm, setSearchTerm] = useState("");
    const [selectedSkills, setSelectedSkills] = useState<string[]>([]);
    const [showFilters, setShowFilters] = useState(true);
    const [minExperience, setMinExperience] = useState<number>(1); // added slider state
    const [mentors, setMentors] = useState<Mentor[]>([]);
    const [recommendedMentors, setRecommendedMentors] = useState<
        RecommendedMentor[]
    >([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    // Direction filters - store Industry enum values instead of strings
    const [selectedDirections, setSelectedDirections] = useState<Industry[]>(
        []
    );

    const directionCategories = Object.values(Industry)
        .filter((v) => typeof v === "number")
        .slice(1) as Industry[];

    // Show more/less state for directions and programming languages
    const [showAllDirections, setShowAllDirections] = useState(false);
    const [showAllLanguages, setShowAllLanguages] = useState(false);
    const DIRECTIONS_PREVIEW_COUNT = 5;
    const LANGUAGES_PREVIEW_COUNT = 5;

    // Helper function to toggle skill selection
    const toggleSkill = (skill: string) => {
        if (selectedSkills.includes(skill)) {
            setSelectedSkills(selectedSkills.filter((s) => s !== skill));
        } else {
            setSelectedSkills([...selectedSkills, skill]);
        }
    };

    // Helper function to toggle direction selection
    const toggleDirection = (direction: Industry) => {
        if (selectedDirections.includes(direction)) {
            setSelectedDirections(
                selectedDirections.filter((d) => d !== direction)
            );
        } else {
            setSelectedDirections([...selectedDirections, direction]);
        }
    };

    // Helper function to get label for an Industry enum value
    const getIndustryLabel = (industry: Industry): string => {
        const found = industriesMapping.find((item) => item.value === industry);
        return found ? found.label : "Unknown";
    };

    // Fetch mentors from API based on filters
    const fetchMentors = async () => {
        setLoading(true);
        setError(null);

        try {
            // Determine industry filter based on selected directions
            let industryFilter: Industry | undefined;
            if (selectedDirections.length === 1) {
                industryFilter = selectedDirections[0];
            }

            const response = await mentorSearchService.searchMentors({
                searchTerm: searchTerm || undefined,
                programmingLanguages:
                    selectedSkills.length > 0 ? selectedSkills : undefined,
                industry: industryFilter,
                minExperienceYears: minExperience,
            });

            if (response.success && response.data) {
                setMentors(response.data);
            } else {
                setError(response.error || "Failed to load mentors");
            }
        } catch (err) {
            console.error("Error fetching mentors:", err);
            setError("An error occurred while fetching mentors");
        } finally {
            setLoading(false);
        }
    }; // Fetch recommended mentors from API based on filters
    const fetchRecommendedMentors = async () => {
        setLoading(true);
        setError(null);

        try {
            // Determine industry filter based on selected directions
            let industryFilter: Industry | undefined;
            if (selectedDirections.length === 1) {
                industryFilter = selectedDirections[0];
            }

            const response = await mentorSearchService.getRecommendedMentors({
                searchTerm: searchTerm || undefined,
                programmingLanguages:
                    selectedSkills.length > 0 ? selectedSkills : undefined,
                industry: industryFilter,
                minExperienceYears: minExperience,
            });

            if (response.success && response.data) {
                setRecommendedMentors(response.data);
            } else {
                setError(
                    response.error || "Failed to load recommended mentors"
                );
            }
        } catch (err) {
            console.error("Error fetching recommended mentors:", err);
            setError("An error occurred while fetching recommended mentors");
        } finally {
            setLoading(false);
        }
    };

    // Call the API when filters change
    useEffect(() => {
        // Use a debounce to avoid too many API calls while typing
        const timeoutId = setTimeout(() => {
            if (activeTab === "mentors") {
                fetchMentors();
            } else {
                fetchRecommendedMentors();
            }
        }, 500);
        return () => clearTimeout(timeoutId);
    }, [
        searchTerm,
        selectedSkills,
        selectedDirections,
        minExperience,
        activeTab,
    ]);

    // Get saved mentors    // Determine which mentors to display based on active tab
    const mentorsToDisplay =
        activeTab === "mentors" ? mentors : recommendedMentors;

    return (
        <div className="flex h-full bg-[#F8FAFC]">
            {/* Filter Panel */}
            {showFilters && (
                <div className="w-64 bg-white border-r border-[#E2E8F0] p-4 overflow-y-auto">
                    <div className="mb-6">
                        <h2 className="text-lg font-bold text-[#1E293B] mb-3">
                            Фільтри
                        </h2>
                    </div>

                    {/* Search input */}
                    <div className="mb-5">
                        <div className="font-medium text-[#1E293B] mb-2">
                            Пошук
                        </div>
                        <div className="relative">
                            <input
                                type="text"
                                placeholder="Шукати менторів..."
                                className="w-full p-2 pl-3 text-sm border border-[#E2E8F0] rounded-md focus:ring-[#4318D1] focus:border-[#4318D1]"
                                value={searchTerm}
                                onChange={(e) => setSearchTerm(e.target.value)}
                            />
                        </div>
                    </div>

                    {/* Direction Filters */}
                    <div className="mb-5">
                        <div className="font-medium text-[#1E293B] mb-2">
                            Напрями
                        </div>
                        <div className="space-y-2">
                            {(showAllDirections
                                ? directionCategories
                                : directionCategories.slice(
                                      0,
                                      DIRECTIONS_PREVIEW_COUNT
                                  )
                            ).map((direction) => (
                                <div
                                    key={direction}
                                    className={`flex items-center cursor-pointer px-3 py-1 ${
                                        selectedDirections.includes(direction)
                                            ? "bg-[#4318D1] text-white"
                                            : "bg-[#F8FAFC] text-[#1E293B]"
                                    } rounded-2xl text-sm hover:bg-gray-400`}
                                    onClick={() => toggleDirection(direction)}
                                >
                                    {getIndustryLabel(direction)}
                                </div>
                            ))}
                            {directionCategories.length >
                                DIRECTIONS_PREVIEW_COUNT && (
                                <button
                                    className="text-xs text-[#4318D1] mt-1 focus:outline-none"
                                    onClick={() =>
                                        setShowAllDirections((v) => !v)
                                    }
                                >
                                    {showAllDirections
                                        ? "Показати менше"
                                        : "Показати більше"}
                                </button>
                            )}
                        </div>
                    </div>

                    {/* Programming Languages */}
                    <div className="mb-5">
                        <div className="font-medium text-[#1E293B] mb-2">
                            Мови програмування
                        </div>
                        <div className="space-y-2">
                            {(showAllLanguages
                                ? programmingLanguages
                                : programmingLanguages.slice(
                                      0,
                                      LANGUAGES_PREVIEW_COUNT
                                  )
                            ).map((language) => (
                                <div
                                    key={language}
                                    className={`flex items-center cursor-pointer text-sm ${
                                        selectedSkills.includes(language)
                                            ? "text-[#4318D1] font-medium"
                                            : "text-[#64748B]"
                                    } hover:text-[#1E293B]`}
                                    onClick={() => toggleSkill(language)}
                                >
                                    {language}
                                </div>
                            ))}
                            {programmingLanguages.length >
                                LANGUAGES_PREVIEW_COUNT && (
                                <button
                                    className="text-xs text-[#4318D1] mt-1 focus:outline-none"
                                    onClick={() =>
                                        setShowAllLanguages((v) => !v)
                                    }
                                >
                                    {showAllLanguages
                                        ? "Показати менше"
                                        : "Показати більше"}
                                </button>
                            )}
                        </div>
                    </div>

                    {/* Experience Slider */}
                    <div className="mb-5">
                        <div className="font-medium text-[#1E293B] mb-2">
                            Досвід (мінімум), років
                        </div>
                        <div className="flex items-center">
                            <input
                                type="range"
                                min={1}
                                max={10}
                                step={1}
                                value={minExperience}
                                onChange={(e) =>
                                    setMinExperience(Number(e.target.value))
                                }
                                className="w-full accent-[#4318D1]"
                            />
                            <span className="ml-2 text-sm text-[#1E293B]">
                                {minExperience < 10 ? minExperience : "10+"}
                            </span>
                        </div>
                    </div>
                </div>
            )}

            {/* Main Content */}
            <div className="flex-1 p-6 overflow-y-auto">
                <div className="mb-8">
                    <div className="flex items-center justify-between mb-6">
                        <h1 className="text-2xl font-bold text-[#1E293B]">
                            Пошук менторів
                        </h1>
                        <button
                            onClick={() => setShowFilters(!showFilters)}
                            className="p-2 text-[#64748B] hover:text-[#1E293B]"
                        >
                            <span className="material-icons">
                                {showFilters
                                    ? "filter_list_off"
                                    : "filter_list"}
                            </span>
                        </button>
                    </div>

                    {/* Selected filters chips */}
                    {(selectedSkills.length > 0 ||
                        selectedDirections.length > 0) && (
                        <div className="flex flex-wrap gap-2 mb-4">
                            {selectedSkills.map((skill) => (
                                <div
                                    key={`selected-${skill}`}
                                    className="px-3 py-1 bg-[#4318D1] text-white rounded-2xl text-sm flex items-center"
                                >
                                    {skill}
                                    <span
                                        className="material-icons text-white ml-1 text-sm cursor-pointer"
                                        onClick={() => toggleSkill(skill)}
                                    >
                                        close
                                    </span>
                                </div>
                            ))}

                            {selectedDirections.map((direction) => (
                                <div
                                    key={`selected-dir-${direction}`}
                                    className="px-3 py-1 bg-[#4318D1] text-white rounded-2xl text-sm flex items-center"
                                >
                                    {getIndustryLabel(direction)}
                                    <span
                                        className="material-icons text-white ml-1 text-sm cursor-pointer"
                                        onClick={() =>
                                            toggleDirection(direction)
                                        }
                                    >
                                        close
                                    </span>
                                </div>
                            ))}
                        </div>
                    )}

                    {/* Tabs */}
                    <div className="border-b border-[#E2E8F0] mb-6">
                        <div className="flex space-x-8">
                            {" "}
                            <button
                                className={`pb-4 font-medium text-base transition-colors ${
                                    activeTab === "mentors"
                                        ? "text-[#4318D1] border-b-2 border-[#4318D1]"
                                        : "text-[#64748B] hover:text-[#1E293B]"
                                }`}
                                onClick={() => {
                                    setActiveTab("mentors");
                                    if (mentors.length === 0) {
                                        fetchMentors();
                                    }
                                }}
                            >
                                Усі ментори
                            </button>
                            <button
                                className={`pb-4 font-medium text-base transition-colors ${
                                    activeTab === "recommendedMentors"
                                        ? "text-[#4318D1] border-b-2 border-[#4318D1]"
                                        : "text-[#64748B] hover:text-[#1E293B]"
                                }`}
                                onClick={() => {
                                    setActiveTab("recommendedMentors");
                                    if (recommendedMentors.length === 0) {
                                        fetchRecommendedMentors();
                                    }
                                }}
                            >
                                {" "}
                                Мої рекомендації
                                {/* {savedMentorIds.length > 0 && (
                                    <span className="ml-2 px-2 py-0.5 bg-[#4318D1] text-white text-xs rounded-full">
                                        {savedMentorIds.length}
                                    </span>
                                )} */}
                            </button>
                        </div>
                    </div>
                </div>
                {/* Loading state */}
                {loading && (
                    <div className="col-span-3 text-center py-12">
                        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-[#4318D1] mx-auto"></div>
                        <p className="mt-4 text-[#64748B]">
                            Завантаження менторів...
                        </p>
                    </div>
                )}
                {/* Error state */}
                {!loading && error && (
                    <div className="col-span-3 text-center py-12">
                        <div className="text-red-500 mb-2">
                            <span className="material-icons text-4xl">
                                error_outline
                            </span>
                        </div>
                        <p className="text-red-500">{error}</p>{" "}
                        <button
                            className="mt-4 px-4 py-2 bg-[#4318D1] text-white rounded-md hover:bg-[#3a15b3]"
                            onClick={
                                activeTab === "mentors"
                                    ? fetchMentors
                                    : fetchRecommendedMentors
                            }
                        >
                            Спробувати ще раз
                        </button>
                    </div>
                )}{" "}
                {/* Grid of mentor cards */}
                {!loading && !error && (
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                        {" "}
                        {mentorsToDisplay.map((mentor) => (
                            <EnhancedMentorCard
                                key={mentor.id}
                                mentor={mentor}
                                showRecommendationScores={
                                    activeTab === "recommendedMentors"
                                }
                            />
                        ))}
                        {mentorsToDisplay.length === 0 && (
                            <div className="col-span-3 text-center py-12">
                                {" "}
                                <p className="text-[#64748B] text-lg">
                                    {activeTab === "recommendedMentors"
                                        ? "У вас ще немає рекомендованих менторів. Взаємодійте з платформою, щоб отримати персоналізовані рекомендації."
                                        : "Не знайдено менторів за вашими критеріями пошуку"}
                                </p>
                            </div>
                        )}
                    </div>
                )}
            </div>
        </div>
    );
};

export default MentorSearchContent;
