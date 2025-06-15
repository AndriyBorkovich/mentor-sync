import React from "react";
import { Chat } from "../../../shared/types";
import { getUserId } from "../../auth";

interface ChatListItemProps {
    chat: Chat;
    isActive: boolean;
    onClick: () => void;
}
export const ChatListItem: React.FC<ChatListItemProps> = ({
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
                            src={
                                chat.participantAvatar
                                    ? chat.participantAvatar
                                    : "https://ui-avatars.com/api/?name=" +
                                      encodeURIComponent(chat.participantName) +
                                      "&background=F3F4F6&color=1E293B&size=64"
                            }
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
                            {lastMessage.senderId === getUserId() && "Ви: "}
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
