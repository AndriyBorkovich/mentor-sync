using Microsoft.AspNetCore.Routing;

namespace MentorSync.SharedKernel.Abstractions.Endpoints;

/// <summary>
/// Interface for minimal API endpoints that can be mapped to routes
/// </summary>
public interface IEndpoint
{
	/// <summary>
	/// Maps the endpoint to the specified route builder
	/// </summary>
	/// <param name="app">The endpoint route builder to map to</param>
	void MapEndpoint(IEndpointRouteBuilder app);
}
