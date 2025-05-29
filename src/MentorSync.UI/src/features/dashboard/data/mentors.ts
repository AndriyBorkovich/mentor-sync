import { Mentor } from "../../../shared/types";

export const recommendedMentors: Mentor[] = [
    {
        id: "1",
        name: "Оксана Лень",
        title: "Senior Software Architect",
        rating: 4.9,
        skills: [
            { id: "1", name: "System Design" },
            { id: "2", name: "Cloud Architecture" },
        ],
        profileImage: "https://randomuser.me/api/portraits/women/44.jpg",
        yearsOfExperience: 12,
        category: "Software Development",
    },
    {
        id: "2",
        name: "Михайло Янків",
        title: "Lead Developer",
        rating: 4.8,
        skills: [
            { id: "3", name: "JavaScript" },
            { id: "4", name: "React" },
        ],
        profileImage: "https://randomuser.me/api/portraits/men/32.jpg",
        yearsOfExperience: 8,
        category: "Software Development",
    },
    {
        id: "3",
        name: "Петро Петров",
        title: "Data Science Manager",
        rating: 4.9,
        skills: [
            { id: "5", name: "Python" },
            { id: "6", name: "Machine Learning" },
        ],
        profileImage: "https://randomuser.me/api/portraits/men/46.jpg",
        yearsOfExperience: 10,
        category: "Data Science",
    },
    {
        id: "6",
        name: "Наталія Шевченко",
        title: "Backend Engineer",
        rating: 4.7,
        skills: [
            { id: "11", name: "Node.js" },
            { id: "12", name: "TypeScript" },
        ],
        profileImage: "https://randomuser.me/api/portraits/women/63.jpg",
        yearsOfExperience: 6,
        category: "Software Development",
    },
    {
        id: "7",
        name: "Андрій Мороз",
        title: "Security Specialist",
        rating: 4.6,
        skills: [
            { id: "13", name: "Java" },
            { id: "14", name: "C#" },
        ],
        profileImage: "https://randomuser.me/api/portraits/men/36.jpg",
        yearsOfExperience: 4,
        category: "Cybersecurity",
    },
    {
        id: "8",
        name: "Олена Павленко",
        title: "UX Designer",
        rating: 4.8,
        skills: [
            { id: "15", name: "CSS" },
            { id: "16", name: "Vue.js" },
        ],
        profileImage: "https://randomuser.me/api/portraits/women/35.jpg",
        yearsOfExperience: 3,
        category: "Software Development",
    },
    {
        id: "9",
        name: "Віктор Коваленко",
        title: "DevOps Lead",
        rating: 4.9,
        skills: [
            { id: "17", name: "Docker" },
            { id: "18", name: "Kubernetes" },
        ],
        profileImage: "https://randomuser.me/api/portraits/men/91.jpg",
        yearsOfExperience: 9,
        category: "DevOps",
    },
];

export const recentlyViewedMentors: Mentor[] = [
    {
        id: "4",
        name: "Дмитро Кім",
        title: "DevOps Engineer",
        rating: 4.7,
        skills: [
            { id: "7", name: "Docker" },
            { id: "8", name: "Kubernetes" },
        ],
        profileImage: "https://randomuser.me/api/portraits/men/22.jpg",
    },
    {
        id: "5",
        name: "Єлизавета Гучко",
        title: "Frontend Lead",
        rating: 4.8,
        skills: [
            { id: "9", name: "Vue.js" },
            { id: "10", name: "CSS" },
        ],
        profileImage: "https://randomuser.me/api/portraits/women/28.jpg",
    },
];
