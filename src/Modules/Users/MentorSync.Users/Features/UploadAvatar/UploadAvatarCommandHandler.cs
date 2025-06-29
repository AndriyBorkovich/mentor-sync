﻿using Ardalis.Result;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MediatR;
using MentorSync.SharedKernel;
using MentorSync.Users.Domain.User;
using MentorSync.Users.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MentorSync.Users.Features.UploadAvatar;

public sealed class UploadAvatarCommandHandler(
    BlobServiceClient blobServiceClient,
    UserManager<AppUser> userManager,
    ILogger<UploadAvatarCommandHandler> logger) : IRequestHandler<UploadAvatarCommand, Result<string>>
{
    private readonly BlobContainerClient _avatarsContainer = blobServiceClient.GetBlobContainerClient(ContainerNames.Avatars);

    public async Task<Result<string>> Handle(UploadAvatarCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
            return Result.NotFound($"User {request.UserId} not found.");

        var blobName = $"{request.UserId}/avatar.jpg";
        _avatarsContainer.CreateIfNotExists(PublicAccessType.BlobContainer, cancellationToken: cancellationToken);
        var blobClient = _avatarsContainer.GetBlobClient(blobName);

        try
        {
            await using var stream = request.File.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true, cancellationToken);

            var uri = blobClient.Uri.ToString();
            user.ProfileImageUrl = uri;

            var updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return Result.Error($"Failed to update user profile: {updateResult.GetErrorMessage()}");
            }

            return Result.Success($"User avatar uploaded: {uri}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error uploading avatar for user {UserId}", request.UserId);
            return Result.Error("An error occurred while uploading the avatar.");
        }
    }
}
