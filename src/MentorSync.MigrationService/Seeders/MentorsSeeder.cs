using Bogus;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.CommonEntities;
using MentorSync.Users.Data;
using MentorSync.Users.Domain.Enums;
using MentorSync.Users.Domain.Mentor;
using MentorSync.Users.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.MigrationService.Seeders;
public static class MentorsSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider, ILogger<Worker> logger)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        var usersContext = serviceProvider.GetRequiredService<UsersDbContext>();

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
                for (var i = 0; i < f.Random.Int(1, 2); i++)
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
                if (industryPositionMappings.TryGetValue(primaryIndustry, out var positions) && positions.Count != 0)
                {
                    mentorProfile.Position = faker.PickRandom(positions);
                }

                // Set industry-specific programming languages
                if (industryLanguagesMappings.TryGetValue(primaryIndustry, out var languages) && languages.Count != 0)
                {
                    // Take 1-3 languages from the industry-specific list, or default if empty
                    mentorProfile.ProgrammingLanguages = languages.Count != 0
                        ? [.. faker.PickRandom(languages, faker.Random.Int(1, Math.Min(3, languages.Count)))]
                        : [.. ProgrammingLanguages.Values.OrderBy(_ => faker.Random.Int()).Take(2)];
                }
                else
                {
                    // Fallback to random languages
                    mentorProfile.ProgrammingLanguages = [.. ProgrammingLanguages.Values.OrderBy(_ => faker.Random.Int()).Take(2)];
                }

                // Set industry-specific skills
                if (industrySkillsMappings.TryGetValue(primaryIndustry, out var skills) && skills.Count != 0)
                {
                    // Pick 3-5 skills from the industry-specific list
                    mentorProfile.Skills = [.. faker.PickRandom(skills, faker.Random.Int(3, Math.Min(5, skills.Count)))];
                }
                else
                {
                    // Fallback to random skills from the general list
                    mentorProfile.Skills = [.. faker.PickRandom(skillsList, faker.Random.Int(3, 5))];
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
}
