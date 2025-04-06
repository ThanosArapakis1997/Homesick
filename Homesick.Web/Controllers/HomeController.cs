using Homesick.Web.Models;
using Homesick.Web.Service.IService;
using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;

namespace Homesick.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IListingService _listingService;

        public HomeController(IListingService listingService)
        {
            _listingService = listingService;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                List<ListingDto> Listings = await LoadUserListings();
                ViewBag.Listings =Listings;
            }
            return View();
        }


        [Authorize]
        private async Task<List<ListingDto>> LoadUserListings()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto? response = await _listingService.GetUserListings(userId);
            if (response != null & response.IsSuccess)
            {
                List<ListingDto> Listings = JsonConvert.DeserializeObject<List<ListingDto>>(Convert.ToString(response.Result));
                return Listings;
            }
            TempData["error"] = "Unauthorized";

            return new List<ListingDto>();
        }

        public IActionResult FilteredListings()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FilteredListings(FilterDto filter)
        {
            ResponseDto? response = await _listingService.GetFilteredListings(filter);
            if (response != null && response.IsSuccess)
            {
                List<ListingDto> Listings = JsonConvert.DeserializeObject<List<ListingDto>>(Convert.ToString(response.Result));
                return View(Listings);
            }
            TempData["error"] = response.Message;
            return RedirectToAction("Index");
        }
        
    }
}
