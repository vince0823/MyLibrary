using Microsoft.AspNetCore.Authorization;

namespace MyLibrary.WebApi.PolicyHandlers
{
    public class MyRestaurantAccessRequirement : IAuthorizationRequirement
    {
        public string Role { get; }
        public MyRestaurantAccessRequirement(string role)
        {
            Role = role;
        }
    }
}
