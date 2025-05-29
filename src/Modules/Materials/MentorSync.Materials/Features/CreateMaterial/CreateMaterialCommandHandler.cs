using Ardalis.Result;
using MediatR;
using MentorSync.Materials.Data;
using MentorSync.Materials.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentorSync.Materials.Features.CreateMaterial;

public class CreateMaterialCommandHandler(
    MaterialsDbContext dbContext,
    ILogger<CreateMaterialCommandHandler> logger) : IRequestHandler<CreateMaterialCommand, Result<CreateMaterialResponse>>
{
    public async Task<Result<CreateMaterialResponse>> Handle(CreateMaterialCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Create new learning material
            var material = new LearningMaterial
            {
                Title = request.Title,
                Description = request.Description,
                Type = request.Type,
                ContentMarkdown = request.ContentMarkdown,
                MentorId = request.MentorId,
                CreatedAt = DateTime.UtcNow,
                Attachments = new List<MaterialAttachment>(),
                Tags = new List<Tag>()
            };

            // Add material to database
            dbContext.LearningMaterials.Add(material);

            // Process tags
            if (request.Tags?.Any() == true)
            {
                foreach (var tagName in request.Tags.Distinct())
                {
                    // Check if tag already exists
                    var existingTag = await dbContext.Tags
                        .FirstOrDefaultAsync(t => t.Name.ToLower() == tagName.ToLower(), cancellationToken);

                    if (existingTag != null)
                    {
                        material.Tags.Add(existingTag);
                    }
                    else
                    {
                        // Create new tag
                        var newTag = new Tag
                        {
                            Name = tagName,
                        };
                        material.Tags.Add(newTag);
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
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating material: {Title}", request.Title);
            return Result.Error($"An error occurred while creating the material: {ex.Message}");
        }
    }
}
