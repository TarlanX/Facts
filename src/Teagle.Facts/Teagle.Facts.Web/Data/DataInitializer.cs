using System;
using System.Linq;
using System.Threading.Tasks;
using Calabonga.Microservices.Core.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Teagle.Facts.Web.Infrastructure;

namespace Teagle.Facts.Web.Data
{
    public static class DataInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            const string username = "dev@teagle.net";
            const string password = "qwerty";
            

            var scope = serviceProvider.CreateScope();
            await using var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            var isExists = context!.GetService<IDatabaseCreator>() is RelationalDatabaseCreator databaseCreator &&
                           await databaseCreator.ExistsAsync();

            if (isExists)
            {
                return;
            }

            await context.Database.MigrateAsync();

            var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
            var roles = AppData.Roles.ToArray();


            if (userManager is null || roleManager is null)
            {
                throw new MicroserviceArgumentNullException("UserManager or RoleManager not registered");
            }

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            if (await userManager.FindByEmailAsync(username) is not null)
            {
                return;
            }

            var user = new IdentityUser
            {
                Email = username,
                EmailConfirmed = true,
                NormalizedEmail = username.ToUpper(),
                UserName = username,
                NormalizedUserName = username.ToUpper()
            };
           

            var identityResult = await userManager.CreateAsync(user, password);
            IdentityResultHandler(identityResult);

            identityResult = await userManager.AddToRolesAsync(user, roles);
            IdentityResultHandler(identityResult);

            await context.SaveChangesAsync();

        }

        private static void IdentityResultHandler(IdentityResult identityResult)
        {
            if (identityResult.Succeeded is false)
            {
                var message = string.Join(", ", identityResult.Errors.Select(x => $"{x.Code}: {x.Description}"));
                throw new MicroserviceDatabaseException(message);
            }
        }
    }
}