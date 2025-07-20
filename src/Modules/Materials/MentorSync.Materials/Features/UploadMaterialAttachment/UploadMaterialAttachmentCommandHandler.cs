using Ardalis.Result;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MentorSync.Materials.Data;
using MentorSync.Materials.Domain;
using MentorSync.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Materials.Features.UploadMaterialAttachment;

public sealed class UploadMaterialAttachmentCommandHandler(
    MaterialsDbContext dbContext,
    BlobServiceClient blobServiceClient)
        : ICommandHandler<UploadMaterialAttachmentCommand, UploadAttachmentResponse>
{
    private readonly BlobContainerClient _attachmentsContainer = blobServiceClient.GetBlobContainerClient(ContainerNames.MaterialsAttachments);

    public async Task<Result<UploadAttachmentResponse>> Handle(UploadMaterialAttachmentCommand request, CancellationToken cancellationToken)
    {
        // Validate material exists and belongs to mentor
        var material = await dbContext.LearningMaterials
            .FirstOrDefaultAsync(m => m.Id == request.MaterialId, cancellationToken);

        if (material == null)
        {
            return Result.NotFound($"Material with id {request.MaterialId} not found");
        }

        if (request.MentorId != 0 && material.MentorId != request.MentorId)
        {
            return Result.Forbidden("You do not have permission to upload attachments to this material");
        }

        var blobFileName = $"{Guid.NewGuid()}-{request.File.FileName}";

        await _attachmentsContainer.CreateIfNotExistsAsync(PublicAccessType.Blob, cancellationToken: cancellationToken);

        var blobClient = _attachmentsContainer.GetBlobClient(blobFileName);

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
}
