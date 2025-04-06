using Homesick.Mud.Models;

namespace Homesick.Mud.Service.IService
{
    public interface IListingService
    {
        Task<ResponseDto> GetUserListings(string userId);

        Task<ResponseDto> GetFilteredListings(FilterDto houseDto);
    }
}
