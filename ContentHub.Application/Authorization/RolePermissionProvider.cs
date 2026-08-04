using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;

namespace ContentHub.Application.Authorization;

public static class RolePermissionProvider
{
    private static readonly Dictionary<string, HashSet<string>> Permissions = new()
    {
        [Roles.Admin] = new HashSet<string>()
        {
            PermissionNames.Posts.Read,
            PermissionNames.Posts.Create,
            PermissionNames.Posts.Update,
            PermissionNames.Posts.Delete,
            PermissionNames.Posts.Publish,

            PermissionNames.Categories.Read,
            PermissionNames.Categories.Create,
            PermissionNames.Categories.Update,
            PermissionNames.Categories.Delete,

            PermissionNames.Comments.Read,
            PermissionNames.Comments.Create,
            PermissionNames.Comments.Delete,

            PermissionNames.Tags.Read,
            PermissionNames.Tags.Create,
            PermissionNames.Tags.Update,
            PermissionNames.Tags.Delete,

            PermissionNames.Users.Read,
            PermissionNames.Users.Update,
            PermissionNames.Users.ChangeRole,
            PermissionNames.Users.Activate            
        },

        [Roles.Author] = new HashSet<string>()
        {
            PermissionNames.Posts.Read,
            PermissionNames.Posts.Create,
            PermissionNames.Posts.Update,

            PermissionNames.Comments.Read,
            PermissionNames.Comments.Create
        }
    };


    public static IReadOnlyCollection<string> GetPermission(IEnumerable<string> roles)
    {
        HashSet<string> permissions = new HashSet<string>();

        foreach (string role in roles)
        {
            if (!Permissions.TryGetValue(role, out var rolePermissions))
            {
                continue;
            }

            permissions.UnionWith(rolePermissions);
        }

        return permissions;
    }
}
