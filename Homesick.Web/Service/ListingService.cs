using Homesick.Web.Models;
using Homesick.Web.Service.IService;
using Homesick.Web.Utility;
using System.Linq.Expressions;

namespace Homesick.Web.Service
{
    public class ListingService : IListingService
    {
        private readonly IBaseService _baseService;
        public ListingService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> GetUserListings(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ListingAPIBase + "/api/listing/GetListings/" + userId
            });
        }

        public async Task<ResponseDto?> GetFilteredListings(FilterDto filter)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = filter,
                Url = SD.ListingAPIBase + "/api/listing/GetFilteredListings"
            });
        }
    }
}
