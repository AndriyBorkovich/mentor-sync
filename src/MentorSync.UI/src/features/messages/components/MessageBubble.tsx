import React from "react";
import { Message } from "../../../shared/types";

interface MessageBubbleProps {
    message: Message;
    isCurrentUser: boolean;
}
export const MessageBubble: React.FC<MessageBubbleProps> = ({
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
