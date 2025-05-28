import React, { useState, useEffect } from "react";
import { toast } from "react-toastify";
import {
    getMentorBookings,
    cancelBooking,
    confirmBooking,
    BookingSession,
} from "../../scheduling/services/schedulingService";
import BookingCard from "../components/BookingCard";

const MentorBookingsPage: React.FC = () => {
    const [bookings, setBookings] = useState<BookingSession[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [cancellingId, setCancellingId] = useState<number | null>(null);
    const [confirmingId, setConfirmingId] = useState<number | null>(null);

    useEffect(() => {
        const fetchBookings = async () => {
            setLoading(true);
            try {
                const bookingsData = await getMentorBookings();
                setBookings(bookingsData);
            } catch (error) {
                console.error("Failed to fetch bookings:", error);
                toast.error("Не вдалося завантажити ваші бронювання");
            } finally {
                setLoading(false);
            }
        };

        fetchBookings();
    }, []);

    const handleCancelBooking = async (bookingId: number) => {
        if (window.confirm("Ви впевнені, що хочете скасувати це бронювання?")) {
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
                toast.success("Бронювання успішно скасовано");
            } catch (error) {
                console.error("Failed to cancel booking:", error);
                toast.error("Не вдалося скасувати бронювання");
            } finally {
                setCancellingId(null);
            }
        }
    };

    const handleConfirmBooking = async (bookingId: number) => {
        setConfirmingId(bookingId);
        try {
            await confirmBooking(bookingId);
            setBookings(
                bookings.map((booking) =>
                    booking.id === bookingId
                        ? { ...booking, status: "Confirmed" }
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

    // Group bookings by status
    const pendingBookings = bookings.filter((b) => b.status === "Pending");
    const confirmedBookings = bookings.filter((b) => b.status === "Confirmed");
    const otherBookings = bookings.filter(
        (b) => b.status !== "Pending" && b.status !== "Confirmed"
    );

    return (
        <div className="container mx-auto p-6">
            <h1 className="text-2xl font-semibold text-[#1E293B] mb-6">
                Мої сесії менторства
            </h1>

            {loading ? (
                <div className="text-center py-8">
                    <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-[#4318D1] mx-auto"></div>
                    <p className="mt-4 text-[#64748B]">Завантаження сесій...</p>
                </div>
            ) : bookings.length === 0 ? (
                <div className="bg-white rounded-lg shadow-sm p-6 text-center">
                    <p className="text-[#64748B]">
                        У вас немає запланованих сесій менторства
                    </p>
                    <button
                        onClick={() =>
                            (window.location.href = "/mentor/availability")
                        }
                        className="mt-4 px-4 py-2 bg-[#4318D1] text-white rounded-md hover:bg-[#3712A5] transition-colors"
                    >
                        Налаштувати доступність
                    </button>
                </div>
            ) : (
                <div className="space-y-8">
                    {pendingBookings.length > 0 && (
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
                                        userRole="mentor"
                                    />
                                ))}
                            </div>
                        </div>
                    )}

                    {confirmedBookings.length > 0 && (
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
                                        userRole="mentor"
                                    />
                                ))}
                            </div>
                        </div>
                    )}

                    {otherBookings.length > 0 && (
                        <div>
                            <h2 className="text-xl font-medium text-[#1E293B] mb-4">
                                Історія сесій
                            </h2>
                            <div className="space-y-4">
                                {otherBookings.map((booking) => (
                                    <BookingCard
                                        key={booking.id}
                                        booking={booking}
                                        userRole="mentor"
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

export default MentorBookingsPage;
