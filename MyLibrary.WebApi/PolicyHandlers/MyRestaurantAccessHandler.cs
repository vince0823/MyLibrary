using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace MyLibrary.WebApi.PolicyHandlers
{
    public class MyRestaurantAccessHandler : AuthorizationHandler<MyRestaurantAccessRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MyRestaurantAccessRequirement requirement)
        {
            if (context.User.IsInRole(requirement.Role))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
