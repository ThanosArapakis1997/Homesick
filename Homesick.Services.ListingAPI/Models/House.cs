using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Homesick.Services.ListingAPI.Models
{
    public class House
    {
        [Key]
        public int HouseId { get; set; }
        [Required]
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
        public string? PropertyType { get; set; }
        public string? HouseType { get; set; }
        public List<string>? Imagepaths { get; set; }
        public List<Image>? Images { get; set; }
    }
}
