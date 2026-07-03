using ContentHub.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Domain.Entities;

public class Category : BaseAuditableEntity
{
    private readonly List<Post> _posts = new();

    private Category() { }

    public Category(string name, string slug)
    {
        Name = name;
        Slug = slug;
    }

    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public IReadOnlyCollection<Post> posts => _posts.AsReadOnly();


    public void Rename(string name)
    {
        Name = name;
        MarkAsModified();
    }
}
