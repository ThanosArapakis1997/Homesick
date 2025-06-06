﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Homesick.Services.ListingAPI.Models.DTO
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
        public string? PropertyType { get; set; }
        public string? HouseType { get; set; }
        public List<string>? ImagePaths { get; set; }
        public List<ImageDto>? Images { get; set; }
    }
}
