import React, { useState, useEffect } from "react";
import {
    getMentorAvailability,
    createMentorAvailability,
    MentorAvailabilitySlot,
} from "../../scheduling/services/schedulingService";
import { toast } from "react-toastify";
import { hasRole } from "../../auth";
import CalendarView from "./CalendarView";
import TimePicker from "react-time-picker";
import "react-time-picker/dist/TimePicker.css";
import "react-clock/dist/Clock.css";
import "./timepicker.css";
import TimePickerField from "./TimePickerField";

interface AvailabilityManagementProps {
    mentorId: number;
}

const AvailabilityManagement: React.FC<AvailabilityManagementProps> = ({
    mentorId,
}) => {
    const [availabilitySlots, setAvailabilitySlots] = useState<
        MentorAvailabilitySlot[]
    >([]);
    const [selectedDate, setSelectedDate] = useState<Date>(new Date());
    const [startDate, setStartDate] = useState<Date>(new Date());
    const [endDate, setEndDate] = useState<Date>(new Date());
    const [startTime, setStartTime] = useState<string | null>("10:00");
    const [endTime, setEndTime] = useState<string | null>("11:00");
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
    const [filteredSlots, setFilteredSlots] = useState<
        MentorAvailabilitySlot[]
    >([]);

    const isMentor = hasRole("Mentor");

    // Load current availability slots
    useEffect(() => {
        const fetchAvailability = async () => {
            if (!mentorId) return;

            setIsLoading(true);
            try {
                // Get availability for the next 30 days
                const today = new Date();
                const thirtyDaysLater = new Date();
                thirtyDaysLater.setDate(today.getDate() + 30);

                const response = await getMentorAvailability(
                    mentorId,
                    today,
                    thirtyDaysLater
                );
                setAvailabilitySlots(response.slots);
                setFilteredSlots(response.slots); // Initialize filtered slots
            } catch (error) {
                console.error("Failed to fetch availability slots:", error);
                toast.error("Не вдалося завантажити доступні слоти часу");
            } finally {
                setIsLoading(false);
            }
        };

        fetchAvailability();
    }, [mentorId]); // Format date for display

    // Format date for input fields (YYYY-MM-DD)
    const formatDateForInput = (date: Date): string => {
        return `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(
            2,
            "0"
        )}-${String(date.getDate()).padStart(2, "0")}`;
    };

    // Format date for input fi    // Ensure time is in 24-hour format
    const ensureTimeFormat = (time: string | null): string => {
        // If time is null or empty, return a default time
        if (!time) {
            return "09:00";
        }

        // If the time is already in 24-hour format, return it with consistent padding
        if (/^([01]?[0-9]|2[0-3]):[0-5][0-9]$/.test(time)) {
            const [hours, minutes] = time.split(":").map(Number);
            return `${hours.toString().padStart(2, "0")}:${minutes
                .toString()
                .padStart(2, "0")}`;
        }

        try {
            // Try to parse the time and convert to 24-hour format
            const [hours, minutes] = time.split(":").map(Number);
            return `${Math.min(23, Math.max(0, hours))
                .toString()
                .padStart(2, "0")}:${Math.min(59, Math.max(0, minutes))
                .toString()
                .padStart(2, "0")}`;
        } catch (e) {
            console.error("Failed to parse time:", e);
            return "09:00"; // Return default if parsing fails
        }
    };

    // Format time for display in 24-hour format
    const formatTime = (dateString: string): string => {
        const date = new Date(dateString);
        return date.toLocaleTimeString("uk-UA", {
            hour: "2-digit",
            minute: "2-digit",
            hour12: false,
        });
    };

    // Handle creating a new availability slot
    const handleCreateSlot = async (e: React.FormEvent) => {
        e.preventDefault();

        if (!isMentor) {
            toast.warning(
                "Тільки ментори можуть створювати доступні слоти часу"
            );
            return;
        } // Validate the times
        if (!startTime || !endTime) {
            toast.warning("Будь ласка, вкажіть час початку та закінчення");
            return;
        }

        if (startTime >= endTime) {
            toast.warning("Час початку має бути раніше за час закінчення");
            return;
        }

        setIsSubmitting(true);
        try {
            // Create date objects with the selected date and time
            // Starting with noon (12:00) as the base time and then setting the actual time
            // This helps avoid timezone issues by ensuring we're firmly in the middle of the day
            const startDateTime = new Date(
                startDate.getFullYear(),
                startDate.getMonth(),
                startDate.getDate(),
                12,
                0,
                0,
                0
            );
            const [startHours, startMinutes] = ensureTimeFormat(startTime)
                .split(":")
                .map(Number);
            startDateTime.setHours(startHours, startMinutes, 0, 0);

            const endDateTime = new Date(
                endDate.getFullYear(),
                endDate.getMonth(),
                endDate.getDate(),
                12,
                0,
                0,
                0
            );
            const [endHours, endMinutes] = ensureTimeFormat(endTime)
                .split(":")
                .map(Number);
            endDateTime.setHours(endHours, endMinutes, 0, 0);

            // Validate dates are in the future
            if (startDateTime <= new Date()) {
                toast.warning("Дата і час початку мають бути в майбутньому");
                return;
            }

            // Validate end date is after start date
            if (endDateTime <= startDateTime) {
                toast.warning(
                    "Дата і час закінчення мають бути після дати і часу початку"
                );
                return;
            }

            // Call API to create availability slot
            await createMentorAvailability(mentorId, {
                start: startDateTime.toISOString(),
                end: endDateTime.toISOString(),
            });

            toast.success("Доступний слот часу успішно створено");

            // Refresh the list of availability slots
            const today = new Date();
            const thirtyDaysLater = new Date();
            thirtyDaysLater.setDate(today.getDate() + 30);

            const response = await getMentorAvailability(
                mentorId,
                today,
                thirtyDaysLater
            );
            setAvailabilitySlots(response.slots); // Reset form
            setStartDate(new Date());
            setEndDate(new Date());
            setStartTime("09:00");
            setEndTime("10:00");
        } catch (error) {
            console.error("Failed to create availability slot:", error);
            toast.error("Не вдалося створити доступний слот часу");
        } finally {
            setIsSubmitting(false);
        }
    }; // Filter slots by selected date

    useEffect(() => {
        if (selectedDate) {
            // Filter slots for the selected date
            const filtered = availabilitySlots.filter((slot) => {
                const slotDate = new Date(slot.start);
                return (
                    slotDate.getFullYear() === selectedDate.getFullYear() &&
                    slotDate.getMonth() === selectedDate.getMonth() &&
                    slotDate.getDate() === selectedDate.getDate()
                );
            });
            setFilteredSlots(filtered);

            // Create a new date object with the selected date at noon
            // Setting to noon (12:00) ensures we're safely in the middle of the day,
            // avoiding any timezone boundary issues that can cause date shifting
            const localDate = new Date(
                selectedDate.getFullYear(),
                selectedDate.getMonth(),
                selectedDate.getDate(),
                12,
                0,
                0,
                0
            );

            setStartDate(localDate);
            setEndDate(localDate);
        } else {
            setFilteredSlots(availabilitySlots);
        }
    }, [selectedDate, availabilitySlots]);

    if (!isMentor) {
        return (
            <div className="bg-yellow-50 border-l-4 border-yellow-400 p-4 mb-4">
                <div className="flex">
                    <div className="flex-shrink-0">
                        <span className="material-icons text-yellow-400">
                            warning
                        </span>
                    </div>
                    <div className="ml-3">
                        <p className="text-sm text-yellow-700">
                            Тільки ментори мають доступ до управління доступними
                            слотами часу.
                        </p>
                    </div>
                </div>
            </div>
        );
    }

    return (
        <div className="bg-white rounded-lg shadow-sm p-6">
            <h2 className="text-xl font-semibold text-[#1E293B] mb-6">
                Управління слотами часу
            </h2>{" "}
            <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 mb-4">
                <div className="bg-white rounded-lg shadow-sm p-4">
                    <h3 className="text-md font-medium text-[#1E293B] mb-2">
                        Календар доступності
                    </h3>
                    <p className="text-xs text-gray-500 mb-4">
                        Виберіть дату для перегляду або створення слотів
                    </p>
                    <CalendarView
                        selectedDate={selectedDate}
                        onDateSelect={setSelectedDate}
                        availabilitySlots={availabilitySlots}
                    />
                </div>

                <div className="bg-white rounded-lg shadow-sm p-4">
                    <h3 className="text-md font-medium text-[#1E293B] mb-2">
                        Слоти на{" "}
                        {selectedDate.toLocaleDateString("uk-UA", {
                            day: "numeric",
                            month: "long",
                            year: "numeric",
                        })}
                    </h3>

                    {isLoading ? (
                        <div className="text-center py-4">
                            Завантаження слотів...
                        </div>
                    ) : filteredSlots?.length === 0 ? (
                        <div className="text-center py-4 text-[#64748B]">
                            На вибрану дату немає доступних слотів
                        </div>
                    ) : (
                        <div className="space-y-3">
                            {filteredSlots?.map((slot) => (
                                <div
                                    key={slot.id}
                                    className="border border-[#E2E8F0] rounded-md p-3"
                                >
                                    <div className="flex justify-between items-center">
                                        <div>
                                            <div className="text-sm text-[#64748B]">
                                                {formatTime(slot.start)} -{" "}
                                                {formatTime(slot.end)}
                                            </div>
                                        </div>
                                        <div
                                            className={
                                                slot.isBooked
                                                    ? "bg-red-100 text-red-800 text-xs py-1 px-2 rounded"
                                                    : "bg-green-100 text-green-800 text-xs py-1 px-2 rounded"
                                            }
                                        >
                                            {slot.isBooked
                                                ? "Заброньований"
                                                : "Доступний"}
                                        </div>
                                    </div>
                                </div>
                            ))}
                        </div>
                    )}
                </div>
            </div>{" "}
            <form
                onSubmit={handleCreateSlot}
                className="mb-8 bg-white shadow-sm p-6 rounded-lg"
            >
                <h3 className="text-md font-medium text-[#1E293B] mb-4">
                    Створити новий слот
                </h3>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    <div>
                        <label className="block text-sm font-medium text-[#64748B] mb-1">
                            Дата початку
                        </label>{" "}
                        <input
                            type="date"
                            className="w-full border border-[#E2E8F0] p-2 rounded-md"
                            value={formatDateForInput(startDate)}
                            onChange={(e) => {
                                const date = new Date(e.target.value);
                                setStartDate(
                                    new Date(
                                        date.getFullYear(),
                                        date.getMonth(),
                                        date.getDate(),
                                        12 // Set to noon to avoid any timezone issues
                                    )
                                );
                            }}
                            min={formatDateForInput(new Date())}
                            required
                        />
                    </div>{" "}
                    <div>
                        <TimePickerField
                            label="Час початку"
                            value={startTime}
                            onChange={(value) => setStartTime(value)}
                            required={true}
                        />
                    </div>{" "}
                    <div>
                        <label className="block text-sm font-medium text-[#64748B] mb-1">
                            Дата закінчення
                        </label>{" "}
                        <input
                            type="date"
                            className="w-full border border-[#E2E8F0] p-2 rounded-md"
                            value={formatDateForInput(endDate)}
                            onChange={(e) => {
                                const date = new Date(e.target.value);
                                setEndDate(
                                    new Date(
                                        date.getFullYear(),
                                        date.getMonth(),
                                        date.getDate(),
                                        12 // Set to noon to avoid any timezone issues
                                    )
                                );
                            }}
                            min={formatDateForInput(startDate)}
                            required
                        />
                    </div>{" "}
                    <div>
                        <TimePickerField
                            label="Час закінчення"
                            value={endTime}
                            onChange={(value) => setEndTime(value)}
                            required={true}
                        />
                    </div>
                </div>

                <button
                    type="submit"
                    disabled={isSubmitting}
                    className="mt-4 bg-[#4318D1] text-white py-2 px-4 rounded-md hover:bg-[#3712A5] transition-colors"
                >
                    {isSubmitting ? "Створення..." : "Створити доступний слот"}
                </button>
            </form>{" "}
        </div>
    );
};

export default AvailabilityManagement;
