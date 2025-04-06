using Homesick.Services.HouseAPI.Models.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Homesick.Services.HouseAPI.Models
{
    public class House
    {
        [Key]
        public int HouseId { get; set; }
        [Required]
        public string Name { get; set; }
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
    }
}
