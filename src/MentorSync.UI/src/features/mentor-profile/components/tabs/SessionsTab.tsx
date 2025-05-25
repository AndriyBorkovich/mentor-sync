import React, { useState } from "react";
import { Mentor } from "../../../dashboard/data/mentors";

interface SessionsTabProps {
    mentor: Mentor;
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

    return (
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
                                onClick={() => setSelectedTimeSlot(slot.time)}
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
    );
};

export default SessionsTab;
