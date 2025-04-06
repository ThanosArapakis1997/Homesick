using Homesick.Mud.Components;
using Homesick.Mud.RetrievingData;
using Homesick.Mud.Service;
using Homesick.Mud.Service.IService;
using Homesick.Mud.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddServerSideBlazor();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IListingService, ListingService>();

builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IListingService, ListingService>();

builder.Services.AddMudServices();

builder.Services.AddSingleton<FilteredListings>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(10);
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });

SD.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"];
SD.ListingAPIBase = builder.Configuration["ServiceUrls:ListingAPI"];
SD.WebAPIBase = builder.Configuration["ServiceUrls:WebAPI"];

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapBlazorHub(); // Map Blazor Hub

app.Run();
