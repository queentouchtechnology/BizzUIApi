using Bizz.DataService.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BizzUIApi.AppSettings;

public static class DependencyInjectionExtensions
{
    public static void AddStandardServices(this WebApplicationBuilder builder)
    {
        // Authentication & Authorization
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                options => builder.Configuration.Bind("Cookie", options ));
        builder.Services.AddAuthorizationBuilder();
        
        // Enable CORS
        builder.Services.AddCors(options =>
        {
            //WithMethods("GET","POST","PUT","DELETE")
            options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });
        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }

    public static void AddProjectServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<DbService>();
        builder.Services.AddTransient<ProductService>();
        builder.Services.AddTransient<FileStorageService>();
        builder.Services.AddTransient<OrganizationService>();
        builder.Services.AddTransient<UserAuthService>();
    }
}