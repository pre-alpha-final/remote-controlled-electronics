using Microsoft.AspNetCore.SignalR;

namespace RceServer.Front.Blazor.Infrastructure
{
	public class UsernameIdProvider : IUserIdProvider
	{
		public string GetUserId(HubConnectionContext connection)
		{
			return connection.User?.FindFirst("username")?.Value;
		}
	}
}
