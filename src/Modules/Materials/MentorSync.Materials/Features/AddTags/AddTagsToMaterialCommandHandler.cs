using Ardalis.Result;
using MediatR;
using MentorSync.Materials.Data;
using MentorSync.Materials.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentorSync.Materials.Features.AddTags;

public class AddTagsToMaterialCommandHandler(
    MaterialsDbContext dbContext,
    ILogger<AddTagsToMaterialCommandHandler> logger) : IRequestHandler<AddTagsToMaterialCommand, Result<AddTagsResponse>>
{
    public async Task<Result<AddTagsResponse>> Handle(AddTagsToMaterialCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate that material exists and belongs to mentor
            var material = await dbContext.LearningMaterials
                .Include(m => m.Tags)
                .FirstOrDefaultAsync(m => m.Id == request.MaterialId, cancellationToken);

            if (material == null)
            {
                return Result.NotFound($"Material with id {request.MaterialId} not found");
            }

            // Verify ownership if MentorId is provided (not 0)
            if (request.MentorId != 0 && material.MentorId != request.MentorId)
            {
                return Result.Forbidden("You do not have permission to add tags to this material");
            }

            // Check if tags are empty
            if (request.TagNames == null || request.TagNames.Count == 0)
            {
                return Result.Error("At least one tag name must be provided");
            }

            // Filter out duplicate tag names
            var uniqueTagNames = request.TagNames
                .Select(t => t.Trim())
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            // Get existing tags from db
            var existingTagsDict = await dbContext.Tags
                .Where(t => uniqueTagNames.Contains(t.Name.ToLower()))
                .ToDictionaryAsync(t => t.Name.ToLower(), t => t, cancellationToken);

            // Create a list to track new tags added
            var addedTags = new List<Tag>();

            // Process each tag
            foreach (var tagName in uniqueTagNames)
            {
                // Check if tag already exists in the database
                if (existingTagsDict.TryGetValue(tagName.ToLower(), out var existingTag))
                {
                    // Check if the material already has this tag
                    if (!material.Tags.Any(t => t.Id == existingTag.Id))
                    {
                        material.Tags.Add(existingTag);
                        addedTags.Add(existingTag);
                    }
                }
                else
                {
                    // Create new tag
                    var newTag = new Tag { Name = tagName };
                    dbContext.Tags.Add(newTag);
                    material.Tags.Add(newTag);
                    addedTags.Add(newTag);
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            // Prepare response
            var response = new AddTagsResponse
            {
                MaterialId = material.Id,
                Tags = material.Tags.Select(t => new TagInfo
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToList()
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding tags to material {MaterialId}", request.MaterialId);
            return Result.Error($"An error occurred while adding tags: {ex.Message}");
        }
    }
}
