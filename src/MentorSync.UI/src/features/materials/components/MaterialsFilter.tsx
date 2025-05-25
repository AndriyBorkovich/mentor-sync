import React, { useState } from "react";

interface MaterialsFilterProps {
    onFilterChange: (filters: {
        search: string;
        types: string[];
        tags: string[];
        sortBy: string;
    }) => void;
    availableTags: string[];
}

const MaterialsFilter: React.FC<MaterialsFilterProps> = ({
    onFilterChange,
    availableTags,
}) => {
    const [search, setSearch] = useState("");
    const [types, setTypes] = useState<string[]>([]);
    const [tags, setTags] = useState<string[]>([]);
    const [sortBy, setSortBy] = useState("newest");
    const [showFilters, setShowFilters] = useState(false);

    // Material types
    const materialTypes = [
        { id: "document", label: "Документи" },
        { id: "video", label: "Відео" },
        { id: "link", label: "Посилання" },
        { id: "presentation", label: "Презентації" },
    ];

    // Handle type toggle
    const handleTypeToggle = (typeId: string) => {
        setTypes((prevTypes) => {
            if (prevTypes.includes(typeId)) {
                return prevTypes.filter((t) => t !== typeId);
            } else {
                return [...prevTypes, typeId];
            }
        });
    };

    // Handle tag toggle
    const handleTagToggle = (tag: string) => {
        setTags((prevTags) => {
            if (prevTags.includes(tag)) {
                return prevTags.filter((t) => t !== tag);
            } else {
                return [...prevTags, tag];
            }
        });
    };

    // Apply filters
    const applyFilters = () => {
        onFilterChange({
            search,
            types,
            tags,
            sortBy,
        });
    };

    // Clear all filters
    const clearFilters = () => {
        setSearch("");
        setTypes([]);
        setTags([]);
        setSortBy("newest");
        onFilterChange({
            search: "",
            types: [],
            tags: [],
            sortBy: "newest",
        });
    };

    return (
        <div className="mb-6">
            <div className="flex flex-col md:flex-row md:items-center md:justify-between mb-4">
                <h1 className="text-2xl font-bold text-[#1E293B] mb-4 md:mb-0">
                    Матеріали
                </h1>

                <div className="flex space-x-2">
                    <button
                        onClick={() => setShowFilters(!showFilters)}
                        className="px-4 py-2 border border-[#E2E8F0] rounded-lg flex items-center text-[#64748B]"
                    >
                        <span className="material-icons mr-2 text-sm">
                            filter_list
                        </span>
                        Фільтри
                    </button>
                    <div className="relative">
                        <select
                            value={sortBy}
                            onChange={(e) => {
                                setSortBy(e.target.value);
                                setTimeout(applyFilters, 0);
                            }}
                            className="appearance-none px-4 py-2 border border-[#E2E8F0] rounded-lg text-[#64748B] pr-8"
                        >
                            <option value="newest">Найновіші</option>
                            <option value="oldest">Найстаріші</option>
                            <option value="az">А-Я</option>
                            <option value="za">Я-А</option>
                        </select>
                        <span className="absolute right-2 top-1/2 transform -translate-y-1/2 pointer-events-none text-[#64748B]">
                            <span className="material-icons">expand_more</span>
                        </span>
                    </div>
                </div>
            </div>

            <div className="relative mb-4">
                <span className="absolute inset-y-0 left-0 flex items-center pl-3">
                    <span className="material-icons text-[#64748B]">
                        search
                    </span>
                </span>
                <input
                    type="text"
                    placeholder="Пошук матеріалів..."
                    value={search}
                    onChange={(e) => setSearch(e.target.value)}
                    className="w-full py-3 pl-10 pr-4 border border-[#E2E8F0] rounded-lg focus:outline-none focus:border-[#6C5DD3] text-[#1E293B] placeholder-[#94A3B8]"
                    onKeyUp={(e) => {
                        if (e.key === "Enter") {
                            applyFilters();
                        }
                    }}
                />
                <button
                    className="absolute right-3 top-1/2 transform -translate-y-1/2 text-[#6C5DD3]"
                    onClick={applyFilters}
                >
                    Знайти
                </button>
            </div>

            {showFilters && (
                <div className="bg-white p-4 rounded-lg shadow-sm mb-4 border border-[#E2E8F0]">
                    <div className="mb-4">
                        <h3 className="font-medium text-[#1E293B] mb-2">
                            Тип матеріалу
                        </h3>
                        <div className="flex flex-wrap gap-2">
                            {materialTypes.map((type) => (
                                <button
                                    key={type.id}
                                    onClick={() => handleTypeToggle(type.id)}
                                    className={`px-3 py-1 rounded-full text-sm ${
                                        types.includes(type.id)
                                            ? "bg-[#6C5DD3] text-white"
                                            : "bg-[#F1F5F9] text-[#64748B]"
                                    }`}
                                >
                                    {type.label}
                                </button>
                            ))}
                        </div>
                    </div>

                    <div className="mb-4">
                        <h3 className="font-medium text-[#1E293B] mb-2">
                            Теги
                        </h3>
                        <div className="flex flex-wrap gap-2">
                            {availableTags.map((tag) => (
                                <button
                                    key={tag}
                                    onClick={() => handleTagToggle(tag)}
                                    className={`px-3 py-1 rounded-full text-sm ${
                                        tags.includes(tag)
                                            ? "bg-[#6C5DD3] text-white"
                                            : "bg-[#F1F5F9] text-[#64748B]"
                                    }`}
                                >
                                    {tag}
                                </button>
                            ))}
                        </div>
                    </div>

                    <div className="flex justify-end">
                        <button
                            onClick={clearFilters}
                            className="px-4 py-2 text-[#64748B] mr-2"
                        >
                            Очистити
                        </button>
                        <button
                            onClick={applyFilters}
                            className="px-4 py-2 bg-[#6C5DD3] text-white rounded-lg"
                        >
                            Застосувати
                        </button>
                    </div>
                </div>
            )}
        </div>
    );
};

export default MaterialsFilter;
