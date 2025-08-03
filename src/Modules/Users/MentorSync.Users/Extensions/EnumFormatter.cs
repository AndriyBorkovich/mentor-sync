using MentorSync.Users.Domain.Enums;

namespace MentorSync.Users.Extensions;

public static class EnumFormatter
{
	public static string ToReadableString(this Availability availability)
	{
		if (availability == Availability.None)
		{
			return "Не доступний";
		}

		var availabilityParts = new List<string>();

		if ((availability & Availability.Morning) == Availability.Morning)
		{
			availabilityParts.Add("Ранок");
		}

		if ((availability & Availability.Afternoon) == Availability.Afternoon)
		{
			availabilityParts.Add("День");
		}

		if ((availability & Availability.Evening) == Availability.Evening)
		{
			availabilityParts.Add("Вечір");
		}

		if ((availability & Availability.Night) == Availability.Night)
		{
			availabilityParts.Add("Ніч");
		}

		return availabilityParts.Count > 0
			? string.Join(", ", availabilityParts)
			: "Не доступний";
	}
}
