import React, { useState, useEffect } from "react";
import { MentorData, isMentorProfile } from "../../types/mentorTypes";
import {
    getMentorAvailability,
    createBooking,
    MentorAvailabilitySlot,
} from "../../../scheduling/services/schedulingService";
import { toast } from "react-toastify";
import { hasRole } from "../../../auth/utils/authUtils";
import CalendarView from "../../components/CalendarView";
import TimeSlots from "../../components/TimeSlots";
import SessionDetails from "../../components/SessionDetails";

interface SessionsTabProps {
    mentor: MentorData;
}

const SessionsTab: React.FC<SessionsTabProps> = ({ mentor }) => {
    const mentorId =
        typeof mentor.id === "string" ? parseInt(mentor.id, 10) : mentor.id;
    const [selectedDate, setSelectedDate] = useState<Date>(new Date());
    const [availabilitySlots, setAvailabilitySlots] = useState<
        MentorAvailabilitySlot[]
    >([]);
    const [selectedTimeSlot, setSelectedTimeSlot] =
        useState<MentorAvailabilitySlot | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [isBooking, setIsBooking] = useState<boolean>(false);
    const isMentee = hasRole("Mentee");

    // Fetch availability when the date changes
    useEffect(() => {
        const fetchAvailability = async () => {
            if (typeof mentor.id === "undefined") return;

            setIsLoading(true);
            try {
                // Set end date to the same day as the selected date
                const startDate = new Date(selectedDate);
                const endDate = new Date(selectedDate);
                // add 1 day to endDate to include the whole day
                endDate.setDate(endDate.getDate() + 1);

                const availabilityResponse = await getMentorAvailability(
                    mentorId,
                    startDate,
                    endDate
                );

                setAvailabilitySlots(availabilityResponse.slots);
            } catch (error) {
                console.error("Failed to fetch mentor availability:", error);
                toast.error("Не вдалося завантажити доступний час");
            } finally {
                setIsLoading(false);
            }
        };

        fetchAvailability();
    }, [mentor.id, selectedDate, mentorId]);

    // Handle booking
    const handleBookSession = async () => {
        if (!selectedTimeSlot) {
            toast.warning("Будь ласка, виберіть час для сесії");
            return;
        }

        if (!isMentee) {
            toast.warning("Тільки менті можуть бронювати сесії");
            return;
        }

        setIsBooking(true);

        try {
            await createBooking({
                mentorId: mentorId,
                availabilitySlotId: selectedTimeSlot.id,
                start: selectedTimeSlot.start,
                end: selectedTimeSlot.end,
            });

            toast.success("Сесію успішно заброньовано!");

            // Reset selection
            setSelectedTimeSlot(null);

            // Refresh availability slots (the booked slot should no longer be available)
            const startDate = new Date(selectedDate);
            const endDate = new Date(selectedDate);
            const availabilityResponse = await getMentorAvailability(
                mentorId,
                startDate,
                endDate
            );

            setAvailabilitySlots(availabilityResponse.slots);
        } catch (error) {
            console.error("Failed to book session:", error);
            toast.error("Не вдалося забронювати сесію. Спробуйте ще раз.");
        } finally {
            setIsBooking(false);
        }
    }; // Format session time from ISO string in 24-hour format
    const formatSessionTime = (dateString: string): string => {
        const date = new Date(dateString);
        return date.toLocaleTimeString("uk-UA", {
            hour: "2-digit",
            minute: "2-digit",
            hour12: false,
        });
    };

    // Translate booking status to Ukrainian
    const translateBookingStatus = (status: string): string => {
        const statusMap: Record<string, string> = {
            pending: "Очікується",
            ponfirmed: "Підтверджено",
            pompleted: "Завершено",
            cancelled: "Скасовано",
            scheduled: "Заплановано",
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
            {renderAvailability()}{" "}
            <h2 className="text-lg font-medium text-[#1E293B] mb-4">
                Забронюйте сеанс
            </h2>
            <div className="grid grid-cols-1 lg:grid-cols-12 gap-6">
                {/* Left column - Calendar */}
                <div className="lg:col-span-5 space-y-4">
                    <div className="bg-white rounded-lg shadow-sm p-4">
                        <CalendarView
                            selectedDate={selectedDate}
                            onDateSelect={(date) => setSelectedDate(date)}
                        />
                    </div>

                    <SessionDetails
                        selectedDate={selectedDate}
                        selectedTimeSlot={selectedTimeSlot}
                        mentor={mentor}
                        isBooking={isBooking}
                        isMentee={isMentee}
                        onBookSession={handleBookSession}
                    />
                </div>

                {/* Right column - Time slots */}
                <div className="lg:col-span-7">
                    <div className="bg-white rounded-lg shadow-sm p-4">
                        <h3 className="text-md font-medium text-[#1E293B] mb-4">
                            {selectedDate.toLocaleDateString("uk-UA", {
                                weekday: "long",
                                day: "numeric",
                                month: "long",
                                year: "numeric",
                            })}
                        </h3>

                        <TimeSlots
                            availabilitySlots={availabilitySlots.filter(
                                (s) => !s.isBooked
                            )}
                            selectedTimeSlot={selectedTimeSlot}
                            isLoading={isLoading}
                            onSelectTimeSlot={setSelectedTimeSlot}
                        />
                    </div>
                </div>
            </div>
            {getUpcomingSessions()}
        </div>
    );
};

export default SessionsTab;
