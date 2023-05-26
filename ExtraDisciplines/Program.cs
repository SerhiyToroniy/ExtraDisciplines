using ExtraDisciplines.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExtraDisciplines
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddIdentity<User, IdentityRole>(options =>
                {
                    options.User.AllowedUserNameCharacters = null;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            CreateRoles(roleManager).Wait();
            CreateDefaultAdmin(userManager).Wait();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();

            async Task CreateRoles(RoleManager<IdentityRole> roleManager)
            {
                string[] roles = { "Admin", "Student", "Guest" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }

            async Task CreateDefaultAdmin(UserManager<User> userManager)
            {
                // Check if the default admin user already exists
                var adminUser = await userManager.FindByNameAsync("admin");

                if (adminUser == null)
                {
                    // Create a new admin user
                    var user = new User
                    {
                        UserName = Credentials.AdminUserName,
                        Email = Credentials.AdminEmail,
                        // Set any additional properties for the admin user
                    };

                    var result = await userManager.CreateAsync(user, Credentials.AdminPassword);

                    if (result.Succeeded)
                    {
                        // Assign the "Admin" role to the admin user
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                }
            }
        }
    }
}