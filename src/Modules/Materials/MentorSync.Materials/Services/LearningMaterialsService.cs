using MentorSync.Materials.Contracts.Models;
using MentorSync.Materials.Contracts.Services;
using MentorSync.Materials.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Materials.Services;

internal sealed class LearningMaterialsService(MaterialsDbContext dbContext) : ILearningMaterialsService
{
    public Task<List<LearningMaterialModel>> GetAllMaterialsAsync(CancellationToken cancellationToken = default)
    {
        var result = dbContext.LearningMaterials
            .Select(m => new LearningMaterialModel
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                Type = m.Type,
                CreatedAt = m.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return result;
    }
}
