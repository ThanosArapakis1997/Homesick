using Homesick.UI.Models;

namespace Homesick.UI.Service.IService
{
    public interface IListingService
    {
        Task<ResponseDto> GetUserListings(string userId);

        Task<ResponseDto> GetFilteredListings(FilterDto houseDto);

        Task<ResponseDto> CreateListing(ListingDto listing);
    }
}
