using Newtonsoft.Json;
using CleanArchitectureTemplate.SharedKernel.Types;
using System.Net;

namespace CleanArchitectureTemplate.API.Middleware
{
	/// <summary>
	/// 
	/// </summary>
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IHostEnvironment hostEnvironment;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="next"></param>
		/// <param name="hostEnvironment"></param>
		public ExceptionMiddleware(RequestDelegate next, IHostEnvironment hostEnvironment)
		{
			_next = next;
			this.hostEnvironment = hostEnvironment;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (BusinessException ex)
			{
				context.Response.ContentType = "application/json";
				context.Response.StatusCode = (int)ex.Status;

				var exception = new ExceptionResponseBase(ex.Message);
				var response = new ResponseBase<ExceptionResponseBase>(exception, ex.Status);
				await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
			}
			catch (Exception ex)
			{
				context.Response.ContentType = "application/json";
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

				var error = hostEnvironment.IsDevelopment() ? ex.Message : "An unexpected error occurred on the server.";
				var exception = new ExceptionResponseBase(error);
				var response = new ResponseBase<ExceptionResponseBase>(exception, HttpStatusCode.InternalServerError);
				await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
			}
		}
	}
}
