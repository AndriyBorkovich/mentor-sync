import React, { useEffect } from "react";
import ChatList from "./ChatList";
import ChatWindow from "./ChatWindow";
import { useChat } from "../hooks/useChat";

interface MessagesContentProps {
    initialChatId?: string | null;
}

const MessagesContent: React.FC<MessagesContentProps> = ({ initialChatId }) => {
    const { chats, messages, loading, activeChat, selectChat, sendMessage } =
        useChat();

    const selectedChat = activeChat
        ? chats.find((chat) => chat.id === activeChat) || null
        : null;

    const chatMessages = activeChat ? messages[activeChat] || [] : [];

    // Select the initial chat if provided
    useEffect(() => {
        if (initialChatId && chats.length > 0 && !activeChat) {
            const chatExists = chats.some((chat) => chat.id === initialChatId);
            if (chatExists) {
                selectChat(initialChatId);
            }
        }
    }, [initialChatId, chats, activeChat, selectChat]);

    const handleChatSelect = (chatId: string) => {
        selectChat(chatId);
    };

    const handleSendMessage = (recipientId: number, content: string) => {
        sendMessage(recipientId, content);
    };

    return (
        <div className="flex flex-1 overflow-hidden">
            {/* Chat List Sidebar */}
            <div className="w-80 flex-shrink-0 border-r border-[#E2E8F0] overflow-hidden">
                {loading ? (
                    <div className="flex justify-center items-center h-full">
                        <span className="material-icons animate-spin">
                            refresh
                        </span>
                    </div>
                ) : (
                    <ChatList
                        chats={chats}
                        activeChat={activeChat}
                        onChatSelect={handleChatSelect}
                    />
                )}
            </div>

            {/* Chat Window */}
            <div className="flex-1 overflow-hidden">
                <ChatWindow
                    chat={selectedChat}
                    messages={chatMessages}
                    onSendMessage={handleSendMessage}
                />
            </div>
        </div>
    );
};

export default MessagesContent;
