using AutoMapper;
using Azure;
using Homesick.Services.HouseAPI.Data;
using Homesick.Services.HouseAPI.Models;
using Homesick.Services.HouseAPI.Models.DTO;
using Homesick.Services.HouseAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Homesick.Services.HouseAPI.Repository
{
    public class HouseRepository : IHouseRepository
    {
        private readonly AppDbContext _db;
        private IMapper _mapper;

        public HouseRepository(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<HouseDto> AddHouse(HouseDto houseDto)
        {
            House house = _mapper.Map<HouseDto, House>(houseDto);
            await _db.Houses.AddAsync(house);
            await _db.SaveChangesAsync();

            return _mapper.Map<House, HouseDto>(house);
        }

        public async Task<IEnumerable<HouseDto>> GetAllHouses()
        {
            List<House> houses = await _db.Houses.ToListAsync();
            return _mapper.Map<List<HouseDto>>(houses);
        }



        public async Task<HouseDto> GetHouse(int? houseId)
        {
            House house = await _db.Houses.FirstOrDefaultAsync(m => m.HouseId == houseId);
            return _mapper.Map<HouseDto>(house);
        }

        public async Task<bool> RemoveHouse(int houseId)
        {
            try
            {
                House House = await _db.Houses.FirstOrDefaultAsync(u => u.HouseId == houseId);

                if (House == null)
                {
                    return false;
                }

                _db.Houses.Remove(House);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<HouseDto> UpdateHouse(HouseDto houseDto)
        {
            House house = _mapper.Map<HouseDto, House>(houseDto);
             _db.Houses.Update(house);
            await _db.SaveChangesAsync();

            return _mapper.Map<House, HouseDto>(house);
        }
    }
}
