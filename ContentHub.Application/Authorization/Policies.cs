using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Authorization;

public static class Policies
{
    // Role Policies
    public const string AdminOnly = nameof(AdminOnly);
    public const string AuthorOrAdmin = nameof(AuthorOrAdmin);

    // Permission Policies
    public const string ViewPosts = nameof(ViewPosts);
    public const string CreatePosts = nameof(CreatePosts);
    public const string UpdatePosts = nameof(UpdatePosts);
    public const string DeletePosts = nameof(DeletePosts);
    public const string PublishPosts = nameof(PublishPosts);
}