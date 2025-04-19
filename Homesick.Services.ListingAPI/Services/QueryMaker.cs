using AutoMapper;
using Homesick.Services.ListingAPI.Data;
using Homesick.Services.ListingAPI.Models;
using Homesick.Services.ListingAPI.Models.DTO;
using Homesick.Services.ListingAPI.Services.IService;
using Homesick.Services.ListingAPI.Utility;

namespace Homesick.Services.ListingAPI.Services
{
    public class QueryMaker : IQueryMaker
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public QueryMaker(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public IQueryable<Listing> MakeQuery(FilterDto filter)
        {
            IQueryable<Listing> query = _db.Listings.AsQueryable();

            query = query.Where(l=> l.Status == SD.StatusApproved);

            if (filter.LowPrice.HasValue)
                query = query.Where(h => h.House.Price >= filter.LowPrice);

            if (filter.HighPrice.HasValue)
                query = query.Where(h => h.House.Price <= filter.HighPrice);

            if (!string.IsNullOrEmpty(filter.Area))
                query = query.Where(h => h.House.Area == filter.Area);

            if (!string.IsNullOrEmpty(filter.ListingType))
                query = query.Where(l => l.ListingType == filter.ListingType);

            if (filter.FloorAreaHigh.HasValue)
                query = query.Where(h => h.House.FloorArea <= filter.FloorAreaHigh);

            if (filter.FloorAreaLow.HasValue)
                query = query.Where(h => h.House.FloorArea >= filter.FloorAreaLow);

            return query;
        }
    }
}
