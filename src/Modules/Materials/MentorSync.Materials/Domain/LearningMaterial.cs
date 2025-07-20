using MentorSync.SharedKernel.CommonEntities.Enums;

namespace MentorSync.Materials.Domain;

public sealed class LearningMaterial
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public MaterialType Type { get; set; }
    /// <summary>
    /// Text in markdown format (.md)
    /// </summary>
    public string ContentMarkdown { get; set; }
    /// <summary>
    /// Reference to mentor, who created this material
    /// </summary>
    public int MentorId { get; set; }                      
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<MaterialAttachment> Attachments { get; set; }
    public List<Tag> Tags { get; set; }
}
