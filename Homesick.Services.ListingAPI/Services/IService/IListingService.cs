using Homesick.Services.ListingAPI.Models.DTO;

namespace Homesick.Services.ListingAPI.Services.IService
{
    public interface IListingService
    {
        Task<List<ListingDto>> GetAllListings();
        Task<ListingDto> GetListingByIdAsync(int id);
        Task<ListingDto> CreateListingAsync(ListingDto listing);
        Task<ListingDto> UpdateListingAsync(ListingDto listing);
        Task<ListingDto> DeleteListingAsync(ListingDto listing);
        Task<List<ListingDto>> GetListingsByUserIdAsync(string userId);
        Task<List<ListingDto>> GetFilteredListingsAsync(FilterDto filter);

    }
}
