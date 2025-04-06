using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Homesick.BlazorWeb;
using MudBlazor.Services;
using Homesick.BlazorWeb.Service.IService;
using Homesick.BlazorWeb.Service;
using Homesick.BlazorWeb.RetrievingData;
using Homesick.BlazorWeb.Utility;
using static System.Net.WebRequestMethods;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IListingService, ListingService>();

builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IListingService, ListingService>();

builder.Services.AddMudServices();

//Retrieving Services
builder.Services.AddSingleton<FilteredListings>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

SD.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"];
SD.ListingAPIBase = "https://localhost:7002";

await builder.Build().RunAsync();
