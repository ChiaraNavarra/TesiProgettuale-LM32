using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TesiMagistraleLM32.Models
{
    public static class RoleViewModel
    {
        public static void Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
        }

        public static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            if(userManager.FindByNameAsync("admin").Result == null)
            {
                var user = new IdentityUser { UserName = "admin" };
            }
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Administrator"
                };
                roleManager.CreateAsync(role);
            }
            if (!roleManager.RoleExistsAsync("SimpleUser").Result)
            {
                var role = new IdentityRole
                {
                    Name = "SimpleUser"
                };
                roleManager.CreateAsync(role);
            }
        }
    }
}
