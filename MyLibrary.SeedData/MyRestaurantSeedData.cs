using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyLibrary.Core;
using MyLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.SeedData
{
    public class MyRestaurantSeedData : IMyRestaurantSeedData
    {
        private readonly IServiceProvider _serviceProvider;
        public MyRestaurantSeedData(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Initialize()
        {
            var logger = _serviceProvider.GetRequiredService<ILogger<MyRestaurantSeedData>>();
            try
            {
                var userManager = _serviceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = _serviceProvider.GetRequiredService<RoleManager<Role>>();
                var superAdminSettings = _serviceProvider.GetRequiredService<IOptions<SuperAdminAccount>>().Value;
                var context = _serviceProvider.GetRequiredService<MyRestaurantContext>();

                await context.Database.MigrateAsync();
                await context.Database.EnsureCreatedAsync();

                var roleTypes = new List<Roles> { Roles.SuperAdmin, Roles.Admin, Roles.Report, Roles.Normal };
                if (!roleManager.Roles.Any())
                {
                    var roles = roleTypes.Select(x => new Role { Name = x.ToString(), NormalizedName = x.ToString().ToUpper() });

                    foreach (var role in roles)
                    {
                        await roleManager.CreateAsync(role);
                    }
                }

                if (!userManager.Users.Any())
                {
                    var user = new User
                    {
                        FirstName = superAdminSettings.FirstName,
                        LastName = superAdminSettings.LastName,
                        Email = superAdminSettings.Email,
                        UserName = superAdminSettings.Email
                    };

                    var result = await userManager.CreateAsync(user, "superAdmin1@#");
                    if (result.Succeeded)
                        await userManager.AddToRolesAsync(user, roleTypes.Select(x => x.ToString()));
                }
            }
            catch (SqlException ex)
            {
                logger.LogError("An error occurred while seeding the database. {ex.Message}", ex.Message);
                throw;
            }
        }
    }
}
