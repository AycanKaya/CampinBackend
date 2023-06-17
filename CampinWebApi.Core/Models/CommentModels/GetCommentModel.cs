namespace CampinWebApi.Core.Models.CommentModels;

public class GetCommentModel
{
    public string Content { get; set; }
    public string AuthorName { get; set; }
    public string AuthorSurname { get; set; }
    public float UserRate { get; set; }
    public DateTime Created { get; set; }
}