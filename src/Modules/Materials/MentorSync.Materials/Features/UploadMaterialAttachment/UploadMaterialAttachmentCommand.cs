using Microsoft.AspNetCore.Http;

namespace MentorSync.Materials.Features.UploadMaterialAttachment;

public record UploadMaterialAttachmentCommand : ICommand<UploadAttachmentResponse>
{
	public int MaterialId { get; init; }
	public IFormFile File { get; init; }
	public int MentorId { get; init; } // To verify ownership
}
