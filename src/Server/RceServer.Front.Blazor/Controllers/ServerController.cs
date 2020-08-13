using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RceServer.Core.Helpers;
using RceServer.Domain.Services;
using RceServer.Front.Blazor.Models;
using RceServer.Front.Blazor.Infrastructure;

namespace RceServer.Front.Blazor.Controllers
{
	[Authorize(Policy = "RceServerApiAccess")]
	[Route("api/server/")]
	public class ServerController : Controller
	{
		private readonly IServerService _serverService;
		private readonly TelemetryClient _telemetryClient;

		public ServerController(IServerService serverService, TelemetryClient telemetryClient)
		{
			_serverService = serverService;
			_telemetryClient = telemetryClient;
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
				_telemetryClient.TrackEvent(TelemetryEvents.SchedulingJob, new Dictionary<string, string>
				{
					{ nameof(workerId), workerId.ToString() },
					{ nameof(runJobModel.JobName), runJobModel.JobName },
					{ nameof(runJobModel.JobPayload), runJobModel.JobPayload },
				});

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
