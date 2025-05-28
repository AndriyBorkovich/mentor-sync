import React from "react";

interface MaterialAnalyticsProps {
    viewCount: number;
    ratingCount?: number;
    averageRating?: number;
    commentCount?: number;
}

const MaterialAnalytics: React.FC<MaterialAnalyticsProps> = ({
    ratingCount = 0,
    averageRating = 0,
    commentCount = 0,
}) => {
    const analyticsItems = [
        {
            label: "оцінок",
            value: ratingCount,
            icon: "star",
            color: "text-yellow-500",
        },
        {
            label: "середня оцінка",
            value: averageRating.toFixed(1),
            icon: "grade",
            color: "text-orange-500",
        },
        {
            label: "коментарів",
            value: commentCount,
            icon: "comment",
            color: "text-purple-500",
        },
    ];

    return (
        <div className="bg-white rounded-lg shadow-sm p-6 mb-6">
            <h3 className="text-lg font-medium text-[#1E293B] mb-4">
                Статистика матеріалу
            </h3>

            <div className="grid grid-cols-1 sm:grid-cols-3 md:grid-cols-5 gap-4">
                {analyticsItems.map((item, index) => (
                    <div key={index} className="flex items-center">
                        <span className={`material-icons ${item.color} mr-2`}>
                            {item.icon}
                        </span>
                        <div>
                            <p className="text-lg font-medium">{item.value}</p>
                            <p className="text-xs text-[#64748B]">
                                {item.label}
                            </p>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default MaterialAnalytics;
