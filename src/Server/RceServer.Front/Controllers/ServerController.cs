using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RceServer.Domain.Services;
using RceServer.Front.Hubs;

namespace RceServer.Front.Controllers
{
	[Authorize(Policy = "RceServerApiAccess")]
	[Route("api/server/")]
	public class ServerController : Controller
	{
		private readonly IClientService _clientService;
		private readonly IHubContext<RceHub> _rceHubContext;

		public ServerController(IClientService clientService, IHubContext<RceHub> rceHubContext)
		{
			_clientService = clientService;
			_rceHubContext = rceHubContext;
		}

		[HttpGet("")]
		public async Task<IActionResult> GetState()
		{
			var messages = await _clientService.GetState();

			return Ok(messages);
		}

		[HttpGet("signalrtest")]
		public async Task<IActionResult> SignalRTest()
		{
			await _rceHubContext.Clients.All.SendAsync("foo", "bar");

			return Ok();
		}
	}
}
