using CampinWebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CampinWebApi.Domain
{
	public interface ICampinDbContext 
	{
        DbSet<Campsite> Campsites { get; set; }
        DbSet<Rezervations> Rezervations { get; set; }
        DbSet<HolidayDestination> HolidayDestinations { get; set; }
        DbSet<City> Cities { get; set; }
        DbSet<Comments> Comments { get; set; }
        DbSet<Rating> Ratings { get; set; }
        DbSet<UserInfo> UserInfo { get; set; }
		DbSet<Card> Cards { get; set; } 
		DbSet<Features> Features { get; set; }
		DbSet<FavoriteCampsites> FavoriteCampsites { get; set; }
		DbSet<CampsiteImages> CampsiteImages { get; set; }
		DbSet<Country> Countries { get; set; }
		Task<int> SaveChanges();
    }
}