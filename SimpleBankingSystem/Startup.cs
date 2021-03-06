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
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Rewrite;
    using SimpleBankingSystem.Hubs;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        => Configuration = configuration;
        

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

            services.AddDbContext<SBSDbContext>(options => options
            .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services
                .AddDefaultIdentity<ApplicationUser>(options =>
                {
                    //can be changed later
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredUniqueChars = 0;
                })

                .AddRoles<IdentityRole>()

                .AddEntityFrameworkStores<SBSDbContext>()

                .AddDefaultTokenProviders()

                .AddErrorDescriber<AppErrorDescriber>();

            services.AddControllersWithViews();

            services.AddServerSideBlazor();

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

            services.AddScoped<DataSeederForPresentation>();

            services.AddSignalR();

            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DefaultAdminDataSeeder seeder,
            DataSeederForPresentation presentationData)
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
                .UseCookiePolicy()
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
                    endpoints.MapBlazorHub();
                    //endpoints.MapFallbackToPage("/home/index");
                    endpoints.MapHub<SignalRChatHub>(SignalRChatHub.HubUrl);
                });

            var options = new RewriteOptions().AddRedirectToHttps();
            app.UseRewriter(options);

            seeder.SeedAdmin().ConfigureAwait(true).GetAwaiter().GetResult();

            presentationData.SeedUsersTransactionsAndNews().ConfigureAwait(true).GetAwaiter().GetResult();
            
            
        }
    }
}
