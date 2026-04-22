using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.FileProviders;
using RMS.Client; // Make sure to import any required namespaces
using RMS.Client.Extensions;
using RMS.Client.Utility;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
    
});

// Configure sessions
builder.Services.AddDistributedMemoryCache(); // Use an in-memory cache for sessions
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(90); // Set the session timeout
});
//builder.Services.AddControllers().AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
//});

builder.Services.AddHostedService<Cronjobservice>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot")),
    RequestPath = new PathString("/wwwroot")  // OR use "/" to map to base URI
});

var supportedCultures = new[] { new CultureInfo("en-GB") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-GB"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Add session middleware
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
