using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Homesick.Services.AuthAPI.Models
{
    public class ApplicationUser: IdentityUser
    {

        [Required]
        public required string Name { get; set; }

        [NotMapped]
        public string? Role { get; set; }

    }
}
