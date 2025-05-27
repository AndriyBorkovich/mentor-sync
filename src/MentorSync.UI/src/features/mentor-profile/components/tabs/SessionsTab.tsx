import React, { useState } from "react";
import { MentorData, isMentorProfile } from "../../types/mentorTypes";

interface SessionsTabProps {
    mentor: MentorData;
}

// Mock session time slots
interface TimeSlot {
    id: string;
    time: string;
}

// Date formatter
const formatDate = (date: Date): string => {
    return date.toLocaleDateString("uk-UA", {
        day: "numeric",
        month: "long",
        year: "numeric",
    });
};

const SessionsTab: React.FC<SessionsTabProps> = ({ mentor }) => {
    const [selectedDate, setSelectedDate] = useState<Date>(new Date());
    const [selectedTimeSlot, setSelectedTimeSlot] = useState<string | null>(
        null
    );

    // Generate time slots for the selected day
    const timeSlots: TimeSlot[] = [
        { id: "1", time: "10:00 AM" },
        { id: "2", time: "2:00 PM" },
        { id: "3", time: "4:00 PM" },
    ];

    // Handle booking
    const handleBookSession = () => {
        if (!selectedTimeSlot) {
            alert("Будь ласка, виберіть час для сесії");
            return;
        }

        alert(
            `Сесію заброньовано з ${mentor.name} на ${formatDate(
                selectedDate
            )} o ${selectedTimeSlot}`
        );
    };

    // Format session time from ISO string
    const formatSessionTime = (dateString: string): string => {
        const date = new Date(dateString);
        return date.toLocaleTimeString("uk-UA", {
            hour: "2-digit",
            minute: "2-digit",
        });
    };

    // Translate booking status to Ukrainian
    const translateBookingStatus = (status: string): string => {
        const statusMap: Record<string, string> = {
            Pending: "Очікується",
            Confirmed: "Підтверджено",
            Completed: "Завершено",
            Cancelled: "Скасовано",
            Scheduled: "Заплановано",
        };

        return statusMap[status] || status;
    };

    // Get status badge color
    const getStatusBadgeColor = (status: string): string => {
        switch (status) {
            case "Scheduled":
            case "Pending":
                return "bg-blue-100 text-blue-800";
            case "Confirmed":
                return "bg-green-100 text-green-800";
            case "Completed":
                return "bg-purple-100 text-purple-800";
            case "Cancelled":
                return "bg-red-100 text-red-800";
            default:
                return "bg-gray-100 text-gray-800";
        }
    };

    // Get upcoming sessions from API data if available
    const getUpcomingSessions = () => {
        if (
            isMentorProfile(mentor) &&
            mentor.upcomingSessions &&
            mentor.upcomingSessions.length > 0
        ) {
            return (
                <div className="mt-6">
                    <h3 className="text-lg font-medium text-[#1E293B] mb-4">
                        Заплановані сесії
                    </h3>
                    <div className="space-y-4">
                        {mentor.upcomingSessions.map((session) => (
                            <div
                                key={session.id}
                                className="border border-[#E2E8F0] p-4 rounded-lg"
                            >
                                <h4 className="text-base font-medium text-[#1E293B]">
                                    {session.title ||
                                        `Сесія з ${session.menteeName}`}
                                </h4>
                                <p className="text-sm text-[#64748B] mt-1">
                                    {session.description ||
                                        "Деталі сесії будуть доступні після підтвердження"}
                                </p>
                                <div className="flex justify-between items-center mt-3">
                                    <div className="flex items-center">
                                        <span className="material-icons mr-1 text-[#64748B]">
                                            calendar_today
                                        </span>{" "}
                                        <span className="text-sm text-[#64748B]">
                                            {new Date(
                                                session.startTime
                                            ).toLocaleDateString("uk-UA", {
                                                day: "numeric",
                                                month: "long",
                                                year: "numeric",
                                            })}{" "}
                                            •{" "}
                                            {formatSessionTime(
                                                session.startTime
                                            )}{" "}
                                            -{" "}
                                            {formatSessionTime(session.endTime)}
                                        </span>
                                    </div>
                                    <span
                                        className={`px-2 py-1 rounded-md text-xs ${getStatusBadgeColor(
                                            session.status
                                        )}`}
                                    >
                                        {translateBookingStatus(session.status)}
                                    </span>
                                </div>
                                {session.menteeName && (
                                    <div className="flex items-center mt-3">
                                        <div className="w-6 h-6 rounded-full overflow-hidden mr-2">
                                            {session.menteeImage ? (
                                                <img
                                                    src={session.menteeImage}
                                                    alt={session.menteeName}
                                                    className="w-full h-full object-cover"
                                                />
                                            ) : (
                                                <div className="w-full h-full bg-gray-200 flex items-center justify-center text-xs">
                                                    {session.menteeName.charAt(
                                                        0
                                                    )}
                                                </div>
                                            )}
                                        </div>
                                        <span className="text-sm text-[#64748B]">
                                            {session.menteeName}
                                        </span>
                                    </div>
                                )}
                            </div>
                        ))}
                    </div>
                </div>
            );
        }
        return null;
    };

    // Based on MentorProfile.cs Availability property
    const renderAvailability = () => {
        if (isMentorProfile(mentor) && mentor.availability) {
            return (
                <div className="mb-4">
                    <h3 className="text-sm font-medium text-[#1E293B] mb-2">
                        Доступність ментора
                    </h3>
                    <div className="p-3 bg-[#F8FAFC] rounded-lg text-sm text-[#64748B]">
                        {mentor.availability}
                    </div>
                </div>
            );
        }
        return null;
    };

    return (
        <div className="flex flex-col">
            {renderAvailability()}

            <div className="flex flex-col md:flex-row gap-6">
                <div className="md:w-2/3">
                    <h2 className="text-lg font-medium text-[#1E293B] mb-4">
                        Забронюйте сеанс
                    </h2>

                    <div className="mb-6">
                        <h3 className="text-sm font-medium text-[#1E293B] mb-2">
                            Виберіть дату
                        </h3>
                        <div className="border border-[#E2E8F0] p-4 rounded-lg">
                            <input
                                type="date"
                                className="w-full border border-[#E2E8F0] p-2 rounded-md"
                                onChange={(e) =>
                                    setSelectedDate(new Date(e.target.value))
                                }
                                min={new Date().toISOString().split("T")[0]}
                            />
                        </div>
                    </div>

                    <div className="mb-6">
                        <h3 className="text-sm font-medium text-[#1E293B] mb-2">
                            Доступний час
                        </h3>
                        <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
                            {timeSlots.map((slot) => (
                                <div
                                    key={slot.id}
                                    className={`border border-[#E2E8F0] p-3 rounded-lg text-center cursor-pointer
                                        ${
                                            selectedTimeSlot === slot.time
                                                ? "bg-[#4318D1] text-white"
                                                : "hover:border-[#4318D1]"
                                        }`}
                                    onClick={() =>
                                        setSelectedTimeSlot(slot.time)
                                    }
                                >
                                    {slot.time}
                                </div>
                            ))}
                        </div>
                    </div>

                    <button
                        className="w-full py-3 bg-[#4318D1] text-white rounded-lg hover:bg-[#3712A5] transition-colors"
                        onClick={handleBookSession}
                    >
                        Забронювати зараз
                    </button>
                </div>

                <div className="md:w-1/3 bg-[#F8FAFC] p-4 rounded-lg">
                    <h3 className="text-sm font-medium text-[#1E293B] mb-2">
                        Деталі сесії
                    </h3>
                    <ul className="space-y-2 text-sm text-[#64748B]">
                        <li>Час: 60 хвилин</li>
                        <li>Формат: Відеоконференція</li>
                        <li>Скасування: можливе за 24 години до сесії</li>
                    </ul>
                </div>
            </div>

            {getUpcomingSessions()}
        </div>
    );
};

export default SessionsTab;
