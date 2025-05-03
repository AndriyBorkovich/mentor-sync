using Ardalis.Result;
using MediatR;
using MentorSync.Users.Domain.User;
using MentorSync.Users.Extensions;
using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Features.Bio.Add;

public sealed class AddBioCommandHandler(UserManager<AppUser> userManager)
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

        return !updateResult.Succeeded ?
            Result.Error(updateResult.GetErrorMessage())
            : Result.Success("Bio added successfully");
    }
}
