using Homesick.UI.Models;
using Homesick.UI.Utility;
using MudBlazor;
using Newtonsoft.Json;

namespace Homesick.UI.Pages.Listings
{
    public partial class Filtered_listings
    {
        private List<ListingDto>? listingData;
        private FilterDto model = new FilterDto();
        private bool showPriceDropdown = false;
        private bool showFloorAreaDropdown = false;
        private bool _loading = true;
        private List<string> _areas = SD.Areas;

        private int _selected = 1; // τρέχουσα σελίδα
        private int _pageSize = 3;

        private IEnumerable<ListingDto> PageListings => listingData
            .Skip((_selected - 1) * _pageSize)
            .Take(_pageSize);

        private int TotalPages => (int)Math.Ceiling((double)listingData.Count / _pageSize);

        protected override async Task OnInitializedAsync()
        {
            listingData = await FilteredListingsService.GetListingsData();

            if (listingData == null)
            {
                Snackbar.Add("Δεν βρέθηκαν αγγελίες με αυτά τα κριτήρια", Severity.Normal);
            }
        }

        private void TogglePriceDropdown()
        {
            showPriceDropdown = !showPriceDropdown;
        }

        private void ToggleFloorAreaDropdown()
        {
            showFloorAreaDropdown = !showFloorAreaDropdown;
        }

        private async Task<IEnumerable<string>> Search(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _areas;

            return _areas.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async void ChangeToRent()
        {
            model.ListingType = SD.ListingTypeRent;
            await HandleSubmit();
        }

        private async void ChangeToSale()
        {
            model.ListingType = SD.ListingTypeBuy;
            await HandleSubmit();
        }

        private async Task HandleSubmit()
        {
            ResponseDto response = await listingService.GetFilteredListings(model);

            if (response != null && response.IsSuccess)
            {
                listingData = JsonConvert.DeserializeObject<List<ListingDto>>(Convert.ToString(response.Result));

            }
            else
            {
                Snackbar.Add(response?.Message ?? "An error occurred", Severity.Warning);
            }
        }
    }
}
