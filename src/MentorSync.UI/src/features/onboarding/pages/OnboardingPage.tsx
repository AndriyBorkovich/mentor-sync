import React, { useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useAuth } from "../../auth/context/AuthContext";
import { OnboardingProvider } from "../context/OnboardingContext";
import OnboardingStepper from "../components/OnboardingStepper";
import { OnboardingContent } from "../components/OnboardingContent";

const OnboardingPage: React.FC = () => {
    const navigate = useNavigate();
    const { role } = useParams<{ role: string }>();
    const { isAuthenticated, isLoading } = useAuth();

    // Redirect to login if not authenticated
    useEffect(() => {
        if (!isLoading && !isAuthenticated) {
            navigate("/login");
        }

        // Validate role parameter
        if (role !== "mentor" && role !== "mentee") {
            navigate("/dashboard");
        }
    }, [isAuthenticated, isLoading, navigate, role]);

    if (isLoading || !role || (role !== "mentor" && role !== "mentee")) {
        return (
            <div className="flex items-center justify-center min-h-screen">
                <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-primary"></div>
            </div>
        );
    }

    const userRole = role as "mentor" | "mentee";

    return (
        <OnboardingProvider initialRole={userRole}>
            <div className="min-h-screen bg-[#F8FAFC] flex flex-col">
                {/* Header */}
                <header className="bg-white py-4 px-6 shadow-sm">
                    <div className="max-w-7xl mx-auto flex items-center">
                        <div className="flex items-center gap-2">
                            <h1 className="text-2xl font-bold text-[#1E293B]">
                                MentorSync
                            </h1>
                        </div>
                    </div>
                </header>

                {/* Main Content */}
                <main className="flex-1 py-8">
                    <div className="max-w-3xl mx-auto px-4">
                        <OnboardingStepper />

                        <div className="mt-8 bg-white p-6 rounded-2xl shadow-sm">
                            <OnboardingContent userRole={userRole} />
                        </div>
                    </div>
                </main>

                {/* Footer */}
                <footer className="bg-white py-4 px-6 border-t border-[#E2E8F0]">
                    <div className="max-w-7xl mx-auto text-center text-[#64748B] text-sm">
                        © {new Date().getFullYear()} MentorSync. Всі права
                        захищені.
                    </div>
                </footer>
            </div>
        </OnboardingProvider>
    );
};

export default OnboardingPage;
