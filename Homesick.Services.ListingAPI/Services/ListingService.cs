using AutoMapper;
using Homesick.Services.ListingAPI.Data;
using Homesick.Services.ListingAPI.Models;
using Homesick.Services.ListingAPI.Models.DTO;
using Homesick.Services.ListingAPI.Services.IService;
using Microsoft.EntityFrameworkCore;

namespace Homesick.Services.ListingAPI.Services
{
    public class ListingService : IListingService
    {
        private IMapper _mapper;
        private readonly AppDbContext _db;
        private IQueryMaker _queryMaker;

        public ListingService(IMapper mapper, AppDbContext db, IQueryMaker queryMaker)
        {
            _mapper = mapper;
            _db = db;
            _queryMaker = queryMaker;
        }

        public async Task<List<ListingDto>> GetAllListings()
        {
            var listings = await _db.Listings
                .Include(l => l.House)
                .ThenInclude(h => h.Images) // Include images if needed
                .ToListAsync();
            return _mapper.Map<List<ListingDto>>(listings);
        }

        public async Task<ListingDto> GetListingByIdAsync(int id)
        {
            var listing = await _db.Listings
                .AsNoTracking()
                .Include(l => l.House)
                .ThenInclude(h => h.Images) // Include images if needed
                .FirstOrDefaultAsync(l => l.ListingId == id);
            return _mapper.Map<ListingDto>(listing);
        }

        public async Task<List<ListingDto>> GetListingsByUserIdAsync(string userId)
        {
            List<ListingDto> listings = _mapper.Map<List<ListingDto>>(
                _db.Listings.Where(l => l.UserId == userId)
                .Include(l => l.House).
                ThenInclude(h => h.Images)).ToList();

            return listings;
        }

        public async Task<List<ListingDto>> GetFilteredListingsAsync(FilterDto filter)
        {
            IQueryable<Listing> listingQuery = _queryMaker.MakeQuery(filter);

            var listings = await _db.Listings
                .Where(l => listingQuery.Any(h => h.HouseId == l.HouseId)) // Ensure only matching houses are included
                .Include(l => l.House)
                .ThenInclude(h => h.Images)  // Include images if needed
                .ToListAsync();

            return _mapper.Map<List<ListingDto>>(listings);
        }


        public async Task<ListingDto> CreateListingAsync(ListingDto listing)
        {
            var listingEntity = _mapper.Map<Listing>(listing);

            // Ensure house exists and map images correctly
            if (listing.House != null)
            {
                listingEntity.House = _mapper.Map<House>(listing.House);
            }

            await _db.Listings.AddAsync(listingEntity);

            await _db.SaveChangesAsync();

            return _mapper.Map<ListingDto>(listingEntity);
        }

        public async Task<ListingDto> UpdateListingAsync(ListingDto listing)
        {
            var listingEntity = _mapper.Map<Listing>(listing);

            // Ensure house exists and map images correctly
            if (listing.House != null)
            {
                listingEntity.House = _mapper.Map<House>(listing.House);
            }

            var updatedImageIds = listing.House.Images.Select(i => i.Id).ToList();

            // Remove images not present in DTO
            var imagesToDelete = _db.HouseImages.Where(img =>img.HouseId==listing.HouseId && !updatedImageIds.Contains(img.Id)).ToList();

            foreach (var image in imagesToDelete)
            {
                _db.HouseImages.Remove(image); // explicitly remove from DbSet
            }

            _db.Listings.Update(listingEntity);

            await _db.SaveChangesAsync();

            return _mapper.Map<ListingDto>(listingEntity);
        }

        public async Task<ListingDto> DeleteListingAsync(ListingDto listing)
        {
            var listingEntity = _mapper.Map<Listing>(listing);
            
            _db.Listings.Remove(listingEntity);
            _db.Houses.Remove(listingEntity.House); // Remove the associated house if needed
            await _db.SaveChangesAsync();

            return _mapper.Map<ListingDto>(listingEntity);
        }      
        

    }
}
