import React, { useState, useEffect, useRef } from "react";
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
} from "../services/mentorProfileService";
import { MentorData } from "../types/mentorTypes";
import { ensureStringId } from "../types/mentorTypes";
import { hasRole } from "../../auth/utils/authUtils";
import { toast } from "react-toastify";
import {
    Mentor,
    MentorBasicInfo,
    MentorMaterials,
    MentorReviews,
    MentorUpcomingSessions,
} from "../../../shared/types";

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
    const [reviewsRefreshTrigger, setReviewsRefreshTrigger] =
        useState<number>(0);

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

    // Use refs to track if data has been loaded for a specific tab
    const reviewsLoadedRef = useRef<boolean>(false);
    const sessionsLoadedRef = useRef<boolean>(false);
    const materialsLoadedRef = useRef<boolean>(false);

    // Use ref for active tab to prevent unnecessary rerenders
    const activeTabRef = useRef<ProfileTabType>("about");

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
    }, [mentorId]);

    // Handle bookmark toggling
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

    // Fetch basic info once on mount
    useEffect(() => {
        const fetchBasicInfo = async () => {
            if (!mentorId || basicInfo || loadingBasicInfo) return;

            try {
                setLoadingBasicInfo(true);
                const mentorIdInt = parseInt(mentorId, 10);
                const basicInfoData = await getMentorBasicInfo(mentorIdInt);

                // Transform skill IDs to string
                const updatedBasicInfo = {
                    ...basicInfoData,
                    skills: basicInfoData.skills.map((skill) => ({
                        ...skill,
                        id: ensureStringId(skill.id),
                    })),
                };

                setBasicInfo(updatedBasicInfo);
                setLoading(false);
                setError(null);
            } catch (err) {
                console.error("Failed to fetch basic info:", err);
                setError(
                    "Failed to load mentor profile. Please try again later."
                );
                setLoading(false);
            } finally {
                setLoadingBasicInfo(false);
            }
        };

        fetchBasicInfo();
    }, [mentorId, basicInfo, loadingBasicInfo]); // Load tab-specific data only when the tab changes
    useEffect(() => {
        const fetchTabData = async () => {
            if (!mentorId || !basicInfo) return;

            // Update ref to track current active tab
            activeTabRef.current = activeTab;
            const mentorIdInt = parseInt(mentorId, 10);
            if (
                activeTab === "reviews" &&
                (!reviewsLoadedRef.current || reviewsRefreshTrigger > 0) &&
                !loadingReviews
            ) {
                setLoadingReviews(true);
                try {
                    const reviewsData = await getMentorReviews(mentorIdInt);

                    // Only update state if this is still the active tab
                    if (activeTabRef.current === "reviews") {
                        setReviews(reviewsData);
                        reviewsLoadedRef.current = true;
                        // Reset the refresh trigger after successfully loading data
                        if (reviewsRefreshTrigger > 0) {
                            setReviewsRefreshTrigger(0);
                        }
                    }
                } catch (err) {
                    console.error("Failed to fetch reviews:", err);
                } finally {
                    setLoadingReviews(false);
                }
            } else if (
                activeTab === "sessions" &&
                !sessionsLoadedRef.current &&
                !loadingSessions
            ) {
                setLoadingSessions(true);
                try {
                    const sessionsData = await getMentorUpcomingSessions(
                        mentorIdInt
                    );

                    // Only update state if this is still the active tab
                    if (activeTabRef.current === "sessions") {
                        setUpcomingSessions(sessionsData);
                        sessionsLoadedRef.current = true;
                    }
                } catch (err) {
                    console.error("Failed to fetch sessions:", err);
                } finally {
                    setLoadingSessions(false);
                }
            } else if (
                activeTab === "materials" &&
                !materialsLoadedRef.current &&
                !loadingMaterials
            ) {
                setLoadingMaterials(true);
                try {
                    const materialsData = await getMentorMaterials(mentorIdInt);

                    // Only update state if this is still the active tab
                    if (activeTabRef.current === "materials") {
                        setMaterials(materialsData);
                        materialsLoadedRef.current = true;
                    }
                } catch (err) {
                    console.error("Failed to fetch materials:", err);
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
                } catch (err) {
                    console.error("Error updating mentor profile:", err);
                }
            }
        };

        fetchTabData();
    }, [
        mentorId,
        activeTab,
        basicInfo,
        loadingReviews,
        loadingSessions,
        loadingMaterials,
        reviewsRefreshTrigger,
    ]);

    const handleTabChange = (tab: ProfileTabType) => {
        setActiveTab(tab);
    }; // Method to refresh reviews data
    const handleRefreshReviews = () => {
        // Only refresh if we're not already loading
        if (!loadingReviews && reviewsRefreshTrigger === 0) {
            // Reset review loaded state to force reload
            reviewsLoadedRef.current = false;
            // Set the refresh trigger to 1 to indicate a refresh is needed
            setReviewsRefreshTrigger(1);
        }
    };

    if (loading && !basicInfo) {
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
                onRefreshReviews={handleRefreshReviews}
            />
        </div>
    );
};

export default MentorProfileContainer;
