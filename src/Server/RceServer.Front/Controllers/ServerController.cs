using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RceServer.Core.Helpers;
using RceServer.Domain.Services;
using RceServer.Front.Controllers.Models;

namespace RceServer.Front.Controllers
{
	[Authorize(Policy = "RceServerApiAccess")]
	[Route("api/server/")]
	public class ServerController : Controller
	{
		private readonly IServerService _serverService;

		public ServerController(IServerService serverService)
		{
			_serverService = serverService;
		}

		[HttpGet("messages")]
		public async Task<IActionResult> GetMessages()
		{
			try
			{
				var messages = await _serverService.GetMyMessages();
				return Ok(RceMessageHelpers.Minimize(messages.ToList()));
			}
			catch (Exception e)
			{
				return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
			}
		}

		[HttpPost("workers/{workerId}/runjob")]
		public async Task<IActionResult> RunJob(Guid workerId, [FromBody] RunJobModel runJobModel)
		{
			try
			{
				await _serverService.RunJob(workerId, runJobModel.JobName, runJobModel.JobPayload);
				return Ok();
			}
			catch (Exception e)
			{
				return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
			}
		}

		[HttpPost("workers/{workerId}/jobs/{jobId}/remove")]
		public async Task<IActionResult> RemoveJob(Guid workerId, Guid jobId)
		{
			try
			{
				await _serverService.RemoveJob(workerId, jobId);
				return Ok();
			}
			catch (Exception e)
			{
				return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
			}
		}
	}
}
