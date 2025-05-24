import React from "react";
import Navbar from "../../../components/layout/Navbar";
import Section from "../../../components/layout/Section";
import Footer from "../../../components/layout/Footer";
import HeroSection from "../components/HeroSection";
import SocialProofSection from "../components/SocialProofSection";
import FeaturesSection from "../components/FeaturesSection";
import CallToAction from "../components/CallToAction";

const LandingPage: React.FC = () => {
    return (
        <div className="bg-background min-h-screen flex flex-col">
            <Navbar />
            <Section>
                <HeroSection />
            </Section>
            <Section className="bg-white">
                <SocialProofSection />
                <FeaturesSection />
            </Section>
            <Section className="bg-background">
                <CallToAction />
            </Section>
            <Footer />
        </div>
    );
};

export default LandingPage;
