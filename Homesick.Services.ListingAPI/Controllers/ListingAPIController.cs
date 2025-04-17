using AutoMapper;
using Homesick.Services.ListingAPI.Data;
using Homesick.Services.ListingAPI.Models.DTO;
using Homesick.Services.ListingAPI.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Homesick.Services.ListingAPI.Controllers
{
    [Route("api/listing")]
    [ApiController]
    public class ListingAPIController : ControllerBase
    {
        private IListingService _listingService;
        private ResponseDto _response;       

        public ListingAPIController(IMapper mapper, IListingService listingService)
        {           
            _response = new ResponseDto();
            _listingService = listingService;
        }

        [HttpGet]
        [Authorize(Roles = "ΔΙΑΧΕΙΡΙΣΤΉΣ")]
        public async Task<ResponseDto> GetAllListings()
        {
            try
            {
                List<ListingDto> listings = await _listingService.GetAllListings();
                if (listings == null)
                {
                    _response.Message = "No Listings Found";
                    _response.IsSuccess = false;
                }
                else
                {
                    _response.Result = listings;
                }
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }


        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ResponseDto> GetListing(int id)
        {
            try
            {
                ListingDto listing = await _listingService.GetListingByIdAsync(id);
                if (listing == null)
                {
                    _response.Message = "No Listings Found";
                    _response.IsSuccess = false;
                }
                else
                {
                    _response.Result = listing;
                }
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpGet("GetListings/{userId}")]
        [Authorize]
        public async Task<ResponseDto> GetUserListings(string userId)
        {
            try
            {
                List<ListingDto> listings = await _listingService.GetListingsByUserIdAsync(userId);

                if (listings == null)
                {
                    _response.Message = "No Listings Found";
                    _response.IsSuccess = false;
                }
                else
                {
                    _response.Result = listings;
                }
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("GetFilteredListings")]
        public async Task<ResponseDto> GetFilteredListings([FromBody] FilterDto filter)
        {
            try
            {
                List<ListingDto> listings = await _listingService.GetFilteredListingsAsync(filter);

                _response.Result = listings;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("CreateListing")]
        [Authorize]
        public async Task<ResponseDto> CreateListing([FromBody] ListingDto listing)
        {
            try
            {
                ListingDto model = await _listingService.CreateListingAsync(listing);
                _response.Result = model;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [Authorize]
        [HttpPut("UpdateListingStatus/{listingId:int}")]
        public async Task<ResponseDto> UpdateListingStatus(int listingId, [FromBody] string newStatus)
        {
            try
            {
                var listing = await _listingService.GetListingByIdAsync(listingId);
                if (listing == null)
                {
                    _response.Message = "No Listing Found";
                    _response.IsSuccess = false;
                }
                else
                {
                    listing.Status = newStatus;
                    await _listingService.UpdateListingAsync(listing);
                    _response.Result = listing;
                }
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [Authorize]
        [HttpPut]
        public async Task<ResponseDto> UpdateListing([FromBody] ListingDto listing)
        {
            try
            {
                if (listing == null)
                {
                    _response.Message = "No Listing Found";
                    _response.IsSuccess = false;
                }
                else
                {
                    await _listingService.UpdateListingAsync(listing);
                    _response.Result = listing;
                }
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [Authorize]
        [HttpDelete("DeleteListing/{listingId:int}")]
        public async Task<ResponseDto> DeleteListing(int listingId)
        {
            try
            {
                var listing = await _listingService.GetListingByIdAsync(listingId);
                if (listing == null)
                {
                    _response.Message = "No Listing Found";
                    _response.IsSuccess = false;
                }
                else
                {
                    await _listingService.DeleteListingAsync(listing);
                    _response.Result = listing;
                }
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

    }
}
