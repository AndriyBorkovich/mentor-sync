using Ardalis.Result;
using MentorSync.Materials.Data;
using MentorSync.Users.Contracts.Services;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Materials.Features.GetMaterials;

/// <summary>
/// Handler for <see cref="GetMaterialsQuery"/>
/// </summary>
/// <param name="dbContext">Database context</param>
/// <param name="mentorProfileService">Service for mentor profile</param>
public sealed class GetMaterialsQueryHandler(
	MaterialsDbContext dbContext,
	IMentorProfileService mentorProfileService)
		: IQueryHandler<GetMaterialsQuery, MaterialsResponse>
{
	/// <inheritdoc />
	public async Task<Result<MaterialsResponse>> Handle(GetMaterialsQuery request, CancellationToken cancellationToken = default)
	{
		var query = dbContext.LearningMaterials.AsNoTracking();

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var searchPattern = $"%{request.Search}%";
			query = query.Where(m =>
				EF.Functions.ILike(m.Title, searchPattern) ||
				EF.Functions.ILike(m.Description, searchPattern) ||
				m.Tags.Any(t => EF.Functions.ILike(t.Name, searchPattern)));
		}

		if (request.Types?.Count > 0)
		{
			var materialTypes = request.Types
				.Select(t => Enum.TryParse<MaterialType>(t, true, out var type) ? (MaterialType?)type : null)
				.Where(t => t.HasValue)
				.Select(t => t!.Value)
				.ToList();

			if (materialTypes.Count != 0)
			{
				query = query.Where(m => materialTypes.Contains(m.Type));
			}
		}

		if (request.Tags?.Count > 0)
		{
			var lowercaseTags = request.Tags.Select(t => t.ToLower()).ToList();
			query = query.Where(m => m.Tags.Any(t => lowercaseTags.Contains(t.Name.ToLower())));
		}

		var totalCount = await query.CountAsync(cancellationToken);

		query = request.SortBy?.ToUpperInvariant() switch
		{
			"OLDEST" => query.OrderBy(m => m.CreatedAt),
			"TITLE" => query.OrderBy(m => m.Title),
			"TITLE_DESC" => query.OrderByDescending(m => m.Title),
			_ => query.OrderByDescending(m => m.CreatedAt) // Default is newest first
		};

		query = query.Skip((request.PageNumber - 1) * request.PageSize)
					  .Take(request.PageSize);

		var materials = await query
			.Include(m => m.Attachments)
			.Include(m => m.Tags)
			.ToListAsync(cancellationToken);

		var mentorIds = materials.Select(m => m.MentorId).Distinct().ToList();

		var mentors = await mentorProfileService.GetAllMentorsAsync([.. mentorIds]);
		var mentorNames = mentors.ToDictionary(m => m.Id, m => m.UserName);

		// TODO: rewrite to raw sql query for performance
		var materialDtos = materials.ConvertAll(m => new MaterialDto
		{
			Id = m.Id,
			Title = m.Title,
			Description = m.Description,
			Type = m.Type.ToString(),
			ContentMarkdown = m.ContentMarkdown,
			MentorId = m.MentorId,
			MentorName = mentorNames.TryGetValue(m.MentorId, out var name) ? name : "Unknown Mentor",
			CreatedAt = m.CreatedAt,
			UpdatedAt = m.UpdatedAt,
			Attachments = m.Attachments?.Select(a => new AttachmentDto
			{
				Id = a.Id,
				FileName = a.FileName,
				FileUrl = a.BlobUri,
				ContentType = a.ContentType,
				UploadedAt = a.UploadedAt
			}).ToList() ?? [],
			Tags = m.Tags?.Select(t => new TagDto
			{
				Id = t.Id,
				Name = t.Name
			}).ToList() ?? []
		});

		var response = new MaterialsResponse
		{
			Items = materialDtos,
			TotalCount = totalCount,
			PageSize = request.PageSize,
			PageNumber = request.PageNumber
		};

		return Result.Success(response);
	}
}
