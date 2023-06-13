namespace CampinWebApi.Core.DTO.CommentDTO;

public class UpdateUserFeedbackDTO
{
    public string CampsiteId { get; set; }
    public int CommentId { get; set; }
    public string Comment { get; set; }
    public float Rate { get; set; }
    
}