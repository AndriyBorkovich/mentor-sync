namespace MentorSync.Materials.Domain;

public sealed class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<LearningMaterial> LearningMaterials { get; set; }
}
