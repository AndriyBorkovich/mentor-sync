import React from "react";

interface DatePickerInputProps {
    label: string;
    value: string | null;
    onChange: (value: string | null) => void;
}

const DatePickerInput: React.FC<DatePickerInputProps> = ({
    label,
    value,
    onChange,
}) => {
    return (
        <div className="relative">
            <label className="block text-sm text-[#000000] mb-1">{label}</label>
            <input
                type="date"
                value={value || ""}
                onChange={(e) => onChange(e.target.value || null)}
                className="w-full p-3 border border-[#E2E8F0] rounded-lg"
            />
        </div>
    );
};

export default DatePickerInput;
