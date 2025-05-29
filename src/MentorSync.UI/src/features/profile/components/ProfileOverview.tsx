import React from "react";
import { UserProfile } from "../../auth/hooks/useUserProfile";

interface ProfileOverviewProps {
    profile?: UserProfile | null;
    isLoading: boolean;
}

const ProfileOverview: React.FC<ProfileOverviewProps> = ({
    profile,
    isLoading,
}) => {
    // Determine which profile to display
    const isMentee = profile?.role === "Mentee";
    const isMentor = profile?.role === "Mentor";
    const menteeProfile = profile?.menteeProfile;
    const mentorProfile = profile?.mentorProfile;

    // Loading state
    if (isLoading) {
        return (
            <div className="bg-white rounded-lg shadow p-6 animate-pulse">
                <div className="h-6 bg-gray-200 rounded mb-4 w-1/3"></div>
                <div className="h-4 bg-gray-200 rounded mb-2 w-full"></div>
                <div className="h-4 bg-gray-200 rounded mb-2 w-full"></div>
                <div className="h-4 bg-gray-200 rounded w-3/4"></div>
            </div>
        );
    }

    // No profile info available
    if (!menteeProfile && !mentorProfile) {
        return (
            <div className="bg-white rounded-lg shadow p-6">
                <h2 className="text-xl font-semibold text-gray-800 mb-4">
                    Інформація про профіль
                </h2>
                <p className="text-gray-600">
                    Профіль не заповнений. Будь ласка, завершіть процес
                    онбордингу, щоб заповнити ваш профіль.
                </p>
            </div>
        );
    }

    const displayProfile = isMentee ? menteeProfile : mentorProfile;

    return (
        <div className="bg-white rounded-lg shadow p-6">
            <h2 className="text-xl font-semibold text-gray-800 mb-4">
                Інформація про профіль
            </h2>

            <div className="space-y-4">
                {displayProfile?.bio && (
                    <div>
                        <h3 className="font-medium text-gray-700">Про мене</h3>
                        <p className="text-gray-600 mt-1">
                            {displayProfile.bio}
                        </p>
                    </div>
                )}

                {displayProfile?.position && (
                    <div>
                        <h3 className="font-medium text-gray-700">Посада</h3>
                        <p className="text-gray-600 mt-1">
                            {displayProfile.position}
                        </p>
                    </div>
                )}

                {displayProfile?.company && (
                    <div>
                        <h3 className="font-medium text-gray-700">Компанія</h3>
                        <p className="text-gray-600 mt-1">
                            {displayProfile.company}
                        </p>
                    </div>
                )}

                {isMentor && mentorProfile?.experienceYears !== undefined && (
                    <div>
                        <h3 className="font-medium text-gray-700">Досвід</h3>
                        <p className="text-gray-600 mt-1">
                            {mentorProfile.experienceYears} років
                        </p>
                    </div>
                )}

                {displayProfile?.skills && displayProfile.skills.length > 0 && (
                    <div>
                        <h3 className="font-medium text-gray-700">Навички</h3>
                        <div className="flex flex-wrap gap-2 mt-1">
                            {displayProfile.skills.map((skill, index) => (
                                <span
                                    key={index}
                                    className="bg-gray-100 text-gray-700 px-2 py-1 rounded-md text-sm"
                                >
                                    {skill}
                                </span>
                            ))}
                        </div>
                    </div>
                )}

                {displayProfile?.programmingLanguages &&
                    displayProfile.programmingLanguages.length > 0 && (
                        <div>
                            <h3 className="font-medium text-gray-700">
                                Мови програмування
                            </h3>
                            <div className="flex flex-wrap gap-2 mt-1">
                                {displayProfile.programmingLanguages.map(
                                    (language, index) => (
                                        <span
                                            key={index}
                                            className="bg-primary/10 text-primary px-2 py-1 rounded-md text-sm"
                                        >
                                            {language}
                                        </span>
                                    )
                                )}
                            </div>
                        </div>
                    )}

                {isMentee &&
                    menteeProfile?.learningGoals &&
                    menteeProfile.learningGoals.length > 0 && (
                        <div>
                            <h3 className="font-medium text-gray-700">
                                Цілі навчання
                            </h3>
                            <ul className="list-disc list-inside text-gray-600 mt-1">
                                {menteeProfile.learningGoals.map(
                                    (goal, index) => (
                                        <li key={index}>{goal}</li>
                                    )
                                )}
                            </ul>
                        </div>
                    )}
            </div>
        </div>
    );
};

export default ProfileOverview;
