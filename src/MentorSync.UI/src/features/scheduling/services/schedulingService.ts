import api from "../../auth/services/api";

export interface MentorAvailabilitySlot {
    id: number;
    mentorId: number;
    start: string; // ISO date string
    end: string; // ISO date string
}

export interface MentorAvailabilityResponse {
    slots: MentorAvailabilitySlot[];
}

export interface BookingSession {
    id: number;
    mentorId: number;
    mentorName?: string;
    mentorImage?: string;
    menteeId: number;
    menteeName?: string;
    menteeImage?: string;
    start: string; // ISO date string
    end: string; // ISO date string
    status: string;
    createdAt?: string;
    updatedAt?: string;
}

export interface BookingRequest {
    mentorId: number;
    availabilitySlotId: number;
    start: string; // ISO date string
    end: string; // ISO date string
}

export interface CreateAvailabilityRequest {
    start: string; // ISO date string
    end: string; // ISO date string
}

/**
 * Fetches a mentor's availability slots within a specified date range
 */
export const getMentorAvailability = async (
    mentorId: number,
    startDate: Date,
    endDate: Date
): Promise<MentorAvailabilityResponse> => {
    const start = startDate.toISOString().split("T")[0]; // YYYY-MM-DD
    const end = endDate.toISOString().split("T")[0];

    const response = await api.get<MentorAvailabilityResponse>(
        `scheduling/mentors/${mentorId}/availability?startDate=${start}&endDate=${end}`
    );
    return response.data;
};

/**
 * Creates a booking for a mentoring session
 */
export const createBooking = async (
    bookingRequest: BookingRequest
): Promise<BookingSession> => {
    const response = await api.post<BookingSession>(
        `scheduling/bookings`,
        bookingRequest
    );
    return response.data;
};

/**
 * Adds a new availability slot for a mentor
 * Only mentors can add their own availability
 */
export const createMentorAvailability = async (
    mentorId: number,
    availabilityRequest: CreateAvailabilityRequest
): Promise<MentorAvailabilitySlot> => {
    const response = await api.post<MentorAvailabilitySlot>(
        `scheduling/mentors/${mentorId}/availability`,
        availabilityRequest
    );
    return response.data;
};

/**
 * Gets bookings for a mentee
 */
export const getMenteeBookings = async (): Promise<BookingSession[]> => {
    const response = await api.get<{ bookings: BookingSession[] }>(
        `scheduling/mentee/bookings`
    );
    return response.data.bookings;
};

/**
 * Gets bookings for a mentor
 */
export const getMentorBookings = async (): Promise<BookingSession[]> => {
    const response = await api.get<{ bookings: BookingSession[] }>(
        `scheduling/mentor/bookings`
    );
    return response.data.bookings;
};

/**
 * Cancel a booking
 */
export const cancelBooking = async (bookingId: number): Promise<void> => {
    await api.post(`scheduling/bookings/${bookingId}/cancel`);
};

/**
 * Confirm a booking (mentors only)
 */
export const confirmBooking = async (bookingId: number): Promise<void> => {
    await api.post(`scheduling/bookings/${bookingId}/confirm`);
};
