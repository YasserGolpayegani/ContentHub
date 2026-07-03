using ContentHub.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Domain.Entities;

public class Tag : BaseAuditableEntity
{
    private readonly List<Post> _posts = new();

    private Tag() { }

    public Tag(string name)
    {
        Name = name;
    }

    public string Name { get; private set; } = string.Empty;
    public IReadOnlyCollection<Post> Posts => _posts.AsReadOnly();

    public void Rename(string name)
    {
        Name = name;
    }
}
