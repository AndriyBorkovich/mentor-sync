using Ardalis.Result;
using MediatR;
using MentorSync.Materials.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentorSync.Materials.Features.GetMentorMaterials;

public class GetMentorMaterialsQueryHandler(MaterialsDbContext dbContext, ILogger<GetMentorMaterialsQueryHandler> logger) : IRequestHandler<GetMentorMaterialsQuery, Result<MentorMaterialsResponse>>
{
    public async Task<Result<MentorMaterialsResponse>> Handle(GetMentorMaterialsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get learning materials using LINQ instead of raw SQL
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

            // Get material IDs for subsequent queries
            var materialIds = materials.Select(m => m.Id).ToList();

            // Get tags for materials
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

            // Get attachments for materials
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
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting materials for MentorId: {MentorId}", request.MentorId);
            return Result.Error($"An error occurred while getting materials: {ex.Message}");
        }
    }
}
