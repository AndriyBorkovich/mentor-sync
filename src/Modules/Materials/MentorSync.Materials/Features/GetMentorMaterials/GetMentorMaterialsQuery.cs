using Ardalis.Result;
using MediatR;

namespace MentorSync.Materials.Features.GetMentorMaterials;

public record GetMentorMaterialsQuery(int MentorId) : IRequest<Result<MentorMaterialsResponse>>;
