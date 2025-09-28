namespace MentorSync.SharedKernel.Extensions;

/// <summary>
/// Utility class for formatting enum values to localized strings
/// </summary>
public static class EnumFormatter
{
	private static readonly (Industry, string)[] _industryMappings =
		[
			(Industry.WebDevelopment, "Веб розробка"),
			(Industry.DataScience, "Наука даних"),
			(Industry.CyberSecurity, "Кібербезпека"),
			(Industry.CloudComputing, "Хмарні обчислення"),
			(Industry.DevOps, "DevOps"),
			(Industry.GameDevelopment, "Розробка ігор"),
			(Industry.ItSupport, "IT підтримка"),
			(Industry.ArtificialIntelligence, "Штучний інтелект"),
			(Industry.Blockchain, "Блокчейн"),
			(Industry.Networking, "Мережі"),
			(Industry.UxUiDesign, "UX/UI дизайн"),
			(Industry.EmbeddedSystems, "Вбудовані системи"),
			(Industry.ItConsulting, "IT консалтинг"),
			(Industry.DatabaseAdministration, "Адміністрування баз даних"),
			(Industry.ProjectManagement, "Управління проектами"),
			(Industry.MobileDevelopment, "Мобільна розробка"),
			(Industry.LowCodeNoCode, "Low-code/No-code"),
			(Industry.QualityControlAssurance, "Контроль якості/тестування"),
			(Industry.MachineLearning, "Машинне навчання")
		];

	/// <summary>
	/// Gets localized category names for the specified industries
	/// </summary>
	/// <param name="industries">The industry flags to format</param>
	/// <returns>A comma-separated string of localized industry names</returns>
	public static string GetCategories(this Industry industries)
	{

		var categories = new List<string>();
		foreach (var (industry, name) in _industryMappings)
		{
			if ((industries & industry) == industry)
			{
				categories.Add(name);
			}
		}

		return string.Join(", ", categories);
	}
}
