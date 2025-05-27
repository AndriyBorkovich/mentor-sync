import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import MentorProfileTabs, { ProfileTabType } from "./MentorProfileTabs";
import {
    getMentorBasicInfo,
    getMentorReviews,
    getMentorUpcomingSessions,
    getMentorMaterials,
    recordMentorViewEvent,
    toggleBookmark,
    checkIfMentorIsBookmarked,
    MentorBasicInfo,
    MentorReviews,
    MentorUpcomingSessions,
    MentorMaterials,
} from "../services/mentorProfileService";
import { MentorData } from "../types/mentorTypes";
import { Mentor } from "../../dashboard/data/mentors";
import { ensureStringId } from "../types/mentorTypes";
import { hasRole } from "../../auth/utils/authUtils";
import { toast } from "react-toastify";

// Fallback mock mentor to display while loading or if error occurs
const mockMentor: Mentor = {
    id: "1",
    name: "Loading...",
    title: "Loading...",
    profileImage: "/assets/avatars/default.jpg",
    rating: 0,
    yearsOfExperience: 0,
    skills: [],
};

const MentorProfileContainer: React.FC = () => {
    const { mentorId } = useParams<{ mentorId: string }>();
    const [mentor, setMentor] = useState<MentorData>(mockMentor);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [activeTab, setActiveTab] = useState<ProfileTabType>("about");
    const [viewEventRecorded, setViewEventRecorded] = useState<boolean>(false);
    const [isBookmarked, setIsBookmarked] = useState<boolean>(false);
    const [bookmarkLoading, setBookmarkLoading] = useState<boolean>(false);

    // Track data from each endpoint
    const [basicInfo, setBasicInfo] = useState<MentorBasicInfo | null>(null);
    const [reviews, setReviews] = useState<MentorReviews | null>(null);
    const [sessions, setUpcomingSessions] =
        useState<MentorUpcomingSessions | null>(null);
    const [materials, setMaterials] = useState<MentorMaterials | null>(null);

    // Track loading state for each endpoint
    const [loadingBasicInfo, setLoadingBasicInfo] = useState<boolean>(false);
    const [loadingReviews, setLoadingReviews] = useState<boolean>(false);
    const [loadingSessions, setLoadingSessions] = useState<boolean>(false);
    const [loadingMaterials, setLoadingMaterials] = useState<boolean>(false);

    // Check if mentor is bookmarked when the component mounts
    useEffect(() => {
        const checkBookmarkStatus = async () => {
            if (mentorId && hasRole("Mentee")) {
                try {
                    const mentorIdInt = parseInt(mentorId, 10);
                    const bookmarked = await checkIfMentorIsBookmarked(
                        mentorIdInt
                    );
                    setIsBookmarked(bookmarked);
                } catch (err) {
                    console.error("Failed to check bookmark status:", err);
                }
            }
        };

        checkBookmarkStatus();
    }, [mentorId]); // Handle bookmark toggling
    const handleToggleBookmark = async () => {
        if (!mentorId || bookmarkLoading) return;

        try {
            setBookmarkLoading(true);
            const mentorIdInt = parseInt(mentorId, 10);
            const success = await toggleBookmark(mentorIdInt, isBookmarked);

            if (success) {
                const newBookmarkState = !isBookmarked;
                setIsBookmarked(newBookmarkState);

                // Show feedback to the user
                if (newBookmarkState) {
                    toast.success("Ментора додано до обраних");
                } else {
                    toast.info("Ментора видалено з обраних");
                }
            } else {
                toast.error("Не вдалося оновити обрані");
            }
        } catch (err) {
            console.error("Failed to toggle bookmark:", err);
            toast.error("Виникла помилка. Спробуйте ще раз пізніше.");
        } finally {
            setBookmarkLoading(false);
        }
    };

    // Record view event when mentor profile is loaded
    useEffect(() => {
        const recordViewEvent = async () => {
            if (mentorId && !viewEventRecorded) {
                try {
                    // Only users with mentee role can trigger view events
                    if (hasRole("Mentee")) {
                        const mentorIdInt = parseInt(mentorId, 10);
                        const success = await recordMentorViewEvent(
                            mentorIdInt
                        );
                        if (success) {
                            setViewEventRecorded(true);
                            console.log(
                                "Mentor view event recorded successfully"
                            );
                        }
                    }
                } catch (err) {
                    console.error("Failed to record mentor view event:", err);
                }
            }
        };

        if (!loading && mentor && mentor.id && !viewEventRecorded) {
            recordViewEvent();
        }
    }, [mentorId, mentor, loading, viewEventRecorded]);

    // Fetch data based on the active tab
    useEffect(() => {
        const fetchMentorData = async () => {
            if (!mentorId) {
                setError("Mentor ID is required");
                setLoading(false);
                return;
            }

            const mentorIdInt = parseInt(mentorId, 10);

            try {
                setLoading(true);

                // Always load basic info
                if (!basicInfo && !loadingBasicInfo) {
                    setLoadingBasicInfo(true);
                    try {
                        const basicInfoData = await getMentorBasicInfo(
                            mentorIdInt
                        );

                        // Transform skill IDs to string
                        const updatedBasicInfo = {
                            ...basicInfoData,
                            skills: basicInfoData.skills.map((skill) => ({
                                ...skill,
                                id: ensureStringId(skill.id),
                            })),
                        };

                        setBasicInfo(updatedBasicInfo);
                    } catch (err) {
                        console.error("Failed to fetch basic info:", err);
                    } finally {
                        setLoadingBasicInfo(false);
                    }
                }

                // Load reviews data if on reviews tab or not yet loaded
                if (
                    (activeTab === "reviews" || activeTab === "about") &&
                    !reviews &&
                    !loadingReviews
                ) {
                    setLoadingReviews(true);
                    try {
                        const reviewsData = await getMentorReviews(mentorIdInt);
                        setReviews(reviewsData);
                    } catch (err) {
                        console.error("Failed to fetch reviews:", err);
                        setReviews({ reviewCount: 0, reviews: [] });
                    } finally {
                        setLoadingReviews(false);
                    }
                }

                // Load sessions data if on sessions tab or not yet loaded
                if (activeTab === "sessions" && !sessions && !loadingSessions) {
                    setLoadingSessions(true);
                    try {
                        const sessionsData = await getMentorUpcomingSessions(
                            mentorIdInt
                        );
                        setUpcomingSessions(sessionsData);
                    } catch (err) {
                        console.error("Failed to fetch sessions:", err);
                        setUpcomingSessions({ upcomingSessions: [] });
                    } finally {
                        setLoadingSessions(false);
                    }
                }

                // Load materials data if on materials tab or not yet loaded
                if (
                    activeTab === "materials" &&
                    !materials &&
                    !loadingMaterials
                ) {
                    setLoadingMaterials(true);
                    try {
                        const materialsData = await getMentorMaterials(
                            mentorIdInt
                        );
                        setMaterials(materialsData);
                    } catch (err) {
                        console.error("Failed to fetch materials:", err);
                        setMaterials({ materials: [] });
                    } finally {
                        setLoadingMaterials(false);
                    }
                }

                // Combine all data into a mentor profile object
                if (basicInfo) {
                    try {
                        const mentorProfile = {
                            ...basicInfo,
                            reviewCount: reviews?.reviewCount || 0,
                            recentReviews: reviews?.reviews || [],
                            upcomingSessions: sessions?.upcomingSessions || [],
                            materials: materials?.materials || [],
                        };

                        setMentor(mentorProfile);
                        setError(null);
                    } catch (err) {
                        setError(
                            "Failed to load mentor profile. Please try again later."
                        );
                    }
                }
            } catch (err) {
                console.error("Failed to fetch mentor data:", err);
                setError(
                    "Failed to load mentor profile. Please try again later."
                );
            } finally {
                setLoading(false);
            }
        };

        fetchMentorData();
    }, [
        mentorId,
        activeTab,
        basicInfo,
        reviews,
        sessions,
        materials,
        loadingBasicInfo,
        loadingReviews,
        loadingSessions,
        loadingMaterials,
    ]);

    const handleTabChange = (tab: ProfileTabType) => {
        setActiveTab(tab);
    };

    if (loading) {
        return <div className="p-6 text-center">Loading mentor profile...</div>;
    }

    if (error) {
        return <div className="p-6 text-center text-red-500">{error}</div>;
    }

    return (
        <div className="container mx-auto p-4">
            <MentorProfileTabs
                mentor={mentor}
                activeTab={activeTab}
                onTabChange={handleTabChange}
                isBookmarked={isBookmarked}
                onToggleBookmark={handleToggleBookmark}
                bookmarkLoading={bookmarkLoading}
            />
        </div>
    );
};

export default MentorProfileContainer;
