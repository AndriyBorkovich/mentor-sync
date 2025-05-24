import React from "react";
import Navbar from "../../../components/layout/Navbar";
import Section from "../../../components/layout/Section";
import Footer from "../../../components/layout/Footer";
import HeroSection from "../components/HeroSection";
import SocialProofSection from "../components/SocialProofSection";
import FeaturesSection from "../components/FeaturesSection";
import TestimonialsSection from "../components/TestimonialsSection";
import CallToAction from "../components/CallToAction";

const LandingPage: React.FC = () => {
    return (
        <div className="bg-background min-h-screen flex flex-col">
            <Navbar />
            <main>
                <Section spacing="xl">
                    <HeroSection />
                </Section>
                <Section className="bg-white" spacing="md">
                    <SocialProofSection />
                </Section>
                <Section className="bg-white" spacing="xl">
                    <FeaturesSection />
                </Section>
                <Section className="bg-background-darker" spacing="xl">
                    <TestimonialsSection />
                </Section>
                <Section className="bg-background" spacing="lg">
                    <CallToAction />
                </Section>
            </main>
            <Footer />
        </div>
    );
};

export default LandingPage;
