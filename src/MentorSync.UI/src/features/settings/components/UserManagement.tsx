import React, { useState } from "react";
import { useUserManagement } from "../hooks/useUserManagement";
import { formatRoleName } from "../../../shared/utils/formatters";

const UserManagement: React.FC = () => {
    const {
        users,
        loading,
        searchTerm,
        setSearchTerm,
        toggleUserActive,
        filterByRole,
        filterByStatus,
        filters,
        clearFilters,
    } = useUserManagement();

    const [confirmationModalOpen, setConfirmationModalOpen] = useState(false);
    const [selectedUser, setSelectedUser] = useState<{
        id: number;
        name: string;
        isActive: boolean;
    } | null>(null);

    const handleToggleActiveClick = (user: {
        id: number;
        name: string;
        isActive: boolean;
    }) => {
        setSelectedUser(user);
        setConfirmationModalOpen(true);
    };

    const confirmToggleActive = async () => {
        if (selectedUser) {
            await toggleUserActive(selectedUser.id);
            setConfirmationModalOpen(false);
            setSelectedUser(null);
        }
    };

    const getRoleBadgeClasses = (role: string): string => {
        switch (role) {
            case "Admin":
                return "bg-purple-100 text-purple-800";
            case "Mentor":
                return "bg-blue-100 text-blue-800";
            case "Mentee":
                return "bg-green-100 text-green-800";
            default:
                return "bg-gray-100 text-gray-800";
        }
    };

    return (
        <div className="bg-white rounded-xl shadow-md overflow-hidden">
            <div className="p-6">
                <h2 className="text-xl font-medium text-[#1E293B] mb-6">
                    Керування користувачами
                </h2>

                {/* Search and filters */}
                <div className="mb-6 flex flex-wrap gap-4">
                    {/* Search input */}
                    <div className="flex-1 min-w-[250px]">
                        <div className="relative">
                            <span className="absolute inset-y-0 left-0 flex items-center pl-3">
                                <img
                                    src="/icons/search-icon.svg"
                                    alt="Search"
                                    className="w-5 h-5"
                                />
                            </span>
                            <input
                                type="text"
                                placeholder="Пошук за ім'ям або email"
                                value={searchTerm}
                                onChange={(e) => setSearchTerm(e.target.value)}
                                className="pl-10 pr-4 py-2 border border-[#E2E8F0] rounded-lg w-full focus:outline-none focus:border-[#4318D1]"
                            />
                        </div>
                    </div>

                    {/* Role filter */}
                    <div>
                        <select
                            value={filters.role || ""}
                            onChange={(e) =>
                                filterByRole(e.target.value || undefined)
                            }
                            className="px-4 py-2 border border-[#E2E8F0] rounded-lg focus:outline-none focus:border-[#4318D1] bg-white"
                        >
                            <option value="">Всі ролі</option>
                            <option value="Admin">Адміністратор</option>
                            <option value="Mentor">Ментор</option>
                            <option value="Mentee">Менті</option>
                        </select>
                    </div>

                    {/* Status filter */}
                    <div>
                        <select
                            value={
                                filters.isActive === undefined
                                    ? ""
                                    : filters.isActive
                                    ? "true"
                                    : "false"
                            }
                            onChange={(e) => {
                                const val = e.target.value;
                                filterByStatus(
                                    val === "" ? undefined : val === "true"
                                );
                            }}
                            className="px-4 py-2 border border-[#E2E8F0] rounded-lg focus:outline-none focus:border-[#4318D1] bg-white"
                        >
                            <option value="">Всі статуси</option>
                            <option value="true">Активні</option>
                            <option value="false">Заблоковані</option>
                        </select>
                    </div>

                    {/* Clear filters */}
                    <button
                        onClick={clearFilters}
                        className="px-4 py-2 text-[#4318D1] hover:bg-[#F8FAFC] rounded-lg"
                    >
                        Очистити фільтри
                    </button>
                </div>

                {/* Users table */}
                <div className="relative overflow-x-auto">
                    <table className="w-full text-left">
                        <thead className="bg-[#F8FAFC] text-[#64748B]">
                            <tr>
                                <th className="px-6 py-3 rounded-l-lg">
                                    Користувач
                                </th>
                                <th className="px-6 py-3">Email</th>
                                <th className="px-6 py-3">Роль</th>
                                <th className="px-6 py-3">Статус</th>
                                <th className="px-6 py-3">Дії</th>
                            </tr>
                        </thead>
                        <tbody className="divide-y divide-[#E2E8F0]">
                            {loading ? (
                                <tr>
                                    <td
                                        colSpan={5}
                                        className="px-6 py-12 text-center"
                                    >
                                        <span className="material-icons animate-spin mr-2">
                                            refresh
                                        </span>
                                        Завантаження...
                                    </td>
                                </tr>
                            ) : users.length === 0 ? (
                                <tr>
                                    <td
                                        colSpan={5}
                                        className="px-6 py-12 text-center text-[#64748B]"
                                    >
                                        Користувачів не знайдено
                                    </td>
                                </tr>
                            ) : (
                                users.map((user) => (
                                    <tr
                                        key={user.id}
                                        className="hover:bg-[#F8FAFC]"
                                    >
                                        <td className="px-6 py-4">
                                            <div className="flex items-center">
                                                <img
                                                    src={
                                                        user.avatarUrl ||
                                                        `https://ui-avatars.com/api/?name=${encodeURIComponent(
                                                            user.name
                                                        )}&background=F3F4F6&color=1E293B&size=40`
                                                    }
                                                    alt={user.name}
                                                    className="w-10 h-10 rounded-full mr-4 border border-[#E2E8F0]"
                                                />
                                                <span className="font-medium text-[#1E293B]">
                                                    {user.name}
                                                </span>
                                            </div>
                                        </td>
                                        <td className="px-6 py-4 text-[#64748B]">
                                            {user.email}
                                            {!user.isEmailConfirmed && (
                                                <span className="ml-2 text-xs bg-yellow-100 text-yellow-800 px-2 py-1 rounded">
                                                    Не підтверджено
                                                </span>
                                            )}
                                        </td>
                                        <td className="px-6 py-4">
                                            {" "}
                                            <span
                                                className={`px-3 py-1 rounded-full text-xs ${getRoleBadgeClasses(
                                                    user.role
                                                )}`}
                                            >
                                                {formatRoleName(user.role)}
                                            </span>
                                        </td>
                                        <td className="px-6 py-4">
                                            <span
                                                className={`px-3 py-1 rounded-full text-xs ${
                                                    user.isActive
                                                        ? "bg-green-100 text-green-800"
                                                        : "bg-red-100 text-red-800"
                                                }`}
                                            >
                                                {user.isActive
                                                    ? "Активний"
                                                    : "Заблоковано"}
                                            </span>
                                        </td>
                                        <td className="px-6 py-4">
                                            <button
                                                onClick={() =>
                                                    handleToggleActiveClick(
                                                        user
                                                    )
                                                }
                                                className={`px-3 py-1 rounded-lg text-sm ${
                                                    user.isActive
                                                        ? "bg-red-50 text-red-600 hover:bg-red-100"
                                                        : "bg-green-50 text-green-600 hover:bg-green-100"
                                                }`}
                                            >
                                                {user.isActive
                                                    ? "Заблокувати"
                                                    : "Активувати"}
                                            </button>
                                        </td>
                                    </tr>
                                ))
                            )}
                        </tbody>
                    </table>
                </div>
            </div>

            {/* Confirmation Modal */}
            {confirmationModalOpen && selectedUser && (
                <div className="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center z-50">
                    <div className="bg-white rounded-xl shadow-md p-6 max-w-md w-full">
                        <h3 className="text-xl font-medium text-[#1E293B] mb-4">
                            {selectedUser.isActive
                                ? "Заблокувати користувача"
                                : "Активувати користувача"}
                        </h3>
                        <p className="text-[#64748B] mb-6">
                            {selectedUser.isActive
                                ? `Ви впевнені, що хочете заблокувати користувача ${selectedUser.name}? 
                                   Користувач не зможе увійти в систему під час блокування.`
                                : `Ви впевнені, що хочете активувати користувача ${selectedUser.name}?
                                   Користувач зможе увійти в систему після активації.`}
                        </p>
                        <div className="flex justify-end gap-4">
                            <button
                                onClick={() => setConfirmationModalOpen(false)}
                                className="px-4 py-2 text-[#64748B] hover:bg-[#F8FAFC] rounded-lg"
                            >
                                Скасувати
                            </button>
                            <button
                                onClick={confirmToggleActive}
                                className={`px-4 py-2 text-white rounded-lg ${
                                    selectedUser.isActive
                                        ? "bg-red-600 hover:bg-red-700"
                                        : "bg-green-600 hover:bg-green-700"
                                }`}
                            >
                                {selectedUser.isActive
                                    ? "Заблокувати"
                                    : "Активувати"}
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default UserManagement;
