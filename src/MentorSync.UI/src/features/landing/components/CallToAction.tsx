import React from "react";
import Button from "../../../components/ui/Button";

const CallToAction: React.FC = () => {
    return (
        <div className="bg-primary rounded-2xl py-16 px-8 text-center text-white">
            <h2 className="text-3xl font-bold mb-4">
                Готові розпочати свою ментор-подорож?
            </h2>
            <p className="max-w-2xl mx-auto mb-10 text-gray-100">
                Приєднуйтесь до MentorSync сьогодні та отримайте доступ до
                мережі досвідчених професіоналів, які готові поділитися своїми
                знаннями або станьте ментором самі та допомагайте іншим.
            </p>

            <div className="flex flex-col sm:flex-row justify-center gap-4">
                <Button className="bg-white text-primary hover:bg-gray-100">
                    Приєднатись як менті
                </Button>
                <Button
                    variant="outline"
                    className="border-white text-white hover:bg-white/10"
                >
                    Стати ментором
                </Button>
            </div>
        </div>
    );
};

export default CallToAction;
