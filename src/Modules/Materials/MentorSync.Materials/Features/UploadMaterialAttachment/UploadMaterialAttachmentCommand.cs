using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MentorSync.Materials.Features.UploadMaterialAttachment;

public record UploadMaterialAttachmentCommand : IRequest<Result<UploadAttachmentResponse>>
{
    public int MaterialId { get; init; }
    public IFormFile File { get; init; }
    public int MentorId { get; init; } // To verify ownership
}
