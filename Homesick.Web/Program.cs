using Homesick.Web.Service;
using Homesick.Web.Service.IService;
using Homesick.Web.Service.RetrievingServices;
using Homesick.Web.Utility;
using Homesick.Web.Views;
using Microsoft.AspNetCore.Authentication.Cookies;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddHttpClient<IListingService, ListingService>();

builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IListingService, ListingService>();


//Retrieving Services
builder.Services.AddSingleton<FilteredListingsService>();

builder.Services.AddMudServices();
builder.Services.AddServerSideBlazor();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(10);
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });

var app = builder.Build();


SD.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"];
SD.ListingAPIBase = builder.Configuration["ServiceUrls:ListingAPI"];
SD.WebAPIBase = builder.Configuration["ServiceUrls:WebAPI"];

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

app.UseAuthorization();
app.UseAuthentication();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapBlazorHub(); // Map Blazor Hub
app.MapFallbackToPage("/_Host"); // Fallback to _Host.cshtml

app.Run();
