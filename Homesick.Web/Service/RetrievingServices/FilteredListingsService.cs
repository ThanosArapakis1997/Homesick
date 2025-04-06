using Homesick.Web.Models;

namespace Homesick.Web.Service.RetrievingServices
{
    public class FilteredListingsService
    {
        public List<ListingDto>? ListingsData { get; private set; }

        public void SetListingsData(List<ListingDto>? data)
        {
            ListingsData = data;
        }
    }
}
