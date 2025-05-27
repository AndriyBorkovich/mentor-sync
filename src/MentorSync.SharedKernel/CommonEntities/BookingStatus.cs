namespace MentorSync.SharedKernel.CommonEntities;

public enum BookingStatus
{
    Pending,
    Confirmed,
    Cancelled,
    Completed,
    NoShow // mentor or mentee did not show up for the meeting
}
