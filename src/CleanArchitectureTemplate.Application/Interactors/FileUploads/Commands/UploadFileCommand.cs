using Microsoft.AspNetCore.Http;
using CleanArchitectureTemplate.Application.Common.FileStorageServices;
using CleanArchitectureTemplate.Application.Common.FileStorageServices.Outputs;
using CleanArchitectureTemplate.SharedKernel.Contracts;
using CleanArchitectureTemplate.SharedKernel.Types;

namespace CleanArchitectureTemplate.Application.Interactors.FileUploads.Commands
{
	public class UploadFileCommand : IInteractorBase<FileStorageOutput>
	{
		public IFormFile File { get; set; }

		public string FileCategory { get; set; }
	}

	public class UploadFileHandler : IInteractorHandlerBase<UploadFileCommand, FileStorageOutput>
	{
		private readonly IFileStorageService fileStorageService;

		public UploadFileHandler(IFileStorageService fileStorageService)
		{
			this.fileStorageService = fileStorageService;
		}

		public async Task<ResponseBase<FileStorageOutput>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
		{
			var response = await fileStorageService.UploadAsync(request.File, request.FileCategory);
			return new ResponseBase<FileStorageOutput>(response);
		}
	}
}
