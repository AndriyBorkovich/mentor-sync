export interface Message {
    id: string;
    senderId: string;
    receiverId: string;
    content: string;
    timestamp: string;
    read: boolean;
}

export interface Chat {
    id: string;
    participantId: string; // The other person in the chat
    participantName: string;
    participantAvatar?: string;
    lastMessage?: Message;
    unreadCount: number;
    isOnline: boolean;
}

export const mockChats: Chat[] = [
    {
        id: "1",
        participantId: "101",
        participantName: "Оксана Лень",
        participantAvatar: "/placeholder-avatar.jpg",
        lastMessage: {
            id: "m1",
            senderId: "101",
            receiverId: "current-user",
            content: "Доброго дня! Як у вас справи?",
            timestamp: "14:32",
            read: false,
        },
        unreadCount: 2,
        isOnline: true,
    },
    {
        id: "2",
        participantId: "102",
        participantName: "Михайло Янків",
        participantAvatar: "/placeholder-avatar.jpg",
        lastMessage: {
            id: "m2",
            senderId: "current-user",
            receiverId: "102",
            content: "Дякую за інформацію. Я уважно розгляну матеріали.",
            timestamp: "10:15",
            read: true,
        },
        unreadCount: 0,
        isOnline: false,
    },
    {
        id: "3",
        participantId: "103",
        participantName: "Петро Петрів",
        participantAvatar: "/placeholder-avatar.jpg",
        lastMessage: {
            id: "m3",
            senderId: "103",
            receiverId: "current-user",
            content: "Гарної вам середи! Не забудьте про нашу зустріч завтра.",
            timestamp: "Вчора",
            read: true,
        },
        unreadCount: 0,
        isOnline: true,
    },
    {
        id: "4",
        participantId: "104",
        participantName: "Дмитро Кім",
        participantAvatar: "/placeholder-avatar.jpg",
        lastMessage: {
            id: "m4",
            senderId: "current-user",
            receiverId: "104",
            content: "Я переглянув ваші коментарі та вніс зміни у код.",
            timestamp: "Вчора",
            read: true,
        },
        unreadCount: 0,
        isOnline: false,
    },
    {
        id: "5",
        participantId: "105",
        participantName: "Юлія Король",
        participantAvatar: "/placeholder-avatar.jpg",
        lastMessage: {
            id: "m5",
            senderId: "105",
            receiverId: "current-user",
            content: "Привіт! Можемо обговорити проєкт сьогодні після 18:00?",
            timestamp: "10.05",
            read: true,
        },
        unreadCount: 0,
        isOnline: false,
    },
];

export const mockMessages: Record<string, Message[]> = {
    "1": [
        {
            id: "m1-1",
            senderId: "101",
            receiverId: "current-user",
            content: "Привіт! Як проходить ваше навчання?",
            timestamp: "14:25",
            read: true,
        },
        {
            id: "m1-2",
            senderId: "current-user",
            receiverId: "101",
            content:
                "Привіт! Все добре, працюю над новим проєктом. Маю декілька запитань щодо React Router.",
            timestamp: "14:28",
            read: true,
        },
        {
            id: "m1-3",
            senderId: "101",
            receiverId: "current-user",
            content:
                "Звичайно, залюбки допоможу. З якими проблемами стикаєтесь?",
            timestamp: "14:30",
            read: true,
        },
        {
            id: "m1-4",
            senderId: "101",
            receiverId: "current-user",
            content: "Доброго дня! Як у вас справи?",
            timestamp: "14:32",
            read: false,
        },
    ],
    "2": [
        {
            id: "m2-1",
            senderId: "current-user",
            receiverId: "102",
            content:
                "Привіт, Михайло! Чи можна записатися на консультацію наступного тижня?",
            timestamp: "10:05",
            read: true,
        },
        {
            id: "m2-2",
            senderId: "102",
            receiverId: "current-user",
            content:
                "Доброго дня! Так, звичайно. У мене є вільний час у вівторок о 15:00 або в четвер о 10:00.",
            timestamp: "10:10",
            read: true,
        },
        {
            id: "m2-3",
            senderId: "current-user",
            receiverId: "102",
            content: "Дякую за інформацію. Я уважно розгляну матеріали.",
            timestamp: "10:15",
            read: true,
        },
    ],
    "3": [
        {
            id: "m3-1",
            senderId: "103",
            receiverId: "current-user",
            content:
                "Привіт! Надсилаю вам матеріали для наступної сесії. Прошу ознайомитись заздалегідь.",
            timestamp: "Вчора",
            read: true,
        },
        {
            id: "m3-2",
            senderId: "current-user",
            receiverId: "103",
            content: "Дякую! Обов'язково ознайомлюсь.",
            timestamp: "Вчора",
            read: true,
        },
        {
            id: "m3-3",
            senderId: "103",
            receiverId: "current-user",
            content: "Гарної вам середи! Не забудьте про нашу зустріч завтра.",
            timestamp: "Вчора",
            read: true,
        },
    ],
    "4": [
        {
            id: "m4-1",
            senderId: "104",
            receiverId: "current-user",
            content:
                "Доброго дня! Переглянув ваш код і маю кілька зауважень щодо структури проєкту.",
            timestamp: "Вчора",
            read: true,
        },
        {
            id: "m4-2",
            senderId: "current-user",
            receiverId: "104",
            content: "Я переглянув ваші коментарі та вніс зміни у код.",
            timestamp: "Вчора",
            read: true,
        },
    ],
    "5": [
        {
            id: "m5-1",
            senderId: "105",
            receiverId: "current-user",
            content: "Привіт! Можемо обговорити проєкт сьогодні після 18:00?",
            timestamp: "10.05",
            read: true,
        },
    ],
};
