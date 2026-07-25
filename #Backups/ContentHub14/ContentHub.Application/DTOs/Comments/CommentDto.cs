namespace ContentHub.Application.DTOs.Comments;

public class CommentDto
{
    public Guid Id { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public Guid PostId { get; set; }
}
