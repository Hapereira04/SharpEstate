using Microsoft.AspNetCore.Identity;

namespace SharpEstate.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // 1. Criar os Cargos (Roles) se não existirem
            string[] cargos = { "Admin", "Consultor", "Cliente" };

            foreach (var cargo in cargos)
            {
                if (!await roleManager.RoleExistsAsync(cargo))
                {
                    await roleManager.CreateAsync(new IdentityRole(cargo));
                }
            }

            // 2. Criar o Utilizador "Super Admin" por defeito
            string emailAdmin = "admin@sharpestate.pt";
            string passwordAdmin = "Admin123!"; // Password forte exigida pelo ASP.NET

            if (await userManager.FindByEmailAsync(emailAdmin) == null)
            {
                var adminUser = new IdentityUser
                {
                    UserName = emailAdmin,
                    Email = emailAdmin,
                    EmailConfirmed = true
                };

                var resultado = await userManager.CreateAsync(adminUser, passwordAdmin);

                if (resultado.Succeeded)
                {
                    // Dá o cargo de "Admin" a este utilizador
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}