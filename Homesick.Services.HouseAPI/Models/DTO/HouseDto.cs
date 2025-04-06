using System.ComponentModel.DataAnnotations;

namespace Homesick.Services.HouseAPI.Models.DTO
{
    public class HouseDto
    {
        public int HouseId { get; set; }
        public string Name { get; set; }
        public int ListingId { get; set; }
        public double Price { get; set; }
        public string? Description { get; set; }
        public string? City { get; set; }
        public string? Area { get; set; }
        public string? Address { get; set; }
        public int? Floor { get; set; }
        public int? Rooms { get; set; }
        public int? FloorArea { get; set; }
        public int? ConstructionYear { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }
        public IFormFile? Image { get; set; }

    }
}
