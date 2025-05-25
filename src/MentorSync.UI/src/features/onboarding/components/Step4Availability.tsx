import React, { useState } from "react";
import { useOnboarding } from "../context/OnboardingContext";
import { TimeSlot } from "../data/OnboardingTypes";

const Step4Availability: React.FC = () => {
    const { data, updateData } = useOnboarding();
    const [activeDay, setActiveDay] =
        useState<keyof typeof data.availabilityHours>("monday");
    const [startTime, setStartTime] = useState<string>("09:00");
    const [endTime, setEndTime] = useState<string>("17:00");

    const days = [
        { id: "monday", label: "Понеділок" },
        { id: "tuesday", label: "Вівторок" },
        { id: "wednesday", label: "Середа" },
        { id: "thursday", label: "Четвер" },
        { id: "friday", label: "П'ятниця" },
        { id: "saturday", label: "Субота" },
        { id: "sunday", label: "Неділя" },
    ];

    const meetingFormats = [
        { id: "online", label: "Онлайн" },
        { id: "inPerson", label: "Особисто" },
        { id: "both", label: "Обидва варіанти" },
    ];

    const handleAddTimeSlot = () => {
        if (!startTime || !endTime) return;

        const newSlot: TimeSlot = {
            start: startTime,
            end: endTime,
        };

        // Check if this time slot overlaps with existing ones
        const hasOverlap = data.availabilityHours[activeDay].some((slot) => {
            return (
                (startTime >= slot.start && startTime < slot.end) ||
                (endTime > slot.start && endTime <= slot.end) ||
                (startTime <= slot.start && endTime >= slot.end)
            );
        });

        if (hasOverlap) {
            alert(
                "Цей часовий проміжок перекривається з існуючими. Будь ласка, виберіть інший час."
            );
            return;
        }

        // Add the new time slot and sort them
        const updatedSlots = [
            ...data.availabilityHours[activeDay],
            newSlot,
        ].sort((a, b) => a.start.localeCompare(b.start));

        // Update availabilityHours for the active day
        updateData({
            availabilityHours: {
                ...data.availabilityHours,
                [activeDay]: updatedSlots,
            },
        });
    };

    const handleRemoveTimeSlot = (index: number) => {
        const updatedSlots = [...data.availabilityHours[activeDay]];
        updatedSlots.splice(index, 1);

        updateData({
            availabilityHours: {
                ...data.availabilityHours,
                [activeDay]: updatedSlots,
            },
        });
    };

    const handlePreferredFormatChange = (
        format: "online" | "inPerson" | "both"
    ) => {
        updateData({ preferredMeetingFormat: format });
    };

    const formatTime = (time: string) => {
        return time.length === 5 ? time : `${time}:00`;
    };

    return (
        <div className="space-y-6">
            <h2 className="text-2xl font-bold text-[#1E293B]">Доступність</h2>
            <p className="text-[#64748B]">
                Вкажіть час, коли ви доступні для менторських сесій
            </p>

            <div className="space-y-4">
                <div>
                    <label className="block text-sm font-medium text-[#1E293B] mb-2">
                        Виберіть день
                    </label>
                    <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-7 gap-2">
                        {days.map((day) => (
                            <button
                                key={day.id}
                                type="button"
                                className={`px-2 py-1 rounded-lg border text-sm ${
                                    activeDay === day.id
                                        ? "border-[#6C5DD3] bg-[#6C5DD3]/10 text-[#6C5DD3]"
                                        : "border-[#E2E8F0] text-[#64748B]"
                                }`}
                                onClick={() =>
                                    setActiveDay(
                                        day.id as keyof typeof data.availabilityHours
                                    )
                                }
                            >
                                {day.label}
                            </button>
                        ))}
                    </div>
                </div>

                <div>
                    <label className="block text-sm font-medium text-[#1E293B] mb-2">
                        Часові проміжки для{" "}
                        {days.find((d) => d.id === activeDay)?.label}
                    </label>
                    <div className="flex gap-2 mb-2">
                        <div className="flex-1">
                            <label className="text-xs text-[#64748B]">
                                Початок
                            </label>
                            <input
                                type="time"
                                value={startTime}
                                onChange={(e) => setStartTime(e.target.value)}
                                className="w-full p-2 border border-[#E2E8F0] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#6C5DD3] focus:border-[#6C5DD3]"
                            />
                        </div>
                        <div className="flex-1">
                            <label className="text-xs text-[#64748B]">
                                Кінець
                            </label>
                            <input
                                type="time"
                                value={endTime}
                                onChange={(e) => setEndTime(e.target.value)}
                                className="w-full p-2 border border-[#E2E8F0] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#6C5DD3] focus:border-[#6C5DD3]"
                            />
                        </div>
                        <div className="flex items-end">
                            <button
                                type="button"
                                onClick={handleAddTimeSlot}
                                className="p-2 bg-[#6C5DD3] text-white rounded-lg hover:bg-[#5B4DC4]"
                            >
                                <span className="material-icons">add</span>
                            </button>
                        </div>
                    </div>

                    {data.availabilityHours[activeDay].length > 0 ? (
                        <div className="space-y-2 mt-4">
                            {data.availabilityHours[activeDay].map(
                                (slot, index) => (
                                    <div
                                        key={index}
                                        className="flex items-center justify-between p-2 bg-[#F1F5F9] rounded-lg"
                                    >
                                        <span>
                                            {formatTime(slot.start)} -{" "}
                                            {formatTime(slot.end)}
                                        </span>
                                        <button
                                            type="button"
                                            onClick={() =>
                                                handleRemoveTimeSlot(index)
                                            }
                                            className="text-[#64748B] hover:text-[#EF4444]"
                                        >
                                            <span className="material-icons text-sm">
                                                close
                                            </span>
                                        </button>
                                    </div>
                                )
                            )}
                        </div>
                    ) : (
                        <div className="text-center py-4 text-[#64748B] bg-[#F1F5F9] rounded-lg mt-2">
                            Немає доданих часових проміжків
                        </div>
                    )}
                </div>

                <div>
                    <label className="block text-sm font-medium text-[#1E293B] mb-2">
                        Переважний формат зустрічей
                    </label>
                    <div className="grid grid-cols-3 gap-2">
                        {meetingFormats.map((format) => (
                            <button
                                key={format.id}
                                type="button"
                                className={`px-3 py-2 rounded-lg border ${
                                    data.preferredMeetingFormat === format.id
                                        ? "border-[#6C5DD3] bg-[#6C5DD3]/10 text-[#6C5DD3]"
                                        : "border-[#E2E8F0] text-[#64748B]"
                                }`}
                                onClick={() =>
                                    handlePreferredFormatChange(
                                        format.id as
                                            | "online"
                                            | "inPerson"
                                            | "both"
                                    )
                                }
                            >
                                {format.label}
                            </button>
                        ))}
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Step4Availability;
