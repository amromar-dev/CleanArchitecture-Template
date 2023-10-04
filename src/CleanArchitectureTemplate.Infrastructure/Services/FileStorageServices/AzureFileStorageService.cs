using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using CleanArchitectureTemplate.Application.Common.FileStorageServices;
using CleanArchitectureTemplate.Application.Common.FileStorageServices.Outputs;
using CleanArchitectureTemplate.SharedKernel.Types;

namespace CleanArchitectureTemplate.Infrastructure.Services.FileStorageServices
{
	public class AzureFileStorageService : IFileStorageService
	{
		private readonly BlobServiceClient blobServiceClient;
		public AzureFileStorageService(IConfiguration configuration)
		{
			this.blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("AzureConnectionString"));
        }

		public async Task<FileStorageOutput> UploadAsync(IFormFile file, string containerName)
        {
            var extension = Path.GetExtension(file.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";

            using Stream stream = file.OpenReadStream();

            return await Upload(containerName, uniqueFileName, stream);
        }

        public async Task<FileStorageOutput> UploadAsync(Stream stream, string fileName, string containerName)
        {
            return await Upload(containerName, fileName, stream);
        }

        public Uri GetBaseUri()
        {
            return this.blobServiceClient.Uri;
        }
        #region Private Methods
      
        private async Task<FileStorageOutput> Upload(string containerName, string uniqueFileName, Stream stream)
        {
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName.ToLower());
            await containerClient.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);

            BlobClient blobClient = containerClient.GetBlobClient(uniqueFileName);

            var response = await blobClient.UploadAsync(stream, true);

            bool success = response.GetRawResponse().Status >= 200 && response.GetRawResponse().Status < 300;
            if (success == false)
                throw new BusinessException("Error while uploading");

			return new FileStorageOutput()
			{
				Name = blobClient.Name,
				Uri = blobClient.Uri.AbsoluteUri,
				AbsolutePath = blobClient.Uri.AbsolutePath,
            };
		}

        #endregion
    }
}