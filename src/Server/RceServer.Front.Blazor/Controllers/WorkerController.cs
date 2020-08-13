﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RceServer.Domain.Services;
using RceServer.Front.Blazor.Models;
using RceServer.Front.Blazor.Infrastructure;

namespace RceServer.Front.Blazor.Controllers
{
	[Route("api/")]
	public class WorkerController : Controller
	{
		private const int ErrorTimeoutMilliseconds = 3000; // to prevent badly written workers from spamming requests
		private readonly IWorkerService _workerService;
		private readonly TelemetryClient _telemetryClient;

		public WorkerController(IWorkerService workerService, TelemetryClient telemetryClient)
		{
			_workerService = workerService;
			_telemetryClient = telemetryClient;
		}

		[HttpPost("workers/register")]
		public async Task<IActionResult> Register([FromBody] RegisterWorkerModel registerWorkerModel)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(registerWorkerModel.Base64Logo) == false &&
					registerWorkerModel.Base64Logo.Length > 50000 * 1.37)
				{
					throw new Exception("Image too large");
				}

				var workerId = await _workerService.Register(registerWorkerModel.Name, registerWorkerModel.Description,
					registerWorkerModel.Base64Logo, registerWorkerModel.JobDescriptions, registerWorkerModel.Owners);

				_telemetryClient.TrackEvent(TelemetryEvents.RegisteringWorker, new Dictionary<string, string>
				{
					{ nameof(workerId), workerId.ToString() },
					{ nameof(registerWorkerModel.Name), registerWorkerModel.Name },
					{ nameof(registerWorkerModel.Description), registerWorkerModel.Description },
					{ nameof(registerWorkerModel.JobDescriptions), JsonConvert.SerializeObject(registerWorkerModel.JobDescriptions) },
					{ nameof(registerWorkerModel.Owners), JsonConvert.SerializeObject(registerWorkerModel.Owners) },
				});

				return Ok(workerId);
			}
			catch (Exception e)
			{
				await Task.Delay(ErrorTimeoutMilliseconds);
				return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
			}
		}

		[HttpGet("workers/{workerId}/jobs/{maxCount?}")]
		public async Task<IActionResult> GetJobs(Guid workerId, int? maxCount)
		{
			try
			{
				var jobAddedMessages = await _workerService.GetJobs(workerId, maxCount);

				_telemetryClient.TrackEvent(TelemetryEvents.WorkerTakingJob, new Dictionary<string, string>
				{
					{ nameof(workerId), workerId.ToString() },
					{ nameof(jobAddedMessages), JsonConvert.SerializeObject(jobAddedMessages) },
				});

				return Ok(jobAddedMessages.Select(e => new JobDto
				{
					JobId = e.JobId,
					JobName = e.Name,
					Payload = e.Payload
				}));
			}
			catch (Exception e)
			{
				await Task.Delay(ErrorTimeoutMilliseconds);
				return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
			}
		}

		[HttpPost("workers/{workerId}/jobs/{jobId}/update")]
		public async Task<IActionResult> UpdateJob(Guid workerId, Guid jobId, [FromBody] JObject output)
		{
			try
			{
				await _workerService.UpdateJob(workerId, jobId, output);
				return Ok();
			}
			catch (Exception e)
			{
				await Task.Delay(ErrorTimeoutMilliseconds);
				return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
			}
		}

		[HttpPost("workers/{workerId}/jobs/{jobId}/complete")]
		public async Task<IActionResult> CompleteJob(Guid workerId, Guid jobId, [FromBody] JObject output)
		{
			try
			{
				await _workerService.CompleteJob(workerId, jobId, output);
				return Ok();
			}
			catch (Exception e)
			{
				await Task.Delay(ErrorTimeoutMilliseconds);
				return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
			}
		}

		[HttpPost("workers/{workerId}/close")]
		public async Task<IActionResult> CloseWorker(Guid workerId)
		{
			try
			{
				await _workerService.CloseWorker(workerId);
				return Ok();
			}
			catch (Exception e)
			{
				await Task.Delay(ErrorTimeoutMilliseconds);
				return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
			}
		}
	}
}
