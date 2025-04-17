using Homesick.Services.ListingAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Homesick.Services.ListingAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Listing> Listings { get; set; }
        public DbSet<House> Houses { get; set; }

        public DbSet<Image> HouseImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var stringListConverter = new ValueConverter<List<string>, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),  // Serialize to JSON
            v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) // Deserialize from JSON
            );

            // Apply the converter to the ImagePaths property
            modelBuilder.Entity<House>()
                .Property(p => p.Imagepaths)
                .HasConversion(stringListConverter);

            // Listing -> House (Cascade delete)
            modelBuilder.Entity<Listing>()
                .HasOne(l => l.House)
                .WithOne()
                .HasForeignKey<Listing>(l => l.HouseId)
                .OnDelete(DeleteBehavior.Cascade);

            // House -> HouseImages (Cascade delete)
            modelBuilder.Entity<House>()
                .HasMany(h => h.Images)
                .WithOne(img => img.House)
                .HasForeignKey(img => img.HouseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
