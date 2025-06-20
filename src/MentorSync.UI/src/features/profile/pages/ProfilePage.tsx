import React, { useState } from "react";
import { useUserProfile } from "../../auth/hooks/useUserProfile";
import Sidebar from "../../../components/layout/Sidebar";
import Header from "../../../components/layout/Header";
import ProfileHeader from "../components/ProfileHeader";
import ProfileOverview from "../components/ProfileOverview";
import ProfileInfoCard from "../components/ProfileInfoCard";
import { ProfileSkeleton } from "../components/ProfileSkeleton";
import { formatRoleName } from "../../../shared/utils/formatters";

const ProfilePage: React.FC = () => {
    const { profile, loading, error } = useUserProfile();
    const [sidebarExpanded, setSidebarExpanded] = useState(false);

    const handleSidebarToggle = (expanded: boolean) => {
        setSidebarExpanded(expanded);
    };

    if (error) {
        return (
            <div className="flex flex-col min-h-screen">
                <Header />
                <div className="flex flex-1">
                    <Sidebar />
                    <main className="flex-1 p-8 bg-slate-50">
                        <div className="max-w-4xl mx-auto mt-10 p-8 bg-white rounded-lg shadow">
                            <h2 className="text-xl font-semibold text-red-600 mb-4">
                                Помилка завантаження профілю
                            </h2>
                            <p>{error}</p>
                            <button
                                onClick={() => window.location.reload()}
                                className="mt-4 px-4 py-2 bg-primary text-white rounded hover:bg-primary-dark"
                            >
                                Спробувати знову
                            </button>
                        </div>
                    </main>
                </div>
            </div>
        );
    }

    const role = formatRoleName(profile?.role || "");

    return (
        <div className="min-h-screen flex bg-[#FFFFFF] overflow-hidden">
            <div
                className={`flex-shrink-0 transition-all duration-300 h-screen ${
                    sidebarExpanded ? "w-[240px]" : "w-[72px]"
                }`}
            >
                <Sidebar onToggle={handleSidebarToggle} activePage="sessions" />
            </div>
            <main className="flex-1 p-4 md:p-8 bg-slate-50">
                {loading ? (
                    <ProfileSkeleton />
                ) : (
                    <div className="max-w-4xl mx-auto">
                        {" "}
                        <ProfileHeader
                            userName={profile?.userName || ""}
                            profileImageUrl={profile?.profileImageUrl || ""}
                            userRole={profile?.role}
                        />
                        <div className="mt-8 grid grid-cols-1 md:grid-cols-3 gap-6">
                            <div className="col-span-1">
                                <ProfileInfoCard
                                    title="Контактна інформація"
                                    items={[
                                        {
                                            label: "Ім'я користувача",
                                            value: profile?.userName || "-",
                                        },
                                        {
                                            label: "Email",
                                            value: profile?.email || "-",
                                        },
                                        {
                                            label: "Роль",
                                            value: role || "-",
                                        },
                                    ]}
                                />
                            </div>
                            <div className="col-span-1 md:col-span-2">
                                <ProfileOverview
                                    profile={profile}
                                    isLoading={loading}
                                />
                            </div>
                        </div>
                    </div>
                )}
            </main>
        </div>
    );
};

export default ProfilePage;
