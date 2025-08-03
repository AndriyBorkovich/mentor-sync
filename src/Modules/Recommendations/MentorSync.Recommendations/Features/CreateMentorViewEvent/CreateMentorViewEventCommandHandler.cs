using Ardalis.Result;
using MentorSync.Recommendations.Data;
using MentorSync.Recommendations.Domain.Tracking;
using Microsoft.Extensions.Logging;

namespace MentorSync.Recommendations.Features.CreateMentorViewEvent;

public sealed class CreateMentorViewEventCommandHandler(
	RecommendationsDbContext dbContext,
	ILogger<CreateMentorViewEventCommandHandler> logger)
		: ICommandHandler<CreateMentorViewEventCommand, string>
{
	public async Task<Result<string>> Handle(
		CreateMentorViewEventCommand request, CancellationToken cancellationToken = default)
	{
		var viewEvent = new MentorViewEvent
		{
			MenteeId = request.MenteeId,
			MentorId = request.MentorId,
			ViewedAt = DateTime.UtcNow,
		};

		dbContext.MentorViewEvents.Add(viewEvent);
		await dbContext.SaveChangesAsync(cancellationToken);

		logger.LogInformation("Created mentor view event for Mentee {MenteeId} and Mentor {MentorId}", request.MenteeId, request.MentorId);

		return Result.Success("Mentor view event created successfully");
	}
}
