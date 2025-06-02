namespace MentorSync.Scheduling.Features.GetUserBookings.Common;

public sealed class UserBookingsResponse
{
    public List<BookingDto> Bookings { get; set; } = [];
}

public sealed class BookingDto
{
    public int Id { get; set; }
    public int MentorId { get; set; }
    public string MentorName { get; set; } = string.Empty;
    public string MentorImage { get; set; }
    public int MenteeId { get; set; }
    public string MenteeName { get; set; } = string.Empty;
    public string MenteeImage { get; set; }
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
