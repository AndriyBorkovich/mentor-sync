using Ardalis.Result;
using MediatR;

namespace MentorSync.Scheduling.Features.MentorAvailability.Delete;

public record DeleteMentorAvailabilityCommand(int MentorId, int AvailabilityId) : IRequest<Result<Unit>>;