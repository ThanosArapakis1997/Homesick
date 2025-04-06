using AutoMapper;
using Homesick.Services.ListingAPI.Models;
using Homesick.Services.ListingAPI.Models.DTO;

namespace Homesick.Services.ListingAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<Listing, ListingDto>().ReverseMap();
                config.CreateMap<House, HouseDto>().ReverseMap();
                config.CreateMap<Image, ImageDto>().ReverseMap();
            });
        }
    }
}
