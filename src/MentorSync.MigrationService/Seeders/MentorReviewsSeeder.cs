using Bogus;
using MentorSync.Ratings.Data;
using MentorSync.Ratings.Domain;
using MentorSync.Users.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.MigrationService.Seeders;

public static class MentorReviewsSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider, ILogger<Worker> logger)
    {
        var ratingsContext = serviceProvider.GetRequiredService<RatingsDbContext>();
        var usersContext = serviceProvider.GetRequiredService<UsersDbContext>();

        if (await ratingsContext.MentorReviews.AnyAsync())
        {
            logger.LogInformation("Mentor reviews already exist, skipping seeding.");
            return;
        }

        logger.LogInformation("Seeding mentor reviews");

        // Fetch all mentors and mentees from the database
        var mentors = await usersContext.MentorProfiles.ToListAsync();
        var mentees = await usersContext.MenteeProfiles.ToListAsync();

        if (mentors.Count == 0 || mentees.Count == 0)
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
