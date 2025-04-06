using Homesick.Mud.Models;

namespace Homesick.Mud.RetrievingData
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
