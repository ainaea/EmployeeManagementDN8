using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmployeeManagement.Security
{
    public class CanEditOnlyOtherAdminRolesAndClaimsHandler : AuthorizationHandler<ManageAdminRolesAndClaimsRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAdminRolesAndClaimsRequirement requirement)
        {            
            var httpContext = GetHttpContext(context.Resource);
            if (httpContext == null)
            {
                return Task.CompletedTask;
            }
            string loggedInAdminId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            string adminIdBeingEdited = httpContext.Request.Query["userId"];

            if (context.User.IsInRole("Admin") && context.User.HasClaim( claim => claim.Type == "Edit Role" && claim.Value == "true" ) && adminIdBeingEdited.ToLower() != loggedInAdminId.ToLower() )
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }

        private HttpContext GetHttpContext(object contextResource)
        {
            if (contextResource is DefaultHttpContext)
            {
                return contextResource as DefaultHttpContext;
            }
            else if (contextResource is AuthorizationFilterContext)
            {
                return (contextResource as AuthorizationFilterContext).HttpContext;
            }
            else
            {
                return null;
            }
        }
    }
}
