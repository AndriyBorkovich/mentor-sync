import React from "react";
import { Link } from "react-router-dom";

const Navbar: React.FC = () => {
    return (
        <nav className="flex justify-between items-center px-16 py-8">
            <div className="font-bold text-2xl text-secondary">MentorSync</div>
            <div className="flex items-center space-x-6">
                <Link to="/login" className="text-textGray">
                    Увійти
                </Link>
                <Link
                    to="/register"
                    className="bg-primary text-white py-2 px-5 rounded-lg"
                >
                    Зареєструватися
                </Link>
            </div>
        </nav>
    );
};

export default Navbar;
