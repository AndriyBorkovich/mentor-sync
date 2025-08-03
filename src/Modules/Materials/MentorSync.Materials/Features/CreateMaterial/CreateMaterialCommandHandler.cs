using Ardalis.Result;
using MentorSync.Materials.Data;
using MentorSync.Materials.Domain;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Materials.Features.CreateMaterial;

public sealed class CreateMaterialCommandHandler(
	MaterialsDbContext dbContext)
		: ICommandHandler<CreateMaterialCommand, CreateMaterialResponse>
{
	public async Task<Result<CreateMaterialResponse>> Handle(CreateMaterialCommand request, CancellationToken cancellationToken = default)
	{
		var material = new LearningMaterial
		{
			Title = request.Title,
			Description = request.Description,
			Type = request.Type,
			ContentMarkdown = request.ContentMarkdown,
			MentorId = request.MentorId,
			CreatedAt = DateTime.UtcNow,
			Attachments = [],
			Tags = []
		};

		dbContext.LearningMaterials.Add(material);

		if (request.Tags?.Count > 0)
		{
			foreach (var tagName in request.Tags.Distinct(StringComparer.OrdinalIgnoreCase))
			{
				var existingTag = await dbContext.Tags
					.FirstOrDefaultAsync(t => EF.Functions.ILike(t.Name, $"%{tagName}%"), cancellationToken);

				if (existingTag != null)
				{
					material.Tags.Add(existingTag);
				}
				else
				{
					material.Tags.Add(new Tag
					{
						Name = tagName,
					});
				}
			}
		}

		await dbContext.SaveChangesAsync(cancellationToken);

		var response = new CreateMaterialResponse
		{
			Id = material.Id,
			Title = material.Title,
			CreatedAt = material.CreatedAt
		};

		return Result.Success(response);
	}
}
