using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.DTOs.Tags;

public class TagDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
