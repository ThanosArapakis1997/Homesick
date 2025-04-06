using Homesick.BlazorWeb.Models;

namespace Homesick.BlazorWeb.RetrievingData
{
    public class FilteredListings
    {
        public List<ListingDto>? ListingsData { get; private set; }

        public void SetListingsData(List<ListingDto>? data)
        {
            ListingsData = data;
        }
    }
}
