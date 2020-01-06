using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RceServer.Domain.Models.Messages;
using RceServer.Domain.Services;
using RceServer.Front.Hubs;

namespace RceServer.Front.Controllers
{
	[Authorize(Policy = "RceServerApiAccess")]
	[Route("api/server/")]
	public class ServerController : Controller
	{
		private readonly IClientService _clientService;
		private readonly IHubContext<RceHub, IRceHub> _rceHubContext;

		public ServerController(IClientService clientService, IHubContext<RceHub, IRceHub> rceHubContext)
		{
			_clientService = clientService;
			_rceHubContext = rceHubContext;
		}

		[HttpGet("")]
		public async Task<IActionResult> GetState()
		{
			//var messages = await _clientService.GetState();

			//return Ok(messages);

			await _rceHubContext.Clients.All.MessageReceived(new KeepAliveSentMessage());
			await Task.Delay(1000);
			return Ok(new List<IRceMessage>
			{
				new WorkerAddedMessage(),
				new JobAddedMessage(),
				new JobPickedUpMessage(),
				new JobUpdatedMessage(),
				new JobCompletedMessage(),
			});
		}
	}
}
