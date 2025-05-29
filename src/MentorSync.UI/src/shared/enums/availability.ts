export enum Availability {
    None = 0,
    Morning = 1 << 0, // 1
    Afternoon = 1 << 1, // 2
    Evening = 1 << 2, // 4
    Night = 1 << 3, // 8
}

export const timeOfDayOptions = [
    { value: Availability.Morning, label: "Ранок", desc: "6:00-12:00" },
    { value: Availability.Afternoon, label: "День", desc: "12:00-17:00" },
    { value: Availability.Evening, label: "Вечір", desc: "17:00-22:00" },
    { value: Availability.Night, label: "Ніч", desc: "22:00-6:00" },
];
