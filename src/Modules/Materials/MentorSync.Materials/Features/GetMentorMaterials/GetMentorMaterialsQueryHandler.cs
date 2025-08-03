using Ardalis.Result;
using MentorSync.Materials.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Materials.Features.GetMentorMaterials;

public sealed class GetMentorMaterialsQueryHandler(
	MaterialsDbContext dbContext)
		: IQueryHandler<GetMentorMaterialsQuery, MentorMaterialsResponse>
{
	public async Task<Result<MentorMaterialsResponse>> Handle(GetMentorMaterialsQuery request, CancellationToken cancellationToken)
	{
		var materials = await dbContext.LearningMaterials
				.Where(lm => lm.MentorId == request.MentorId)
				.OrderByDescending(lm => lm.CreatedAt)
				.Select(lm => new MaterialInfo
				{
					Id = lm.Id,
					Title = lm.Title,
					Description = lm.Description,
					Type = lm.Type.ToString(),
					ContentMarkdown = lm.ContentMarkdown,
					Url = $"/materials/{lm.Id}",
					CreatedOn = lm.CreatedAt,
					UpdatedOn = lm.UpdatedAt,
				})
				.ToListAsync(cancellationToken);

		var materialIds = materials.ConvertAll(m => m.Id);

		var materialTagsDict = await dbContext.Tags
			.Where(t => t.LearningMaterials.Any(lm => lm.MentorId == request.MentorId))
			.Join(
				dbContext.LearningMaterials,
				t => t.Id,
				lm => lm.Id,
				(t, lm) => new { MaterialId = lm.Id, TagName = t.Name }
			)
			.GroupBy(x => x.MaterialId)
			.ToDictionaryAsync(
				g => g.Key,
				g => g.Select(t => t.TagName).ToList(),
				cancellationToken
			);

		var materialAttachmentsDict = await dbContext.MaterialAttachments
			.Where(a => materialIds.Contains(a.MaterialId))
			.Select(a => new
			{
				a.Id,
				a.MaterialId,
				a.FileName,
				FileUrl = a.BlobUri
			})
			.GroupBy(a => a.MaterialId)
			.ToDictionaryAsync(
				g => g.Key,
				g => g.Select(a => new MaterialAttachmentInfo
				{
					Id = a.Id,
					FileName = a.FileName,
					FileUrl = a.FileUrl
				}).ToList(),
				cancellationToken
			);

		foreach (var material in materials)
		{
			if (materialTagsDict.TryGetValue(material.Id, out var tags))
			{
				material.Tags.AddRange(tags);
			}

			if (materialAttachmentsDict.TryGetValue(material.Id, out var attachments))
			{
				material.Attachments.AddRange(attachments);
			}
		}

		var response = new MentorMaterialsResponse
		{
			Materials = materials
		};

		return Result.Success(response);
	}
}
