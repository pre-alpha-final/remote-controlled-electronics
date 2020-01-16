using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RceServer.Domain.Services;
using RceServer.Front.Controllers.Models;

namespace RceServer.Front.Controllers
{
	[Route("api/")]
	public class WorkerController : Controller
	{
		private readonly IWorkerService _workerService;

		public WorkerController(IWorkerService workerService)
		{
			_workerService = workerService;
		}

		[HttpPost("workers/register")]
		public async Task<IActionResult> Register([FromBody] RegisterWorkerModel registerWorkerModel)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(registerWorkerModel.Base64Logo) == false &&
					registerWorkerModel.Base64Logo.Length > 30000)
				{
					throw new Exception("Image too large");
				}

				var workerId = await _workerService.Register(registerWorkerModel.Name, registerWorkerModel.Description,
					registerWorkerModel.Base64Logo, registerWorkerModel.JobDescriptions);
				return Ok(workerId);
			}
			catch (Exception e)
			{
				return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
			}
		}

		[HttpGet("workers/{workerId}/jobs/{maxCount?}")]
		public async Task<IActionResult> GetJobs(Guid workerId, int? maxCount)
		{
			try
			{
				var jobAddedMessages = await _workerService.GetJobs(workerId, maxCount);

				return Ok(jobAddedMessages.Select(e => new JobDto
				{
					JobId = e.JobId,
					JobName = e.Name,
					Payload = e.Payload
				}));
			}
			catch (Exception e)
			{
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
				return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
			}
		}
	}
}
