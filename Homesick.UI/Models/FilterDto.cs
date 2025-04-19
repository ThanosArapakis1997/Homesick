﻿namespace Homesick.UI.Models
{
    public class FilterDto
    {
        public double? HighPrice { get; set; }
        public double? LowPrice { get; set; }
        public int? FloorAreaLow { get; set; }
        public int? FloorAreaHigh { get; set; }
        public string? Area { get; set; }
        public string? ListingType { get; set; }

    }
}
