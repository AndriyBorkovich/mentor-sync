export interface Session {
    id: string;
    mentorId: string;
    mentorName: string;
    title: string;
    date: string;
    time: string;
}

export const upcomingSessions: Session[] = [
    {
        id: "1",
        mentorId: "1",
        mentorName: "Оксана Лень",
        title: "System Design Principles",
        date: "January 20",
        time: "10:00 AM",
    },
    {
        id: "2",
        mentorId: "2",
        mentorName: "Михайло Янків",
        title: "Advanced React Patterns",
        date: "January 22",
        time: "03:00 PM",
    },
];
