using CampinWebApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CampinWebApi.Domain
{
	public class CampinDbContext : IdentityDbContext<IdentityUser> , ICampinDbContext
    {
        public CampinDbContext(DbContextOptions<CampinDbContext> options)
            : base(options)
        {
        }
        
        public  DbSet<Campsite> Campsites { get; set; }
        public  DbSet<Rezervations> Rezervations { get; set; }
        public  DbSet<HolidayDestination> HolidayDestinations { get; set; }
        public DbSet<City> Cities { get; set; }
        public  DbSet<Comments> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Features> Features { get; set; }
        public DbSet<FavoriteCampsites> FavoriteCampsites { get; set; }
        public DbSet<CampsiteImages> CampsiteImages { get; set; }
        public DbSet<Country> Countries { get; set; }

        public async Task<int> SaveChanges()
        {
          return await base.SaveChangesAsync();
        }
          protected override void OnModelCreating(ModelBuilder modelBuilder)
          {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Campsite>(entity =>
            {
              entity.HasKey(c => c.CampsiteId);
              entity.ToTable(name: "Campsite");
            });
            modelBuilder.Entity<Rezervations>(entity =>
            {
              entity.HasKey(r => r.Id);
              entity.ToTable(name: "Rezervations");
            });
            modelBuilder.Entity<Comments>(entity =>
            {
              entity.HasKey(c => c.Id);
              entity.ToTable(name: "Comments");
            });
            modelBuilder.Entity<HolidayDestination>(entity =>
            {
              entity.HasKey(v => v.Id);
              entity.ToTable(name: "HolidayDestination");
            });
            modelBuilder.Entity<Rating>(entity => 
            {
              entity.HasKey(r => r.RatingId);
              entity.ToTable(name: "Rating");
            });
            modelBuilder.Entity<UserInfo>(entity =>
            {
              entity.HasKey(u => u.UserID);
              entity.ToTable(name: "UserInfo");
            });
            modelBuilder.Entity<Card>(entity =>
            {
              entity.HasKey(c => c.Id);
              entity.ToTable(name: "Card");
            });
            modelBuilder.Entity<City>(entity =>
            {
              entity.HasKey(c => c.Id);
              entity.ToTable(name: "City");
            });
            modelBuilder.Entity<Features>(entity =>
            {
              entity.HasKey(f => f.Id);
              entity.ToTable(name: "Features");
            });
            modelBuilder.Entity<FavoriteCampsites>(entity =>
            {
              entity.HasKey(f => f.Id);
              entity.ToTable(name: "FavoriteCampsites");
            });
            modelBuilder.Entity<CampsiteImages>(entity =>
            {
              entity.HasNoKey();
              entity.ToTable(name: "CampsiteImages");
            });
            modelBuilder.Entity<Country>(entity =>
            {
              entity.HasKey(c => c.Id);
              entity.ToTable(name: "Country");
            });
          }
    }   
}
