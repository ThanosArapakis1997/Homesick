using Homesick.Services.ListingAPI.Models;
using Homesick.Services.ListingAPI.Models.DTO;

namespace Homesick.Services.ListingAPI.Services.IService
{
    public interface IQueryMaker
    {
        IQueryable<Listing> MakeQuery(FilterDto filter);
    }
}
