import { RouterProvider } from "react-router-dom";
import router from "./routes";
import "./App.css";
import { AuthProvider } from "./features/auth/context/AuthContext";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { useNotifications } from "./features/notifications/useNotifications";

function App() {
    useNotifications();
    return (
        <AuthProvider>
            <RouterProvider router={router} />
            <ToastContainer
                position="top-right"
                autoClose={3000}
                hideProgressBar={false}
                newestOnTop={true}
                closeOnClick
                rtl={false}
                pauseOnFocusLoss
                draggable
                pauseOnHover
                theme="light"
            />
        </AuthProvider>
    );
}

export default App;
