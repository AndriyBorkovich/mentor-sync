using Ardalis.Result;
using MediatR;

namespace MentorSync.Materials.Features.GetMaterialById;

public record GetMaterialByIdQuery(int Id) : IRequest<Result<MaterialResponse>>;
