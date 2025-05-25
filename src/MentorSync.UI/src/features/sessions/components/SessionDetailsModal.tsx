import React from "react";
import { Session } from "../../dashboard/data/sessions";
import { useForm } from "react-hook-form";

interface SessionDetailsModalProps {
    session: Session;
    onClose: () => void;
}

interface SessionDetailsFormData {
    attendSession: boolean;
}

const SessionDetailsModal: React.FC<SessionDetailsModalProps> = ({
    session,
    onClose,
}) => {
    // Initialize React Hook Form
    const {
        handleSubmit,
        formState: { isSubmitting },
    } = useForm<SessionDetailsFormData>({
        defaultValues: {
            attendSession: false,
        },
    });

    // These would be populated with actual data in a real application
    const sessionDetails = {
        description:
            "В рамках цієї сесії ми розглянемо основні принципи системного дизайну та архітектури, які допоможуть вам створювати масштабовані та відмовостійкі системи.",
        topics: [
            "Базові принципи системного дизайну",
            "Масштабованість та продуктивність",
            "Надійність та стійкість до відмов",
            "Розподілені системи",
        ],
        duration: "60 хвилин",
        location: "Google Meet",
    };

    const onJoinSession = handleSubmit((_data) => {
        // Simulate joining the session
        return new Promise<void>((resolve) => {
            setTimeout(() => {
                console.log("Joining session:", session.id);
                // Here you would implement logic to join the session
                resolve();
            }, 500);
        });
    });

    return (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
            <div className="bg-white rounded-lg max-w-2xl w-full mx-4">
                <div className="p-6">
                    <div className="flex justify-between items-center mb-6">
                        <h2 className="text-xl font-bold text-[#1E293B]">
                            Деталі сесії
                        </h2>
                        <button
                            className="text-[#64748B] hover:text-[#1E293B]"
                            onClick={onClose}
                        >
                            <span className="material-icons">close</span>
                        </button>
                    </div>

                    <div className="mb-6">
                        <div className="flex items-center mb-4">
                            <img
                                src="/placeholder-avatar.jpg"
                                alt={session.mentorName}
                                className="w-12 h-12 rounded-full bg-gray-200 mr-4"
                            />
                            <div>
                                <h3 className="font-medium text-[#1E293B]">
                                    {session.mentorName}
                                </h3>
                                <p className="text-sm text-[#64748B]">
                                    {session.title.split(" ")[0]}
                                </p>
                            </div>
                        </div>

                        <h4 className="text-xl font-medium text-[#1E293B] mb-2">
                            {session.title}
                        </h4>

                        <div className="flex items-center text-sm text-[#64748B] mb-4">
                            <span className="material-icons text-sm mr-1">
                                calendar_today
                            </span>
                            <span>
                                {session.date} о {session.time}
                            </span>
                            <span className="mx-2">|</span>
                            <span className="material-icons text-sm mr-1">
                                schedule
                            </span>
                            <span>{sessionDetails.duration}</span>
                            <span className="mx-2">|</span>
                            <span className="material-icons text-sm mr-1">
                                videocam
                            </span>
                            <span>{sessionDetails.location}</span>
                        </div>

                        <div className="mb-6">
                            <h5 className="font-medium text-[#1E293B] mb-2">
                                Опис
                            </h5>
                            <p className="text-[#64748B]">
                                {sessionDetails.description}
                            </p>
                        </div>

                        <div>
                            <h5 className="font-medium text-[#1E293B] mb-2">
                                Теми
                            </h5>
                            <ul className="list-disc pl-5">
                                {sessionDetails.topics.map((topic, index) => (
                                    <li
                                        key={index}
                                        className="text-[#64748B] mb-1"
                                    >
                                        {topic}
                                    </li>
                                ))}
                            </ul>
                        </div>
                    </div>

                    <form onSubmit={onJoinSession}>
                        <div className="flex justify-end space-x-4">
                            <button
                                type="button"
                                className="px-6 py-3 border border-[#E2E8F0] rounded-lg text-[#64748B] hover:bg-[#F8FAFC]"
                                onClick={onClose}
                                disabled={isSubmitting}
                            >
                                Закрити
                            </button>
                            <button
                                type="submit"
                                className="px-6 py-3 bg-[#6C5DD3] text-white rounded-lg hover:bg-[#5B4DC4]"
                                disabled={isSubmitting}
                            >
                                {isSubmitting
                                    ? "Підключення..."
                                    : "Приєднатися до сесії"}
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
};

export default SessionDetailsModal;
