import React from "react";
import { BookingSession } from "../../scheduling/services/schedulingService";

interface BookingCardProps {
    booking: BookingSession;
    onCancel?: () => void;
    onConfirm?: () => void;
    isCancelling?: boolean;
    isConfirming?: boolean;
    userRole: "mentor" | "mentee";
}

const BookingCard: React.FC<BookingCardProps> = ({
    booking,
    onCancel,
    onConfirm,
    isCancelling = false,
    isConfirming = false,
    userRole,
}) => {
    const formatDate = (date: string): string => {
        return new Date(date).toLocaleDateString("uk-UA", {
            day: "numeric",
            month: "long",
            year: "numeric",
        });
    };
    const formatTime = (date: string): string => {
        return new Date(date).toLocaleTimeString("uk-UA", {
            hour: "2-digit",
            minute: "2-digit",
            hour12: false,
        });
    };

    // Translate booking status to Ukrainian
    const translateBookingStatus = (status: string): string => {
        const statusMap: Record<string, string> = {
            pending: "Очікується",
            confirmed: "Підтверджено",
            completed: "Завершено",
            cancelled: "Скасовано",
            noShow: "Не відбулось",
        };

        return statusMap[status] || status;
    };

    // Get status badge color
    const getStatusBadgeColor = (status: string): string => {
        switch (status) {
            case "pending":
                return "bg-blue-100 text-blue-800";
            case "confirmed":
                return "bg-green-100 text-green-800";
            case "completed":
                return "bg-purple-100 text-purple-800";
            case "cancelled":
                return "bg-red-100 text-red-800";
            case "noShow":
                return "bg-gray-100 text-gray-800";
            default:
                return "bg-gray-100 text-gray-800";
        }
    };

    // Calculate if this booking is for today
    const isToday = (): boolean => {
        const today = new Date();
        const bookingDate = new Date(booking.start);
        return (
            today.getFullYear() === bookingDate.getFullYear() &&
            today.getMonth() === bookingDate.getMonth() &&
            today.getDate() === bookingDate.getDate()
        );
    };

    // If the booking is today, show special indicator
    const todayIndicator = isToday() && (
        <div className="inline-block px-2 py-1 text-xs bg-yellow-100 text-yellow-800 rounded-md mr-2">
            Сьогодні
        </div>
    );

    // Determine if actions should be shown
    const showActions =
        (booking.status == "pending" || booking.status == "confirmed") &&
        (onCancel || onConfirm);

    const partnerName =
        userRole === "mentee"
            ? `Ментор: ${booking.mentorName || "Ім'я ментора"}`
            : `Менті: ${booking.menteeName || "Ім'я менті"}`;

    const partnerImage =
        userRole === "mentee" ? booking.mentorImage : booking.menteeImage;

    return (
        <div className="bg-white rounded-lg shadow-sm p-6">
            <div className="flex flex-col md:flex-row md:justify-between md:items-center">
                <div className="flex items-center mb-4 md:mb-0">
                    <div className="w-12 h-12 rounded-full bg-gray-200 overflow-hidden mr-4">
                        {partnerImage ? (
                            <img
                                src={partnerImage}
                                alt={
                                    userRole === "mentee" ? "Mentor" : "Mentee"
                                }
                                className="w-full h-full object-cover"
                            />
                        ) : (
                            <div className="w-full h-full flex items-center justify-center text-gray-400">
                                <span className="material-icons">person</span>
                            </div>
                        )}
                    </div>
                    <div>
                        <h3 className="text-lg font-medium text-[#1E293B]">
                            {partnerName}
                        </h3>
                        <div className="flex items-center text-sm text-[#64748B]">
                            {todayIndicator}
                            <span>
                                {formatDate(booking.start)},{" "}
                                {formatTime(booking.start)} -{" "}
                                {formatTime(booking.end)}
                            </span>
                        </div>
                    </div>
                </div>

                <div>
                    <span
                        className={`inline-block px-3 py-1 text-xs rounded-full ${getStatusBadgeColor(
                            booking.status
                        )}`}
                    >
                        {translateBookingStatus(booking.status)}
                    </span>
                </div>
            </div>

            {showActions && (
                <div className="mt-6 flex justify-end space-x-4">
                    {onCancel && booking.status != "cancelled" && (
                        <button
                            onClick={onCancel}
                            disabled={isCancelling}
                            className="px-4 py-2 border border-red-500 text-red-500 rounded-md hover:bg-red-50 transition-colors"
                        >
                            {isCancelling ? "Скасування..." : "Скасувати"}
                        </button>
                    )}

                    {onConfirm &&
                        booking.status === "pending" &&
                        userRole === "mentor" && (
                            <button
                                onClick={onConfirm}
                                disabled={isConfirming}
                                className="px-4 py-2 bg-[#4318D1] text-white rounded-md hover:bg-[#3712A5] transition-colors"
                            >
                                {isConfirming
                                    ? "Підтвердження..."
                                    : "Підтвердити"}
                            </button>
                        )}
                </div>
            )}
        </div>
    );
};

export default BookingCard;
