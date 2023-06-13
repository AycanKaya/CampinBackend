using System;
namespace CampinWebApi.Domain.Entities
{
	public class Comments
	{
		public int Id { get; set; }
		public string CampsiteId { get; set; }
		public string AuthorId { get; set; }
		public string Content { get; set; }
		public string AuthorName { get; set; }
		public DateTime Created { get; set; }
		public bool IsDeleted { get; set; }
    }
}

