using Ardalis.Result;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MediatR;
using MentorSync.Materials.Data;
using MentorSync.Materials.Domain;
using MentorSync.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentorSync.Materials.Features.UploadMaterialAttachment;

public class UploadMaterialAttachmentCommandHandler(
    MaterialsDbContext dbContext,
    BlobServiceClient blobServiceClient,
    ILogger<UploadMaterialAttachmentCommandHandler> logger) : IRequestHandler<UploadMaterialAttachmentCommand, Result<UploadAttachmentResponse>>
{
    private readonly BlobContainerClient _attachmentsContainer = blobServiceClient.GetBlobContainerClient(ContainerNames.MaterialsAttachments);

    public async Task<Result<UploadAttachmentResponse>> Handle(UploadMaterialAttachmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate material exists and belongs to mentor
            var material = await dbContext.LearningMaterials
                .FirstOrDefaultAsync(m => m.Id == request.MaterialId, cancellationToken);

            if (material == null)
            {
                return Result.NotFound($"Material with id {request.MaterialId} not found");
            }

            // Verify ownership if MentorId is provided (not 0)
            if (request.MentorId != 0 && material.MentorId != request.MentorId)
            {
                return Result.Forbidden("You do not have permission to upload attachments to this material");
            }

            // Generate a unique blob name
            var blobFileName = $"{Guid.NewGuid()}-{request.File.FileName}";

            // Create the container client
            await _attachmentsContainer.CreateIfNotExistsAsync(PublicAccessType.Blob, cancellationToken: cancellationToken);

            // Create the blob client
            var blobClient = _attachmentsContainer.GetBlobClient(blobFileName);

            // Upload the file
            using (var stream = request.File.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = request.File.ContentType
                    }
                }, cancellationToken);
            }

            // Create attachment record in database
            var attachment = new MaterialAttachment
            {
                MaterialId = request.MaterialId,
                FileName = request.File.FileName,
                BlobUri = blobClient.Uri.ToString(),
                ContentType = request.File.ContentType,
                Size = request.File.Length,
                UploadedAt = DateTime.UtcNow
            };

            dbContext.MaterialAttachments.Add(attachment);
            await dbContext.SaveChangesAsync(cancellationToken);

            var response = new UploadAttachmentResponse
            {
                Id = attachment.Id,
                FileName = attachment.FileName,
                FileUrl = attachment.BlobUri,
                ContentType = attachment.ContentType,
                FileSize = attachment.Size,
                UploadedAt = attachment.UploadedAt
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error uploading attachment for material {MaterialId}", request.MaterialId);
            return Result.Error($"An error occurred while uploading the attachment: {ex.Message}");
        }
    }
}
