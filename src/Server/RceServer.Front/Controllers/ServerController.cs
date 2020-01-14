using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RceServer.Domain.Services;
using RceServer.Front.Controllers.Models;

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
			var messages = await _clientService.GetMessages();

			return Ok(messages);
		}

		[HttpPost("workers/{workerId}/runjob")]
		public async Task<IActionResult> RunJob(Guid workerId, [FromBody] RunJobModel runJobModel)
		{

			return Ok();
		}

		[HttpPost("workers/{workerId}/jobs/{jobId}/remove")]
		public async Task<IActionResult> RemoveJob(Guid workerId, Guid jobId)
		{

			return Ok();
		}
	}
}
