namespace SimpleBankingSystem
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using SimpleBankingSystem.Data;
    using SimpleBankingSystem.Infrastructure;
    using SimpleBankingSystem.Data.Models;
    using SimpleBankingSystem.Services;
    using SimpleBankingSystem.Data.DataSeeding;
    using System.Threading.Tasks;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        => Configuration = configuration;
        

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SBSDbContext>(options => options
            .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services
                .AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.Password.RequireDigit = false;  //can be changed later
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })

                .AddRoles<IdentityRole>()

                .AddEntityFrameworkStores<SBSDbContext>()

                .AddDefaultTokenProviders()

                .AddErrorDescriber<AppErrorDescriber>();

            services.AddControllersWithViews();

            services.ConfigureApplicationCookie
                (options =>
                {
                    options.LoginPath = "/user/login";
                    options.AccessDeniedPath = "/home/error404";
                });

            services.AddScoped<IErrorCollector, ErrorCollector>();

            services.AddScoped<IGetUserService, GetUserService>();

            services.AddScoped<IGetTransactions, GetTransactionsService>();

            services.AddScoped<DefaultAdminDataSeeder>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DefaultAdminDataSeeder seeder)
        {
            app.PrepareDatabase();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapRazorPages();
                    endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                });

            seeder.SeedAdmin().ConfigureAwait(true).GetAwaiter().GetResult();
            
            
        }
    }
}
