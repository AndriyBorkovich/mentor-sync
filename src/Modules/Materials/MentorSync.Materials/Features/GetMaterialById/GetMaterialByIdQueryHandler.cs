using Ardalis.Result;
using MediatR;
using MentorSync.Materials.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentorSync.Materials.Features.GetMaterialById;

public class GetMaterialByIdQueryHandler(
    MaterialsDbContext dbContext,
    ILogger<GetMaterialByIdQueryHandler> logger) : IRequestHandler<GetMaterialByIdQuery, Result<MaterialResponse>>
{
    public async Task<Result<MaterialResponse>> Handle(GetMaterialByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var material = await dbContext.LearningMaterials
                .AsNoTracking()
                .Include(m => m.Attachments)
                .Include(m => m.Tags)
                .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

            if (material == null)
            {
                return Result.NotFound($"Learning material with ID {request.Id} not found");
            }

            // TODO: In a real application, get the mentor name from a Users service
            var mentorName = $"Mentor {material.MentorId}";

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
                Attachments = material.Attachments.Select(a => new AttachmentInfo
                {
                    Id = a.Id,
                    FileName = a.FileName,
                    FileUrl = a.BlobUri,
                    ContentType = a.ContentType,
                    FileSize = a.Size,
                    UploadedAt = a.UploadedAt
                }).ToList(),
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
            logger.LogError(ex, "Error getting material with ID: {MaterialId}", request.Id);
            return Result.Error($"An error occurred while getting the material: {ex.Message}");
        }
    }
}
