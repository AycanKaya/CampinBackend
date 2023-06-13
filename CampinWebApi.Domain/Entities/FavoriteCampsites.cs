using System.Runtime.CompilerServices;

namespace CampinWebApi.Domain.Entities;

public class FavoriteCampsites
{
    public int Id { get; set; }
    public string userId { get; set; }
    public string campsiteId { get; set; }
}