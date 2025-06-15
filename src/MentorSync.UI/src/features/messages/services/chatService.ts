import { HubConnection } from "@microsoft/signalr";
import { Message } from "../../../shared/types";

export interface ChatService {
    sendMessage: (recipientId: number, content: string) => Promise<void>;
    markMessageAsRead: (messageId: string) => Promise<void>;
    onReceiveMessage: (callback: (message: Message) => void) => void;
    onMessageSent: (callback: (message: Message) => void) => void;
    onMessageRead: (callback: (messageId: string) => void) => void;
}

export function createChatService(connection: HubConnection): ChatService {
    return {
        sendMessage: async (recipientId: number, content: string) => {
            await connection.invoke("SendChatMessage", recipientId, content);
        },

        markMessageAsRead: async (messageId: string) => {
            await connection.invoke("MarkMessageAsRead", messageId);
        },

        onReceiveMessage: (callback: (message: Message) => void) => {
            connection.on("ReceiveChatMessage", (messageDto: any) => {
                const message: Message = {
                    id: messageDto.id,
                    senderId: messageDto.senderId,
                    receiverId: messageDto.receiverId,
                    content: messageDto.content,
                    timestamp: formatMessageTimestamp(messageDto.timestamp),
                    read: messageDto.isRead,
                };
                callback(message);
            });
        },

        onMessageSent: (callback: (message: Message) => void) => {
            connection.on("MessageSent", (messageDto: any) => {
                const message: Message = {
                    id: messageDto.id,
                    senderId: messageDto.senderId,
                    receiverId: messageDto.receiverId,
                    content: messageDto.content,
                    timestamp: formatMessageTimestamp(messageDto.timestamp),
                    read: messageDto.isRead,
                };
                callback(message);
            });
        },

        onMessageRead: (callback: (messageId: string) => void) => {
            connection.on("MessageRead", (messageId: string) => {
                callback(messageId);
            });
        },
    };
}

function formatMessageTimestamp(timestamp: string | Date): string {
    const date =
        typeof timestamp === "string" ? new Date(timestamp) : timestamp;
    const now = new Date();

    // If today, return time
    if (date.toDateString() === now.toDateString()) {
        return date.toLocaleTimeString("uk-UA", {
            hour: "2-digit",
            minute: "2-digit",
        });
    }

    // If yesterday
    const yesterday = new Date(now);
    yesterday.setDate(now.getDate() - 1);
    if (date.toDateString() === yesterday.toDateString()) {
        return "Вчора";
    }

    // Otherwise return short date
    return date.toLocaleDateString("uk-UA", {
        day: "2-digit",
        month: "2-digit",
    });
}
