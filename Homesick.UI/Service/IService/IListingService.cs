using Homesick.UI.Models;

namespace Homesick.UI.Service.IService
{
    public interface IListingService
    {
        Task<ResponseDto> GetUserListings(string userId);

        Task<ResponseDto> GetFilteredListings(FilterDto houseDto);

        Task<ResponseDto> GetAllListings();

        Task<ResponseDto?> GetListing(int listingId);

        Task<ResponseDto?> UpdateListingStatus(int listingId, string newStatus);

        Task<ResponseDto?> UpdateListing(ListingDto listing);

        Task<ResponseDto> CreateListing(ListingDto listing);

        Task<ResponseDto> DeleteListing(int listingId);
    }
}
