import React, { useState, useRef, useEffect } from "react";
import { Chat, Message } from "../data/chats";

interface MessageBubbleProps {
    message: Message;
    isCurrentUser: boolean;
}

const MessageBubble: React.FC<MessageBubbleProps> = ({
    message,
    isCurrentUser,
}) => {
    return (
        <div
            className={`flex mb-4 ${
                isCurrentUser ? "justify-end" : "justify-start"
            }`}
        >
            {!isCurrentUser && (
                <div className="w-8 h-8 rounded-full bg-gray-200 mr-2 mt-1 flex-shrink-0"></div>
            )}
            <div
                className={`max-w-[70%] px-4 py-3 rounded-lg ${
                    isCurrentUser
                        ? "bg-[#6C5DD3] text-white rounded-tr-none"
                        : "bg-[#F1F5F9] text-[#1E293B] rounded-tl-none"
                }`}
            >
                <p className="break-words">{message.content}</p>
                <div
                    className={`text-xs mt-1 ${
                        isCurrentUser ? "text-[#E2E8F0]" : "text-[#94A3B8]"
                    } text-right`}
                >
                    {message.timestamp}
                    {isCurrentUser && (
                        <span className="material-icons text-xs ml-1">
                            {message.read ? "done_all" : "done"}
                        </span>
                    )}
                </div>
            </div>
        </div>
    );
};

interface ChatWindowProps {
    chat: Chat | null;
    messages: Message[];
}

const ChatWindow: React.FC<ChatWindowProps> = ({ chat, messages }) => {
    const [newMessage, setNewMessage] = useState("");
    const messagesEndRef = useRef<HTMLDivElement>(null);

    const scrollToBottom = () => {
        messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
    };

    useEffect(() => {
        scrollToBottom();
    }, [messages]);

    const handleSendMessage = (e: React.FormEvent) => {
        e.preventDefault();
        if (newMessage.trim() && chat) {
            // In a real app, we would send this message to an API
            console.log(
                `Sending message to ${chat.participantName}: ${newMessage}`
            );
            // Clear the input
            setNewMessage("");
        }
    };

    if (!chat) {
        return (
            <div className="flex h-full items-center justify-center text-center p-8 text-[#64748B]">
                <div>
                    <span className="material-icons text-[#94A3B8] text-5xl mb-4">
                        chat
                    </span>
                    <h3 className="text-xl font-medium mb-2">
                        Виберіть чат для початку спілкування
                    </h3>
                    <p>
                        Оберіть співрозмовника зі списку ліворуч або почніть
                        нову розмову.
                    </p>
                </div>
            </div>
        );
    }

    return (
        <div className="h-full flex flex-col">
            {/* Chat Header */}
            <div className="p-4 border-b border-[#E2E8F0] flex items-center">
                <div className="relative mr-3">
                    <div className="h-10 w-10 rounded-full bg-gray-200">
                        {chat.participantAvatar && (
                            <img
                                src={chat.participantAvatar}
                                alt={chat.participantName}
                                className="h-full w-full rounded-full object-cover"
                            />
                        )}
                    </div>
                    {chat.isOnline && (
                        <div className="absolute bottom-0 right-0 h-2.5 w-2.5 rounded-full bg-green-500 border-2 border-white"></div>
                    )}
                </div>
                <div>
                    <h3 className="font-medium text-[#1E293B]">
                        {chat.participantName}
                    </h3>
                    <p className="text-xs text-[#64748B]">
                        {chat.isOnline ? "В мережі" : "Не в мережі"}
                    </p>
                </div>
                <div className="ml-auto">
                    <button className="p-2 hover:bg-[#F1F5F9] rounded-full">
                        <span className="material-icons text-[#64748B]">
                            more_vert
                        </span>
                    </button>
                </div>
            </div>

            {/* Messages Area */}
            <div className="flex-1 overflow-y-auto p-4 bg-white">
                {messages.map((message) => (
                    <MessageBubble
                        key={message.id}
                        message={message}
                        isCurrentUser={message.senderId === "current-user"}
                    />
                ))}
                <div ref={messagesEndRef} />
            </div>

            {/* Message Input */}
            <div className="p-4 border-t border-[#E2E8F0] bg-white">
                <form onSubmit={handleSendMessage} className="flex">
                    <input
                        type="text"
                        value={newMessage}
                        onChange={(e) => setNewMessage(e.target.value)}
                        placeholder="Введіть повідомлення..."
                        className="flex-1 py-3 px-4 border border-[#E2E8F0] rounded-l-lg focus:outline-none focus:border-[#6C5DD3]"
                    />
                    <button
                        type="submit"
                        className="bg-[#6C5DD3] ml-1 text-white px-4 rounded-r-lg hover:bg-[#5B4DC4] focus:outline-none"
                        disabled={!newMessage.trim()}
                    >
                        <span className="material-icons">send</span>
                    </button>
                </form>
            </div>
        </div>
    );
};

export default ChatWindow;
