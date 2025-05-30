using System.Diagnostics;
using Bogus;
using MentorSync.Materials.Data;
using MentorSync.Ratings.Data;
using MentorSync.Ratings.Domain;
using MentorSync.Recommendations.Data;
using MentorSync.Scheduling.Data;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.CommonEntities;
using MentorSync.Users.Data;
using MentorSync.Users.Domain.Enums;
using MentorSync.Users.Domain.Mentee;
using MentorSync.Users.Domain.Mentor;
using MentorSync.Users.Domain.Role;
using MentorSync.Users.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace MentorSync.MigrationService;

public sealed class Worker(
    IServiceProvider serviceProvider,
    ILogger<Worker> logger,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource _activitySource = new(ActivitySourceName); protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = _activitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();

            await MigrateAsync<UsersDbContext>(scope.ServiceProvider, cancellationToken, postMigrationSteps: [SeedRolesAsync, SeedMentorsAsync, SeedMenteesAsync]);
            await MigrateAsync<SchedulingDbContext>(scope.ServiceProvider, cancellationToken);
            await MigrateAsync<MaterialsDbContext>(scope.ServiceProvider, cancellationToken);
            await MigrateAsync<RatingsDbContext>(scope.ServiceProvider, cancellationToken, postMigrationSteps: [SeedMentorReviewsAsync]);
            await MigrateAsync<RecommendationsDbContext>(scope.ServiceProvider, cancellationToken);

            logger.LogInformation("Migrated database successfully.");
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }

    private static async Task MigrateAsync<T>(
        IServiceProvider sp,
        CancellationToken cancellationToken,
        params Func<Task>?[] postMigrationSteps)
        where T : DbContext
    {
        var context = sp.GetRequiredService<T>();
        await EnsureDatabaseAsync(context, cancellationToken);
        await RunMigrationAsync(context, cancellationToken);

        foreach (var step in postMigrationSteps.Where(s => s != null))
        {
            await step!();
        }
    }

    private static async Task EnsureDatabaseAsync<T>(T dbContext, CancellationToken cancellationToken)
        where T : DbContext
    {
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            if (!await dbCreator.ExistsAsync(cancellationToken))
            {
                await dbCreator.CreateAsync(cancellationToken);
            }
        });
    }

    private static async Task RunMigrationAsync<T>(T dbContext, CancellationToken cancellationToken)
        where T : DbContext
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        });
    }

    private async Task SeedRolesAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

        await CreateRole(Roles.Admin);
        await CreateRole(Roles.Mentor);
        await CreateRole(Roles.Mentee);

        return;

        async Task CreateRole(string roleName)
        {
            var appRole = await roleManager.FindByNameAsync(roleName);

            if (appRole is null)
            {
                appRole = new AppRole
                {
                    Name = roleName
                };

                await roleManager.CreateAsync(appRole);
            }
        }
    }
    private async Task SeedMentorsAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var usersContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();

        if (await usersContext.MentorProfiles.AnyAsync())
        {
            logger.LogInformation("Mentor profiles already exist, skipping seeding.");
            return;
        }

        logger.LogInformation("Seeding mentor data");

        // Define industry to position mappings for realistic data
        var industryPositionMappings = new Dictionary<Industry, List<string>>
        {
            { Industry.WebDevelopment, new List<string> { "Frontend Developer", "Backend Developer", "Full Stack Developer", "Web Developer", "UX Engineer", "UI Developer" } },
            { Industry.DataScience, new List<string> { "Data Scientist", "Data Analyst", "Data Engineer", "Business Intelligence Analyst", "Research Scientist" } },
            { Industry.CyberSecurity, new List<string> { "Security Engineer", "Penetration Tester", "Security Analyst", "Security Architect", "Security Consultant" } },
            { Industry.CloudComputing, new List<string> { "Cloud Architect", "Cloud Engineer", "DevOps Engineer", "Solutions Architect", "Cloud Administrator" } },
            { Industry.DevOps, new List<string> { "DevOps Engineer", "Site Reliability Engineer", "Platform Engineer", "Release Manager", "DevSecOps Engineer" } },
            { Industry.GameDevelopment, new List<string> { "Game Developer", "Game Designer", "3D Artist", "Unity Developer", "Game Engine Programmer" } },
            { Industry.ItSupport, new List<string> { "IT Support Specialist", "System Administrator", "Network Engineer", "Help Desk Technician", "IT Coordinator" } },
            { Industry.ArtificialIntelligence, new List<string> { "AI Engineer", "Machine Learning Engineer", "AI Research Scientist", "NLP Engineer", "AI Product Manager" } },
            { Industry.Blockchain, new List<string> { "Blockchain Developer", "Smart Contract Engineer", "Blockchain Architect", "Blockchain Consultant", "Cryptocurrency Specialist" } },
            { Industry.Networking, new List<string> { "Network Engineer", "Network Administrator", "Network Architect", "Network Security Engineer", "Telecommunications Specialist" } },
            { Industry.UxUiDesign, new List<string> { "UX Designer", "UI Designer", "Product Designer", "Interaction Designer", "UX Researcher" } },
            { Industry.EmbeddedSystems, new List<string> { "Embedded Systems Engineer", "IoT Developer", "Firmware Engineer", "Hardware Engineer", "Embedded Software Developer" } },
            { Industry.ItConsulting, new List<string> { "IT Consultant", "Technology Strategist", "IT Project Manager", "Business Analyst", "Solutions Consultant" } },
            { Industry.DatabaseAdministration, new List<string> { "Database Administrator", "Database Engineer", "Data Architect", "SQL Developer", "Database Reliability Engineer" } },
            { Industry.ProjectManagement, new List<string> { "Project Manager", "Program Manager", "Scrum Master", "Agile Coach", "Delivery Manager" } },
            { Industry.MobileDevelopment, new List<string> { "iOS Developer", "Android Developer", "Mobile Engineer", "React Native Developer", "Flutter Developer" } },
            { Industry.LowCodeNoCode, new List<string> { "Low-Code Developer", "Business Process Specialist", "CRM Administrator", "No-Code Solution Architect", "Digital Transformation Specialist" } },
            { Industry.QualityControlAssurance, new List<string> { "QA Engineer", "Test Automation Engineer", "QA Analyst", "Quality Assurance Manager", "Test Lead" } },
            { Industry.MachineLearning, new List<string> { "Machine Learning Engineer", "ML Operations Engineer", "AI Researcher", "Computer Vision Engineer", "ML Platform Engineer" } }
        };

        // Define programming languages for each industry (some industries don't require programming)
        var industryLanguagesMappings = new Dictionary<Industry, List<string>>
        {
            { Industry.WebDevelopment, new List<string> { "JavaScript", "TypeScript", "HTML", "CSS", "PHP", "Python", "Ruby", "Java" } },
            { Industry.DataScience, new List<string> { "Python", "R", "SQL", "Julia", "Scala" } },
            { Industry.CyberSecurity, new List<string> { "Python", "Shell Scripting", "PowerShell", "C", "C++" } },
            { Industry.CloudComputing, new List<string> { "Python", "JavaScript", "TypeScript", "Go", "Powelshell", "Bash" } },
            { Industry.DevOps, new List<string> { "Python", "Go", "Shell Scripting", "PowerShell", "Ruby" } },
            { Industry.GameDevelopment, new List<string> { "C++", "C#", "JavaScript", "Python", "Lua" } },
            { Industry.ItSupport, new List<string> { "Shell Scripting", "PowerShell", "Python", "Batch" } },
            { Industry.ArtificialIntelligence, new List<string> { "Python", "R", "C++", "Java", "Julia" } },
            { Industry.Blockchain, new List<string> { "Solidity", "JavaScript", "Python", "Go", "Rust" } },
            { Industry.Networking, new List<string> { "Python", "Shell Scripting", "PowerShell", "C" } },
            { Industry.UxUiDesign, new List<string> { "JavaScript", "TypeScript", "HTML", "CSS" } },
            { Industry.EmbeddedSystems, new List<string> { "C", "C++", "Python", "Assembly", "Rust" } },
            { Industry.ItConsulting, new List<string> { "SQL", "Python", "JavaScript", "Java", "C#" } },
            { Industry.DatabaseAdministration, new List<string> { "SQL", "Python", "PowerShell", "Shell Scripting" } },
            { Industry.ProjectManagement, new List<string>() }, // Often doesn't require programming
            { Industry.MobileDevelopment, new List<string> { "Swift", "Kotlin", "Java", "Objective-C", "Dart", "JavaScript" } },
            { Industry.LowCodeNoCode, new List<string>() }, // By definition doesn't require programming
            { Industry.QualityControlAssurance, new List<string> { "Python", "JavaScript", "Java", "C#", "Ruby" } },
            { Industry.MachineLearning, new List<string> { "Python", "R", "C++", "Java", "Julia" } }
        };

        // Define skills relevant to each industry
        var industrySkillsMappings = new Dictionary<Industry, List<string>>
        {
            { Industry.WebDevelopment, new List<string> { "React", "Angular", "Vue.js", "Node.js", "RESTful API", "GraphQL", "Web Security", "CI/CD", "Responsive Design", "Webpack", "SASS/LESS", "Git", "Docker" } },
            { Industry.DataScience, new List<string> { "Machine Learning", "Data Visualization", "Statistical Analysis", "Big Data", "Database Design", "Data Mining", "ETL", "Pandas", "NumPy", "Jupyter", "Tableau", "Power BI" } },
            { Industry.CyberSecurity, new List<string> { "Network Security", "Penetration Testing", "SIEM", "Ethical Hacking", "Risk Assessment", "Encryption", "Security Auditing", "Vulnerability Assessment", "Firewall Configuration", "Incident Response" } },
            { Industry.CloudComputing, new List<string> { "AWS", "Azure", "GCP", "Kubernetes", "Docker", "Terraform", "IaaS", "PaaS", "CloudFormation", "Serverless", "Microservices", "Load Balancing" } },
            { Industry.DevOps, new List<string> { "CI/CD", "Docker", "Kubernetes", "Jenkins", "GitLab CI", "GitHub Actions", "Ansible", "Terraform", "Monitoring", "Infrastructure as Code", "Observability", "Site Reliability Engineering" } },
            { Industry.GameDevelopment, new List<string> { "Unity", "Unreal Engine", "3D Modeling", "Animation", "Graphics Programming", "Game Physics", "Multiplayer Networking", "Game Design", "Mobile Gaming", "Console Development" } },
            { Industry.ItSupport, new List<string> { "Troubleshooting", "System Administration", "Network Configuration", "Active Directory", "IT Service Management", "Help Desk", "IT Infrastructure", "ITIL", "User Support" } },
            { Industry.ArtificialIntelligence, new List<string> { "Machine Learning", "Neural Networks", "Deep Learning", "NLP", "Computer Vision", "TensorFlow", "PyTorch", "Reinforcement Learning", "AI Ethics", "Feature Engineering" } },
            { Industry.Blockchain, new List<string> { "Smart Contracts", "Ethereum", "Bitcoin", "Decentralized Applications", "Tokenomics", "Web3", "Cryptography", "Consensus Mechanisms", "NFTs", "DeFi" } },
            { Industry.Networking, new List<string> { "TCP/IP", "Routing", "Switching", "Firewalls", "VPN", "DHCP", "DNS", "Network Security", "Cisco", "Load Balancing", "Network Virtualization" } },
            { Industry.UxUiDesign, new List<string> { "User Research", "Wireframing", "Prototyping", "Usability Testing", "Information Architecture", "Design Systems", "Figma", "Adobe XD", "Sketch", "Accessibility", "Interaction Design" } },
            { Industry.EmbeddedSystems, new List<string> { "Microcontrollers", "RTOS", "Circuit Design", "PCB Design", "IoT", "Firmware Development", "Signal Processing", "Low-Level Programming", "Sensors", "Actuators" } },
            { Industry.ItConsulting, new List<string> { "Business Analysis", "Requirements Gathering", "Solution Architecture", "Project Management", "Digital Transformation", "Change Management", "Strategic Planning", "TOGAF", "Enterprise Architecture" } },
            { Industry.DatabaseAdministration, new List<string> { "SQL", "Database Optimization", "Data Modeling", "Database Security", "ETL", "Backup/Recovery", "High Availability", "Replication", "PostgreSQL", "MySQL", "MongoDB", "Oracle" } },
            { Industry.ProjectManagement, new List<string> { "Agile", "Scrum", "Kanban", "Waterfall", "Risk Management", "Stakeholder Management", "Resource Allocation", "Budgeting", "JIRA", "MS Project", "Communication", "Leadership" } },
            { Industry.MobileDevelopment, new List<string> { "iOS Development", "Android Development", "React Native", "Flutter", "Mobile UI Design", "App Store Optimization", "Push Notifications", "Mobile Security", "Responsive Design", "Cross-platform Development" } },
            { Industry.LowCodeNoCode, new List<string> { "Bubble.io", "Zapier", "Airtable", "Power Apps", "Microsoft Power Automate", "OutSystems", "Mendix", "Business Process Automation", "Workflow Design", "Citizen Development" } },
            { Industry.QualityControlAssurance, new List<string> { "Test Automation", "Selenium", "Cypress", "JUnit", "TestNG", "Manual Testing", "API Testing", "Performance Testing", "Security Testing", "Bug Tracking", "Test Plans", "Quality Metrics" } },
            { Industry.MachineLearning, new List<string> { "TensorFlow", "PyTorch", "Scikit-learn", "Feature Engineering", "Model Optimization", "Data Preprocessing", "MLOps", "Neural Networks", "Computer Vision", "NLP", "Reinforcement Learning" } }
        };

        var userFaker = new Faker<AppUser>()
            .RuleFor(u => u.UserName, f => f.Internet.UserName())
            .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.UserName!))
            .RuleFor(u => u.EmailConfirmed, true)
            .RuleFor(u => u.PhoneNumberConfirmed, true)
            .RuleFor(u => u.ProfileImageUrl, f => f.Internet.Avatar())
            .RuleFor(u => u.IsActive, true)
            .RuleFor(u => u.Country, f => f.Address.Country());

        var skillsList = new List<string>
        {
            "Docker", "Kubernetes", "AWS", "Azure",
            "Машинне навчання", "Аналіз даних", "Кібербезпека", "Блокчейн",
            "UI/UX Дизайн", "Розробка ігор", "Розробка IoT",
            "Гнучкі методології", "Розробка API", "Архітектура мікросервісів",
            "Великі дані", "Штучний інтелект", "Тестування програмного забезпечення",
            "Контроль версій", "Веб-технології", "RESTful API",
            "GraphQL", "Серверless архітектура", "CI/CD процеси",
            "Agile та Scrum", "Системи моніторингу та логування", "Управління проектами",
            "Командна робота", "Системи управління контентом (CMS)", "Веб-безпека",
            "Менеджмент проектів", "Аналіз бізнес-процесів", "Крос-функціональна команда",
            "Стратегічне планування", "Управління ризиками", "Фінансовий аналіз",
        };

        var mentorProfileFaker = new Faker<MentorProfile>()
            .RuleFor(m => m.Bio, f => f.Lorem.Paragraphs(2))
            // Position and Industry will be related in the actual generation logic
            // Company will be set in the generation loop
            .RuleFor(m => m.Industries, f =>
            {
                // Pick random industry flags
                var industryValues = Enum.GetValues<Industry>().Where(i => i != Industry.None).ToArray();
                var selectedIndustries = new List<Industry>();

                // Select 1-2 random industries
                for (int i = 0; i < f.Random.Int(1, 2); i++)
                {
                    selectedIndustries.Add(f.PickRandom(industryValues));
                }

                // Combine flags
                return selectedIndustries.Aggregate(Industry.None, (current, industry) => current | industry);
            })
            // Skills and ProgrammingLanguages will be industry-appropriate in the generation loop
            .RuleFor(m => m.ExperienceYears, f => f.Random.Int(1, 20))
            .RuleFor(m => m.Availability, f =>
            {
                // Generate a combination of availability times
                var availabilities = new[] {
                    Availability.Morning,
                    Availability.Afternoon,
                    Availability.Evening,
                    Availability.Night
                };

                // Select 1-3 random availability options
                var selected = f.Random.ListItems(availabilities, f.Random.Int(1, 4)).ToArray();
                return selected.Aggregate(Availability.None, (current, availability) => current | availability);
            });

        for (var i = 0; i < 100; i++)
        {
            // Create and save the AppUser
            var appUser = userFaker.Generate();
            var password = "String123!";

            var createResult = await userManager.CreateAsync(appUser, password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                logger.LogError("Failed to create mentor user: {Errors}", errors);
                continue;
            }

            // Add to Mentor role
            var roleResult = await userManager.AddToRoleAsync(appUser, Roles.Mentor);
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                logger.LogError("Failed to add mentor role: {Errors}", errors);
                continue;
            }

            // Create and save the MentorProfile with industry-appropriate data
            var faker = new Faker();
            var mentorProfile = mentorProfileFaker.Generate();
            mentorProfile.MentorId = appUser.Id;
            mentorProfile.User = appUser;

            // Get the primary industry (just take the first flag that is set)
            var primaryIndustry = Enum.GetValues<Industry>()
                .FirstOrDefault(i => i != Industry.None && mentorProfile.Industries.HasFlag(i));

            if (primaryIndustry != Industry.None)
            {
                // Set industry-specific position
                if (industryPositionMappings.TryGetValue(primaryIndustry, out var positions) && positions.Any())
                {
                    mentorProfile.Position = faker.PickRandom(positions);
                }

                // Set industry-specific programming languages
                if (industryLanguagesMappings.TryGetValue(primaryIndustry, out var languages) && languages.Any())
                {
                    // Take 1-3 languages from the industry-specific list, or default if empty
                    mentorProfile.ProgrammingLanguages = languages.Any()
                        ? faker.PickRandom(languages, faker.Random.Int(1, Math.Min(3, languages.Count))).ToList()
                        : ProgrammingLanguages.Values.OrderBy(_ => faker.Random.Int()).Take(2).ToList();
                }
                else
                {
                    // Fallback to random languages
                    mentorProfile.ProgrammingLanguages = ProgrammingLanguages.Values.OrderBy(_ => faker.Random.Int()).Take(2).ToList();
                }

                // Set industry-specific skills
                if (industrySkillsMappings.TryGetValue(primaryIndustry, out var skills) && skills.Any())
                {
                    // Pick 3-5 skills from the industry-specific list
                    mentorProfile.Skills = faker.PickRandom(skills, faker.Random.Int(3, Math.Min(5, skills.Count))).ToList();
                }
                else
                {
                    // Fallback to random skills from the general list
                    mentorProfile.Skills = faker.PickRandom(skillsList, faker.Random.Int(3, 5)).ToList();
                }

                // Set company name - more senior positions (containing "Senior", "Lead", "Manager", "Architect", etc.) 
                // are more likely to be from well-known companies
                var isSeniorPosition = mentorProfile.Position.Contains("Senior") ||
                                       mentorProfile.Position.Contains("Lead") ||
                                       mentorProfile.Position.Contains("Manager") ||
                                       mentorProfile.Position.Contains("Architect") ||
                                       mentorProfile.Position.Contains("Director") ||
                                       mentorProfile.ExperienceYears > 8;

                mentorProfile.Company = isSeniorPosition && faker.Random.Bool(0.7f)
                    ? faker.Company.CompanyName(0) + " " + faker.Commerce.ProductName().Split(' ')[0] // Generate more recognizable company name
                    : faker.Company.CompanyName();
            }

            usersContext.MentorProfiles.Add(mentorProfile);
        }

        await usersContext.SaveChangesAsync();
        logger.LogInformation("Successfully seeded {Count} mentors", 100);
    }
    private async Task SeedMenteesAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var usersContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();

        if (await usersContext.MenteeProfiles.AnyAsync())
        {
            logger.LogInformation("Mentee profiles already exist, skipping seeding.");
            return;
        }

        logger.LogInformation("Seeding mentee data");

        // Industry-specific mappings (duplicating from SeedMentorsAsync for availability)
        var industryPositionMappings = new Dictionary<Industry, List<string>>
        {
            { Industry.WebDevelopment, new List<string> { "Frontend Developer", "Backend Developer", "Full Stack Developer", "Web Developer", "UX Engineer", "UI Developer" } },
            { Industry.DataScience, new List<string> { "Data Scientist", "Data Analyst", "Data Engineer", "Business Intelligence Analyst", "Research Scientist" } },
            { Industry.CyberSecurity, new List<string> { "Security Engineer", "Penetration Tester", "Security Analyst", "Security Architect", "Security Consultant" } },
            { Industry.CloudComputing, new List<string> { "Cloud Architect", "Cloud Engineer", "DevOps Engineer", "Solutions Architect", "Cloud Administrator" } },
            { Industry.DevOps, new List<string> { "DevOps Engineer", "Site Reliability Engineer", "Platform Engineer", "Release Manager", "DevSecOps Engineer" } },
            { Industry.GameDevelopment, new List<string> { "Game Developer", "Game Designer", "3D Artist", "Unity Developer", "Game Engine Programmer" } },
            { Industry.ItSupport, new List<string> { "IT Support Specialist", "System Administrator", "Network Engineer", "Help Desk Technician", "IT Coordinator" } },
            { Industry.ArtificialIntelligence, new List<string> { "AI Engineer", "Machine Learning Engineer", "AI Research Scientist", "NLP Engineer", "AI Product Manager" } },
            { Industry.Blockchain, new List<string> { "Blockchain Developer", "Smart Contract Engineer", "Blockchain Architect", "Blockchain Consultant", "Cryptocurrency Specialist" } },
            { Industry.Networking, new List<string> { "Network Engineer", "Network Administrator", "Network Architect", "Network Security Engineer", "Telecommunications Specialist" } },
            { Industry.UxUiDesign, new List<string> { "UX Designer", "UI Designer", "Product Designer", "Interaction Designer", "UX Researcher" } },
            { Industry.EmbeddedSystems, new List<string> { "Embedded Systems Engineer", "IoT Developer", "Firmware Engineer", "Hardware Engineer", "Embedded Software Developer" } },
            { Industry.ItConsulting, new List<string> { "IT Consultant", "Technology Strategist", "IT Project Manager", "Business Analyst", "Solutions Consultant" } },
            { Industry.DatabaseAdministration, new List<string> { "Database Administrator", "Database Engineer", "Data Architect", "SQL Developer", "Database Reliability Engineer" } },
            { Industry.ProjectManagement, new List<string> { "Project Manager", "Program Manager", "Scrum Master", "Agile Coach", "Delivery Manager" } },
            { Industry.MobileDevelopment, new List<string> { "iOS Developer", "Android Developer", "Mobile Engineer", "React Native Developer", "Flutter Developer" } },
            { Industry.LowCodeNoCode, new List<string> { "Low-Code Developer", "Business Process Specialist", "CRM Administrator", "No-Code Solution Architect", "Digital Transformation Specialist" } },
            { Industry.QualityControlAssurance, new List<string> { "QA Engineer", "Test Automation Engineer", "QA Analyst", "Quality Assurance Manager", "Test Lead" } },
            { Industry.MachineLearning, new List<string> { "Machine Learning Engineer", "ML Operations Engineer", "AI Researcher", "Computer Vision Engineer", "ML Platform Engineer" } }
        };

        // Define programming languages for each industry (some industries don't require programming)
        var industryLanguagesMappings = new Dictionary<Industry, List<string>>
        {
            { Industry.WebDevelopment, new List<string> { "JavaScript", "TypeScript", "HTML", "CSS", "PHP", "Python", "Ruby", "Java" } },
            { Industry.DataScience, new List<string> { "Python", "R", "SQL", "Julia", "Scala" } },
            { Industry.CyberSecurity, new List<string> { "Python", "Shell Scripting", "PowerShell", "C", "C++" } },
            { Industry.CloudComputing, new List<string> { "Python", "JavaScript", "TypeScript", "Go", "Powelshell", "Bash" } },
            { Industry.DevOps, new List<string> { "Python", "Go", "Shell Scripting", "PowerShell", "Ruby" } },
            { Industry.GameDevelopment, new List<string> { "C++", "C#", "JavaScript", "Python", "Lua" } },
            { Industry.ItSupport, new List<string> { "Shell Scripting", "PowerShell", "Python", "Batch" } },
            { Industry.ArtificialIntelligence, new List<string> { "Python", "R", "C++", "Java", "Julia" } },
            { Industry.Blockchain, new List<string> { "Solidity", "JavaScript", "Python", "Go", "Rust" } },
            { Industry.Networking, new List<string> { "Python", "Shell Scripting", "PowerShell", "C" } },
            { Industry.UxUiDesign, new List<string> { "JavaScript", "TypeScript", "HTML", "CSS" } },
            { Industry.EmbeddedSystems, new List<string> { "C", "C++", "Python", "Assembly", "Rust" } },
            { Industry.ItConsulting, new List<string> { "SQL", "Python", "JavaScript", "Java", "C#" } },
            { Industry.DatabaseAdministration, new List<string> { "SQL", "Python", "PowerShell", "Shell Scripting" } },
            { Industry.ProjectManagement, new List<string>() }, // Often doesn't require programming
            { Industry.MobileDevelopment, new List<string> { "Swift", "Kotlin", "Java", "Objective-C", "Dart", "JavaScript" } },
            { Industry.LowCodeNoCode, new List<string>() }, // By definition doesn't require programming
            { Industry.QualityControlAssurance, new List<string> { "Python", "JavaScript", "Java", "C#", "Ruby" } },
            { Industry.MachineLearning, new List<string> { "Python", "R", "C++", "Java", "Julia" } }
        };

        // Define skills relevant to each industry
        var industrySkillsMappings = new Dictionary<Industry, List<string>>
        {
            { Industry.WebDevelopment, new List<string> { "React", "Angular", "Vue.js", "Node.js", "RESTful API", "GraphQL", "Web Security", "CI/CD", "Responsive Design", "Webpack", "SASS/LESS", "Git", "Docker" } },
            { Industry.DataScience, new List<string> { "Machine Learning", "Data Visualization", "Statistical Analysis", "Big Data", "Database Design", "Data Mining", "ETL", "Pandas", "NumPy", "Jupyter", "Tableau", "Power BI" } },
            { Industry.CyberSecurity, new List<string> { "Network Security", "Penetration Testing", "SIEM", "Ethical Hacking", "Risk Assessment", "Encryption", "Security Auditing", "Vulnerability Assessment", "Firewall Configuration", "Incident Response" } },
            { Industry.CloudComputing, new List<string> { "AWS", "Azure", "GCP", "Kubernetes", "Docker", "Terraform", "IaaS", "PaaS", "CloudFormation", "Serverless", "Microservices", "Load Balancing" } },
            { Industry.DevOps, new List<string> { "CI/CD", "Docker", "Kubernetes", "Jenkins", "GitLab CI", "GitHub Actions", "Ansible", "Terraform", "Monitoring", "Infrastructure as Code", "Observability", "Site Reliability Engineering" } },
            { Industry.GameDevelopment, new List<string> { "Unity", "Unreal Engine", "3D Modeling", "Animation", "Graphics Programming", "Game Physics", "Multiplayer Networking", "Game Design", "Mobile Gaming", "Console Development" } },
            { Industry.ItSupport, new List<string> { "Troubleshooting", "System Administration", "Network Configuration", "Active Directory", "IT Service Management", "Help Desk", "IT Infrastructure", "ITIL", "User Support" } },
            { Industry.ArtificialIntelligence, new List<string> { "Machine Learning", "Neural Networks", "Deep Learning", "NLP", "Computer Vision", "TensorFlow", "PyTorch", "Reinforcement Learning", "AI Ethics", "Feature Engineering" } },
            { Industry.Blockchain, new List<string> { "Smart Contracts", "Ethereum", "Bitcoin", "Decentralized Applications", "Tokenomics", "Web3", "Cryptography", "Consensus Mechanisms", "NFTs", "DeFi" } },
            { Industry.Networking, new List<string> { "TCP/IP", "Routing", "Switching", "Firewalls", "VPN", "DHCP", "DNS", "Network Security", "Cisco", "Load Balancing", "Network Virtualization" } },
            { Industry.UxUiDesign, new List<string> { "User Research", "Wireframing", "Prototyping", "Usability Testing", "Information Architecture", "Design Systems", "Figma", "Adobe XD", "Sketch", "Accessibility", "Interaction Design" } },
            { Industry.EmbeddedSystems, new List<string> { "Microcontrollers", "RTOS", "Circuit Design", "PCB Design", "IoT", "Firmware Development", "Signal Processing", "Low-Level Programming", "Sensors", "Actuators" } },
            { Industry.ItConsulting, new List<string> { "Business Analysis", "Requirements Gathering", "Solution Architecture", "Project Management", "Digital Transformation", "Change Management", "Strategic Planning", "TOGAF", "Enterprise Architecture" } },
            { Industry.DatabaseAdministration, new List<string> { "SQL", "Database Optimization", "Data Modeling", "Database Security", "ETL", "Backup/Recovery", "High Availability", "Replication", "PostgreSQL", "MySQL", "MongoDB", "Oracle" } },
            { Industry.ProjectManagement, new List<string> { "Agile", "Scrum", "Kanban", "Waterfall", "Risk Management", "Stakeholder Management", "Resource Allocation", "Budgeting", "JIRA", "MS Project", "Communication", "Leadership" } },
            { Industry.MobileDevelopment, new List<string> { "iOS Development", "Android Development", "React Native", "Flutter", "Mobile UI Design", "App Store Optimization", "Push Notifications", "Mobile Security", "Responsive Design", "Cross-platform Development" } },
            { Industry.LowCodeNoCode, new List<string> { "Bubble.io", "Zapier", "Airtable", "Power Apps", "Microsoft Power Automate", "OutSystems", "Mendix", "Business Process Automation", "Workflow Design", "Citizen Development" } },
            { Industry.QualityControlAssurance, new List<string> { "Test Automation", "Selenium", "Cypress", "JUnit", "TestNG", "Manual Testing", "API Testing", "Performance Testing", "Security Testing", "Bug Tracking", "Test Plans", "Quality Metrics" } },
            { Industry.MachineLearning, new List<string> { "TensorFlow", "PyTorch", "Scikit-learn", "Feature Engineering", "Model Optimization", "Data Preprocessing", "MLOps", "Neural Networks", "Computer Vision", "NLP", "Reinforcement Learning" } }
        };

        var userFaker = new Faker<AppUser>()
            .RuleFor(u => u.UserName, f => f.Internet.UserName())
            .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.UserName!))
            .RuleFor(u => u.EmailConfirmed, true)
            .RuleFor(u => u.PhoneNumberConfirmed, true)
            .RuleFor(u => u.ProfileImageUrl, f => f.Internet.Avatar())
            .RuleFor(u => u.IsActive, true)
            .RuleFor(u => u.Country, f => f.Address.Country());

        var learningGoalsList = new List<string>
        {
            "Оволодіти веб-розробкою",
            "Вивчити хмарні обчислення",
            "Стати експертом з машинного навчання",
            "Зрозуміти практики DevOps",
            "Розробляти мобільні додатки",
            "Вивчити кібербезпеку",
            "Зрозуміти технологію блокчейн",
            "Покращити навички програмування",
            "Почати з науки про дані",
            "Вивчити UI/UX дизайн",
            "Розвивати навички програмування ігор",
            "Зрозуміти архітектуру мікросервісів",
            "Почати з розробки IoT",
            "Оволодіти гнучкими методологіями",
            "Вивчити розробку API",
            "Зрозуміти контейнеризацію",
            "Покращити навички роботи з базами даних",
            "Розвивати навички роботи з великими даними",
            "Зрозуміти основи штучного інтелекту",
            "Вивчити основи тестування програмного забезпечення",
            "Розвивати навички роботи з аналітикою даних",
            "Зрозуміти основи розробки програмного забезпечення",
            "Покращити навички роботи з системами контролю версій",
            "Розвивати навички роботи з веб-технологіями",
            "Зрозуміти основи роботи з RESTful API",
            "Вивчити основи роботи з GraphQL",
            "Розвивати навички роботи з мікрофронтендами",
            "Зрозуміти основи роботи з серверless архітектурою",
            "Покращити навички роботи з CI/CD процесами",
            "Розвивати навички роботи з Agile та Scrum",
            "Зрозуміти основи роботи з системами моніторингу та логування",
            "Вивчити основи управління проектами",
            "Розвивати навички роботи з командою",
            "Зрозуміти основи роботи з системами управління контентом (CMS)",
            "Покращити навички роботи з веб-безпекою",
        };

        var skillsList = new List<string>
        {
            "Docker", "Kubernetes", "AWS", "Azure",
            "Машинне навчання", "Аналіз даних", "Кібербезпека", "Блокчейн",
            "UI/UX Дизайн", "Розробка ігор", "Розробка IoT",
            "Гнучкі методології", "Розробка API", "Архітектура мікросервісів",
            "Великі дані", "Штучний інтелект", "Тестування програмного забезпечення",
            "Контроль версій", "Веб-технології", "RESTful API",
            "GraphQL", "Серверless архітектура", "CI/CD процеси",
            "Agile та Scrum", "Системи моніторингу та логування", "Управління проектами",
            "Командна робота", "Системи управління контентом (CMS)", "Веб-безпека"
        };

        var menteeProfileFaker = new Faker<MenteeProfile>()
            .RuleFor(m => m.Bio, f => f.Lorem.Paragraphs(2))
            // Position will be set in the generation loop
            // Company will only be populated for 30% of mentees
            .RuleFor(m => m.Company, f => f.Random.Bool(0.3f) ? f.Company.CompanyName() : string.Empty)
            .RuleFor(m => m.Industries, f =>
            {
                // Pick random industry flags
                var industryValues = Enum.GetValues<Industry>().Where(i => i != Industry.None).ToArray();
                var selectedIndustries = new List<Industry>();

                // Mentees often are interested in 1-2 industries
                for (int i = 0; i < f.Random.Int(1, 2); i++)
                {
                    selectedIndustries.Add(f.PickRandom(industryValues));
                }

                return selectedIndustries.Aggregate(Industry.None, (current, industry) => current | industry);
            });

        // Skills, ProgrammingLanguages, and LearningGoals will be set in the generation loop

        for (var i = 0; i < 50; i++)
        {
            // Create and save the AppUser
            var appUser = userFaker.Generate();
            var password = "String123!";

            var createResult = await userManager.CreateAsync(appUser, password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                logger.LogError("Failed to create mentee user: {Errors}", errors);
                continue;
            }

            // Add to Mentee role
            var roleResult = await userManager.AddToRoleAsync(appUser, Roles.Mentee);
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                logger.LogError("Failed to add mentee role: {Errors}", errors);
                continue;
            }

            // Create and save the MenteeProfile with industry-appropriate data
            var faker = new Faker();
            var menteeProfile = menteeProfileFaker.Generate();
            menteeProfile.MenteeId = appUser.Id;

            // Get the primary industry (just take the first flag that is set)
            var primaryIndustry = Enum.GetValues<Industry>()
                .FirstOrDefault(i => i != Industry.None && menteeProfile.Industries.HasFlag(i));

            if (primaryIndustry != Industry.None)
            {
                // Set industry-specific position - mentees often have more junior positions
                if (industryPositionMappings.TryGetValue(primaryIndustry, out var positions) && positions.Any())
                {
                    // For mentees, modify position titles to make them more junior
                    var position = faker.PickRandom(positions);
                    // 60% chance of being a junior position
                    if (faker.Random.Bool(0.6f))
                    {
                        // Add junior prefixes or modify the title to sound more junior
                        var juniorPrefixes = new[] { "Junior ", "Entry-Level ", "Associate ", "Trainee ", "" };

                        // Make position more junior by replacing senior terms
                        position = position.Replace("Senior ", "");
                        position = position.Replace("Lead ", "");
                        position = position.Replace("Manager", "Specialist");
                        position = position.Replace("Architect", "Developer");

                        // Add junior prefix
                        position = faker.PickRandom(juniorPrefixes) + position;
                    }

                    menteeProfile.Position = position;
                }
                else
                {
                    menteeProfile.Position = faker.Name.JobTitle();
                }

                // Set industry-specific programming languages
                if (industryLanguagesMappings.TryGetValue(primaryIndustry, out var languages) && languages.Any())
                {
                    // Take 1-2 languages from the industry-specific list, or default if empty
                    menteeProfile.ProgrammingLanguages = languages.Any()
                        ? faker.PickRandom(languages, faker.Random.Int(1, Math.Min(2, languages.Count))).ToList()
                        : ProgrammingLanguages.Values.OrderBy(_ => faker.Random.Int()).Take(2).ToList();
                }
                else
                {
                    // Fallback to random languages
                    menteeProfile.ProgrammingLanguages = ProgrammingLanguages.Values.OrderBy(_ => faker.Random.Int()).Take(2).ToList();
                }

                // Set industry-specific skills - mentees have fewer skills than mentors
                if (industrySkillsMappings.TryGetValue(primaryIndustry, out var skills) && skills.Any())
                {
                    // Pick 2-4 skills from the industry-specific list
                    menteeProfile.Skills = faker.PickRandom(skills, faker.Random.Int(2, Math.Min(4, skills.Count))).ToList();
                }
                else
                {
                    // Fallback to random skills from the general list
                    menteeProfile.Skills = faker.PickRandom(skillsList, faker.Random.Int(2, 4)).ToList();
                }

                // Generate learning goals that are related to the industry and skills
                var relevantGoals = new List<string>();

                // Add industry-specific goals
                if (industrySkillsMappings.TryGetValue(primaryIndustry, out var industrySkills) && industrySkills.Any())
                {
                    // Pick 1-3 skills they don't have yet but want to learn
                    var skillsToLearn = industrySkills
                        .Except(menteeProfile.Skills)
                        .OrderBy(_ => faker.Random.Int())
                        .Take(faker.Random.Int(1, 3));

                    foreach (var skill in skillsToLearn)
                    {
                        relevantGoals.Add($"Оволодіти навичкою {skill}");
                    }
                }

                // Add language-specific goals
                if (industryLanguagesMappings.TryGetValue(primaryIndustry, out var industryLanguages) && industryLanguages.Any())
                {
                    // Pick 0-2 languages they don't know yet but want to learn
                    var languagesToLearn = industryLanguages
                        .Except(menteeProfile.ProgrammingLanguages)
                        .OrderBy(_ => faker.Random.Int())
                        .Take(faker.Random.Int(0, 2));

                    foreach (var language in languagesToLearn)
                    {
                        relevantGoals.Add($"Вивчити мову програмування {language}");
                    }
                }

                // Fill in with general goals until we have 2-5 total
                var generalGoalsCount = 5 - relevantGoals.Count;
                if (generalGoalsCount > 0)
                {
                    var generalGoals = faker.PickRandom(learningGoalsList, faker.Random.Int(Math.Min(2, generalGoalsCount), generalGoalsCount));
                    relevantGoals.AddRange(generalGoals);
                }

                menteeProfile.LearningGoals = relevantGoals.ToList();
            }
            else
            {
                // Fallback to random values if no industry is selected
                menteeProfile.Position = faker.Name.JobTitle();
                menteeProfile.Skills = faker.PickRandom(skillsList, faker.Random.Int(2, 4)).ToList();
                menteeProfile.ProgrammingLanguages = ProgrammingLanguages.Values.OrderBy(_ => faker.Random.Int()).Take(2).ToList();
                menteeProfile.LearningGoals = faker.PickRandom(learningGoalsList, faker.Random.Int(2, 5)).ToList();
            }

            usersContext.MenteeProfiles.Add(menteeProfile);
        }

        await usersContext.SaveChangesAsync();
        logger.LogInformation("Successfully seeded {Count} mentees", 50);
    }

    private async Task SeedMentorReviewsAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var ratingsContext = scope.ServiceProvider.GetRequiredService<RatingsDbContext>();
        var usersContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();

        if (await ratingsContext.MentorReviews.AnyAsync())
        {
            logger.LogInformation("Mentor reviews already exist, skipping seeding.");
            return;
        }

        logger.LogInformation("Seeding mentor reviews");

        // Fetch all mentors and mentees from the database
        var mentors = await usersContext.MentorProfiles.ToListAsync();
        var mentees = await usersContext.MenteeProfiles.ToListAsync();

        if (!mentors.Any() || !mentees.Any())
        {
            logger.LogWarning("No mentors or mentees found, skipping review seeding.");
            return;
        }

        var faker = new Faker("uk");
        var reviews = new List<MentorReview>();

        // Positive Ukrainian review templates for higher ratings (4-5)
        var positiveReviewTemplates = new List<string>
        {
            "Чудовий ментор! {0}. Завжди пояснює зрозуміло і доступно. Рекомендую!",
            "Дуже задоволений(а) співпрацею. {0}. Професіонал своєї справи.",
            "Відмінний досвід навчання з цим ментором. {0}. Допоміг мені значно покращити мої навички.",
            "Неймовірно корисні сесії. {0}. Завжди знаходить час відповісти на всі питання.",
            "Надзвичайно компетентний ментор. {0}. Детально пояснює складні концепції.",
            "Завдяки цьому ментору я отримав(ла) нову роботу! {0}. Безмежно вдячний(а).",
            "Один з найкращих менторів, з якими я працював(ла). {0}. Дуже уважний(а) до деталей.",
            "Фантастичний досвід менторства. {0}. Завжди пунктуальний(а) і підготовлений(а).",
            "Дуже цінні поради та зворотній зв'язок. {0}. Допоміг мені зрозуміти складні концепції.",
            "Високий рівень професіоналізму. {0}. Завжди готовий(а) допомогти."
        };

        // Medium Ukrainian review templates for middle rating (3)
        var mediumReviewTemplates = new List<string>
        {
            "Непоганий ментор, але іноді {0}. Загалом корисні сесії.",
            "В цілому задовільно. {0}. Є моменти, які можна покращити.",
            "Середній рівень менторства. {0}. Інколи складно отримати чітку відповідь.",
            "Мені допомогло, але {0}. Можна працювати краще.",
            "Нормальний досвід. {0}. Є і позитивні, і негативні аспекти."
        };

        // Negative Ukrainian review templates for low ratings (1-2)
        var negativeReviewTemplates = new List<string>
        {
            "На жаль, не дуже корисний досвід. {0}. Не рекомендую.",
            "Дуже розчарований(а) менторством. {0}. Витрачений час і гроші.",
            "Не відповідає очікуванням. {0}. Складно комунікувати.",
            "Погано організовані сесії. {0}. Багато часу витрачається даремно.",
            "Низька якість менторства. {0}. Шкода, що обрав(ла) цього ментора."
        };

        // Positive specific details for Ukrainian reviews
        var positiveDetails = new List<string>
        {
            "Завжди дає практичні поради",
            "Має величезний досвід у галузі",
            "Пояснює складні речі простими словами",
            "Дуже терплячий і уважний",
            "Надає цінні матеріали",
            "Завжди доступний для запитань",
            "Дає корисний зворотній зв'язок",
            "Допомагає вирішувати реальні проблеми",
            "Ділиться актуальними знаннями",
            "Вміє мотивувати до навчання"
        };

        // Medium specific details for Ukrainian reviews
        var mediumDetails = new List<string>
        {
            "не завжди вчасно відповідає",
            "іноді пояснення занадто складні",
            "не всі приклади актуальні",
            "деякі матеріали застарілі",
            "інколи сесії затягуються"
        };

        // Negative specific details for Ukrainian reviews
        var negativeDetails = new List<string>
        {
            "часто скасовує сесії в останню мить",
            "дуже поверхневі пояснення",
            "не вистачає структурованості",
            "занадто мало практики",
            "погано підготовлений до сесій"
        };

        // Choose certain percentage of mentees to leave reviews (around 70-80%)
        var reviewingMenteeIds = mentees
            .OrderBy(_ => faker.Random.Int())
            .Take((int)(mentees.Count * 0.8)) // Leave around 20% without reviews
            .Select(m => m.MenteeId)
            .ToList();

        // Each mentee from the selected group will review 1-3 mentors
        foreach (var menteeId in reviewingMenteeIds)
        {
            // How many mentors this mentee will review (1-3)
            var reviewCount = faker.Random.Int(1, 3);

            // Select random mentors for this mentee to review
            var selectedMentors = mentors
                .OrderBy(_ => faker.Random.Int())
                .Take(Math.Min(reviewCount, mentors.Count))
                .ToList();

            foreach (var mentor in selectedMentors)
            {
                // Generate a random rating (1-5)
                var rating = faker.Random.Int(1, 5);

                // Generate review text based on the rating
                string reviewText;
                if (rating >= 4)
                {
                    var detail = faker.PickRandom(positiveDetails);
                    var template = faker.PickRandom(positiveReviewTemplates);
                    reviewText = string.Format(template, detail);
                }
                else if (rating == 3)
                {
                    var detail = faker.PickRandom(mediumDetails);
                    var template = faker.PickRandom(mediumReviewTemplates);
                    reviewText = string.Format(template, detail);
                }
                else
                {
                    var detail = faker.PickRandom(negativeDetails);
                    var template = faker.PickRandom(negativeReviewTemplates);
                    reviewText = string.Format(template, detail);
                }

                // Create the mentor review
                var review = new MentorReview
                {
                    MentorId = mentor.MentorId,
                    MenteeId = menteeId,
                    Rating = rating,
                    ReviewText = reviewText,
                    CreatedAt = faker.Date.Between(DateTime.UtcNow.AddMonths(-6), DateTime.UtcNow)
                };

                reviews.Add(review);
            }
        }        // Add all reviews to the context and save
        ratingsContext.MentorReviews.AddRange(reviews);
        await ratingsContext.SaveChangesAsync();

        logger.LogInformation("Successfully seeded {Count} mentor reviews", reviews.Count);
    }
}
