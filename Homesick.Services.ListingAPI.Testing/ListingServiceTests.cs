using AutoMapper;
using Homesick.Services.ListingAPI.Controllers;
using Homesick.Services.ListingAPI.Data;
using Homesick.Services.ListingAPI.Models;
using Homesick.Services.ListingAPI.Models.DTO;
using Homesick.Services.ListingAPI.Services;
using Homesick.Services.ListingAPI.Services.IService;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Homesick.Services.ListingAPI.Testing
{
    public class ListingServiceTests
    {

        private readonly ListingAPIController _controller;
        private readonly Mock<IListingService> _mockListingService;
        private readonly Mock<IMapper> _mockMapper;

        public ListingServiceTests()
        {
            _mockListingService = new Mock<IListingService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new ListingAPIController(_mockMapper.Object, _mockListingService.Object);
        }

        [Fact]
        public async Task GetAllListings_Returns_Listings_WhenExist()
        {
            // Arrange
            var fakeListings = new List<ListingDto>
            {
                new ListingDto { ListingId = 1, ListingType = "Ενοικίαση" },
                new ListingDto { ListingId = 2, ListingType = "Αγορά" }
            };

            _mockListingService.Setup(s => s.GetAllListings()).ReturnsAsync(fakeListings);

            // Act
            var result = await _controller.GetAllListings();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task GetListing_Returns_NotFound_WhenIdIsInvalid()
        {
            // Arrange
            int listingId = 99;
            _mockListingService.Setup(s => s.GetListingByIdAsync(listingId)).ReturnsAsync((ListingDto)null);

            // Act
            var result = await _controller.GetListing(listingId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("No Listings Found", result.Message);
        }

        [Fact]
        public async Task GetFilteredListings_Returns_Listings()
        {
            // Arrange
            var fakeFilter = new FilterDto { ListingType = "Ενοικίαση" };
            var fakeListings = new List<ListingDto>
            {
                new ListingDto { ListingId = 1, ListingType = "Ενοικίαση" },
                new ListingDto { ListingId = 2, ListingType = "Ενοικίαση" }
            };

            _mockListingService.Setup(s => s.GetFilteredListingsAsync(fakeFilter)).ReturnsAsync(fakeListings);

            // Act
            var result = await _controller.GetFilteredListings(fakeFilter);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task GetUserListings_Returns_NotFound_WhenIdIsInvalid()
        {
            // Arrange
            string userId = "99";

            _mockListingService.Setup(s => s.GetListingsByUserIdAsync(userId)).ReturnsAsync((List<ListingDto>)null);

            // Act
            var result = await _controller.GetUserListings(userId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("No Listings Found", result.Message);
        }

        [Fact]
        public async Task GetUserListings_Returns_Listings_WhenUserExists()
        {
            // Arrange
            string userId = "user123";
            var listings = new List<ListingDto>
            {
                new ListingDto { ListingId = 1, UserId = userId },
                new ListingDto { ListingId = 2, UserId = userId }
            };

            _mockListingService.Setup(s => s.GetListingsByUserIdAsync(userId)).ReturnsAsync(listings);

            // Act
            var result = await _controller.GetUserListings(userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            var resultList = Assert.IsType<List<ListingDto>>(result.Result);
            Assert.Equal(userId, resultList.First().UserId);
        }

        [Fact]
        public async Task CreateListing_Returns_CreatedListing()
        {
            // Arrange
            var listingToCreate = new ListingDto { Name = "New Listing" };
            var createdListing = new ListingDto { ListingId = 1, Name = "New Listing" };

            _mockListingService.Setup(s => s.CreateListingAsync(listingToCreate)).ReturnsAsync(createdListing);

            // Act
            var result = await _controller.CreateListing(listingToCreate);

            // Assert
            Assert.True(result.IsSuccess);
            var resultListing = Assert.IsType<ListingDto>(result.Result);
            Assert.Equal("New Listing", resultListing.Name);
        }

        [Fact]
        public async Task UpdateListing_Returns_UpdatedListing()
        {
            // Arrange
            var listingToUpdate = new ListingDto { Name = "Edit Listing" };
            var updatedListing = new ListingDto { ListingId = 1, Name = "Edited Listing" };

            _mockListingService.Setup(s => s.CreateListingAsync(listingToUpdate)).ReturnsAsync(updatedListing);

            // Act
            var result = await _controller.CreateListing(listingToUpdate);

            // Assert
            Assert.True(result.IsSuccess);
            var resultListing = Assert.IsType<ListingDto>(result.Result);
            Assert.Equal("Edited Listing", resultListing.Name);
        }

        [Fact]
        public async Task DeleteListing_Returns_Success_WhenListingExists()
        {
            // Arrange
            int listingId = 1;
            var listing = new ListingDto { ListingId = listingId, Name = "To Delete" };

            _mockListingService.Setup(s => s.GetListingByIdAsync(listingId)).ReturnsAsync(listing);
            _mockListingService.Setup(s => s.DeleteListingAsync(listing)).ReturnsAsync(listing); // Fix: Ensure DeleteListingAsync returns Task<ListingDto>

            // Act
            var result = await _controller.DeleteListing(listingId);

            // Assert
            Assert.True(result.IsSuccess);
            ListingDto deletedListing = Assert.IsType<ListingDto>(result.Result);
            Assert.Equal(listingId, deletedListing.ListingId);
        }
    }

}