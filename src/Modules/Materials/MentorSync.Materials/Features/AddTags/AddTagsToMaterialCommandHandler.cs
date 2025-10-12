using Ardalis.Result;
using MentorSync.Materials.Data;
using MentorSync.Materials.Domain;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Materials.Features.AddTags;

/// <summary>
/// Command handler for adding tags to a learning material
/// </summary>
/// <param name="dbContext">Database context</param>
public sealed class AddTagsToMaterialCommandHandler(
	MaterialsDbContext dbContext)
		: ICommandHandler<AddTagsToMaterialCommand, AddTagsResponse>
{
	/// <inheritdoc />
	public async Task<Result<AddTagsResponse>> Handle(AddTagsToMaterialCommand request, CancellationToken cancellationToken = default)
	{
		// Validate that material exists and belongs to mentor
		var material = await dbContext.LearningMaterials
			.Include(m => m.Tags)
			.FirstOrDefaultAsync(m => m.Id == request.MaterialId, cancellationToken);

		if (!IsRequestValid(request, material, out var errorResult))
		{
			return errorResult;
		}

		// Filter out duplicate tag names
		var uniqueTagNames = request.TagNames
			.Select(t => t.Trim())
			.Where(t => !string.IsNullOrWhiteSpace(t))
			.Distinct(StringComparer.OrdinalIgnoreCase)
			.Select(t => t.ToUpperInvariant())
			.ToList();

		// Get existing tags from db
		var existingTagsDict = await dbContext.Tags
			.Where(t => uniqueTagNames.Contains(t.Name.ToUpper()))
			.ToDictionaryAsync(t => t.Name.ToUpperInvariant(), t => t, cancellationToken);

		// Process each tag
		foreach (var tagName in uniqueTagNames)
		{
			// Check if tag already exists in the database
			if (existingTagsDict.TryGetValue(tagName.ToUpperInvariant(), out var existingTag))
			{
				// Check if the material already has this tag
				if (material.Tags.All(t => t.Id != existingTag.Id))
				{
					material.Tags.Add(existingTag);
				}
			}
			else
			{
				// Create new tag
				var newTag = new Tag { Name = tagName };
				dbContext.Tags.Add(newTag);
				material.Tags.Add(newTag);
			}
		}

		await dbContext.SaveChangesAsync(cancellationToken);

		// Prepare response
		var response = new AddTagsResponse
		{
			MaterialId = material.Id,
			Tags = [.. material.Tags.Select(t => new TagInfo
				{
					Id = t.Id,
					Name = t.Name
				})]
		};

		return Result.Success(response);
	}

	private static bool IsRequestValid(AddTagsToMaterialCommand request, LearningMaterial material, out Result<AddTagsResponse> result)
	{
		if (material is null)
		{
			result = Result.NotFound($"Material with id {request.MaterialId} not found");
			return false;
		}

		if (request.MentorId != 0 && material.MentorId != request.MentorId)
		{
			result = Result.Forbidden("You do not have permission to add tags to this material");
			return false;
		}

		if (request.TagNames is null || request.TagNames.Count == 0)
		{
			result = Result.Error("At least one tag name must be provided");
			return false;
		}

		result = null;
		return true;
	}
}
