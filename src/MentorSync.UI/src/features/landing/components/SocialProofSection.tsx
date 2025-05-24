import React from "react";
import Avatar from "../../../components/ui/Avatar";

const SocialProofSection: React.FC = () => {
    return (
        <div className="py-12">
            <div className="flex items-center">
                <div className="flex -space-x-2">
                    <Avatar size="md" className="z-40" />
                    <Avatar size="md" className="z-30" />
                    <Avatar size="md" className="z-20" />
                    <Avatar size="md" className="z-10" />
                </div>
                <div className="ml-6">
                    <p className="text-secondary font-medium">
                        Приєднуйтесь до 2 000+ членів
                    </p>
                    <p className="text-textGray">
                        Навчайтесь та зростайте разом
                    </p>
                </div>
            </div>
        </div>
    );
};

export default SocialProofSection;
