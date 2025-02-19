using Ardalis.Result;
using MediatR;
using MentorSync.Users.MappingExtensions;
using MentorSync.Users.Services;

namespace MentorSync.Users.Features.Bio.Search;

public record SearchUsersByBioRequest(string Query) : IRequest<Result<List<SearchUserByBioResponse>>>;
public record SearchUserByBioResponse(int UserId, string Bio);

public class SearchUsersByBioHandler(IElasticSearchService elasticsearchService)
    : IRequestHandler<SearchUsersByBioRequest, Result<List<SearchUserByBioResponse>>>
{
    public async Task<Result<List<SearchUserByBioResponse>>> Handle(SearchUsersByBioRequest request, CancellationToken cancellationToken)
    {
        var users = await elasticsearchService.SearchUsersByBioAsync(request.Query);

        return Result.Success(users.Select(u => u.ToSearchByBioResponse()).ToList());
    }
}
