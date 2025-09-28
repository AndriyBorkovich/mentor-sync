using MentorSync.Scheduling.Data;
using MentorSync.Scheduling.Domain;
using MentorSync.Users.Data;
using MentorSync.Users.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.MigrationService.Seeders;

/// <summary>
/// Seeds mentor availability slots based on mentor profiles
/// </summary>
public static class MentorAvailabilitySeeder
{
	public static async Task SeedAsync(IServiceProvider serviceProvider, ILogger<Worker> logger)
	{
		var schedulingContext = serviceProvider.GetRequiredService<SchedulingDbContext>();
		var usersContext = serviceProvider.GetRequiredService<UsersDbContext>();

		// Check if availability slots already exist
		if (await schedulingContext.MentorAvailabilities.AnyAsync())
		{
			logger.LogInformation("Mentor availability slots already exist, skipping seeding.");
			return;
		}

		logger.LogInformation("Seeding mentor availability data");

		// Get all mentors from users context
		var mentors = await usersContext.MentorProfiles.ToListAsync();

		if (mentors.Count == 0)
		{
			logger.LogWarning("No mentors found in the database, skipping mentor availability seeding");
			return;
		}

		logger.LogInformation("Found {Count} mentors to seed availability for", mentors.Count);

		var utcNow = DateTime.UtcNow.Date;
		// Set seed date to start from tomorrow
		var seedStartDate = utcNow.AddDays(1);

		var availabilitySlots = new List<MentorAvailability>();
		var random = new Random();

		// Generate availability slots for each mentor
		foreach (var mentor in mentors)
		{
			// Get mentor's preferred availability times
			var preferredTimes = GetTimeRangesFromAvailability(mentor.Availability);

			if (preferredTimes.Count == 0)
			{
				// If no preferred times, set defaults - let's assume Morning + Evening
				preferredTimes = GetTimeRangesFromAvailability(Availability.Morning | Availability.Evening);
			}

			// Generate slots for next 2 months (approximately 60 days)
			for (var day = 0; day < 60; day++)
			{
				var currentDate = seedStartDate.AddDays(day);

				// Skip weekends (Saturday = 6, Sunday = 0)
				if (currentDate.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
				{
					continue;
				}

				// For each preferred time range, generate a slot with 50% probability (to make it more realistic)
				foreach (var (startHour, endHour) in preferredTimes)
				{
					// 50% probability to skip this time slot
					if (random.Next(2) == 0)
					{
						continue;
					}

					// Add at least 2 non-colliding slots for each mentor in different time ranges
					var numSlots = preferredTimes.Count == 1 ? 2 : 1; // Ensure at least 2 slots for mentors with just one time range

					for (var slotNum = 0; slotNum < numSlots; slotNum++)
					{
						var slotStart = new DateTimeOffset(currentDate.Year, currentDate.Month, currentDate.Day,
							startHour, 0, 0, TimeSpan.Zero);

						// Make slots 1-2 hours long
						var durationHours = random.Next(1, 3);

						// Ensure we don't exceed the end hour
						var actualDuration = Math.Min(durationHours, endHour - startHour);

						// If we have multiple slots in the same time range, stagger them
						if (slotNum > 0)
						{
							slotStart = slotStart.AddHours(actualDuration + 0.5); // Add 30 minutes buffer

							// Check if we'll exceed the time range
							if (slotStart.Hour + actualDuration > endHour)
							{
								continue; // Skip this additional slot if it would exceed the time range
							}
						}

						var slotEnd = slotStart.AddHours(actualDuration);

						availabilitySlots.Add(new MentorAvailability
						{
							MentorId = mentor.MentorId,
							Start = slotStart,
							End = slotEnd
						});
					}
				}
			}
		}

		// Save availability slots to database
		await schedulingContext.MentorAvailabilities.AddRangeAsync(availabilitySlots);
		await schedulingContext.SaveChangesAsync();

		logger.LogInformation("Successfully seeded {Count} mentor availability slots", availabilitySlots.Count);
	}

	/// <summary>
	/// Converts Availability flags to time ranges (start hour, end hour)
	/// </summary>
	private static List<(int StartHour, int EndHour)> GetTimeRangesFromAvailability(Availability availability)
	{
		var ranges = new List<(int StartHour, int EndHour)>();

		if (availability.HasFlag(Availability.Morning))
		{
			ranges.Add((8, 12)); // 8 AM - 12 PM
		}

		if (availability.HasFlag(Availability.Afternoon))
		{
			ranges.Add((12, 17)); // 12 PM - 5 PM
		}

		if (availability.HasFlag(Availability.Evening))
		{
			ranges.Add((17, 21)); // 5 PM - 9 PM
		}

		if (availability.HasFlag(Availability.Night))
		{
			ranges.Add((21, 23)); // 9 PM - 11 PM
		}

		return ranges;
	}
}
