using System.Globalization;
using Ardalis.Result;
using Azure.Storage.Blobs;
using MentorSync.Users.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MentorSync.Users.Features.DeleteAvatar;

/// <summary>
/// Handler for deleting a user's avatar
/// </summary>
/// <param name="blobServiceClient">Azure blob service</param>
/// <param name="userManager">Identity manager</param>
/// <param name="logger">Logger</param>
public sealed class DeleteAvatarCommandHandler(
	BlobServiceClient blobServiceClient,
	UserManager<AppUser> userManager,
	ILogger<DeleteAvatarCommandHandler> logger)
		: ICommandHandler<DeleteAvatarCommand, string>
{
	private readonly BlobContainerClient _avatarsContainer = blobServiceClient.GetBlobContainerClient(ContainerNames.Avatars);

	/// <inheritdoc />
	public async Task<Result<string>> Handle(DeleteAvatarCommand request, CancellationToken cancellationToken = default)
	{
		var user = await userManager.FindByIdAsync(request.UserId.ToString(CultureInfo.InvariantCulture));
		if (user is null)
		{
			return Result.NotFound($"User {request.UserId} not found.");
		}

		var blobName = $"{request.UserId}/avatar.jpg";
		var blobClient = _avatarsContainer.GetBlobClient(blobName);

		try
		{
			await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
			user.ProfileImageUrl = null;
			await userManager.UpdateAsync(user);
			return Result.Success("Avatar deleted");
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error deleting avatar for user {UserId}", request.UserId);
			return Result.Error("An error occurred while deleting the avatar.");
		}
	}
}
