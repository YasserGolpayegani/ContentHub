using ContentHub.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Domain.Entities;

public sealed class Post : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public bool IsPublished { get; private set; }

    private Post()
    {
    }

    public Post(string title,string content,string slug)
    {
        Title = title;
        Content = content;
        Slug = slug;

        IsPublished = false;            
    }

    public void Publish()
    {
        IsPublished = true;
    }

    public void Update(string title,string content)
    {
        Title = title;
        Content = content;
    }
}
