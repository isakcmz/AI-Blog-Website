using BtkAkademiAIBlog.WebApi.Context;
using BtkAkademiAIBlog.WebApi.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace BtkAkademiAIBlog.WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddDbContext<BlogAIContext>();
            
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<BlogAIContext>()
                .AddDefaultTokenProviders();

            return services;
        }



        public static IServiceCollection AddAuthServices(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/api/Logins";
                    options.AccessDeniedPath = "/api/logins";
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                });

            services.AddAuthorization();

            return services;
        }

    }
}
