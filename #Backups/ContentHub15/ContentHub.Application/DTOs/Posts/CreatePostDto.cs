namespace ContentHub.Application.DTOs.Posts;

public class CreatePostDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
}
