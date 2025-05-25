import { Session } from "../../dashboard/data/sessions";

export interface PastSession extends Session {
    reviewed: boolean;
}

export const pastSessions: PastSession[] = [
    {
        id: "3",
        mentorId: "3",
        mentorName: "Петро Петрів",
        title: "Introduction to Machine Learning",
        date: "January 15",
        time: "10:00 AM",
        reviewed: false,
    },
    {
        id: "4",
        mentorId: "4",
        mentorName: "Дмитро Кім",
        title: "CI/CD Pipeline Setup",
        date: "January 12",
        time: "02:00 PM",
        reviewed: false,
    },
];
