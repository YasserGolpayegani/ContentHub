using ContentHub.Api.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace ContentHub.Api.Authorization;

public sealed class PermissionPolicyProvider:DefaultAuthorizationPolicyProvider
{
	public PermissionPolicyProvider(IOptions<AuthorizationOptions> options):base(options)
	{
		
	}

	public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
	{
		AuthorizationPolicy policy= new AuthorizationPolicyBuilder()
			.AddRequirements(new PermissionRequirement(policyName))
			.Build();

		return Task.FromResult<AuthorizationPolicy?>(policy);
	}
}
