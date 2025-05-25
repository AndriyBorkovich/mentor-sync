import React from "react";
import { Mentor } from "../../../dashboard/data/mentors";

interface AboutTabProps {
    mentor: Mentor;
}

const AboutTab: React.FC<AboutTabProps> = ({ mentor }) => {
    // Mock languages data since it's not in the mentor model
    const languages = [
        { id: "1", flag: "🇺🇦", name: "Українська" },
        { id: "2", flag: "🇺🇸", name: "English" },
    ];

    return (
        <div className="flex">
            <div className="w-2/3 pr-6">
                <h2 className="text-lg font-medium text-[#1E293B] mb-3">
                    Інформація
                </h2>

                <p className="text-[#64748B] mb-6">
                    Старший архітектор програмного забезпечення з{" "}
                    {mentor.yearsOfExperience}-річним досвідом розподілених
                    систем та хмарної архітектури. Пристрасно допомагаю іншим
                    зростати у своїй технічній кар'єрі. Спеціалізуюсь на
                    проектуванні системи, архітектурі мікросервісів та хмарних
                    програмах.
                </p>

                <h2 className="text-lg font-medium text-[#1E293B] mb-3 mt-6">
                    Навички
                </h2>
                <div className="flex flex-wrap gap-2">
                    {mentor.skills.map((skill) => (
                        <div
                            key={skill.id}
                            className="px-3 py-1 bg-[#F8FAFC] rounded-2xl"
                        >
                            <span className="text-xs text-[#1E293B]">
                                {skill.name}
                            </span>
                        </div>
                    ))}
                    <div className="px-3 py-1 bg-[#F8FAFC] rounded-2xl">
                        <span className="text-xs text-[#1E293B]">Docker</span>
                    </div>
                    <div className="px-3 py-1 bg-[#F8FAFC] rounded-2xl">
                        <span className="text-xs text-[#1E293B]">
                            Kubernetes
                        </span>
                    </div>
                    <div className="px-3 py-1 bg-[#F8FAFC] rounded-2xl">
                        <span className="text-xs text-[#1E293B]">AWS</span>
                    </div>
                </div>
            </div>

            <div className="w-1/3 bg-white rounded-xl shadow-sm p-6">
                <h2 className="text-lg font-medium text-[#1E293B] mb-3">
                    Мови
                </h2>
                <div className="flex flex-wrap gap-2">
                    {languages.map((lang) => (
                        <div
                            key={lang.id}
                            className="px-3 py-1 bg-[#F8FAFC] rounded-2xl"
                        >
                            <span className="text-xs text-[#1E293B]">
                                {lang.flag} {lang.name}
                            </span>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
};

export default AboutTab;
