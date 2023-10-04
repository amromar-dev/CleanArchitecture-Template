using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.SharedKernel.Types;

namespace CleanArchitectureTemplate.API.Common
{
    /// <summary>
    /// 
    /// </summary>
    [ProducesResponseType(typeof(ResponseBase<ExceptionResponseBase>), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public class CustomControllerBase : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        protected ActionResult<ResponseBase<T>> CustomResponse<T>(ResponseBase<T> response)
        {
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
