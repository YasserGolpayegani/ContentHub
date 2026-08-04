namespace ContentHub.Api.Models.Users;

public sealed class ChangeUserRolesRequest
{
    public IReadOnlyCollection<string> Roles { get; init; } = [];
}
