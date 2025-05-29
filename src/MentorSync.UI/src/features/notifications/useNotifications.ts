import { useEffect, useRef } from "react";
import {
    HubConnectionBuilder,
    HubConnection,
    LogLevel,
} from "@microsoft/signalr";
import { toast } from "react-toastify";

const SIGNALR_URL = `${import.meta.env.VITE_API_URL}/notificationHub`;

export function useNotifications(onBookingStatusChanged?: (data: any) => void) {
    const connectionRef = useRef<HubConnection | null>(null);

    useEffect(() => {
        const connection = new HubConnectionBuilder()
            .withUrl(SIGNALR_URL)
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Information)
            .build();

        connection.on("BookingStatusChanged", (json: string) => {
            let data;
            try {
                data = JSON.parse(json);
            } catch {
                data = json;
            }
            toast.info(
                data?.Title
                    ? `${data.Title}: ${
                          data.Message || "Booking status updated"
                      }`
                    : "Booking status updated"
            );
            if (onBookingStatusChanged) onBookingStatusChanged(data);
        });

        connection.start().catch((err) => {
            // eslint-disable-next-line no-console
            console.error("SignalR connection error:", err);
        });

        connectionRef.current = connection;
        return () => {
            connection.stop();
        };
    }, [onBookingStatusChanged]);

    return connectionRef;
}
