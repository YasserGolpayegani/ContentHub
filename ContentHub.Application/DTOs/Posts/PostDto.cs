using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.DTOs.Posts;

public class PostDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public DateTime CreatedOnUtc { get; set; }
}
