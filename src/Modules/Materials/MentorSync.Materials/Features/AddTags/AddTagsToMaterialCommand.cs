using Ardalis.Result;
using MediatR;

namespace MentorSync.Materials.Features.AddTags;

public record AddTagsToMaterialCommand : IRequest<Result<AddTagsResponse>>
{
    public int MaterialId { get; init; }
    public List<string> TagNames { get; init; } = new();
    public int MentorId { get; init; } // To verify ownership
}
