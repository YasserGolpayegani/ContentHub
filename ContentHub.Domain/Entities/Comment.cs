using ContentHub.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Domain.Entities;

public class Comment:BaseAuditableEntity
{
    private Comment() { }

	public Comment(string authorName, string content)
	{
		AuthorName = authorName;
		Content = content;
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
