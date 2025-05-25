import React, { useState } from "react";
import { PastSession } from "../data/pastSessions";
import ReviewModal from "./ReviewModal";

interface PastSessionCardProps {
    session: PastSession;
}

const PastSessionCard: React.FC<PastSessionCardProps> = ({ session }) => {
    const [showReviewModal, setShowReviewModal] = useState(false);
    const [isReviewed, setIsReviewed] = useState(session.reviewed);

    const handleReviewSubmit = (
        sessionId: string,
        rating: number,
        comment: string
    ) => {
        // In a real app, we'd send this to an API
        console.log(`Session ${sessionId} rated ${rating}/5: ${comment}`);
        setIsReviewed(true);
    };

    return (
        <>
            <div className="bg-white rounded-lg shadow-sm p-6 mb-6">
                <div className="flex flex-col md:flex-row">
                    {/* Mentor profile section */}
                    <div className="flex items-start mb-4 md:mb-0 md:mr-6">
                        <div className="w-12 h-12 bg-gray-200 rounded-full mr-4 flex-shrink-0"></div>
                        <div>
                            <h3 className="text-base font-medium text-[#000000]">
                                {session.mentorName}
                            </h3>
                            <p className="text-sm text-[#64748B]">
                                {session.title.split(" ")[0]}
                            </p>
                        </div>
                    </div>

                    {/* Session title and details */}
                    <div className="flex-1">
                        <div>
                            <h4 className="text-base font-medium text-[#000000] mb-1">
                                {session.title}
                            </h4>
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
                    {!isReviewed && (
                        <button
                            className="bg-[#6C5DD3] text-white py-3 px-6 rounded-lg hover:bg-[#5B4DC4]"
                            onClick={() => setShowReviewModal(true)}
                        >
                            Залишити відгук
                        </button>
                    )}
                </div>
            </div>

            {/* Review Modal */}
            {showReviewModal && (
                <ReviewModal
                    session={session}
                    onClose={() => setShowReviewModal(false)}
                    onSubmit={handleReviewSubmit}
                />
            )}
        </>
    );
};

export default PastSessionCard;
