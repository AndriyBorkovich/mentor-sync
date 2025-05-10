namespace MentorSync.Materials.Domain;

/// <summary>
/// Attachments for materials (e.g., images, PDFs) stored in Blob storage.
/// </summary>
public sealed class MaterialAttachment
{
    public int Id { get; set; }
    /// <summary>
    /// Reference to the learning material this attachment belongs to
    /// </summary>
    public int MaterialId { get; set; }
    public string FileName { get; set; } = default!;
    /// <summary>
    /// Blob URI for the attachment in Azure Blob Storage
    /// </summary>
    public string BlobUri { get; set; } = default!;
    public string ContentType { get; set; }
    public DateTime UploadedAt { get; set; }

    public LearningMaterial Material { get; set; } = default!;
}
