/** @type {import('tailwindcss').Config} */
export default {
    content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
    theme: {
        extend: {
            colors: {
                primary: "#4318D1", // Purple color from the design
                secondary: "#1E293B", // Dark text color
                textGray: "#64748B", // Gray text color
                background: "#F8FAFC", // Light background color
            },
            fontFamily: {
                sans: ["Inter", "sans-serif"],
            },
        },
    },
    plugins: [],
};
