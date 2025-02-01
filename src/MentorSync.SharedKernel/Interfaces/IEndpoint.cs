using Microsoft.AspNetCore.Routing;

namespace MentorSync.SharedKernel.Interfaces;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}