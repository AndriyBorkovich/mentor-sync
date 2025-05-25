import React, { useState } from "react";
import { mockChats, mockMessages } from "../data/chats";
import ChatList from "./ChatList";
import ChatWindow from "./ChatWindow";

const MessagesContent: React.FC = () => {
    const [activeChat, setActiveChat] = useState<string | null>(null);

    const selectedChat = activeChat
        ? mockChats.find((chat) => chat.id === activeChat) || null
        : null;

    const chatMessages = activeChat ? mockMessages[activeChat] || [] : [];

    const handleChatSelect = (chatId: string) => {
        setActiveChat(chatId);
    };

    return (
        <div className="flex flex-1 overflow-hidden">
            {/* Chat List Sidebar */}
            <div className="w-80 flex-shrink-0 border-r border-[#E2E8F0] overflow-hidden">
                <ChatList
                    chats={mockChats}
                    activeChat={activeChat}
                    onChatSelect={handleChatSelect}
                />
            </div>

            {/* Chat Window */}
            <div className="flex-1 overflow-hidden">
                <ChatWindow chat={selectedChat} messages={chatMessages} />
            </div>
        </div>
    );
};

export default MessagesContent;
