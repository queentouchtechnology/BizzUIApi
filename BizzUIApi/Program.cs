// using Microsoft.Extensions.Configuration;
// using System.Configuration;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;

using BizzUIApi.AppSettings;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.AddStandardServices();
builder.AddProjectServices();

var app = builder.Build();
// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }
app.UseCors("AllowAll");
if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath,"Images")),
    RequestPath = new PathString("/Images"),
    // HttpsCompression = HttpsCompressionMode.Compress
});

app.UseAuthorization();

app.MapControllers();

app.Run();
