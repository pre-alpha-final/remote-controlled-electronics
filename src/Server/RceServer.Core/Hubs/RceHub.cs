using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace RceServer.Core.Hubs
{
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Authorize(Policy = "RceServerApiAccess")]
	public class RceHub : Hub<IRceHub>
	{
	}
}
