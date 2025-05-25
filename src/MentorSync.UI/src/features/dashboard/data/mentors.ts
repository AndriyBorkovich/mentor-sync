export interface Skill {
    id: string;
    name: string;
}

export interface Mentor {
    id: string;
    name: string;
    title: string;
    rating: number;
    skills: Skill[];
    profileImage: string;
}

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
