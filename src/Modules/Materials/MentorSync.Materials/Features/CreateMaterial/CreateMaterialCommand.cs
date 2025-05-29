using Ardalis.Result;
using MediatR;
using MentorSync.Materials.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MentorSync.Materials.Features.CreateMaterial;

public record CreateMaterialCommand : IRequest<Result<CreateMaterialResponse>>
{
    [Required]
    [StringLength(200)]
    public string Title { get; init; }

    [StringLength(2000)]
    public string Description { get; init; }

    [Required]
    public MaterialType Type { get; init; }

    public string ContentMarkdown { get; init; }

    [Required]
    public int MentorId { get; init; }

    public List<string> Tags { get; init; } = new();
}
