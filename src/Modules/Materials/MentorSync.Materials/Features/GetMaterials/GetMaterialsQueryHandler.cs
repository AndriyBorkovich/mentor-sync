using Ardalis.Result;
using MediatR;
using MentorSync.Materials.Data;
using MentorSync.SharedKernel.CommonEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentorSync.Materials.Features.GetMaterials;

public class GetMaterialsQueryHandler(
    MaterialsDbContext dbContext,
    ILogger<GetMaterialsQueryHandler> logger)
        : IRequestHandler<GetMaterialsQuery, Result<MaterialsResponse>>
{
    public async Task<Result<MaterialsResponse>> Handle(GetMaterialsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Start building query
            var query = dbContext.LearningMaterials.AsNoTracking();

            // Apply search filter if provided
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var searchTerm = request.Search.ToLower();
                query = query.Where(m =>
                    m.Title.ToLower().Contains(searchTerm) ||
                    m.Description.ToLower().Contains(searchTerm) ||
                    m.Tags.Any(t => t.Name.ToLower().Contains(searchTerm)));
            }

            // Apply type filter if provided
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

            // Apply tags filter if provided
            if (request.Tags?.Count > 0)
            {
                var lowercaseTags = request.Tags.Select(t => t.ToLower()).ToList();
                query = query.Where(m => m.Tags.Any(t => lowercaseTags.Contains(t.Name.ToLower())));
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync(cancellationToken);

            // Apply sorting
            query = request.SortBy?.ToLower() switch
            {
                "oldest" => query.OrderBy(m => m.CreatedAt),
                "title" => query.OrderBy(m => m.Title),
                "title_desc" => query.OrderByDescending(m => m.Title),
                _ => query.OrderByDescending(m => m.CreatedAt) // Default is newest first
            };

            query = query.Skip((request.PageNumber - 1) * request.PageSize)
                          .Take(request.PageSize);

            // Execute the query with related data
            var materials = await query
                .Include(m => m.Attachments)
                .Include(m => m.Tags)
                .ToListAsync(cancellationToken);

            var mentorIds = materials.Select(m => m.MentorId).Distinct().ToList();

            // TODO: In a real application, you would fetch mentor names from a user service or database
            var mentorNames = mentorIds.ToDictionary(id => id, id => $"Mentor {id}");

            var materialDtos = materials.Select(m => new MaterialDto
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
            }).ToList();

            var response = new MaterialsResponse
            {
                Items = materialDtos,
                TotalCount = totalCount,
                PageSize = request.PageSize,
                PageNumber = request.PageNumber
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting materials with filters: {Search}, {Types}, {Tags}", request.Search, request.Types, request.Tags);
            return Result.Error($"An error occurred while getting materials: {ex.Message}");
        }
    }
}
