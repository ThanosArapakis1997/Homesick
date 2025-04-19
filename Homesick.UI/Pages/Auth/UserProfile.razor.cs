using Homesick.UI.Layout;
using Homesick.UI.Models;
using Homesick.UI.Utility;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using MudBlazor;
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

        private async Task DeleteItemAsync(ListingDto listing)
        {
            var parameters = new DialogParameters<ConfirmationDialog> { { x => x.Listing, listing } };

            var dialog = await DialogService.ShowAsync<ConfirmationDialog>("Διαγραφή Αγγελίας", parameters);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                ResponseDto response = await listingService.DeleteListing(listing.ListingId);
                if (response.IsSuccess)
                {
                    Snackbar.Add("Η αγγελία διαγράφηκε επιτυχώς!", Severity.Success);
                    listings.Remove(listing);
                }
                else
                {
                    Snackbar.Add("Η διαγραφή απέτυχε!", Severity.Error);
                }
            }
        }
    }
}
