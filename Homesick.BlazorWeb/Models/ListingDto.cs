namespace Homesick.BlazorWeb.Models
{
    public class ListingDto
    {
        public int ListingId { get; set; }
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? ListingType { get; set; }
        public string? Phone { get; set; }
        public int HouseId { get; set; }
        public HouseDto? House { get; set; }
        public string? Status { get; set; }

    }
}
