import React from "react";
import { Chat } from "../data/chats";

interface ChatListItemProps {
    chat: Chat;
    isActive: boolean;
    onClick: () => void;
}

const ChatListItem: React.FC<ChatListItemProps> = ({
    chat,
    isActive,
    onClick,
}) => {
    const { participantName, lastMessage, unreadCount, isOnline } = chat;

    return (
        <div
            onClick={onClick}
            className={`flex p-4 cursor-pointer transition-colors ${
                isActive
                    ? "bg-[#F8FAFC]"
                    : "hover:bg-[#F8FAFC] hover:bg-opacity-60"
            } mb-1 rounded-lg`}
        >
            <div className="relative mr-3">
                <div className="h-12 w-12 rounded-full bg-gray-200 flex-shrink-0">
                    {chat.participantAvatar && (
                        <img
                            src={chat.participantAvatar}
                            alt={participantName}
                            className="h-full w-full rounded-full object-cover"
                        />
                    )}
                </div>
                {isOnline && (
                    <div className="absolute bottom-0 right-0 h-3 w-3 rounded-full bg-green-500 border-2 border-white"></div>
                )}
            </div>
            <div className="flex-1 min-w-0">
                <div className="flex justify-between items-center mb-1">
                    <h4 className="font-medium text-[#1E293B] text-base truncate">
                        {participantName}
                    </h4>
                    {lastMessage && (
                        <span className="text-xs text-[#64748B]">
                            {lastMessage.timestamp}
                        </span>
                    )}
                </div>
                {lastMessage && (
                    <div className="flex justify-between items-center">
                        <p
                            className={`text-sm truncate ${
                                unreadCount > 0
                                    ? "text-[#1E293B] font-medium"
                                    : "text-[#64748B]"
                            }`}
                        >
                            {lastMessage.senderId === "current-user" && "Ви: "}
                            {lastMessage.content}
                        </p>
                        {unreadCount > 0 && (
                            <div className="flex-shrink-0 ml-2">
                                <span className="bg-[#6C5DD3] text-white text-xs px-2 py-1 rounded-full">
                                    {unreadCount}
                                </span>
                            </div>
                        )}
                    </div>
                )}
            </div>
        </div>
    );
};

interface ChatListProps {
    chats: Chat[];
    activeChat: string | null;
    onChatSelect: (chatId: string) => void;
}

const ChatList: React.FC<ChatListProps> = ({
    chats,
    activeChat,
    onChatSelect,
}) => {
    return (
        <div className="h-full flex flex-col">
            <div className="p-4 border-b border-[#E2E8F0]">
                <h2 className="text-xl font-bold text-[#1E293B]">
                    Повідомлення
                </h2>
                <div className="relative mt-4">
                    <span className="absolute inset-y-0 left-0 flex items-center pl-3">
                        <span className="material-icons text-[#64748B]">
                            search
                        </span>
                    </span>
                    <input
                        type="text"
                        placeholder="Пошук..."
                        className="w-full py-3 pl-10 pr-4 border border-[#E2E8F0] rounded-lg focus:outline-none focus:border-[#6C5DD3] text-[#1E293B] placeholder-[#94A3B8]"
                    />
                </div>
            </div>
            <div className="flex-1 overflow-y-auto">
                {chats.map((chat) => (
                    <ChatListItem
                        key={chat.id}
                        chat={chat}
                        isActive={chat.id === activeChat}
                        onClick={() => onChatSelect(chat.id)}
                    />
                ))}
            </div>
        </div>
    );
};

export default ChatList;
