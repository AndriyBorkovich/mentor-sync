namespace MentorSync.Scheduling.Features.Booking.Create;

/// <summary>
/// Command to create a booking
/// </summary>
/// <param name="MenteeId">The ID of the mentee</param>
/// <param name="MentorId">The ID of the mentor</param>
/// <param name="AvailabilitySlotId">The ID of the availability slot</param>
/// <param name="Start">The start time of the booking</param>
/// <param name="End">The end time of the booking</param>
public sealed record CreateBookingCommand(
	int MenteeId,
	int MentorId,
	int AvailabilitySlotId,
	DateTimeOffset Start,
	DateTimeOffset End) : ICommand<CreateBookingResult>;
