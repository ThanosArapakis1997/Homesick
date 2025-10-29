using Homesick.UI.Service;
using Homesick.UI.RetrievingData;
using Homesick.UI.Service.IService;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Homesick.UI.Utility;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorDownloadFile;

namespace Homesick.UI;

public class Program
{
    public static async Task Main(string[] args)
    {

        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHttpClient();
        builder.Services.AddHttpClient<IListingService, ListingService>();
        builder.Services.AddHttpClient<IAgentService, AgentService>();

        builder.Services.AddScoped<IBaseService, BaseService>();
        builder.Services.AddScoped<IListingService, ListingService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IStreamService, StreamService>();

        // Register CustomAuthStateProvider correctly
        builder.Services.AddScoped<CustomAuthStateProvider>(); // Explicit registration
        builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());

        // Add Authorization Core
        builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["ServiceUrls:ListingAPI"]) });
        builder.Services.AddScoped<FilteredListings>();

        builder.Services.AddMudServices();
        builder.Services.AddBlazoredSessionStorage();

        SD.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"];
        SD.ListingAPIBase = builder.Configuration["ServiceUrls:ListingAPI"];
        SD.AgentAPIBase = builder.Configuration["ServiceUrls:AgentAPI"];

        await builder.Build().RunAsync();
    }
}
