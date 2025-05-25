import React, { useState, useRef, useEffect } from "react";
import { upcomingSessions } from "../../dashboard/data/sessions";
import { pastSessions } from "../data/pastSessions";
import UpcomingSessionCard from "./UpcomingSessionCard";
import PastSessionCard from "./PastSessionCard";
import DatePickerInput from "./DatePickerInput";

type FilterType = "upcoming" | "past" | "all";
const SessionsContent: React.FC = () => {
    const [searchTerm, setSearchTerm] = useState("");
    const [filterType, setFilterType] = useState<FilterType>("all");
    const [startDate, setStartDate] = useState<string | null>(null);
    const [endDate, setEndDate] = useState<string | null>(null);
    const [showFilterDropdown, setShowFilterDropdown] = useState(false);
    const filterDropdownRef = useRef<HTMLDivElement>(null);

    // Close dropdown when clicking outside
    useEffect(() => {
        function handleClickOutside(event: MouseEvent) {
            if (
                filterDropdownRef.current &&
                !filterDropdownRef.current.contains(event.target as Node)
            ) {
                setShowFilterDropdown(false);
            }
        }

        document.addEventListener("mousedown", handleClickOutside);
        return () => {
            document.removeEventListener("mousedown", handleClickOutside);
        };
    }, []);

    // Filter sessions based on search term (mentor name) and date range
    const filteredUpcomingSessions = upcomingSessions.filter((session) => {
        const nameMatches = session.mentorName
            .toLowerCase()
            .includes(searchTerm.toLowerCase());

        // Additional date filtering would be implemented here if we had actual date objects
        // For demo purposes, we'll just filter by name

        return nameMatches;
    });

    const filteredPastSessions = pastSessions.filter((session) => {
        const nameMatches = session.mentorName
            .toLowerCase()
            .includes(searchTerm.toLowerCase());

        // Additional date filtering would be implemented here

        return nameMatches;
    });

    const handleFilterChange = (newFilter: "upcoming" | "past" | "all") => {
        setFilterType(newFilter);
        setShowFilterDropdown(false);
    };

    return (
        <div className="flex-1 p-6 overflow-y-auto bg-[#F9FAFB]">
            <h1 className="text-2xl font-bold text-[#1E293B] mb-6">
                Мої сесії
            </h1>

            {/* Filters panel */}
            <div className="bg-white rounded-lg p-4 mb-8 border border-[#E2E8F0]">
                <div className="flex flex-col md:flex-row gap-4">
                    <div className="flex-1 md:max-w-xs">
                        <DatePickerInput
                            label="Від"
                            value={startDate}
                            onChange={(date) => setStartDate(date)}
                        />
                    </div>
                    <div className="flex-1 md:max-w-xs">
                        <DatePickerInput
                            label="До"
                            value={endDate}
                            onChange={(date) => setEndDate(date)}
                        />
                    </div>
                    <div className="flex-1">
                        <div className="relative">
                            <input
                                type="text"
                                placeholder="Шукати по імені ментора..."
                                className="w-full p-3 border border-[#E2E8F0] rounded-lg"
                                value={searchTerm}
                                onChange={(e) => setSearchTerm(e.target.value)}
                            />
                        </div>
                    </div>
                    <div
                        ref={filterDropdownRef}
                        className="relative w-full md:w-auto"
                    >
                        <button
                            className="w-full md:w-auto px-6 py-3 border border-[#E2E8F0] rounded-lg flex items-center justify-between bg-white"
                            onClick={() =>
                                setShowFilterDropdown(!showFilterDropdown)
                            }
                        >
                            <span className="text-[#000000]">
                                {filterType === "upcoming"
                                    ? "Майбутні"
                                    : filterType === "past"
                                    ? "Минулі"
                                    : "Всі"}
                            </span>
                            <span className="material-icons text-[#000000] ml-2">
                                {showFilterDropdown
                                    ? "expand_less"
                                    : "expand_more"}
                            </span>
                        </button>

                        {/* Dropdown menu */}
                        {showFilterDropdown && (
                            <div className="absolute z-10 mt-1 w-full bg-white border border-[#E2E8F0] rounded-lg shadow-lg">
                                <div
                                    className="py-2 px-4 hover:bg-[#F8FAFC] cursor-pointer"
                                    onClick={() =>
                                        handleFilterChange("upcoming")
                                    }
                                >
                                    Майбутні
                                </div>
                                <div
                                    className="py-2 px-4 hover:bg-[#F8FAFC] cursor-pointer"
                                    onClick={() => handleFilterChange("past")}
                                >
                                    Минулі
                                </div>
                                <div
                                    className="py-2 px-4 hover:bg-[#F8FAFC] cursor-pointer"
                                    onClick={() => handleFilterChange("all")}
                                >
                                    Всі
                                </div>
                            </div>
                        )}
                    </div>
                </div>
            </div>

            {/* Upcoming sessions */}
            {(filterType === "upcoming" || filterType === "all") && (
                <>
                    <div className="flex items-center mb-4">
                        <h2 className="text-lg font-semibold text-[#1E293B]">
                            Майбутні сесії
                        </h2>
                        {filteredUpcomingSessions.length > 0 && (
                            <div className="ml-2 flex items-center justify-center bg-[#6C5DD3] rounded-full w-6 h-6">
                                <span className="text-xs font-semibold text-white">
                                    {filteredUpcomingSessions.length}
                                </span>
                            </div>
                        )}
                    </div>

                    {filteredUpcomingSessions.length > 0 ? (
                        filteredUpcomingSessions.map((session) => (
                            <UpcomingSessionCard
                                key={session.id}
                                session={session}
                            />
                        ))
                    ) : (
                        <div className="bg-white rounded-lg p-6 mb-6 text-center">
                            <p className="text-[#64748B]">
                                Немає майбутніх сесій
                            </p>
                        </div>
                    )}
                </>
            )}

            {/* Past sessions */}
            {(filterType === "past" || filterType === "all") && (
                <>
                    <div className="flex items-center mt-8 mb-4">
                        <h2 className="text-lg font-semibold text-[#1E293B]">
                            Минулі сесії
                        </h2>
                    </div>

                    {filteredPastSessions.length > 0 ? (
                        filteredPastSessions.map((session) => (
                            <PastSessionCard
                                key={session.id}
                                session={session}
                            />
                        ))
                    ) : (
                        <div className="bg-white rounded-lg p-6 mb-6 text-center">
                            <p className="text-[#64748B]">
                                Немає минулих сесій
                            </p>
                        </div>
                    )}
                </>
            )}
        </div>
    );
};

export default SessionsContent;
