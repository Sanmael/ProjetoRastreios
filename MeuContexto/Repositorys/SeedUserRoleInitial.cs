using Microsoft.AspNetCore.Identity;

namespace MeuContexto.Repositorys
{
    public class SeedUserRoleInitial
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<UserIdentity> _userManager;

        public SeedUserRoleInitial(RoleManager<IdentityRole> roleManager, UserManager<UserIdentity> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void SeedUsers()
        {
            if (_userManager.FindByEmailAsync("usuario@localhost").Result == null)
            {
                UserIdentity userIdentity = new UserIdentity()
                {
                    UserName = "usuario@localhost",
                    Email = "usuario@localhost",
                    NormalizedEmail = "USUARIO@LOCALHOST",
                    NormalizedUserName = "USUARIO@LOCALHOST",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                IdentityResult identityResult = _userManager.CreateAsync(userIdentity, "Mael03112012Aa@").Result;

                if (identityResult.Succeeded)
                {
                    _userManager.AddToRoleAsync(userIdentity, "User").Wait();
                }
            }
            if (_userManager.FindByEmailAsync("admin@localhost").Result == null)
            {
                UserIdentity userIdentity = new UserIdentity()
                {
                    UserName = "admin@localhost",
                    Email = "admin@localhost",
                    NormalizedEmail = "ADMIN@LOCALHOST",
                    NormalizedUserName = "ADMIN@LOCALHOST",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                IdentityResult identityResult = _userManager.CreateAsync(userIdentity, "Mael03112012Aa@").Result;

                if (identityResult.Succeeded)
                {
                    _userManager.AddToRoleAsync(userIdentity, "Admin").Wait();
                }
            }
        }
        public void SeedRoles()
        {
            if (_roleManager.FindByNameAsync("User").Result == null)
            {
                IdentityRole identityRole = new IdentityRole()
                {
                    Name = "User",
                    NormalizedName = "USER",
                };
                IdentityResult identityResult = _roleManager.CreateAsync(identityRole).Result;
            }

            if(_roleManager.FindByNameAsync("Admin").Result == null)
            {
                IdentityRole identityRole = new IdentityRole()
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                };
                IdentityResult identityResult = _roleManager.CreateAsync(identityRole).Result;
            }
        }
    }
}
