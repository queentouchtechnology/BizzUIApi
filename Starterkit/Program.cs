using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
//using Starterkit.Data;
using Starterkit._keenthemes;
using Starterkit._keenthemes.libs;

using Bizz.DataService.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);




//For Google Auth
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
   .AddCookie(options =>
   {
       options.Cookie.Name = "YourAppCookieName";
       options.LoginPath = "/Account/Login"; // Redirect to this path for unauthorized users
   })
   .AddGoogle(options =>
   {
       options.ClientId = "52903987029-7ncu10ma95e6r98cdtcsdmc32u1fhu8t.apps.googleusercontent.com";
       options.ClientSecret = "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";
   });

 builder.Services.AddCascadingAuthenticationState();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
//builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<DbService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CustomersService>();
builder.Services.AddScoped<AuthService>();


builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<OrganizationService>();
builder.Services.AddScoped<AuthenticationService>();






builder.Services.AddAuthorizationCore();

// builder.Services.AddScoped<CookieAuthenticationStateProvider>();
// builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
//     provider.GetRequiredService<CookieAuthenticationStateProvider>());





//builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://bizzuiapi.queentouchtech.in/api/") });

builder.Services.AddSingleton<IKTTheme, KTTheme>();
builder.Services.AddSingleton<IBootstrapBase, BootstrapBase>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

IConfiguration themeConfiguration = new ConfigurationBuilder()
                            .AddJsonFile("_keenthemes/config/themesettings.json")
                            .Build();

IConfiguration iconsConfiguration = new ConfigurationBuilder()
                            .AddJsonFile("_keenthemes/config/icons.json")
                            .Build();

KTThemeSettings.init(themeConfiguration);
KTIconsSettings.init(iconsConfiguration);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.UseAuthentication();
app.UseAuthorization();

app.Run();
