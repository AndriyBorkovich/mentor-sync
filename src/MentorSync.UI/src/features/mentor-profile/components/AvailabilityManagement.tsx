import React, { useState, useEffect } from "react";
import {
    getMentorAvailability,
    createMentorAvailability,
    MentorAvailabilitySlot,
} from "../../scheduling/services/schedulingService";
import { toast } from "react-toastify";
import { hasRole } from "../../auth";

interface AvailabilityManagementProps {
    mentorId: number;
}

const AvailabilityManagement: React.FC<AvailabilityManagementProps> = ({
    mentorId,
}) => {
    const [availabilitySlots, setAvailabilitySlots] = useState<
        MentorAvailabilitySlot[]
    >([]);
    const [startDate, setStartDate] = useState<Date>(new Date());
    const [endDate, setEndDate] = useState<Date>(new Date());
    const [startTime, setStartTime] = useState<string>("09:00");
    const [endTime, setEndTime] = useState<string>("10:00");
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [isSubmitting, setIsSubmitting] = useState<boolean>(false);

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
                console.log("Fetched availability slots:", response);
                setAvailabilitySlots(response.slots);
            } catch (error) {
                console.error("Failed to fetch availability slots:", error);
                toast.error("Не вдалося завантажити доступні слоти часу");
            } finally {
                setIsLoading(false);
            }
        };

        fetchAvailability();
    }, [mentorId]);

    // Format date for display
    const formatDate = (dateString: string): string => {
        const date = new Date(dateString);
        return date.toLocaleDateString("uk-UA", {
            day: "numeric",
            month: "long",
            year: "numeric",
        });
    };

    // Format time for display
    const formatTime = (dateString: string): string => {
        const date = new Date(dateString);
        return date.toLocaleTimeString("uk-UA", {
            hour: "2-digit",
            minute: "2-digit",
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
        }

        // Validate the times
        if (startTime >= endTime) {
            toast.warning("Час початку має бути раніше за час закінчення");
            return;
        }

        setIsSubmitting(true);

        try {
            // Create date objects with the selected date and time
            const startDateTime = new Date(startDate);
            const [startHours, startMinutes] = startTime.split(":").map(Number);
            startDateTime.setHours(startHours, startMinutes, 0, 0);

            const endDateTime = new Date(endDate);
            const [endHours, endMinutes] = endTime.split(":").map(Number);
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
            setAvailabilitySlots(response.slots);

            // Reset form
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
    };

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
                Управління доступними слотами часу
            </h2>

            <form
                onSubmit={handleCreateSlot}
                className="mb-8 bg-[#F8FAFC] p-4 rounded-lg"
            >
                <h3 className="text-md font-medium text-[#1E293B] mb-4">
                    Створити новий доступний слот
                </h3>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    <div>
                        <label className="block text-sm font-medium text-[#64748B] mb-1">
                            Дата початку
                        </label>
                        <input
                            type="date"
                            className="w-full border border-[#E2E8F0] p-2 rounded-md"
                            value={startDate.toISOString().split("T")[0]}
                            onChange={(e) =>
                                setStartDate(new Date(e.target.value))
                            }
                            min={new Date().toISOString().split("T")[0]}
                            required
                        />
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-[#64748B] mb-1">
                            Час початку
                        </label>
                        <input
                            type="time"
                            className="w-full border border-[#E2E8F0] p-2 rounded-md"
                            value={startTime}
                            onChange={(e) => setStartTime(e.target.value)}
                            required
                        />
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-[#64748B] mb-1">
                            Дата закінчення
                        </label>
                        <input
                            type="date"
                            className="w-full border border-[#E2E8F0] p-2 rounded-md"
                            value={endDate.toISOString().split("T")[0]}
                            onChange={(e) =>
                                setEndDate(new Date(e.target.value))
                            }
                            min={startDate.toISOString().split("T")[0]}
                            required
                        />
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-[#64748B] mb-1">
                            Час закінчення
                        </label>
                        <input
                            type="time"
                            className="w-full border border-[#E2E8F0] p-2 rounded-md"
                            value={endTime}
                            onChange={(e) => setEndTime(e.target.value)}
                            required
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
            </form>

            <h3 className="text-md font-medium text-[#1E293B] mb-4">
                Ваші доступні слоти часу
            </h3>

            {isLoading ? (
                <div className="text-center py-4">
                    Завантаження доступних слотів часу...
                </div>
            ) : availabilitySlots?.length === 0 ? (
                <div className="text-center py-4 text-[#64748B]">
                    У вас немає доступних слотів часу. Створіть новий слот вище.
                </div>
            ) : (
                <div className="space-y-3">
                    {availabilitySlots?.map((slot) => (
                        <div
                            key={slot.id}
                            className="border border-[#E2E8F0] rounded-md p-3"
                        >
                            <div className="flex justify-between items-center">
                                <div>
                                    <div className="font-medium">
                                        {formatDate(slot.start)}
                                    </div>
                                    <div className="text-sm text-[#64748B]">
                                        {formatTime(slot.start)} -{" "}
                                        {formatTime(slot.end)}
                                    </div>
                                </div>
                                <div className="bg-green-100 text-green-800 text-xs py-1 px-2 rounded">
                                    Доступний
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};

export default AvailabilityManagement;
