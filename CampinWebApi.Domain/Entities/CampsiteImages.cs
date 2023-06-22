using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampinWebApi.Domain.Entities;

public class CampsiteImages
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string CampsiteId { get; set; }
    public string ImageUrl { get; set; }
}