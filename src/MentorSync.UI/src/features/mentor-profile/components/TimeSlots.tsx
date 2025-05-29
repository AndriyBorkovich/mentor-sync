import React from "react";
import { MentorAvailabilitySlot } from "../../scheduling/services/schedulingService";

interface TimeSlotsProps {
    availabilitySlots: MentorAvailabilitySlot[];
    selectedTimeSlot: MentorAvailabilitySlot | null;
    isLoading: boolean;
    onSelectTimeSlot: (slot: MentorAvailabilitySlot) => void;
}

const TimeSlots: React.FC<TimeSlotsProps> = ({
    availabilitySlots,
    selectedTimeSlot,
    isLoading,
    onSelectTimeSlot,
}) => {
    // Format time for display in 24-hour format
    const formatTimeSlot = (dateString: string): string => {
        const date = new Date(dateString);
        return date.toLocaleTimeString("uk-UA", {
            hour: "2-digit",
            minute: "2-digit",
            hour12: false,
        });
    };

    // Group time slots by morning, afternoon, and evening
    const morningSlots = availabilitySlots.filter((slot) => {
        const hour = new Date(slot.start).getHours();
        return hour >= 0 && hour < 12;
    });

    const afternoonSlots = availabilitySlots.filter((slot) => {
        const hour = new Date(slot.start).getHours();
        return hour >= 12 && hour < 17;
    });

    const eveningSlots = availabilitySlots.filter((slot) => {
        const hour = new Date(slot.start).getHours();
        return hour >= 17 && hour < 24;
    });

    // Render a group of time slots with a heading
    const renderSlotGroup = (
        slots: MentorAvailabilitySlot[],
        title: string,
        icon: string
    ) => {
        if (slots.length === 0) return null;

        return (
            <div className="mb-6">
                <div className="flex items-center mb-3">
                    <span className="material-icons text-gray-600 mr-2">
                        {icon}
                    </span>
                    <h4 className="text-sm font-medium text-[#64748B]">
                        {title}
                    </h4>
                </div>
                <div className="grid grid-cols-2 sm:grid-cols-3 gap-2">
                    {slots.map((slot) => (
                        <div
                            key={slot.id}
                            className={`border p-2 rounded-lg text-center cursor-pointer transition-colors
                ${
                    selectedTimeSlot?.id === slot.id
                        ? "border-[#4318D1] bg-[#4318D1] text-white"
                        : "border-[#E2E8F0] hover:border-[#4318D1]"
                }`}
                            onClick={() => onSelectTimeSlot(slot)}
                        >
                            {formatTimeSlot(slot.start)}
                        </div>
                    ))}
                </div>
            </div>
        );
    };

    if (isLoading) {
        return (
            <div className="p-4 text-center">
                <div className="animate-spin rounded-full h-6 w-6 border-t-2 border-b-2 border-[#4318D1] mx-auto mb-2"></div>
                <p className="text-sm text-[#64748B]">
                    Завантаження доступних часових слотів...
                </p>
            </div>
        );
    }

    if (availabilitySlots.length === 0) {
        return (
            <div className="p-4 text-center border border-dashed border-[#E2E8F0] rounded-lg">
                <span className="material-icons text-gray-400 text-3xl mb-2">
                    event_busy
                </span>
                <p className="text-[#64748B]">
                    На цю дату відсутні доступні слоти часу
                </p>
            </div>
        );
    }
    // Show all slots sequentially like in screenshot if no slots in morning/afternoon/evening groups
    if (
        morningSlots.length === 0 &&
        afternoonSlots.length === 0 &&
        eveningSlots.length === 0
    ) {
        return (
            <div>
                <div className="mb-4">
                    <h4 className="text-sm font-medium text-[#64748B] mb-3">
                        Оберіть час
                    </h4>
                    <div className="grid grid-cols-2 sm:grid-cols-4 gap-3">
                        {availabilitySlots.map((slot) => (
                            <div
                                key={slot.id}
                                className={`border p-3 rounded-lg text-center cursor-pointer transition-colors
                  ${
                      selectedTimeSlot?.id === slot.id
                          ? "border-[#4318D1] bg-[#4318D1] text-white"
                          : "border-[#E2E8F0] hover:border-[#4318D1]"
                  }`}
                                onClick={() => onSelectTimeSlot(slot)}
                            >
                                {formatTimeSlot(slot.start)}
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        );
    }

    return (
        <div>
            <h4 className="text-sm font-medium text-[#64748B] mb-3">
                Оберіть час
            </h4>
            {renderSlotGroup(morningSlots, "Ранок", "wb_twilight")}
            {renderSlotGroup(afternoonSlots, "День", "wb_sunny")}
            {renderSlotGroup(eveningSlots, "Вечір", "nights_stay")}
        </div>
    );
};

export default TimeSlots;
