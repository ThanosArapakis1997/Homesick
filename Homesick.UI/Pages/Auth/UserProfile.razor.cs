using Homesick.UI.Models;
using Homesick.UI.Utility;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Data;

namespace Homesick.UI.Pages.Auth
{
    public partial class UserProfile
    {
        private List<ListingDto> listings = new List<ListingDto>();
        private AuthenticationState authState;
        private string role = string.Empty;
        private string email = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                role = user.FindFirst("role")?.Value;
                email = user.FindFirst("email")?.Value;
                if (role.Equals(SD.RoleCustomer, StringComparison.CurrentCultureIgnoreCase))
                {
                    string userId = user.FindFirst("sub")?.Value;
                    ResponseDto response = await listingService.GetUserListings(userId);

                    listings = JsonConvert.DeserializeObject<List<ListingDto>>(Convert.ToString(response.Result));
                }
            }
            else
            {
                Console.WriteLine($"Ρόλος: Unauthorized");
            }

        }
    }
}
