using Homesick.Web.Models;
using System.Linq.Expressions;

namespace Homesick.Web.Service.IService
{
    public interface IListingService
    {
        Task<ResponseDto> GetUserListings(string userId);

        Task<ResponseDto> GetFilteredListings(FilterDto houseDto);
    }
}
