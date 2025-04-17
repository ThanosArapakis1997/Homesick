using Homesick.UI.Models;
using Homesick.UI.Service.IService;
using Homesick.UI.Utility;
using Microsoft.AspNetCore.Http;

namespace Homesick.UI.Service
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

        public async Task<ResponseDto?> GetListing(int listingId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ListingAPIBase + "/api/listing/" + listingId
            });
        }

        public async Task<ResponseDto?> GetAllListings()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ListingAPIBase + "/api/listing"
            });
        }

        public async Task<ResponseDto?> CreateListing(ListingDto listing)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = listing,
                Url = SD.ListingAPIBase + "/api/listing/CreateListing"
            });
        }

        public async Task<ResponseDto?> UpdateListingStatus(int listingId, string newStatus)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.PUT,
                Data = newStatus,
                Url = SD.ListingAPIBase + "/api/listing/UpdateListingStatus/" + listingId
            });
        }

        public async Task<ResponseDto?> UpdateListing(ListingDto listing)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.PUT,
                Data = listing,
                Url = SD.ListingAPIBase + "/api/listing"
            });
        }

        public async Task<ResponseDto?> DeleteListing(int listingId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.ListingAPIBase + "/api/listing/DeleteListing/" + listingId
            });
        }
    }
}
