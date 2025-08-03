using Bogus;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.CommonEntities.Enums;
using MentorSync.Users.Data;
using MentorSync.Users.Domain.Mentee;
using MentorSync.Users.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.MigrationService.Seeders;

public static class MenteesSeeder
{
	public static async Task SeedAsync(IServiceProvider serviceProvider, ILogger<Worker> logger)
	{
		var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
		var usersContext = serviceProvider.GetRequiredService<UsersDbContext>();

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
			.RuleFor(u => u.Country, f => f.Address.Country()); var learningGoalsList = new List<string>
		{
            // Original goals
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

            // More specific and personalized career goals
            "Отримати першу роботу в IT компанії протягом 6 місяців",
			"Підготуватися до співбесіди в IT компанію",
			"Розробити власне портфоліо проєктів",
			"Пройти сертифікацію з AWS/Azure/Google Cloud",
			"Зрозуміти процес розробки в реальних IT проєктах",
			"Вивчити сучасний фреймворк React/Angular/Vue",
			"Навчитися писати чистий та ефективний код",
			"Розібратися в принципах об'єктно-орієнтованого програмування",
			"Навчитися створювати та оптимізувати запити до баз даних",
			"Зрозуміти основи алгоритмів та структур даних",
			"Підготуватися до технічного інтерв'ю в великих IT компаніях",
			"Розвинути навички комунікації в технічних командах",
			"Вивчити методології розробки програмного забезпечення",
			"Створити власний стартап на основі набутих знань",
			"Навчитися створювати мобільні додатки для iOS/Android",
			"Опанувати інструменти для аналізу великих даних",
			"Навчитися застосовувати машинне навчання для вирішення бізнес-задач",
			"Зрозуміти принципи розробки безпечних веб-додатків",
			"Освоїти інструменти для автоматизації тестування програмного забезпечення",
			"Підготуватися до переходу на роль технічного лідера в майбутньому",
			"Створити власний pet-проєкт для демонстрації вміння",
			"Навчитися ефективно працювати в розподіленій команді",
			"Розвинути навички презентації технічних рішень",
			"Підготуватися до участі у відкритих source-проєктах",
			"Опанувати декілька мов програмування на базовому рівні",
			"Навчитися ефективно використовувати версійний контроль Git",
			"Зрозуміти процеси оптимізації та масштабування веб-додатків",
			"Вивчити інструменти для аналізу та покращення продуктивності додатків",
			"Навчитися розробляти крос-платформені застосунки",
			"Підготуватися до роботи з великими промисловими кодовими базами"
		};

		var skillsList = new List<string>
		{
            // Original skills
            "Docker", "Kubernetes", "AWS", "Azure",
			"Машинне навчання", "Аналіз даних", "Кібербезпека", "Блокчейн",
			"UI/UX Дизайн", "Розробка ігор", "Розробка IoT",
			"Гнучкі методології", "Розробка API", "Архітектура мікросервісів",
			"Великі дані", "Штучний інтелект", "Тестування програмного забезпечення",
			"Контроль версій", "Веб-технології", "RESTful API",
			"GraphQL", "Серверless архітектура", "CI/CD процеси",
			"Agile та Scrum", "Системи моніторингу та логування", "Управління проектами",
			"Командна робота", "Системи управління контентом (CMS)", "Веб-безпека",

            // Additional modern technologies and skills
            "React", "Angular", "Vue.js", "Node.js", "TypeScript",
			"Django", "Flask", "Ruby on Rails", "Spring Boot",
			"PostgreSQL", "MongoDB", "Redis", "MySQL", "SQLite",
			"TensorFlow", "PyTorch", "Keras", "scikit-learn",
			"Git", "GitHub", "GitLab", "Bitbucket", "Jira",
			"Figma", "Sketch", "Adobe XD", "Photoshop", "Illustrator",
			"Аналіз вимог", "Технічна документація", "Code review",
			"Continuous Integration", "Continuous Delivery",
			"Функціональне програмування", "Об'єктно-орієнтоване програмування",
			"Автоматизоване тестування", "Юніт-тестування", "Інтеграційне тестування",
			"Розгортання застосунків", "Налагодження помилок", "Рефакторинг коду",
			"Оптимізація продуктивності",
			"Progressive Web Apps", "Розробка мобільних додатків",
			"Кросбраузерна сумісність", "Адаптивний дизайн", "Семантична верстка"
		};// Define meaningful Ukrainian bio templates for mentees
		var menteeBioTemplates = new[]
		{
			"Зацікавлений в {0} та прагну розвиватись у сфері {1}. {2} Маю базові знання в {3} та хочу вдосконалитись у {4}. Шукаю ментора, який допоможе мені досягти мети: {5}. Відкритий до нових знань та готовий інвестувати час у навчання.",

			"Навчаюсь у сфері {1} з фокусом на {0}. {2} Вже маю деякі навички в {3} і хочу глибше розібратися в {4}. Моя головна ціль - {5}. Ціную практичні поради та чіткий зворотній зв'язок від ментора.",

			"Моя професійна мета - стати спеціалістом у {1}, особливо в області {0}. {2} Працюю над покращенням навичок у {3} та {4}. Шукаю регулярну менторську підтримку для досягнення мети: {5}.",

			"Почав кар'єрний шлях у {1} з акцентом на {0}. {2} Вже знайомий з основами {3} та {4}, але потребую структурованого підходу до навчання. Хочу досягти: {5}. Готовий виконувати домашні завдання та активно розвиватись.",

			"Прагну розвиватись як фахівець у галузі {1}, з фокусом на {0}. {2} Маю початковий досвід роботи з {3} і {4}. Моя ціль на найближчі пів року: {5}. Шукаю ментора, який допоможе прокласти чіткий шлях професійного зростання.",

			"Змінюю професійний напрямок на {1}, зокрема цікавить {0}. {2} Вже вивчаю самостійно {3} та {4}. Потребую допомоги ментора для ефективного досягнення мети: {5}. Відкритий до критики та порад, готовий наполегливо працювати.",

			"Студент, який вивчає {1} з особливим інтересом до {0}. {2} Маю практику в {3} та теоретичні знання в {4}. Шукаю ментора для переходу від теорії до практики та досягнення мети: {5}.",

			"Молодий спеціаліст у галузі {1}, зосереджений на вивченні {0}. {2} Вже працював з {3} та знайомий з {4}. Потребую системного підходу до навчання для досягнення: {5}. Ціную чіткі пояснення та приклади з реального життя.",

			"Займаюсь самоосвітою в напрямку {1}, особливо цікавить {0}. {2} Маю базові навички в {3} та експериментую з {4}. Головна ціль мого навчання: {5}. Шукаю ментора, який допоможе уникнути типових помилок початківця.",

			"Планую кар'єрний перехід у сферу {1} з фокусом на {0}. {2} Вже маю досвід роботи з {3} та {4} в іншому контексті. Моя конкретна мета: {5}. Готовий до інтенсивного навчання під керівництвом досвідченого ментора."
		};

		// Define statements about mentee background
		var menteeBackgrounds = new[]
		{
			"Закінчив курси на освітній платформі та прагну застосувати отримані знання на практиці.",
			"Маю освіту в суміжній галузі, що дає мені гарне розуміння базових концепцій.",
			"Зараз навчаюсь в університеті та хочу паралельно розвивати практичні навички.",
			"Працюю в іншій сфері, але зацікавився IT і вивчаю нові технології у вільний час.",
			"Пройшов декілька онлайн-курсів та створив кілька невеликих навчальних проєктів.",
			"Нещодавно брав участь у хакатоні, що посилило моє бажання розвиватись у цьому напрямку.",
			"Маю базову технічну освіту та шукаю можливості для практичного застосування знань.",
			"Вже півроку самостійно вивчаю технології через відкриті освітні ресурси.",
			"Працюю над власним проєктом і хочу покращити свої технічні навички.",
			"Навчаюсь на курсах перекваліфікації та прагну знайти свій шлях в IT.",
			"Маю друзів у цій галузі, які надихнули мене почати вивчати ці технології.",
			"Читаю профільну літературу та проходжу практичні завдання на навчальних платформах.",
			"Беру участь у навчальній програмі від місцевої IT-компанії.",
			"Нещодавно приєднався до онлайн-спільноти розробників, де активно навчаюсь та спілкуюсь.",
			"Вирішив змінити напрямок кар'єри після декількох років роботи в іншій сфері."
		};

		var menteeProfileFaker = new Faker<MenteeProfile>()
			.RuleFor(m => m.Bio, (f, _) =>
			{
				// Get focused area and industry
				var industryValues = Enum.GetValues<Industry>().Where(i => i != Industry.None).ToList();
				var randomIndustry = f.PickRandom(industryValues);
				var industry = randomIndustry.ToString();
				var focusArea = f.Random.ArrayElement(skillsList.ToArray());

				// Get background
				var background = f.Random.ArrayElement(menteeBackgrounds);

				// Get skills
				var skill1 = f.Random.ArrayElement(skillsList.ToArray());
				var skill2 = f.Random.ArrayElement(skillsList.Where(s => !string.Equals(s, skill1, StringComparison.OrdinalIgnoreCase)).ToArray());

				// Get learning goal
				var learningGoal = f.Random.ArrayElement(learningGoalsList.ToArray());

				// Format the template with values
				return string.Format(f.Random.ArrayElement(menteeBioTemplates),
					focusArea, industry, background, skill1, skill2, learningGoal);
			})
			// Position will be set in the generation loop
			// Company will only be populated for 30% of mentees
			.RuleFor(m => m.Company, f => f.Random.Bool(0.3f) ? f.Company.CompanyName() : string.Empty)
			.RuleFor(m => m.Industries, f =>
			{
				// Pick random industry flags
				var industryValues = Enum.GetValues<Industry>().Where(i => i != Industry.None).ToArray();
				var selectedIndustries = new List<Industry>();

				// Mentees often are interested in 1-2 industries
				for (var i = 0; i < f.Random.Int(1, 2); i++)
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
				if (industryPositionMappings.TryGetValue(primaryIndustry, out var positions) && positions.Count != 0)
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
				if (industryLanguagesMappings.TryGetValue(primaryIndustry, out var languages) && languages.Count != 0)
				{
					// Take 1-2 languages from the industry-specific list, or default if empty
					menteeProfile.ProgrammingLanguages = languages.Count != 0
						? [.. faker.PickRandom(languages, faker.Random.Int(1, Math.Min(2, languages.Count)))]
						: [.. ProgrammingLanguages.Values.OrderBy(_ => faker.Random.Int()).Take(2)];
				}
				else
				{
					// Fallback to random languages
					menteeProfile.ProgrammingLanguages = [.. ProgrammingLanguages.Values.OrderBy(_ => faker.Random.Int()).Take(2)];
				}

				// Set industry-specific skills - mentees have fewer skills than mentors
				if (industrySkillsMappings.TryGetValue(primaryIndustry, out var skills) && skills.Count != 0)
				{
					// Pick 2-4 skills from the industry-specific list
					menteeProfile.Skills = [.. faker.PickRandom(skills, faker.Random.Int(2, Math.Min(4, skills.Count)))];
				}
				else
				{
					// Fallback to random skills from the general list
					menteeProfile.Skills = [.. faker.PickRandom(skillsList, faker.Random.Int(2, 4))];
				}

				// Generate learning goals that are related to the industry and skills
				var relevantGoals = new List<string>();

				// Add industry-specific goals
				if (industrySkillsMappings.TryGetValue(primaryIndustry, out var industrySkills) && industrySkills.Count != 0)
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
				if (industryLanguagesMappings.TryGetValue(primaryIndustry, out var industryLanguages) && industryLanguages.Count != 0)
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

				menteeProfile.LearningGoals = [.. relevantGoals];
			}
			else
			{
				// Fallback to random values if no industry is selected
				menteeProfile.Position = faker.Name.JobTitle();
				menteeProfile.Skills = [.. faker.PickRandom(skillsList, faker.Random.Int(2, 4))];
				menteeProfile.ProgrammingLanguages = [.. ProgrammingLanguages.Values.OrderBy(_ => faker.Random.Int()).Take(2)];
				menteeProfile.LearningGoals = [.. faker.PickRandom(learningGoalsList, faker.Random.Int(2, 5))];
			}

			usersContext.MenteeProfiles.Add(menteeProfile);
		}

		await usersContext.SaveChangesAsync();
		logger.LogInformation("Successfully seeded {Count} mentees", 50);
	}
}
