using Microsoft.AspNetCore.Http;
using CleanArchitectureTemplate.Application.Common.FileStorageServices.Outputs;

namespace CleanArchitectureTemplate.Application.Common.FileStorageServices
{
	public interface IFileStorageService
	{
        Uri GetBaseUri();
        Task<FileStorageOutput> UploadAsync(IFormFile file, string containerName);
		Task<FileStorageOutput> UploadAsync(Stream stream, string fileName, string containerName);

    }
}
