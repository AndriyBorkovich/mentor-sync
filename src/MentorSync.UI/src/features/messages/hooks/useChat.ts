import { useEffect, useState, useRef } from "react";
import { useNotifications } from "../../notifications/useNotifications";
import { Chat, Message } from "../../../shared/types";
import { ChatService, createChatService } from "../services/chatService";
import { getUserId } from "../../auth";
import api from "../../../shared/services/api";

export function useChat() {
    const [chats, setChats] = useState<Chat[]>([]);
    const [messages, setMessages] = useState<Record<string, Message[]>>({});
    const [loading, setLoading] = useState(true);
    const [activeChat, setActiveChat] = useState<string | null>(null);

    const connectionRef = useNotifications();
    const chatServiceRef = useRef<ChatService | null>(null);

    // Initialize the chat service when connection is established
    useEffect(() => {
        if (!connectionRef.current) return;

        const chatService = createChatService(connectionRef.current);
        chatServiceRef.current = chatService;

        // Setup listeners for incoming messages
        chatService.onReceiveMessage((message) => {
            // Find the chat this message belongs to
            const chatId = chats.find(
                (chat) => chat.participantId === message.senderId
            )?.id;

            if (chatId) {
                // Update messages for the chat
                setMessages((prevMessages) => ({
                    ...prevMessages,
                    [chatId]: [...(prevMessages[chatId] || []), message],
                }));

                // Update chat with latest message and unread count
                setChats((prevChats) =>
                    prevChats.map((chat) => {
                        if (chat.id === chatId) {
                            return {
                                ...chat,
                                lastMessage: message,
                                unreadCount: chat.unreadCount + 1,
                            };
                        }
                        return chat;
                    })
                );

                // If this chat is active, mark the message as read
                if (activeChat === chatId) {
                    chatService.markMessageAsRead(message.id);
                }
            }
        });

        // Handle sent messages
        chatService.onMessageSent((message) => {
            // Find the chat this message belongs to
            const chatId = chats.find(
                (chat) => chat.participantId === message.receiverId
            )?.id;

            if (chatId) {
                // Update messages for the chat
                setMessages((prevMessages) => ({
                    ...prevMessages,
                    [chatId]: [...(prevMessages[chatId] || []), message],
                }));

                // Update chat with latest message
                setChats((prevChats) =>
                    prevChats.map((chat) => {
                        if (chat.id === chatId) {
                            return {
                                ...chat,
                                lastMessage: message,
                            };
                        }
                        return chat;
                    })
                );
            }
        });

        // Handle message read status updates
        chatService.onMessageRead((messageId) => {
            setMessages((prevMessages) => {
                const updatedMessages = { ...prevMessages };

                // Find and update the message in all chat histories
                Object.keys(updatedMessages).forEach((chatId) => {
                    updatedMessages[chatId] = updatedMessages[chatId].map(
                        (msg) =>
                            msg.id === messageId ? { ...msg, read: true } : msg
                    );
                });

                return updatedMessages;
            });
        });

        // Load initial data
        fetchChats();
    }, [connectionRef.current]); // Fetch all chats from the API
    const fetchChats = async () => {
        try {
            setLoading(true);
            const response = await api.get("/chat/rooms");
            // Axios throws errors for non-2xx responses, so no need to check status

            const data = response.data;
            // Get all chat participants
            const participantsResponse = await api.get("/chat/participants");
            const participantsData = participantsResponse.data;
            // Create a map of participant IDs to names
            interface ParticipantInfo {
                name: string;
                avatar?: string;
            }
            const participantsMap = new Map<string, ParticipantInfo>(
                participantsData.map((participant: any) => [
                    participant.id,
                    {
                        name: participant.fullName,
                        avatar: participant.avatarUrl,
                    },
                ])
            );
            const userId = getUserId();
            const fetchedChats: Chat[] = data.map((room: any) => ({
                id: room.id,
                participantId: room.participantId,
                participantName:
                    participantsMap.get(room.participantId)?.name ||
                    room.participantId,
                participantAvatar: participantsMap.get(room.participantId)
                    ?.avatar,
                lastMessage: room.lastMessage
                    ? {
                          id: room.lastMessage.id,
                          senderId: room.lastMessage.senderId,
                          receiverId:
                              room.participantId === room.lastMessage.senderId
                                  ? userId
                                  : room.participantId,
                          content: room.lastMessage.content,
                          timestamp: new Date(
                              room.lastMessage.createdAt
                          ).toLocaleTimeString("uk-UA", {
                              hour: "2-digit",
                              minute: "2-digit",
                          }),
                          read: room.lastMessage.isRead,
                      }
                    : undefined,
                unreadCount: room.unreadCount,
                isOnline: false, // Online status not implemented yet
            }));

            setChats(fetchedChats);
        } catch (error) {
            console.error("Error fetching chats:", error);
        } finally {
            setLoading(false);
        }
    }; // Fetch messages for a specific chat
    const fetchMessages = async (chatId: string) => {
        try {
            const response = await api.get(`/chat/messages/${chatId}`);
            // Axios throws errors for non-2xx responses, so no need to check status

            const data = response.data;
            const fetchedMessages: Message[] = data.map((msg: any) => ({
                id: msg.id,
                senderId: msg.senderId,
                receiverId: msg.receiverId,
                content: msg.content,
                timestamp: new Date(msg.timestamp).toLocaleTimeString("uk-UA", {
                    hour: "2-digit",
                    minute: "2-digit",
                }),
                read: msg.isRead,
            }));

            setMessages((prevMessages) => ({
                ...prevMessages,
                [chatId]: fetchedMessages,
            }));

            // Mark all unread messages in this chat as read
            const unreadMessages = fetchedMessages.filter(
                (msg) => !msg.read && msg.senderId !== getUserId()
            );
            unreadMessages.forEach((msg) => {
                chatServiceRef.current?.markMessageAsRead(msg.id);
            });

            // Update unread count for this chat
            setChats((prevChats) =>
                prevChats.map((chat) => {
                    if (chat.id === chatId) {
                        return {
                            ...chat,
                            unreadCount: 0,
                        };
                    }
                    return chat;
                })
            );
        } catch (error) {
            console.error(`Error fetching messages for chat ${chatId}:`, error);
        }
    };

    // Handle sending a new message
    const sendMessage = async (recipientId: number, content: string) => {
        if (!chatServiceRef.current) return;

        try {
            await chatServiceRef.current.sendMessage(recipientId, content);
        } catch (error) {
            console.error("Error sending message:", error);
        }
    };

    // Handle marking a message as read
    const markMessageAsRead = async (messageId: string) => {
        if (!chatServiceRef.current) return;

        try {
            await chatServiceRef.current.markMessageAsRead(messageId);
        } catch (error) {
            console.error("Error marking message as read:", error);
        }
    };

    // Handle selecting a chat
    const selectChat = (chatId: string) => {
        setActiveChat(chatId);
        fetchMessages(chatId);
    };

    return {
        chats,
        messages,
        loading,
        activeChat,
        selectChat,
        sendMessage,
        markMessageAsRead,
    };
}
