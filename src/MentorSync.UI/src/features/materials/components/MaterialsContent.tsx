import React from "react";
import MaterialCard from "./MaterialCard";
import MaterialsFilter from "./MaterialsFilter";
import { Material, RecommendedMaterial } from "../../../shared/types";

interface MaterialsContentProps {
    materials: Material[] | RecommendedMaterial[];
    onFilterChange: (filters: any) => void;
    currentFilters: {
        search: string;
        types: string[];
        tags: string[];
        sortBy: string;
        pageNumber: number;
        pageSize: number;
    };
    totalCount?: number;
    isRecommended?: boolean;
}

const MaterialsContent: React.FC<MaterialsContentProps> = ({
    materials,
    onFilterChange,
    currentFilters,
    totalCount = 0,
    isRecommended = false,
}) => {
    // Use the current page directly from filters
    const currentPage = currentFilters.pageNumber; // Extract all unique tags from materials
    const allTags = Array.from(
        new Set(materials.flatMap((material) => material.tags || []))
    ).sort();

    // Handle page change
    const handlePageChange = (page: number) => {
        console.log("Changing to page:", page);
        onFilterChange({ ...currentFilters, pageNumber: page });
    }; // Handle filter changes
    const handleFilterUpdate = (newFilters: {
        search?: string;
        types?: string[];
        tags?: string[];
        sortBy?: string;
    }) => {
        onFilterChange({
            ...currentFilters,
            ...newFilters,
            pageNumber: 1, // Reset to page 1 when filters change
        });
    };

    const itemsPerPage = currentFilters.pageSize;

    const totalPages = Math.max(1, Math.ceil(totalCount / itemsPerPage));

    // Generate page numbers
    const pageNumbers = [];
    for (let i = 1; i <= totalPages; i++) {
        pageNumbers.push(i);
    }

    return (
        <div className="container mx-auto px-4 py-6">
            <MaterialsFilter
                onFilterChange={handleFilterUpdate}
                availableTags={allTags}
                currentFilters={currentFilters}
            />

            {materials.length === 0 ? (
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
                    {" "}
                    <div className="mb-4 text-sm text-[#64748B]">
                        Показано {materials.length} з {totalCount} матеріалів
                    </div>
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                        {" "}
                        {materials.map((material) => (
                            <MaterialCard
                                key={material.id}
                                material={material}
                                showRecommendationScores={isRecommended}
                            />
                        ))}
                    </div>{" "}
                    {totalPages > 1 && (
                        <div className="flex justify-center mt-8">
                            <nav className="flex items-center">
                                <button
                                    onClick={() => handlePageChange(1)}
                                    disabled={currentPage === 1}
                                    className="flex items-center px-2 py-2 rounded-md mr-1 text-[#64748B] disabled:opacity-50"
                                    title="Перша сторінка"
                                >
                                    <span className="material-icons">
                                        first_page
                                    </span>
                                </button>

                                <button
                                    onClick={() =>
                                        handlePageChange(currentPage - 1)
                                    }
                                    disabled={currentPage === 1}
                                    className="flex items-center px-2 py-2 rounded-md mr-2 text-[#64748B] disabled:opacity-50"
                                    title="Попередня сторінка"
                                >
                                    <span className="material-icons">
                                        chevron_left
                                    </span>
                                </button>

                                {totalPages <= 7 ? (
                                    // When there are 7 or fewer pages, show all pages
                                    pageNumbers.map((number) => (
                                        <button
                                            key={number}
                                            onClick={() =>
                                                handlePageChange(number)
                                            }
                                            className={`px-4 py-2 mx-1 rounded-md ${
                                                number === currentPage
                                                    ? "bg-[#6C5DD3] text-white"
                                                    : "bg-white text-[#64748B] border border-[#E2E8F0]"
                                            }`}
                                        >
                                            {number}
                                        </button>
                                    ))
                                ) : (
                                    // When there are more than 7 pages, show some pages with ellipsis
                                    <>
                                        {/* Always show first page */}
                                        {currentPage > 3 && (
                                            <button
                                                onClick={() =>
                                                    handlePageChange(1)
                                                }
                                                className="px-4 py-2 mx-1 rounded-md bg-white text-[#64748B] border border-[#E2E8F0]"
                                            >
                                                1
                                            </button>
                                        )}

                                        {/* Show ellipsis if needed */}
                                        {currentPage > 4 && (
                                            <span className="px-3 py-2">
                                                ...
                                            </span>
                                        )}

                                        {/* Pages around current page */}
                                        {pageNumbers
                                            .filter(
                                                (num) =>
                                                    num >=
                                                        Math.max(
                                                            1,
                                                            currentPage - 1
                                                        ) &&
                                                    num <=
                                                        Math.min(
                                                            totalPages,
                                                            currentPage + 1
                                                        )
                                            )
                                            .map((number) => (
                                                <button
                                                    key={number}
                                                    onClick={() =>
                                                        handlePageChange(number)
                                                    }
                                                    className={`px-4 py-2 mx-1 rounded-md ${
                                                        number === currentPage
                                                            ? "bg-[#6C5DD3] text-white"
                                                            : "bg-white text-[#64748B] border border-[#E2E8F0]"
                                                    }`}
                                                >
                                                    {number}
                                                </button>
                                            ))}

                                        {/* Show ellipsis if needed */}
                                        {currentPage < totalPages - 3 && (
                                            <span className="px-3 py-2">
                                                ...
                                            </span>
                                        )}

                                        {/* Always show last page */}
                                        {currentPage < totalPages - 2 && (
                                            <button
                                                onClick={() =>
                                                    handlePageChange(totalPages)
                                                }
                                                className="px-4 py-2 mx-1 rounded-md bg-white text-[#64748B] border border-[#E2E8F0]"
                                            >
                                                {totalPages}
                                            </button>
                                        )}
                                    </>
                                )}

                                <button
                                    onClick={() =>
                                        handlePageChange(currentPage + 1)
                                    }
                                    disabled={currentPage === totalPages}
                                    className="flex items-center px-2 py-2 rounded-md ml-2 text-[#64748B] disabled:opacity-50"
                                    title="Наступна сторінка"
                                >
                                    <span className="material-icons">
                                        chevron_right
                                    </span>
                                </button>

                                <button
                                    onClick={() => handlePageChange(totalPages)}
                                    disabled={currentPage === totalPages}
                                    className="flex items-center px-2 py-2 rounded-md ml-1 text-[#64748B] disabled:opacity-50"
                                    title="Остання сторінка"
                                >
                                    <span className="material-icons">
                                        last_page
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
