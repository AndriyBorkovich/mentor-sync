namespace MentorSync.SharedKernel.CommonEntities.Enums;

/// <summary>
/// Represents different technology industries/specializations in the mentor-sync system
/// </summary>
[Flags]
public enum Industry
{
	/// <summary>
	/// No industry specified
	/// </summary>
	None = 0,

	/// <summary>
	/// Web development industry
	/// </summary>
	WebDevelopment = 1 << 0,  // 1

	/// <summary>
	/// Data science and analytics industry
	/// </summary>
	DataScience = 1 << 1,          // 2

	/// <summary>
	/// Cybersecurity industry
	/// </summary>
	CyberSecurity = 1 << 2,        // 4

	/// <summary>
	/// Cloud computing industry
	/// </summary>
	CloudComputing = 1 << 3,       // 8

	/// <summary>
	/// DevOps and infrastructure industry
	/// </summary>
	DevOps = 1 << 4,               // 16

	/// <summary>
	/// Game development industry
	/// </summary>
	GameDevelopment = 1 << 5,      // 32

	/// <summary>
	/// IT support and helpdesk industry
	/// </summary>
	ItSupport = 1 << 6,            // 64

	/// <summary>
	/// Artificial intelligence industry
	/// </summary>
	ArtificialIntelligence = 1 << 7, // 128

	/// <summary>
	/// Blockchain and cryptocurrency industry
	/// </summary>
	Blockchain = 1 << 8,           // 256

	/// <summary>
	/// Network administration and engineering industry
	/// </summary>
	Networking = 1 << 9,           // 512

	/// <summary>
	/// UX/UI design industry
	/// </summary>
	UxUiDesign = 1 << 10,          // 1024

	/// <summary>
	/// Embedded systems development industry
	/// </summary>
	EmbeddedSystems = 1 << 11,     // 2048

	/// <summary>
	/// IT consulting industry
	/// </summary>
	ItConsulting = 1 << 12,        // 4096

	/// <summary>
	/// Database administration industry
	/// </summary>
	DatabaseAdministration = 1 << 13, // 8192

	/// <summary>
	/// Project management industry
	/// </summary>
	ProjectManagement = 1 << 14, // 16384

	/// <summary>
	/// Mobile application development industry
	/// </summary>
	MobileDevelopment = 1 << 15, // 32768

	/// <summary>
	/// Low-code/no-code development industry
	/// </summary>
	LowCodeNoCode = 1 << 16, // 65536

	/// <summary>
	/// Quality control and assurance industry
	/// </summary>
	QualityControlAssurance = 1 << 17, // 131072

	/// <summary>
	/// Machine learning industry
	/// </summary>
	MachineLearning = 1 << 18, // 262144
}
