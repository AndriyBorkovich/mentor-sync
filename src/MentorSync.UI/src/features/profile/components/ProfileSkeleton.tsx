import React from "react";

export const ProfileSkeleton: React.FC = () => {
    return (
        <div className="max-w-4xl mx-auto space-y-8">
            {/* Profile Header Skeleton */}
            <div className="bg-white rounded-lg shadow p-6">
                <div className="flex flex-col md:flex-row items-center">
                    <div className="w-24 h-24 rounded-full bg-gray-200 animate-pulse"></div>
                    <div className="md:ml-6 mt-4 md:mt-0 text-center md:text-left flex-1">
                        <div className="h-8 bg-gray-200 rounded w-48 animate-pulse"></div>
                        <div className="mt-2 h-6 bg-gray-200 rounded w-24 animate-pulse"></div>
                    </div>
                    <div className="flex-1 flex justify-end mt-4 md:mt-0">
                        <div className="h-10 bg-gray-200 rounded-lg w-32 animate-pulse"></div>
                    </div>
                </div>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                {/* Contact Info Card Skeleton */}
                <div className="col-span-1">
                    <div className="bg-white rounded-lg shadow p-6">
                        <div className="h-7 bg-gray-200 rounded w-3/4 mb-4 animate-pulse"></div>
                        <div className="space-y-4">
                            <div>
                                <div className="h-4 bg-gray-200 rounded w-1/2 animate-pulse mb-1"></div>
                                <div className="h-5 bg-gray-200 rounded w-3/4 animate-pulse"></div>
                            </div>
                            <div>
                                <div className="h-4 bg-gray-200 rounded w-1/2 animate-pulse mb-1"></div>
                                <div className="h-5 bg-gray-200 rounded w-3/4 animate-pulse"></div>
                            </div>
                            <div>
                                <div className="h-4 bg-gray-200 rounded w-1/2 animate-pulse mb-1"></div>
                                <div className="h-5 bg-gray-200 rounded w-3/4 animate-pulse"></div>
                            </div>
                        </div>
                    </div>
                </div>

                {/* Profile Overview Skeleton */}
                <div className="col-span-1 md:col-span-2">
                    <div className="bg-white rounded-lg shadow p-6">
                        <div className="h-7 bg-gray-200 rounded w-1/3 mb-6 animate-pulse"></div>

                        <div className="space-y-6">
                            <div>
                                <div className="h-5 bg-gray-200 rounded w-1/4 mb-2 animate-pulse"></div>
                                <div className="h-4 bg-gray-200 rounded w-full animate-pulse"></div>
                                <div className="h-4 bg-gray-200 rounded w-full mt-1 animate-pulse"></div>
                                <div className="h-4 bg-gray-200 rounded w-3/4 mt-1 animate-pulse"></div>
                            </div>

                            <div>
                                <div className="h-5 bg-gray-200 rounded w-1/4 mb-2 animate-pulse"></div>
                                <div className="h-4 bg-gray-200 rounded w-1/2 animate-pulse"></div>
                            </div>

                            <div>
                                <div className="h-5 bg-gray-200 rounded w-1/4 mb-2 animate-pulse"></div>
                                <div className="flex flex-wrap gap-2">
                                    {[...Array(4)].map((_, i) => (
                                        <div
                                            key={i}
                                            className="h-8 bg-gray-200 rounded w-20 animate-pulse"
                                        ></div>
                                    ))}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};
