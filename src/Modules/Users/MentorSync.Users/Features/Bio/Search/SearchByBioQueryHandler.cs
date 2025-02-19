using Ardalis.Result;
using MediatR;
using MentorSync.Users.Domain;
using MentorSync.Users.Services;

namespace MentorSync.Users.Features.Bio.Search;

public record SearchUsersByBioRequest(string Query) : IRequest<Result<IEnumerable<AppUser>>>;

public class SearchUsersByBioHandler(IElasticSearchService elasticsearchService)
    : IRequestHandler<SearchUsersByBioRequest, Result<IEnumerable<AppUser>>>
{
    public async Task<Result<IEnumerable<AppUser>>> Handle(SearchUsersByBioRequest request, CancellationToken cancellationToken)
    {
        var users = await elasticsearchService.SearchUsersByBioAsync(request.Query);

        return Result.Success(users);
    }
}
