namespace Homesick.Services.ListingAPI.Models
{
    public class Image
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Data { get; set; } = string.Empty;
        public int HouseId { get; set; }          // FK
        public House House { get; set; } = null!; // Navigation property
    }
}
