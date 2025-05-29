/**
 * Industry enum - matches the C# Industry enum
 */
// Enums matching the backend
export enum Industry {
    None = 0,
    WebDevelopment = 1 << 0, // 1
    DataScience = 1 << 1, // 2
    CyberSecurity = 1 << 2, // 4
    CloudComputing = 1 << 3, // 8
    DevOps = 1 << 4, // 16
    GameDevelopment = 1 << 5, // 32
    ItSupport = 1 << 6, // 64
    ArtificialIntelligence = 1 << 7, // 128
    Blockchain = 1 << 8, // 256
    Networking = 1 << 9, // 512
    UxUiDesign = 1 << 10, // 1024
    EmbeddedSystems = 1 << 11, // 2048
    ItConsulting = 1 << 12, // 4096
    DatabaseAdministration = 1 << 13, // 8192
    ProjectManagement = 1 << 14, // 16384
    MobileDevelopment = 1 << 15, // 32768
    LowCodeNoCode = 1 << 16, // 65536
    QualityControlAssurance = 1 << 17, // 131072
    MachineLearning = 1 << 18, // 262144
}

export const industriesMapping = [
    { value: Industry.WebDevelopment, label: "Веб-розробка" },
    { value: Industry.DataScience, label: "Data Science" },
    { value: Industry.CyberSecurity, label: "Кібербезпека" },
    { value: Industry.CloudComputing, label: "Хмарні обчислення" },
    { value: Industry.DevOps, label: "DevOps" },
    { value: Industry.GameDevelopment, label: "Розробка ігор" },
    { value: Industry.ItSupport, label: "ІТ-підтримка" },
    { value: Industry.ArtificialIntelligence, label: "Штучний інтелект" },
    { value: Industry.Blockchain, label: "Блокчейн" },
    { value: Industry.Networking, label: "Мережі" },
    { value: Industry.UxUiDesign, label: "UX/UI дизайн" },
    { value: Industry.EmbeddedSystems, label: "Вбудовані системи" },
    { value: Industry.ItConsulting, label: "ІТ-консалтинг" },
    {
        value: Industry.DatabaseAdministration,
        label: "Адміністрування баз даних",
    },
    { value: Industry.ProjectManagement, label: "Управління проектами" },
    { value: Industry.MobileDevelopment, label: "Мобільна розробка" },
    { value: Industry.LowCodeNoCode, label: "Low-code/No-code" },
    {
        value: Industry.QualityControlAssurance,
        label: "QA/QC",
    },
    { value: Industry.MachineLearning, label: "Машинне навчання" },
];
