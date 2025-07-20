using Microsoft.AspNetCore.Routing;

namespace MentorSync.SharedKernel.Abstractions.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
