using Homesick.Web.Models;

namespace Homesick.Web.Service.IService
{
    public interface IHouseService
    {
        Task<ResponseDto?> GetAllHousesAsync();
        Task<ResponseDto?> CreateHouseAsync(HouseDto houseDto);
        Task<ResponseDto?> UpdateHouseAsync(HouseDto houseDto);
        Task<ResponseDto?> DeleteHouseAsync(int id);
    }
}
