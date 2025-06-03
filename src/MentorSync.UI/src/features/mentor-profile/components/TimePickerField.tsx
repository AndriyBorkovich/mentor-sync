import React from "react";
import TimePicker from "react-time-picker";
import "react-time-picker/dist/TimePicker.css";
import "react-clock/dist/Clock.css";
import "./timepicker.css";

interface TimePickerFieldProps {
    label: string;
    value: string | null;
    onChange: (value: string) => void;
    required?: boolean;
}

/**
 * A reusable TimePicker field component with consistent styling
 */
const TimePickerField: React.FC<TimePickerFieldProps> = ({
    label,
    value,
    onChange,
    required = false,
}) => {
    const handleChange = (newValue: string | null) => {
        onChange(
            newValue !== null && newValue !== undefined ? newValue : "00:00"
        );
    };

    return (
        <div className="time-picker-field">
            <label className="block text-sm font-medium text-[#64748B] mb-1">
                {label}
            </label>
            <TimePicker
                onChange={handleChange}
                value={value}
                format="HH:mm"
                disableClock={true}
                clearIcon={null}
                clockIcon={null}
                required={required}
                locale="uk-UA"
                className="w-full border border-[#E2E8F0] rounded-md time-picker-custom"
            />
        </div>
    );
};

export default TimePickerField;
