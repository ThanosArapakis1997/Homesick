using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;
using Homesick.Services.ListingAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Homesick.Services.ListingAPI.Models
{
    public class Listing
    {
        [Key]
        public int ListingId { get; set; }

        public string? UserId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? ListingType { get; set; }
        public string? Phone { get; set; }
        [Required]
        public string? Email { get; set; }

        public int HouseId { get; set; }

        [ForeignKey("HouseId")]
        [ValidateNever]
        public House? House { get; set; }

        public string? Status { get; set; }

    }
}
