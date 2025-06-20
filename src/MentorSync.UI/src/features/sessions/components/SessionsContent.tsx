import React, { useState, useRef, useEffect } from "react";
import { Link } from "react-router-dom";
import { toast } from "react-toastify";
import { hasRole } from "../../auth/utils/authUtils";
import {
    getMenteeBookings,
    getMentorBookings,
    confirmBooking,
    cancelBooking,
    BookingSession,
} from "../../scheduling/services/schedulingService";
import BookingCard from "../../bookings/components/BookingCard";

type FilterType = "pending" | "confirmed" | "past" | "all";

const SessionsContent: React.FC = () => {
    const [bookings, setBookings] = useState<BookingSession[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [filterType, setFilterType] = useState<FilterType>("all");
    const [showFilterDropdown, setShowFilterDropdown] = useState(false);
    const [confirmingId, setConfirmingId] = useState<number | null>(null);
    const [cancellingId, setCancellingId] = useState<number | null>(null);
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

    useEffect(() => {
        const fetchBookings = async () => {
            setLoading(true);
            try {
                let bookingsData: BookingSession[] = [];

                if (hasRole("Mentee")) {
                    bookingsData = await getMenteeBookings();
                } else if (hasRole("Mentor")) {
                    bookingsData = await getMentorBookings();
                }

                setBookings(bookingsData);
            } catch (error) {
                console.error("Failed to fetch bookings:", error);
                toast.error("Не вдалося завантажити ваші сесії");
            } finally {
                setLoading(false);
            }
        };

        fetchBookings();
    }, []);

    const handleConfirmBooking = async (bookingId: number) => {
        setConfirmingId(bookingId);
        try {
            await confirmBooking(bookingId);
            setBookings(
                bookings.map((booking) =>
                    booking.id === bookingId
                        ? { ...booking, status: "confirmed" }
                        : booking
                )
            );
            toast.success("Бронювання успішно підтверджено");
        } catch (error) {
            console.error("Failed to confirm booking:", error);
            toast.error("Не вдалося підтвердити бронювання");
        } finally {
            setConfirmingId(null);
        }
    };

    const handleCancelBooking = async (bookingId: number) => {
        if (window.confirm("Ви впевнені, що хочете скасувати цю сесію?")) {
            setCancellingId(bookingId);
            try {
                await cancelBooking(bookingId);
                setBookings(
                    bookings.map((booking) =>
                        booking.id === bookingId
                            ? { ...booking, status: "Cancelled" }
                            : booking
                    )
                );
                toast.success("Сесію успішно скасовано");
            } catch (error) {
                console.error("Failed to cancel booking:", error);
                toast.error("Не вдалося скасувати сесію");
            } finally {
                setCancellingId(null);
            }
        }
    };

    const handleFilterChange = (newFilter: FilterType) => {
        setFilterType(newFilter);
        setShowFilterDropdown(false);
    };

    // Filter bookings based on status and date
    const filteredBookings = bookings.filter((booking) => {
        if (filterType === "all") return true;
        if (filterType === "pending") return booking.status === "Pending";
        if (filterType === "confirmed") return booking.status === "Confirmed";
        if (filterType === "past") {
            // Consider completed, cancelled, and other non-active statuses as past
            return (
                booking.status !== "Pending" && booking.status !== "Confirmed"
            );
        }
        return true;
    });

    // Group bookings by status for display
    const pendingBookings = filteredBookings.filter(
        (b) => b.status === "Pending"
    );
    const confirmedBookings = filteredBookings.filter(
        (b) => b.status === "Confirmed"
    );
    const pastBookings = filteredBookings.filter(
        (b) => b.status !== "Pending" && b.status !== "Confirmed"
    );

    // Determine if we need to show a group based on filter
    const showPending = filterType === "all" || filterType === "pending";
    const showConfirmed = filterType === "all" || filterType === "confirmed";
    const showPast = filterType === "all" || filterType === "past";

    return (
        <div className="flex-1 p-6 bg-[#F8FAFC] overflow-y-auto">
            <div className="flex justify-between items-center mb-8">
                <h1 className="text-2xl font-semibold text-[#1E293B]">
                    Мої сесії
                </h1>

                <div className="flex space-x-4">
                    {hasRole("Mentor") && (
                        <Link
                            to="/mentor/availability"
                            className="py-2 px-4 rounded-md flex items-center"
                        >
                            <span className="material-icons mr-2 text-sm">
                                calendar_today
                            </span>
                            Керувати доступністю
                        </Link>
                    )}
                </div>
            </div>

            <div className="flex items-center justify-between mb-6">
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
                            {filterType === "pending"
                                ? "Очікують підтвердження"
                                : filterType === "confirmed"
                                ? "Підтверджені"
                                : filterType === "past"
                                ? "Минулі сесії"
                                : "Всі сесії"}
                        </span>
                        <span className="material-icons text-[#000000] ml-2">
                            {showFilterDropdown ? "expand_less" : "expand_more"}
                        </span>
                    </button>

                    {/* Dropdown menu */}
                    {showFilterDropdown && (
                        <div className="absolute z-10 mt-1 w-full bg-white border border-[#E2E8F0] rounded-lg shadow-lg">
                            <div
                                className="py-2 px-4 hover:bg-[#F8FAFC] cursor-pointer"
                                onClick={() => handleFilterChange("all")}
                            >
                                Всі сесії
                            </div>
                            <div
                                className="py-2 px-4 hover:bg-[#F8FAFC] cursor-pointer"
                                onClick={() => handleFilterChange("pending")}
                            >
                                Очікують підтвердження
                            </div>
                            <div
                                className="py-2 px-4 hover:bg-[#F8FAFC] cursor-pointer"
                                onClick={() => handleFilterChange("confirmed")}
                            >
                                Підтверджені
                            </div>
                            <div
                                className="py-2 px-4 hover:bg-[#F8FAFC] cursor-pointer"
                                onClick={() => handleFilterChange("past")}
                            >
                                Минулі сесії
                            </div>
                        </div>
                    )}
                </div>
            </div>

            {loading ? (
                <div className="text-center py-8">
                    <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-[#4318D1] mx-auto"></div>
                    <p className="mt-4 text-[#64748B]">Завантаження сесій...</p>
                </div>
            ) : bookings.length === 0 ? (
                <div className="bg-white rounded-lg shadow-sm p-6 text-center">
                    <p className="text-[#64748B]">
                        У вас ще немає ніяких сесій
                    </p>
                    {hasRole("Mentee") && (
                        <button
                            onClick={() => (window.location.href = "/mentors")}
                            className="mt-4 px-4 py-2 bg-[#4318D1] text-white rounded-md hover:bg-[#3712A5] transition-colors"
                        >
                            Знайти ментора
                        </button>
                    )}
                </div>
            ) : (
                <div className="space-y-8">
                    {/* Pending sessions */}
                    {showPending && pendingBookings.length > 0 && (
                        <div>
                            <h2 className="text-xl font-medium text-[#1E293B] mb-4">
                                Очікують підтвердження
                            </h2>
                            <div className="space-y-4">
                                {pendingBookings.map((booking) => (
                                    <BookingCard
                                        key={booking.id}
                                        booking={booking}
                                        onConfirm={() =>
                                            handleConfirmBooking(booking.id)
                                        }
                                        onCancel={() =>
                                            handleCancelBooking(booking.id)
                                        }
                                        isConfirming={
                                            confirmingId === booking.id
                                        }
                                        isCancelling={
                                            cancellingId === booking.id
                                        }
                                        userRole={
                                            hasRole("Mentee")
                                                ? "mentee"
                                                : "mentor"
                                        }
                                    />
                                ))}
                            </div>
                        </div>
                    )}

                    {/* Confirmed sessions */}
                    {showConfirmed && confirmedBookings.length > 0 && (
                        <div>
                            <h2 className="text-xl font-medium text-[#1E293B] mb-4">
                                Підтверджені
                            </h2>
                            <div className="space-y-4">
                                {confirmedBookings.map((booking) => (
                                    <BookingCard
                                        key={booking.id}
                                        booking={booking}
                                        onCancel={() =>
                                            handleCancelBooking(booking.id)
                                        }
                                        isCancelling={
                                            cancellingId === booking.id
                                        }
                                        userRole={
                                            hasRole("Mentee")
                                                ? "mentee"
                                                : "mentor"
                                        }
                                    />
                                ))}
                            </div>
                        </div>
                    )}

                    {/* Past sessions */}
                    {showPast && pastBookings.length > 0 && (
                        <div>
                            <h2 className="text-xl font-medium text-[#1E293B] mb-4">
                                Історія сесій
                            </h2>
                            <div className="space-y-4">
                                {pastBookings.map((booking) => (
                                    <BookingCard
                                        key={booking.id}
                                        booking={booking}
                                        userRole={
                                            hasRole("Mentee")
                                                ? "mentee"
                                                : "mentor"
                                        }
                                    />
                                ))}
                            </div>
                        </div>
                    )}
                </div>
            )}
        </div>
    );
};

export default SessionsContent;
