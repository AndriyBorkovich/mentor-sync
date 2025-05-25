import React, { useState, useEffect } from "react";
import { Material } from "../data/materials";
import MaterialCard from "./MaterialCard";
import MaterialsFilter from "./MaterialsFilter";

interface MaterialsContentProps {
    materials: Material[];
}

const MaterialsContent: React.FC<MaterialsContentProps> = ({ materials }) => {
    const [filteredMaterials, setFilteredMaterials] =
        useState<Material[]>(materials);
    const [filters, setFilters] = useState({
        search: "",
        types: [] as string[],
        tags: [] as string[],
        sortBy: "newest",
    });
    const [currentPage, setCurrentPage] = useState(1);
    const itemsPerPage = 9;

    // Extract all unique tags from materials
    const allTags = Array.from(
        new Set(materials.flatMap((material) => material.tags))
    ).sort();

    // Update filtered materials when source materials change
    useEffect(() => {
        setFilteredMaterials(materials);
    }, [materials]);

    // Apply filters when they change
    useEffect(() => {
        let result = [...materials];

        // Apply search filter
        if (filters.search) {
            const searchLower = filters.search.toLowerCase();
            result = result.filter(
                (item) =>
                    item.title.toLowerCase().includes(searchLower) ||
                    item.description.toLowerCase().includes(searchLower) ||
                    item.mentorName.toLowerCase().includes(searchLower) ||
                    item.tags.some((tag) =>
                        tag.toLowerCase().includes(searchLower)
                    )
            );
        }

        // Apply type filter
        if (filters.types.length > 0) {
            result = result.filter((item) => filters.types.includes(item.type));
        }

        // Apply tags filter
        if (filters.tags.length > 0) {
            result = result.filter((item) =>
                item.tags.some((tag) => filters.tags.includes(tag))
            );
        }

        // Apply sorting
        switch (filters.sortBy) {
            case "newest":
                // Assuming createdAt can be parsed as a date
                result.sort(
                    (a, b) =>
                        new Date(b.createdAt).getTime() -
                        new Date(a.createdAt).getTime()
                );
                break;
            case "oldest":
                result.sort(
                    (a, b) =>
                        new Date(a.createdAt).getTime() -
                        new Date(b.createdAt).getTime()
                );
                break;
            case "az":
                result.sort((a, b) => a.title.localeCompare(b.title));
                break;
            case "za":
                result.sort((a, b) => b.title.localeCompare(a.title));
                break;
            default:
                break;
        }

        setFilteredMaterials(result);
        setCurrentPage(1); // Reset to first page when filters change
    }, [filters, materials]);

    const handleFilterChange = (newFilters: {
        search: string;
        types: string[];
        tags: string[];
        sortBy: string;
    }) => {
        setFilters(newFilters);
    };

    // Calculate pagination
    const indexOfLastItem = currentPage * itemsPerPage;
    const indexOfFirstItem = indexOfLastItem - itemsPerPage;
    const currentItems = filteredMaterials.slice(
        indexOfFirstItem,
        indexOfLastItem
    );
    const totalPages = Math.ceil(filteredMaterials.length / itemsPerPage);

    // Handle page change
    const handlePageChange = (pageNumber: number) => {
        setCurrentPage(pageNumber);
    };

    // Generate page numbers
    const pageNumbers = [];
    for (let i = 1; i <= totalPages; i++) {
        pageNumbers.push(i);
    }

    return (
        <div className="container mx-auto px-4 py-6">
            <MaterialsFilter
                onFilterChange={handleFilterChange}
                availableTags={allTags}
            />

            {filteredMaterials.length === 0 ? (
                <div className="text-center py-12">
                    <div className="mb-4">
                        <span className="material-icons text-[#94A3B8] text-5xl">
                            search_off
                        </span>
                    </div>
                    <h3 className="text-xl font-medium text-[#1E293B] mb-2">
                        Матеріали не знайдено
                    </h3>
                    <p className="text-[#64748B]">
                        Спробуйте змінити параметри пошуку або фільтри
                    </p>
                </div>
            ) : (
                <>
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                        {currentItems.map((material) => (
                            <MaterialCard
                                key={material.id}
                                material={material}
                            />
                        ))}
                    </div>

                    {totalPages > 1 && (
                        <div className="flex justify-center mt-8">
                            <nav className="flex items-center">
                                <button
                                    onClick={() =>
                                        handlePageChange(currentPage - 1)
                                    }
                                    disabled={currentPage === 1}
                                    className="flex items-center px-3 py-2 rounded-md mr-2 text-[#64748B] disabled:opacity-50"
                                >
                                    <span className="material-icons">
                                        chevron_left
                                    </span>
                                </button>

                                {pageNumbers.map((number) => (
                                    <button
                                        key={number}
                                        onClick={() => handlePageChange(number)}
                                        className={`px-4 py-2 mx-1 rounded-md ${
                                            number === currentPage
                                                ? "bg-[#6C5DD3] text-white"
                                                : "bg-white text-[#64748B] border border-[#E2E8F0]"
                                        }`}
                                    >
                                        {number}
                                    </button>
                                ))}

                                <button
                                    onClick={() =>
                                        handlePageChange(currentPage + 1)
                                    }
                                    disabled={currentPage === totalPages}
                                    className="flex items-center px-3 py-2 rounded-md ml-2 text-[#64748B] disabled:opacity-50"
                                >
                                    <span className="material-icons">
                                        chevron_right
                                    </span>
                                </button>
                            </nav>
                        </div>
                    )}
                </>
            )}
        </div>
    );
};

export default MaterialsContent;
