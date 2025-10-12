using Microsoft.AspNetCore.Http;

namespace MentorSync.Materials.Features.UploadMaterialAttachment;

/// <summary>
/// Command to upload an attachment to a material
/// </summary>
public sealed record UploadMaterialAttachmentCommand : ICommand<UploadAttachmentResponse>
{
	/// <summary>
	/// The ID of the material to which the attachment will be uploaded
	/// </summary>
	public int MaterialId { get; init; }
	/// <summary>
	/// The file to be uploaded as an attachment
	/// </summary>
	public IFormFile File { get; init; }
	/// <summary>
	/// The ID of the mentor uploading the attachment
	/// <remarks>To verify ownership</remarks>
	/// </summary>
	public int MentorId { get; init; }
}
