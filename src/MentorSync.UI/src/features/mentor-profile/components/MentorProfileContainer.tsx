import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import MentorProfileTabs, { ProfileTabType } from "./MentorProfileTabs";
import {
    getMentorBasicInfo,
    getMentorReviews,
    getMentorUpcomingSessions,
    getMentorMaterials,
    MentorBasicInfo,
    MentorReviews,
    MentorUpcomingSessions,
    MentorMaterials,
} from "../services/mentorProfileService";
import { MentorData } from "../types/mentorTypes";
import { Mentor } from "../../dashboard/data/mentors";
import { ensureStringId } from "../types/mentorTypes";

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
            />
        </div>
    );
};

export default MentorProfileContainer;
