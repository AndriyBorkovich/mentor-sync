import api from "../../../shared/services/api";
import { useNavigate } from "react-router-dom";

export interface InitiateChatResponse {
    chatRoomId: string;
}

/**
 * Initiates a chat with a mentor
 * @param recipientId The ID of the mentor to chat with
 * @returns The chat room ID
 */
export const initiateChat = async (recipientId: number): Promise<string> => {
    try {
        const response = await api.post<InitiateChatResponse>(
            "/chat/initiate",
            { recipientId }
        );
        return response.data.chatRoomId;
    } catch (error) {
        console.error("Error initiating chat:", error);
        throw error;
    }
};

/**
 * Custom hook that provides navigation to chat screen after initiating a chat
 * @returns Function to initiate chat and navigate
 */
export const useInitiateChat = () => {
    const navigate = useNavigate();

    const startChatWithMentor = async (mentorId: number) => {
        try {
            // Initiate chat and get room ID
            const chatRoomId = await initiateChat(mentorId);

            // Navigate to the messages page with the chat room ID as a query parameter
            // This will allow the messages page to open this specific chat
            navigate(`/messages?chatId=${chatRoomId}`);

            return chatRoomId;
        } catch (error) {
            console.error("Failed to start chat:", error);
            throw error;
        }
    };

    return { startChatWithMentor };
};
