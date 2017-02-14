using System.Linq;
using System.Net;
using Fillager.Controllers;
using Fillager.DataAccessLayer;
using Fillager.Models.Account;
using Fillager.Models.Menu;
using Fillager.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySQL.Data.Entity.Extensions;
using StackExchange.Redis;

namespace Fillager
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)  
        {
            //add db connection
            services.AddDbContext<ApplicationDbContext>(
                options => { options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")); });

            //add identity/authentication service
            services.AddIdentity<ApplicationUser, UserRole>(identityOptions =>
                {
                    identityOptions.Password.RequiredLength = 8;
                    identityOptions.Password.RequireUppercase = true;
                    identityOptions.Password.RequireLowercase = true;
                    identityOptions.Password.RequireNonAlphanumeric = true;
                    identityOptions.Password.RequireDigit = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //add authorization service
            services.AddAuthorization(
                authorizationOptions =>
                {
                    authorizationOptions.AddPolicy("ElevatedRights", policy => policy.RequireRole("Admin"));
                });

            //add redis identity key store for distributed identity support
            var redisHost = Configuration.GetValue<string>("Redis:Host");
            var redisPort = Configuration.GetValue<int>("Redis:Port");
            var redisIpAddress = Dns.GetHostEntryAsync(redisHost).Result.AddressList.Last();
            var redis = ConnectionMultiplexer.Connect($"{redisIpAddress}:{redisPort}");

            services.AddDataProtection().PersistKeysToRedis(redis, "DataProtection-Keys");
            services.AddOptions();

            //add mvc support...
            services.AddMvc();

            //add services to IoC container
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddTransient<IMinioService, MinioService>();
            services.AddScoped<IBackupQueueService, BackupQueueService>();


            services.AddScoped<MenuDataRepository>();
            services.AddScoped<AccountController>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Shared/Error");
            }
            app.UseStaticFiles();
            app.UseIdentity();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}