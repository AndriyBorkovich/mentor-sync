using MentorSync.SharedKernel.CommonEntities;

namespace MentorSync.SharedKernel.Extensions;

public static partial class EnumFormatter
{
    public static string GetCategories(this Industry industries)
    {
        var categories = new List<string>();

        if ((industries & Industry.WebDevelopment) == Industry.WebDevelopment)
            categories.Add("Веб розробка");
        if ((industries & Industry.DataScience) == Industry.DataScience)
            categories.Add("Наука даних");
        if ((industries & Industry.CyberSecurity) == Industry.CyberSecurity)
            categories.Add("Кібербезпека");
        if ((industries & Industry.CloudComputing) == Industry.CloudComputing)
            categories.Add("Хмарні обчислення");
        if ((industries & Industry.DevOps) == Industry.DevOps)
            categories.Add("DevOps");
        if ((industries & Industry.GameDevelopment) == Industry.GameDevelopment)
            categories.Add("Розробка ігор");
        if ((industries & Industry.ItSupport) == Industry.ItSupport)
            categories.Add("IT підтримка");
        if ((industries & Industry.ArtificialIntelligence) == Industry.ArtificialIntelligence)
            categories.Add("Штучний інтелект");
        if ((industries & Industry.Blockchain) == Industry.Blockchain)
            categories.Add("Блокчейн");
        if ((industries & Industry.Networking) == Industry.Networking)
            categories.Add("Мережі");
        if ((industries & Industry.UxUiDesign) == Industry.UxUiDesign)
            categories.Add("UX/UI дизайн");
        if ((industries & Industry.EmbeddedSystems) == Industry.EmbeddedSystems)
            categories.Add("Вбудовані системи");
        if ((industries & Industry.ItConsulting) == Industry.ItConsulting)
            categories.Add("IT консалтинг");
        if ((industries & Industry.DatabaseAdministration) == Industry.DatabaseAdministration)
            categories.Add("Адміністрування баз даних");
        if ((industries & Industry.ProjectManagement) == Industry.ProjectManagement)
            categories.Add("Проєктний менеджемент");
        if ((industries & Industry.MobileDevelopment) == Industry.MobileDevelopment)
            categories.Add("Мобільна розробка");
        if ((industries & Industry.LowCodeNoCode) == Industry.LowCodeNoCode)
            categories.Add("Low/No кодування");
        if ((industries & Industry.QualityControlAssurance) == Industry.QualityControlAssurance)
            categories.Add("QA/QC");
        if ((industries & Industry.MachineLearning) == Industry.MachineLearning)
            categories.Add("Машинне навчання");

        return categories.Count > 0 ? string.Join(", ", categories) : "Інше";
    }
}
