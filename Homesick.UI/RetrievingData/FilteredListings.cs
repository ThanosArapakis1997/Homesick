using Homesick.UI.Models;
using Blazored.SessionStorage;

namespace Homesick.UI.RetrievingData
{
    public class FilteredListings
    {
        private readonly ISessionStorageService _sessionStorage;

        public FilteredListings(ISessionStorageService sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        private List<ListingDto> _listings = new();

        public async Task SetListingsData(List<ListingDto> listings)
        {
            _listings = listings;
            await _sessionStorage.SetItemAsync("FilteredListings", listings);
        }

        public async Task<List<ListingDto>> GetListingsData()
        {
            if (_listings.Count == 0)
            {
                _listings = await _sessionStorage.GetItemAsync<List<ListingDto>>("FilteredListings") ?? new List<ListingDto>();
            }
            return _listings;
        }
    }
}
