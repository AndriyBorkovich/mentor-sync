import React, { useState, useEffect } from "react";

interface CalendarViewProps {
    selectedDate: Date;
    onDateSelect: (date: Date) => void;
}

const CalendarView: React.FC<CalendarViewProps> = ({
    selectedDate,
    onDateSelect,
}) => {
    const [currentMonth, setCurrentMonth] = useState<Date>(
        new Date(selectedDate)
    );
    const [calendarDays, setCalendarDays] = useState<Date[]>([]);

    // Generate calendar days for the current month view
    useEffect(() => {
        const days: Date[] = [];
        const year = currentMonth.getFullYear();
        const month = currentMonth.getMonth();

        // First day of the month
        const firstDay = new Date(year, month, 1);
        const startingDayOfWeek = firstDay.getDay() || 7; // Sunday is 0, convert to 7

        // Last day of the month
        const lastDay = new Date(year, month + 1, 0);

        // Add previous month days to fill the first week
        const prevMonthLastDay = new Date(year, month, 0).getDate();
        for (let i = startingDayOfWeek - 1; i >= 1; i--) {
            days.push(new Date(year, month - 1, prevMonthLastDay - i + 1));
        }

        // Add current month days
        for (let day = 1; day <= lastDay.getDate(); day++) {
            days.push(new Date(year, month, day));
        }

        // Add next month days to complete the last week if needed
        const daysNeeded = 42 - days.length; // 6 rows of 7 days
        for (let day = 1; day <= daysNeeded; day++) {
            days.push(new Date(year, month + 1, day));
        }

        setCalendarDays(days);
    }, [currentMonth]);

    // Handle navigation to previous month
    const prevMonth = () => {
        setCurrentMonth(
            new Date(currentMonth.getFullYear(), currentMonth.getMonth() - 1, 1)
        );
    };

    // Handle navigation to next month
    const nextMonth = () => {
        setCurrentMonth(
            new Date(currentMonth.getFullYear(), currentMonth.getMonth() + 1, 1)
        );
    };

    // Check if a date is today
    const isToday = (date: Date): boolean => {
        const today = new Date();
        return (
            date.getDate() === today.getDate() &&
            date.getMonth() === today.getMonth() &&
            date.getFullYear() === today.getFullYear()
        );
    };

    // Check if a date is selected
    const isSelected = (date: Date): boolean => {
        return (
            date.getDate() === selectedDate.getDate() &&
            date.getMonth() === selectedDate.getMonth() &&
            date.getFullYear() === selectedDate.getFullYear()
        );
    };

    // Check if a date is in the current month being displayed
    const isCurrentMonth = (date: Date): boolean => {
        return date.getMonth() === currentMonth.getMonth();
    };

    // Check if a date is in the past
    const isPastDate = (date: Date): boolean => {
        const today = new Date();
        today.setHours(0, 0, 0, 0);
        return date < today;
    };

    // Format date as Ukrainian month and year
    const formatMonthAndYear = (date: Date): string => {
        return date.toLocaleDateString("uk-UA", {
            month: "long",
            year: "numeric",
        });
    };

    // Array of day names in Ukrainian
    const weekdays = ["Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Нд"];
    return (
        <div>
            <div className="flex items-center justify-between mb-4">
                <button
                    onClick={prevMonth}
                    className="p-1 rounded-full hover:bg-gray-100 focus:outline-none"
                    aria-label="Попередній місяць"
                >
                    <span className="material-icons text-gray-600">
                        chevron_left
                    </span>
                </button>
                <h3 className="text-md font-medium text-[#1E293B] capitalize">
                    {formatMonthAndYear(currentMonth)}
                </h3>
                <button
                    onClick={nextMonth}
                    className="p-1 rounded-full hover:bg-gray-100 focus:outline-none"
                    aria-label="Наступний місяць"
                >
                    <span className="material-icons text-gray-600">
                        chevron_right
                    </span>
                </button>
            </div>

            <div className="grid grid-cols-7 gap-1">
                {/* Weekday headers */}
                {weekdays.map((day, index) => (
                    <div
                        key={`header-${index}`}
                        className="text-center text-xs font-medium text-gray-500 py-2"
                    >
                        {day}
                    </div>
                ))}

                {/* Calendar days */}
                {calendarDays.map((day, index) => {
                    const isSelectedDay = isSelected(day) && !isPastDate(day);
                    const isTodayDay = isToday(day);
                    const isAvailableDay =
                        isCurrentMonth(day) && !isPastDate(day);

                    return (
                        <div
                            key={`day-${index}`}
                            className={`
                text-center py-2 cursor-pointer relative
                ${!isCurrentMonth(day) ? "text-gray-300" : ""}
                ${isPastDate(day) ? "text-gray-300 cursor-not-allowed" : ""}
                ${isTodayDay ? "font-medium" : ""}
                ${
                    isAvailableDay && !isTodayDay && !isSelectedDay
                        ? "hover:bg-gray-100"
                        : ""
                }
              `}
                            onClick={() => {
                                if (!isPastDate(day)) {
                                    onDateSelect(day);
                                }
                            }}
                        >
                            <div
                                className={`
                flex items-center justify-center w-8 h-8 rounded-full mx-auto
                ${isSelectedDay ? "bg-[#4318D1] text-white" : ""}
                ${
                    isTodayDay && !isSelectedDay
                        ? "bg-blue-50 text-blue-600"
                        : ""
                }
              `}
                            >
                                {day.getDate()}
                            </div>

                            {/* Add a dot indicator for days that have available slots (this is a mockup) */}
                            {isAvailableDay &&
                                !isPastDate(day) &&
                                day.getDate() % 3 === 1 && (
                                    <div className="absolute bottom-0.5 w-full flex justify-center">
                                        <div
                                            className={`w-1 h-1 rounded-full ${
                                                isSelectedDay
                                                    ? "bg-white"
                                                    : "bg-[#4318D1]"
                                            }`}
                                        ></div>
                                    </div>
                                )}
                        </div>
                    );
                })}
            </div>
        </div>
    );
};

export default CalendarView;
