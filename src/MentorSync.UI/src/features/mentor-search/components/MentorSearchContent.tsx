import React, { useState } from "react";
import { recommendedMentors } from "../../dashboard/data/mentors";
import { EnhancedMentorCard } from "./EnhancedMentorCard";

// Tabs for mentors and saved mentors
type TabType = "mentors" | "recommendedMentors";

const MentorSearchContent: React.FC = () => {
    const [activeTab, setActiveTab] = useState<TabType>("mentors");
    const [searchTerm, setSearchTerm] = useState("");
    const [selectedSkills, setSelectedSkills] = useState<string[]>([]);
    const [savedMentorIds, setSavedMentorIds] = useState<string[]>([]);
    const [showFilters, setShowFilters] = useState(true);
    const [minExperience, setMinExperience] = useState<number>(1); // added slider state

    // Direction filters
    const [selectedDirections, setSelectedDirections] = useState<string[]>([]);

    // Direction categories based on design
    const directionCategories = [
        "Software Development",
        "Data Science",
        "Cloud Computing",
        "Cybersecurity",
        "AI/ML",
        "DevOps",
    ];

    // Programming languages based on design
    const programmingLanguages = [
        "JavaScript",
        "Python",
        "C++",
        "Ruby",
        "Go",
        "Java",
        "TypeScript",
        "R",
    ];

    // Helper function to toggle skill selection
    const toggleSkill = (skill: string) => {
        if (selectedSkills.includes(skill)) {
            setSelectedSkills(selectedSkills.filter((s) => s !== skill));
        } else {
            setSelectedSkills([...selectedSkills, skill]);
        }
    };

    // Helper function to toggle direction selection
    const toggleDirection = (direction: string) => {
        if (selectedDirections.includes(direction)) {
            setSelectedDirections(
                selectedDirections.filter((d) => d !== direction)
            );
        } else {
            setSelectedDirections([...selectedDirections, direction]);
        }
    };

    // Toggle mentor saved status
    const toggleSaveMentor = (mentorId: string) => {
        if (savedMentorIds.includes(mentorId)) {
            setSavedMentorIds(savedMentorIds.filter((id) => id !== mentorId));
        } else {
            setSavedMentorIds([...savedMentorIds, mentorId]);
        }
    };

    // Filter mentors based on search term, selected skills, and directions
    const filteredMentors = recommendedMentors.filter((mentor) => {
        const matchesSearch =
            searchTerm === "" ||
            mentor.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
            mentor.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
            mentor.skills.some((skill) =>
                skill.name.toLowerCase().includes(searchTerm.toLowerCase())
            );

        const matchesSkills =
            selectedSkills.length === 0 ||
            mentor.skills.some((skill) => selectedSkills.includes(skill.name));

        const matchesDirections =
            selectedDirections.length === 0 ||
            selectedDirections.includes(mentor.category ?? "");
        const matchesExperience =
            mentor.yearsOfExperience ?? 0 >= minExperience; // filter by experience

        return (
            matchesSearch &&
            matchesSkills &&
            matchesDirections &&
            matchesExperience
        );
    });

    // Get saved mentors
    const savedMentors = recommendedMentors.filter((mentor) =>
        savedMentorIds.includes(mentor.id)
    );

    // Determine which mentors to display based on active tab
    const mentorsToDisplay =
        activeTab === "mentors" ? filteredMentors : savedMentors;

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
                            {directionCategories.map((direction) => (
                                <div
                                    key={direction}
                                    className="flex items-center cursor-pointer px-3 py-1 bg-[#F8FAFC] rounded-2xl text-[#1E293B] text-sm  hover:bg-gray-400"
                                    onClick={() => toggleDirection(direction)}
                                >
                                    {direction}
                                </div>
                            ))}
                        </div>
                    </div>

                    {/* Programming Languages */}
                    <div className="mb-5">
                        <div className="font-medium text-[#1E293B] mb-2">
                            Мови програмування
                        </div>
                        <div className="space-y-2">
                            {programmingLanguages.map((language) => (
                                <div
                                    key={language}
                                    className="flex items-center cursor-pointer text-sm text-[#64748B] hover:text-[#1E293B]"
                                    onClick={() => toggleSkill(language)}
                                >
                                    {language}
                                </div>
                            ))}
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

                    {/* Communication Method */}
                    <div className="mb-5">
                        <div className="font-medium text-[#1E293B] mb-2">
                            Мова спілкування
                        </div>
                        <div className="relative w-full">
                            <select className="block w-full p-2 text-sm border border-[#E2E8F0] rounded-md focus:ring-[#4318D1] focus:border-[#4318D1] appearance-none bg-white">
                                <option value="">Будь-яка</option>
                                <option value="uk">Українська</option>
                                <option value="en">Англійська</option>
                            </select>
                            <div className="absolute inset-y-0 right-0 flex items-center pr-2 pointer-events-none">
                                <span className="material-icons text-[#94A3B8] text-lg">
                                    expand_more
                                </span>
                            </div>
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
                                    {direction}
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
                            <button
                                className={`pb-4 font-medium text-base transition-colors ${
                                    activeTab === "mentors"
                                        ? "text-[#4318D1] border-b-2 border-[#4318D1]"
                                        : "text-[#64748B] hover:text-[#1E293B]"
                                }`}
                                onClick={() => setActiveTab("mentors")}
                            >
                                Усі ментори
                            </button>
                            <button
                                className={`pb-4 font-medium text-base transition-colors ${
                                    activeTab === "recommendedMentors"
                                        ? "text-[#4318D1] border-b-2 border-[#4318D1]"
                                        : "text-[#64748B] hover:text-[#1E293B]"
                                }`}
                                onClick={() =>
                                    setActiveTab("recommendedMentors")
                                }
                            >
                                {" "}
                                Мої рекомендації
                                {savedMentorIds.length > 0 && (
                                    <span className="ml-2 px-2 py-0.5 bg-[#4318D1] text-white text-xs rounded-full">
                                        {savedMentorIds.length}
                                    </span>
                                )}
                            </button>
                        </div>
                    </div>
                </div>

                {/* Grid of mentor cards */}
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                    {mentorsToDisplay.map((mentor) => (
                        <EnhancedMentorCard
                            key={mentor.id}
                            mentor={mentor}
                            isSaved={savedMentorIds.includes(mentor.id)}
                            onToggleSave={toggleSaveMentor}
                        />
                    ))}
                    {mentorsToDisplay.length === 0 && (
                        <div className="col-span-3 text-center py-12">
                            <p className="text-[#64748B] text-lg">
                                {activeTab === "recommendedMentors"
                                    ? "У вас ще немає рекомендованих менторів"
                                    : "Не знайдено менторів за вашими критеріями пошуку"}
                            </p>
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default MentorSearchContent;
