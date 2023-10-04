using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.API.Common;
using CleanArchitectureTemplate.Application.Common.EmailTemplates;
using CleanArchitectureTemplate.Application.Common.Encryptions;
using CleanArchitectureTemplate.Application.Interactors.Account.Commands;
using CleanArchitectureTemplate.Application.Interactors.Account.Outputs;
using CleanArchitectureTemplate.Application.Interactors.Account.Queries;
using CleanArchitectureTemplate.SharedKernel.Contracts;
using CleanArchitectureTemplate.SharedKernel.Types;

namespace CleanArchitectureTemplate.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
	[ApiController]
	public class AccountController : CustomControllerBase
	{
		private readonly IInteractorExecution interactorExecution;
		private readonly IEmailTemplateHelper emailTemplateHelper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="interactorExecution"></param>
        /// <param name="emailTemplateHelper"></param>
        public AccountController(IInteractorExecution interactorExecution, IEmailTemplateHelper emailTemplateHelper)
        {
            this.interactorExecution = interactorExecution;
            this.emailTemplateHelper = emailTemplateHelper;
        }

        /// <summary>
        /// to send authentication code if the user exist otherwise will redirect the user to registration flow. 
        /// </summary>
        /// <returns></returns>
        [HttpPost("SendAuthenticationCode")]
		public async Task<ActionResult<ResponseBase<SendAuthenticationCodeOutput>>> Authenticate(SendAuthenticationCodeCommand command)
		{
			var result = await interactorExecution.ExecuteAsync(command);
			return CustomResponse(result);
		}

		/// <summary>
		/// register new user.
		/// </summary>
		/// <returns></returns>
		[HttpPost("Register")]
		public async Task<ActionResult<ResponseBase<RegistrationOutput>>> Register(RegistrationCommand command)
		{
			var result = await interactorExecution.ExecuteAsync(command);
			return CustomResponse(result);
		}

		/// <summary>
		/// validate authentication code and generate access token
		/// </summary>
		/// <returns></returns>
		[HttpPost("Authenticate")]
		public async Task<ActionResult<ResponseBase<AuthenticationOutput>>> Authenticate(AuthenticationCommand command)
		{
			var result = await interactorExecution.ExecuteAsync(command);
			return CustomResponse(result);
		}

		/// <summary>
		/// get a new access token when expired using a refresh token
		/// </summary>
		/// <returns></returns>
		[HttpPost("RefreshToken")]
		public async Task<ActionResult<ResponseBase<AuthenticationOutput>>> RefreshToken(RefreshTokenCommand command)
		{
			var result = await interactorExecution.ExecuteAsync(command);
			return CustomResponse(result);
		}

		/// <summary>
		/// get logged-in user profile
		/// </summary>
		/// <returns></returns>
		[HttpGet("Profile")]
		[Authorize]
		public async Task<ActionResult<ResponseBase<ProfileOutput>>> Profile()
		{
			var result = await interactorExecution.ExecuteAsync(new GetUserProfileQuery());
			return CustomResponse(result);
		}

		/// <summary>
		/// update logged-in user profile
		/// </summary>
		/// <returns></returns>
		[HttpPost("Profile")]
		[Authorize]
		public async Task<ActionResult<ResponseBase<ProfileOutput>>> UpdateProfile(UpdateProfileCommand command)
		{
			var result = await interactorExecution.ExecuteAsync(command);
			return CustomResponse(result);
		}

		/// <summary>
		/// Preview email html
		/// </summary>
		/// <returns></returns>
		[HttpGet("PreviewEmail")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ContentResult PreviewEmail(string data)
        {
			try
			{
				var decryptedData = EncryptionHelper.Decrypte(data);
				var parameters = decryptedData.Split("&").ToList();

				var parameterTemplateName = parameters.FirstOrDefault(s => s.Contains("templateName"));
				if (parameterTemplateName == null)
					return Content("");

				var parameterTemplateNameValue = parameterTemplateName.Split("=")[1];
				emailTemplateHelper.PrepareTemplate($"{parameterTemplateNameValue}.html", false);

				foreach (var parameter in parameters)
				{
					var parameterSplit = parameter.Split("=");
					var parameterName = parameterSplit[0];
					var parameterValue = parameterSplit[1];

					emailTemplateHelper.AddTemplateParameter(parameterName, parameterValue);
				}

				return new ContentResult
				{
					Content = emailTemplateHelper.GetEmailContent(),
					ContentType = "text/html"
				};
			}
			catch (Exception)
			{
                return Content("");
            }
        }

        /// <summary>
        /// get logged-in user referral QR code url
        /// </summary>
        /// <returns></returns>
        [HttpGet("ReferralQRCode")]
		[Authorize]
        public async Task<ActionResult<ResponseBase<ReferralCodeQROutput>>> GetReferralQRCode()
		{
            var result = await interactorExecution.ExecuteAsync(new GetUserReferralCodeQRCodeQuery());
            return CustomResponse(result);
        }
    }
}
