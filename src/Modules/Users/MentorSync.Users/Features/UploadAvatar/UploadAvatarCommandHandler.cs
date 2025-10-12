using System.Globalization;
using Ardalis.Result;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MentorSync.Users.Domain.User;
using MentorSync.Users.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MentorSync.Users.Features.UploadAvatar;

/// <summary>
/// Handler for uploading a user's avatar
/// </summary>
/// <param name="blobServiceClient">Azure blob service client</param>
/// <param name="userManager">User manager</param>
/// <param name="logger">Logger</param>
public sealed class UploadAvatarCommandHandler(
	BlobServiceClient blobServiceClient,
	UserManager<AppUser> userManager,
	ILogger<UploadAvatarCommandHandler> logger) : ICommandHandler<UploadAvatarCommand, string>
{
	private readonly BlobContainerClient _avatarsContainer = blobServiceClient.GetBlobContainerClient(ContainerNames.Avatars);

	/// <inheritdoc />
	public async Task<Result<string>> Handle(UploadAvatarCommand request, CancellationToken cancellationToken = default)
	{
		var user = await userManager.FindByIdAsync(request.UserId.ToString(CultureInfo.InvariantCulture));
		if (user is null)
		{
			return Result.NotFound($"User {request.UserId} not found.");
		}

		var blobName = $"{request.UserId}/avatar.jpg";
		await _avatarsContainer.CreateIfNotExistsAsync(PublicAccessType.BlobContainer, cancellationToken: cancellationToken);
		var blobClient = _avatarsContainer.GetBlobClient(blobName);

		try
		{
			await using var stream = request.File.OpenReadStream();
			await blobClient.UploadAsync(stream, overwrite: true, cancellationToken);

			var uri = blobClient.Uri.ToString();
			user.ProfileImageUrl = uri;

			var updateResult = await userManager.UpdateAsync(user);
			return !updateResult.Succeeded ? Result.Error($"Failed to update user profile: {updateResult.GetErrorMessage()}") : Result.Success($"User avatar uploaded: {uri}");
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error uploading avatar for user {UserId}", request.UserId);
			return Result.Error("An error occurred while uploading the avatar.");
		}
	}
}
