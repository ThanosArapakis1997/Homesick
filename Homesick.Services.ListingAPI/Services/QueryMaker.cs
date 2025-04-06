using AutoMapper;
using Homesick.Services.ListingAPI.Data;
using Homesick.Services.ListingAPI.Models;
using Homesick.Services.ListingAPI.Models.DTO;
using Homesick.Services.ListingAPI.Services.IService;

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

        public IQueryable<House> MakeQuery(FilterDto filter)
        {
            var query = _db.Houses.AsQueryable();

            if (filter.LowPrice.HasValue)
                query = query.Where(h => h.Price >= filter.LowPrice);

            if (filter.HighPrice.HasValue)
                query = query.Where(h => h.Price <= filter.HighPrice);

            if (!string.IsNullOrEmpty(filter.Area))
                query = query.Where(h => h.Area == filter.Area);

            if (filter.FloorAreaHigh.HasValue)
                query = query.Where(h => h.FloorArea <= filter.FloorAreaHigh);

            if (filter.FloorAreaLow.HasValue)
                query = query.Where(h => h.FloorArea >= filter.FloorAreaLow);

            return query;
        }
    }
}
