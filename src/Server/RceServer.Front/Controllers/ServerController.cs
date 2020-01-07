using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RceServer.Domain.Services;

namespace RceServer.Front.Controllers
{
	[Authorize(Policy = "RceServerApiAccess")]
	[Route("api/server/")]
	public class ServerController : Controller
	{
		private readonly IClientService _clientService;

		public ServerController(IClientService clientService)
		{
			_clientService = clientService;
		}

		[HttpGet("")]
		public async Task<IActionResult> GetState()
		{
			var messages = await _clientService.GetState();

			return Ok(messages);
		}
	}
}
