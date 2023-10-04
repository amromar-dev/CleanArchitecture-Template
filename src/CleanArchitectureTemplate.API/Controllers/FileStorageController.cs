using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.API.Common;
using CleanArchitectureTemplate.Application.Common.FileStorageServices.Outputs;
using CleanArchitectureTemplate.SharedKernel.Contracts;
using CleanArchitectureTemplate.SharedKernel.Types;
using CleanArchitectureTemplate.Application.Interactors.FileUploads.Commands;

namespace CleanArchitectureTemplate.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
	[ApiController]
	public class FileStorageController : CustomControllerBase
	{
		private readonly IInteractorExecution interactorExecution;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="interactorExecution"></param>
		public FileStorageController(IInteractorExecution interactorExecution)
		{
			this.interactorExecution = interactorExecution;
		}

		/// <summary>
		/// upload file. {FileCategory} refer to the categories of the uploaded file like (Profile , ... etc)
		/// </summary>
		/// <returns></returns>
		[HttpPost("Upload")]
		[Authorize]
		public async Task<ActionResult<ResponseBase<FileStorageOutput>>> Upload([FromForm] UploadFileCommand command)
		{
			var result = await interactorExecution.ExecuteAsync(command);
			return CustomResponse(result);
		}
	}
}
