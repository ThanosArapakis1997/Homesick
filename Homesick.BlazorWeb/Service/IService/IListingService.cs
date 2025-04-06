using Homesick.BlazorWeb.Models;

namespace Homesick.BlazorWeb.Service.IService
{
    public interface IListingService
    {
        Task<ResponseDto> GetUserListings(string userId);

        Task<ResponseDto> GetFilteredListings(FilterDto houseDto);
    }
}
