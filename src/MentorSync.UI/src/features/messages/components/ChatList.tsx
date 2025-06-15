import React from "react";
import { Chat } from "../../../shared/types";
import { ChatListItem } from "./ChatListItem";

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
