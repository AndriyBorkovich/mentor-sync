namespace MentorSync.Recommendations.Domain.Interaction;

/// <summary>
/// Used to store the interaction between a mentee and learning material for collaborative filtering.
/// </summary>
public sealed class MenteeMaterialInteraction : BaseInteraction
{
	/// <summary>
	/// Identifier of the learning material
	/// </summary>
	public int MaterialId { get; set; }
}
