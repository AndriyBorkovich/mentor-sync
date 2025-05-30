using Ardalis.Result;
using MediatR;

namespace MentorSync.Scheduling.Features.Booking;

public sealed record GetMentorBookingsQuery(
    int MentorId,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate) : IRequest<Result<MentorBookingsResult>>;

public sealed record MentorBookingsResult(
    int MentorId,
    List<BookingDto> Bookings);

public sealed record BookingDto(
    int Id,
    int MentorId,
    int MenteeId,
    string MenteeName,
    string MenteeImage,
    DateTimeOffset Start,
    DateTimeOffset End,
    string Title,
    string Description,
    string Status);
