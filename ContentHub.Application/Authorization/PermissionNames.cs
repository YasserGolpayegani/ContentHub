using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Authorization;

/// <summary>
/// Contains all application permissions.
/// </summary>
public static class PermissionNames
{
    public static class Posts
    {
        public const string Read = "Posts.Read";
        public const string Create = "Posts.Create";
        public const string Update = "Posts.Update";
        public const string Delete = "Posts.Delete";
        public const string Publish = "Posts.Publish";
    }

    public static class Categories
    {
        public const string Read = "Categories.Read";
        public const string Create = "Categories.Create";
        public const string Update = "Categories.Update";
        public const string Delete = "Categories.Delete";
    }

    public static class Comments
    {
        public const string Read = "Comments.Read";
        public const string Create = "Comments.Create";
        public const string Delete = "Comments.Delete";
    }

    public static class Tags
    {
        public const string Read = "Tags.Read";
        public const string Create = "Tags.Create";
        public const string Update = "Tags.Update";
        public const string Delete = "Tags.Delete";
    }

    public static class Users
    {
        public const string Read = "Users.Read";
        public const string Update = "Users.Update";
        public const string ChangeRole = "Users.ChangeRole";
        public const string Activate = "Users.Activate";
    }
}
