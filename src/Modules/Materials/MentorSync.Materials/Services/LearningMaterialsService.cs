using MentorSync.Materials.Contracts.Models;
using MentorSync.Materials.Contracts.Services;
using MentorSync.Materials.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Materials.Services;

internal sealed class LearningMaterialsService(MaterialsDbContext dbContext) : ILearningMaterialsService
{
	public async Task<IEnumerable<LearningMaterialModel>> GetAllMaterialsAsync(CancellationToken cancellationToken = default)
	{
		return await dbContext.LearningMaterials
			.AsNoTracking()
			.Select(m => new LearningMaterialModel
			{
				Id = m.Id,
				Title = m.Title,
				Description = m.Description,
				Type = m.Type,
				CreatedAt = m.CreatedAt,
				Tags = m.Tags.Select(t => t.Name).ToList(),
			})
			.ToListAsync(cancellationToken);
	}
}
