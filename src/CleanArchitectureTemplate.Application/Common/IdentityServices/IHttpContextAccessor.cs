using System.Security.Claims;

namespace CleanArchitectureTemplate.Application
{
    public static class IHttpContextAccessorExtension
    {
        public static int GetUserId(this Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext)
        {
            if (httpContext.HttpContext.User.Identity.IsAuthenticated == false)
                return 0;

            var nameIdentifier = httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            _ = int.TryParse(nameIdentifier, out var userId);
            return userId;
        }

        public static int? GetUserIdIfExist(this Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext)
        {
            var userId = httpContext.GetUserId();
            if (userId == 0)
                return null;

            return userId;
        }
    }
}
