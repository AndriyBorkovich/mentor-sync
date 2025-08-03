namespace MentorSync.SharedKernel.CommonEntities.Enums;

[Flags]
public enum Industry
{
	None = 0,
	WebDevelopment = 1 << 0,  // 1
	DataScience = 1 << 1,          // 2
	CyberSecurity = 1 << 2,        // 4
	CloudComputing = 1 << 3,       // 8
	DevOps = 1 << 4,               // 16
	GameDevelopment = 1 << 5,      // 32
	ItSupport = 1 << 6,            // 64
	ArtificialIntelligence = 1 << 7, // 128
	Blockchain = 1 << 8,           // 256
	Networking = 1 << 9,           // 512
	UxUiDesign = 1 << 10,          // 1024
	EmbeddedSystems = 1 << 11,     // 2048
	ItConsulting = 1 << 12,        // 4096
	DatabaseAdministration = 1 << 13, // 8192
	ProjectManagement = 1 << 14, // 16384
	MobileDevelopment = 1 << 15, // 32768
	LowCodeNoCode = 1 << 16, // 65536
	QualityControlAssurance = 1 << 17, // 131072
	MachineLearning = 1 << 18, // 262144
}
