import React from "react";
import { recommendedMentors, recentlyViewedMentors } from "../data/mentors";
import { upcomingSessions } from "../data/sessions";
import MentorCard from "./MentorCard";
import SessionCard from "./SessionCard";
import RecentlyViewedItem from "./RecentlyViewedItem";

interface SectionProps {
    title: string;
    children: React.ReactNode;
}

const Section: React.FC<SectionProps> = ({ title, children }) => {
    return (
        <div className="bg-white rounded-2xl shadow-md p-6 mb-8 w-full">
            <h2 className="text-lg font-bold text-[#1E293B] mb-6">{title}</h2>
            {children}
        </div>
    );
};

const DashboardContent: React.FC = () => {
    return (
        <div className="flex-1 p-6 overflow-y-auto">
            <h1 className="text-xl font-bold text-[#1E293B] mb-6">
                Рекомендовані ментори
            </h1>

            <div className="grid grid-cols-1 lg:grid-cols-3 gap-6 mb-8">
                {recommendedMentors.map((mentor) => (
                    <MentorCard key={mentor.id} mentor={mentor} />
                ))}
            </div>

            <div className="flex flex-col lg:flex-row gap-6">
                <div className="w-full lg:w-3/4">
                    <Section title="Наступні сесії">
                        {upcomingSessions.map((session) => (
                            <SessionCard key={session.id} session={session} />
                        ))}
                    </Section>
                </div>

                <div className="w-full lg:w-1/4">
                    <Section title="Нещодавно переглянуті">
                        {recentlyViewedMentors.map((mentor) => (
                            <RecentlyViewedItem
                                key={mentor.id}
                                mentor={mentor}
                            />
                        ))}
                    </Section>
                </div>
            </div>
        </div>
    );
};

export default DashboardContent;
