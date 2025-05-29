import React from "react";
import { MentorAvailabilitySlot } from "../../scheduling/services/schedulingService";
import { MentorData } from "../types/mentorTypes";

interface SessionDetailsProps {
    selectedDate: Date;
    selectedTimeSlot: MentorAvailabilitySlot | null;
    mentor: MentorData;
    isBooking: boolean;
    isMentee: boolean;
    onBookSession: () => void;
}

const SessionDetails: React.FC<SessionDetailsProps> = ({
    selectedDate,
    selectedTimeSlot,
    mentor,
    isBooking,
    isMentee,
    onBookSession,
}) => {
    const formatDate = (date: Date): string => {
        return date.toLocaleDateString("uk-UA", {
            day: "numeric",
            month: "long",
            year: "numeric",
        });
    };

    const formatTime = (dateString: string): string => {
        const date = new Date(dateString);
        return date.toLocaleTimeString("uk-UA", {
            hour: "2-digit",
            minute: "2-digit",
            hour12: false,
        });
    };

    const getDuration = (): string => {
        if (!selectedTimeSlot) return "";

        const start = new Date(selectedTimeSlot.start);
        const end = new Date(selectedTimeSlot.end);
        const diff = Math.abs(end.getTime() - start.getTime());
        const minutes = Math.floor(diff / 1000 / 60);

        if (minutes < 60) {
            return `${minutes} хв`;
        } else {
            const hours = Math.floor(minutes / 60);
            const remainingMinutes = minutes % 60;
            return remainingMinutes > 0
                ? `${hours} год ${remainingMinutes} хв`
                : `${hours} год`;
        }
    };
    return (
        <div className="bg-white rounded-lg shadow-sm p-4">
            <h3 className="text-md font-medium text-[#1E293B] mb-3">
                Деталі сесії
            </h3>

            {selectedTimeSlot ? (
                <div>
                    <div className="flex items-center mb-4">
                        <div className="w-10 h-10 rounded-full overflow-hidden mr-3">
                            {mentor.profileImage ? (
                                <img
                                    src={mentor.profileImage}
                                    alt={mentor.name || "Mentor"}
                                    className="w-full h-full object-cover"
                                />
                            ) : (
                                <div className="w-full h-full bg-gray-200 flex items-center justify-center">
                                    {mentor.name?.charAt(0) || "M"}
                                </div>
                            )}
                        </div>
                        <div>
                            <h4 className="text-sm font-medium">
                                {mentor.name || "Ментор"}
                            </h4>
                            <p className="text-xs text-gray-500">
                                {mentor.title || "Консультація"}
                            </p>
                        </div>
                    </div>

                    <div className="border-t border-b border-gray-200 py-4 mb-4">
                        <div className="space-y-3 text-sm">
                            <div className="flex items-center">
                                <span className="material-icons text-gray-500 mr-2 text-base">
                                    calendar_today
                                </span>
                                <span>{formatDate(selectedDate)}</span>
                            </div>
                            <div className="flex items-center">
                                <span className="material-icons text-gray-500 mr-2 text-base">
                                    schedule
                                </span>
                                <span>
                                    {formatTime(selectedTimeSlot.start)} -{" "}
                                    {formatTime(selectedTimeSlot.end)}
                                </span>
                            </div>
                            <div className="flex items-center">
                                <span className="material-icons text-gray-500 mr-2 text-base">
                                    timelapse
                                </span>
                                <span>Тривалість: {getDuration()}</span>
                            </div>
                        </div>
                    </div>

                    <div className="text-sm mb-6">
                        <h4 className="font-medium mb-2">Про сесію:</h4>
                        <p className="text-gray-600">
                            Індивідуальна менторська сесія з{" "}
                            {mentor.name || "ментором"} для обговорення
                            професійних питань та отримання рекомендацій.
                        </p>
                    </div>

                    <button
                        onClick={onBookSession}
                        disabled={isBooking || !isMentee}
                        className={`w-full py-3 rounded-lg ${
                            isMentee
                                ? "bg-[#4318D1] text-white hover:bg-[#3712A5] transition-colors"
                                : "bg-gray-300 text-gray-700 cursor-not-allowed"
                        }`}
                    >
                        {isBooking ? "Бронювання..." : "Забронювати зараз"}
                    </button>

                    {!isMentee && (
                        <p className="text-xs text-red-500 mt-2 text-center">
                            Тільки менті можуть бронювати сесії
                        </p>
                    )}
                </div>
            ) : (
                <div className="p-6 text-center border border-dashed border-[#E2E8F0] rounded-lg">
                    <span className="material-icons text-gray-400 text-3xl mb-2">
                        event_available
                    </span>
                    <p className="text-[#64748B] text-sm">
                        Будь ласка, виберіть дату і час для бронювання сесії
                    </p>
                </div>
            )}
        </div>
    );
};

export default SessionDetails;
