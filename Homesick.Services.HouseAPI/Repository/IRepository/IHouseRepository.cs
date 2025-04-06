using Homesick.Services.HouseAPI.Models.DTO;

namespace Homesick.Services.HouseAPI.Repository.IRepository
{
    public interface IHouseRepository
    {
        Task<IEnumerable<HouseDto>> GetAllHouses();
        Task<HouseDto> GetHouse(int? id);
        Task<HouseDto> AddHouse(HouseDto house);
        Task<HouseDto> UpdateHouse(HouseDto house);
        Task<bool> RemoveHouse(int houseId);
    }
}
