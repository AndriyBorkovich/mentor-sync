using Ardalis.Result;
using MentorSync.Materials.Data;
using MentorSync.Materials.Domain;
using MentorSync.SharedKernel.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Materials.Features.CreateMaterial;

public sealed class CreateMaterialCommandHandler(
    MaterialsDbContext dbContext)
        : ICommandHandler<CreateMaterialCommand, CreateMaterialResponse>
{
    public async Task<Result<CreateMaterialResponse>> Handle(CreateMaterialCommand request, CancellationToken cancellationToken)
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
            foreach (var tagName in request.Tags.Distinct())
            {
                var existingTag = await dbContext.Tags
                    .FirstOrDefaultAsync(t => t.Name.ToLower() == tagName.ToLower(), cancellationToken);

                if (existingTag != null)
                {
                    material.Tags.Add(existingTag);
                }
                else
                {
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
}
