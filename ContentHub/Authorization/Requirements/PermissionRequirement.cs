using Microsoft.AspNetCore.Authorization;

namespace ContentHub.Api.Authorization.Requirements;

public sealed class PermissionRequirement: IAuthorizationRequirement
{
	public PermissionRequirement(string permission)
	{
		Permission = permission;
	}
	
	public string Permission { get; }
}
