using ContentHub.Domain.Abstractions;
using ContentHub.Domain.Common;

namespace ContentHub.Domain.Entities;

/// <summary>
/// Represents a comment submitted for a blog post.
/// </summary>
public class Comment : BaseAuditableEntity, IAuditableEntity
{
    private Comment() { }

    public Comment(string authorName, string content, Guid postId)
    {
        AuthorName = authorName;
        Content = content;
        PostId = postId;
    }

    public string AuthorName { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;

    public Guid PostId { get; private set; }
    public Post Post { get; private set; } = null!;

    public void Update(string content)
    {
        Content = content;        
    }


}
