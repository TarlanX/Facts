﻿using System;
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
            var scope = serviceProvider.CreateScope();
            await using var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            var isExists = context!.GetService<IDatabaseCreator>() is RelationalDatabaseCreator databaseCreator &&
                           await databaseCreator.ExistsAsync();

            if (isExists)
            {
                return;
            }

            await context.Database.MigrateAsync();

            var roles = AppData.Roles.ToArray();
            var roleStore = new RoleStore<IdentityRole>(context);
            foreach (var role in roles)
            {
                if (!context.Roles.Any(x => x.Name == role))
                {
                    await roleStore.CreateAsync(new IdentityRole(role)
                    {
                        NormalizedName = role.ToUpper()
                    });
                }
            }

            const string userName = "dev@teagle.net";

            if (context.Users.Any(x => x.Email == userName))
            {
                return;
            }

            var user = new IdentityUser()
            {
                Email = userName,
                EmailConfirmed = true,
                NormalizedEmail = userName.ToUpper(),
                UserName = userName,
                NormalizedUserName = userName.ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            var passwordHasher = new PasswordHasher<IdentityUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, "qwerty");

            var userStore = new UserStore<IdentityUser>(context);
            var identityResult = await userStore.CreateAsync(user);
            if (!identityResult.Succeeded)
            {
                var message = string.Join(", ", identityResult.Errors.Select(x => $"{x.Code}: {x.Description}" ));
                throw new MicroserviceDatabaseException(message);
            }

            var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
            foreach (var role in roles)
            {
                var identityResultRole = await userManager.AddToRoleAsync(user, role);
                if (!identityResultRole.Succeeded)
                {
                    var message = string.Join(", ", identityResultRole.Errors.Select(x => $"{x.Code}: {x.Description}"));
                    throw new MicroserviceDatabaseException(message);
                }
            }

            await context.SaveChangesAsync();

        }
    }
}