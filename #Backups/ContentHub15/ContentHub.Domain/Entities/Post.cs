using ContentHub.Domain.Common;

namespace ContentHub.Domain.Entities;

public sealed class Post : BaseAuditableEntity
{
    public string Title { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public bool IsPublished { get; private set; }

    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;

    private readonly List<Tag> _tags = new();
    private readonly List<Comment> _comments = new();

    public IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();
    public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

    private Post()
    {
    }

    public Post(string title, string content, string slug, Guid categoryId)
    {
        Title = title;
        Content = content;
        Slug = slug;
        IsPublished = false;
        CategoryId = categoryId;
    }

    public void Publish()
    {
        IsPublished = true;
    }

    public void Update(string title, string content,string slug, Guid categoryId)
    {
        Title = title;
        Content = content;
        Slug = slug;
        CategoryId = categoryId;

        //maybe it is not require enymore
        MarkAsModified();
    }
}
