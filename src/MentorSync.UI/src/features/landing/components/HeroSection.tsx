import React from "react";
import Button from "../../../components/ui/Button";
import PlaceholderImage from "../../../components/ui/PlaceholderImage";

const HeroSection: React.FC = () => {
    return (
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
            <div className="py-16">
                <h1 className="text-5xl font-bold text-secondary leading-tight mb-8">
                    Прискоріть свою кар'єру за допомогою наставництва
                </h1>
                <p className="text-xl text-textGray mb-10">
                    Зв'яжіться з досвідченими технічними професіоналами або
                    поділіться своєю досвідом. Приєднуйтесь до нашої спільноти
                    учнів та лідерів.
                </p>

                <div className="flex flex-col sm:flex-row gap-4">
                    <Button variant="primary">Приєднуйтесь як менті</Button>
                    <Button variant="outline">Приєднуйтесь як ментор</Button>
                </div>
            </div>{" "}
            <div className="hidden lg:flex items-center justify-center">
                {/* Placeholder for hero image */}
                <PlaceholderImage
                    height="400px"
                    text="Hero Image - Professional mentorship illustration"
                    backgroundColor="#f0f4f8"
                    className="rounded-lg shadow-lg"
                />
            </div>
        </div>
    );
};

export default HeroSection;
