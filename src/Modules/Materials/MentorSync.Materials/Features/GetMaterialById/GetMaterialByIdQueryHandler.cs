using Ardalis.Result;
using MentorSync.Materials.Data;
using MentorSync.Users.Contracts.Services;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Materials.Features.GetMaterialById;

public sealed class GetMaterialByIdQueryHandler(
	MaterialsDbContext dbContext,
	IMentorProfileService mentorProfileService)
		: IQueryHandler<GetMaterialByIdQuery, MaterialResponse>
{
	public async Task<Result<MaterialResponse>> Handle(GetMaterialByIdQuery request, CancellationToken cancellationToken = default)
	{
		var material = await dbContext.LearningMaterials
				.AsNoTracking()
				.AsSplitQuery()
				.Include(m => m.Attachments)
				.Include(m => m.Tags)
				.FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

		if (material == null)
		{
			return Result.NotFound($"Learning material with ID {request.Id} not found");
		}

		// TODO: rewrite this to raw sql query with joins or define a view
		var mentors = await mentorProfileService.GetAllMentorsAsync();
		var mentorName = mentors.FirstOrDefault(m => m.Id == material.MentorId)?.UserName ?? "Unknown Mentor";

		var response = new MaterialResponse
		{
			Id = material.Id,
			Title = material.Title,
			Description = material.Description,
			Type = material.Type.ToString(),
			ContentMarkdown = material.ContentMarkdown,
			MentorId = material.MentorId,
			MentorName = mentorName,
			CreatedAt = material.CreatedAt,
			UpdatedAt = material.UpdatedAt,
			Attachments = [.. material.Attachments.Select(a => new AttachmentInfo
				{
					Id = a.Id,
					FileName = a.FileName,
					FileUrl = a.BlobUri,
					ContentType = a.ContentType,
					FileSize = a.Size,
					UploadedAt = a.UploadedAt
				})],
			Tags = [.. material.Tags.Select(t => new TagInfo
				{
					Id = t.Id,
					Name = t.Name
				})]
		};

		return Result.Success(response);
	}
}
