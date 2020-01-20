using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace RceServer.Data.Identity
{
	/*
	 * This service is optional, used to manipulate returned token
	 */
	public class IdentityClaimsProfileService : IProfileService
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IUserClaimsPrincipalFactory<IdentityUser> _claimsFactory;

		public IdentityClaimsProfileService(UserManager<IdentityUser> userManager,
			IUserClaimsPrincipalFactory<IdentityUser> claimsFactory)
		{
			_userManager = userManager;
			_claimsFactory = claimsFactory;
		}

		public async Task GetProfileDataAsync(ProfileDataRequestContext context)
		{
			var subjectId = context.Subject.GetSubjectId();
			var user = await _userManager.FindByIdAsync(subjectId);
			var principal = await _claimsFactory.CreateAsync(user);
			var roles = await _userManager.GetRolesAsync(user);
			var claims = principal.Claims.ToList();

			// Magic additional clamis here
			claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
			claims.AddRange(roles.Select(role => new Claim(JwtClaimTypes.Role, role)));
			claims.Add(new Claim("username", user.UserName));
			//claims.Add(new Claim(JwtClaimTypes.Email, ""));
			//claims.Add(new Claim(IdentityServerConstants.StandardScopes.Email, ""));

			context.IssuedClaims = claims;
		}

		public async Task IsActiveAsync(IsActiveContext context)
		{
			var sub = context.Subject.GetSubjectId();
			var user = await _userManager.FindByIdAsync(sub);
			context.IsActive = user != null;
		}
	}
}
