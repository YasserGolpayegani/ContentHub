using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Authorization;

public static class Permissions
{
    public static class Posts
    {
        public const string View = "Posts.View";

        public const string Create = "Posts.Create";

        public const string Update = "Posts.Update";

        public const string Delete = "Posts.Delete";

        public const string Publish = "Posts.Publish";
    }

    public static class Categories
    {
        public const string View = "Categories.View";

        public const string Create = "Categories.Create";

        public const string Update = "Categories.Update";

        public const string Delete = "Categories.Delete";
    }

    public static class Comments
    {
        public const string View = "Comments.View";

        public const string Create = "Comments.Create";

        public const string Delete = "Comments.Delete";
    }

    public static class Tags
    {
        public const string View = "Tags.View";

        public const string Create = "Tags.Create";

        public const string Update = "Tags.Update";

        public const string Delete = "Tags.Delete";
    }
}
