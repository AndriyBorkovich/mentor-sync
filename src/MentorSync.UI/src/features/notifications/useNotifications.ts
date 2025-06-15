import { useEffect, useRef } from "react";
import {
    HubConnectionBuilder,
    HubConnection,
    LogLevel,
} from "@microsoft/signalr";
import { toast } from "react-toastify";
import { getAuthTokens } from "../auth/services/authStorage";

const SIGNALR_URL = `${import.meta.env.VITE_API_URL}/notificationHub`;

export function useNotifications(onBookingStatusChanged?: (data: any) => void) {
    const connectionRef = useRef<HubConnection | null>(null);
    useEffect(() => {
        // Get token for connection
        const authTokens = getAuthTokens();
        const token = authTokens?.token || "";

        // Add token in both ways: query string and accessTokenFactory
        const connection = new HubConnectionBuilder()
            .withUrl(`${SIGNALR_URL}`, {
                accessTokenFactory: () => token,
            })
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Warning)
            .build();

        // Booking status notifications
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

        // Basic chat notifications
        connection.on("ReceiveChatMessage", (messageDto) => {
            if (messageDto?.content) {
                toast.info(
                    `New message: ${messageDto.content.substring(0, 50)}${
                        messageDto.content.length > 50 ? "..." : ""
                    }`
                );
            }
        });

        connection.start().catch((err: unknown) => {
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
