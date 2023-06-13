namespace CampinWebApi.Domain.Entities;

public class Rating
{
    public string RatingId { get; set; }
    public string CampsiteId { get; set; }
    public string UserId { get; set; }
    public float Rate { get; set; }
}