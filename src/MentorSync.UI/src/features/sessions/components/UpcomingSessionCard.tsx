import React, { useState } from "react";
import { Session } from "../../dashboard/data/sessions";
import SessionDetailsModal from "./SessionDetailsModal";

interface UpcomingSessionCardProps {
    session: Session;
}

const UpcomingSessionCard: React.FC<UpcomingSessionCardProps> = ({
    session,
}) => {
    const [showDetailsModal, setShowDetailsModal] = useState(false);

    return (
        <>
            <div className="bg-white rounded-lg shadow-sm p-6 mb-6">
                <div className="flex flex-col md:flex-row">
                    {/* Mentor profile section */}
                    <div className="flex items-start mb-4 md:mb-0 md:mr-6">
                        <div className="w-12 h-12 bg-gray-200 rounded-full mr-4 flex-shrink-0"></div>
                        <h3 className="text-base font-medium text-[#000000]">
                            {session.mentorName}
                        </h3>
                    </div>

                    {/* Session title and details */}
                    <div className="flex-1">
                        <div className="flex justify-between items-start">
                            <div>
                                <h4 className="text-base font-medium text-[#000000] mb-1">
                                    {session.title}
                                </h4>
                            </div>
                            <div className="md:ml-4">
                                <button className="text-[#64748B] hover:text-[#1E293B]">
                                    <span className="material-icons">
                                        more_horiz
                                    </span>
                                </button>
                            </div>
                        </div>

                        <div className="flex items-center text-xs text-[#64748B] mt-2">
                            <span className="material-icons text-sm mr-1">
                                schedule
                            </span>
                            <span>
                                {session.date} о {session.time}
                            </span>
                        </div>
                    </div>
                </div>

                <div className="flex justify-end mt-4">
                    <button
                        className="border border-[#E2E8F0] text-[#64748B] py-3 px-6 rounded-lg hover:bg-[#F8FAFC]"
                        onClick={() => setShowDetailsModal(true)}
                    >
                        Показати деталі
                    </button>
                </div>
            </div>

            {/* Session Details Modal */}
            {showDetailsModal && (
                <SessionDetailsModal
                    session={session}
                    onClose={() => setShowDetailsModal(false)}
                />
            )}
        </>
    );
};

export default UpcomingSessionCard;
