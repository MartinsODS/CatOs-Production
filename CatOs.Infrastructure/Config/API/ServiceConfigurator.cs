using CatOs.Core.Interface.SKU;
using CatOs.Core.Interface.Stock;
using CatOs.Core.Interface.Ticket;
using CatOs.Infrastructure.AppContextDb;
using CatOs.Infrastructure.Implements.SKU;
using CatOs.Infrastructure.Implements.Stock;
using CatOs.Infrastructure.Implements.Ticket;
using DotNetEnv;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CatOs.Infrastructure.Config.API
{
    public static class ServiceConfigurator
    {
        public static void EnvironmentSetup(this IServiceCollection services, WebApplicationBuilder application)
        {
            if (application.Environment.IsDevelopment())
                Env.Load(path: "../.env");

            application.Configuration.AddEnvironmentVariables(prefix: "CATOS_");
        }

        public static void AddApplicationServices(this IServiceCollection services)
        {
            #region ASP.NET
            services.AddSignalR();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddHttpClient();
            services.AddHttpContextAccessor();
            #endregion

            #region Repositories
            services.AddScoped<IStock, Stock>();
            services.AddScoped<ISku, Sku>();
            services.AddScoped<ITicket, Ticket>();
            #endregion
        }

        public static void AddDataBase(this IServiceCollection services, IConfiguration configuration)
        {
            if (string.IsNullOrEmpty(configuration["DATA_BASE_CONNECTION"]))
                throw new Exception("Database connection string is not configured.");

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(configuration["DATA_BASE_CONNECTION"], b => b.MigrationsAssembly("CatOs.ApiHost"));
            });
        }
        public static void AddApplicationConfig(this IServiceCollection services, WebApplication app)
        {
            #region Application Config ASP.NET
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                app.UseCors("DevCorsPolicy");

                app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
            }
            else
                app.UseCors("CorsPolicy");


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.MapControllers();
            #endregion
        }

        public static void AddCorsPolicies(this IServiceCollection services)
        {
            var allowedOrigins = new[] { "http://localhost:5500", "http://localhost:8080", "http://localhost:5132", "http://127.0.0.1:5500", "http://192.168.148.19:3121" };

            services.AddCors(options =>
            {
                options.AddPolicy("DevCorsPolicy", policy =>
                {
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            var productionOrigins = new[]
            {
                "http://192.168.148.19:8751", //CatOs PrintManagement
                "http://192.168.148.19:3121" // CatOs Invite Frontend
            };
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.WithOrigins(productionOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
        }
    }
}