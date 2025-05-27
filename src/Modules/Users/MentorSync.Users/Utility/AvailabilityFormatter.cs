using System.Collections.Generic;
using System.Text;
using MentorSync.Users.Domain.Enums;

namespace MentorSync.Users.Utility;

public static class AvailabilityFormatter
{
    public static string ToReadableString(Availability availability)
    {
        if (availability == Availability.None)
            return "Not available";

        var availabilityParts = new List<string>();

        if ((availability & Availability.Morning) == Availability.Morning)
            availabilityParts.Add("Morning");

        if ((availability & Availability.Afternoon) == Availability.Afternoon)
            availabilityParts.Add("Afternoon");

        if ((availability & Availability.Evening) == Availability.Evening)
            availabilityParts.Add("Evening");

        if ((availability & Availability.Night) == Availability.Night)
            availabilityParts.Add("Night");

        return availabilityParts.Count > 0
            ? string.Join(", ", availabilityParts)
            : "Not available";
    }
}
