using AutoMapper;
using Homesick.Services.HouseAPI.Models;
using Homesick.Services.HouseAPI.Models.DTO;

namespace Homesick.Services.HouseAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<House, HouseDto>().ReverseMap();
            });
        }
    }
}
