using Ardalis.Result;
using MediatR;
using MentorSync.Users.Domain;
using MentorSync.Users.Services;
using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Features.Bio.Add;

public sealed class AddBioCommandHandler(UserManager<AppUser> userManager, IElasticSearchService elasticSearchService)
    : IRequestHandler<AddBioRequest, Result<string>>
{
    public async Task<Result<string>> Handle(AddBioRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return Result.NotFound("User not found");
        }

        user.Bio = request.Bio;
        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            return Result.Error(string.Join(',', updateResult.Errors.Select(e => e.Description)));
        }

        await elasticSearchService.IndexUserAsync(user);

        return Result.Success("Bio added and user indexed successfully");
    }
}
