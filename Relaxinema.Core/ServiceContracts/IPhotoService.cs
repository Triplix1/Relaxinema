using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Relaxinema.Core.ServiceContracts;

public interface IPhotoService
{
    Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
    Task<DeletionResult> DeletePhotoAsync(string publicId);
}