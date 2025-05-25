import React from "react";
import { Session } from "../data/sessions";

interface SessionCardProps {
    session: Session;
}

const SessionCard: React.FC<SessionCardProps> = ({ session }) => {
    return (
        <div className="bg-[#F8FAFC] rounded-lg p-4 mb-4">
            <div className="flex justify-between items-start">
                <div>
                    <h3 className="text-lg font-medium text-[#1E293B]">
                        {session.mentorName}
                    </h3>
                    <p className="text-sm text-[#64748B]">{session.title}</p>
                </div>
                <div className="text-right">
                    <p className="text-sm font-medium text-[#1E293B]">
                        {session.time}
                    </p>
                    <p className="text-sm text-[#64748B]">{session.date}</p>
                </div>
            </div>
            <div className="mt-4 flex justify-end">
                <button className="bg-[#4318D1] text-white py-2 px-6 rounded-lg text-sm">
                    Приєднатись
                </button>
            </div>
        </div>
    );
};

export default SessionCard;
