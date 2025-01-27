using Microsoft.Extensions.DependencyInjection;

namespace MentorSync.Users;

public static class ModuleRegistration
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services)
    {
        return services;
    }
}