using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RceServer.Domain.Models;
using RceServer.Domain.Models.Messages;
using RceServer.Domain.Services;

namespace RceServer.Core.Services.Implementation
{
	public class WorkerService : IWorkerService
	{
		private readonly IMessageRepository _messageRepository;

		public WorkerService(IMessageRepository messageRepository)
		{
			_messageRepository = messageRepository;
		}

		public async Task<Guid> Register(string name, string description,
			string base64Logo, List<JobDescription> jobDescriptions)
		{
			VerifyJobNamesUnique(jobDescriptions);

			var workerId = Guid.NewGuid();
			await _messageRepository.AddMessage(new WorkerAddedMessage
			{
				WorkerId = workerId,
				Name = name,
				Description = description,
				Base64Logo = base64Logo,
				JobDescriptions = jobDescriptions
			});

			return workerId;
		}

		public async Task<List<JobAddedMessage>> GetJobs(Guid workerId, int? maxCount)
		{
			await _messageRepository.AddMessage(new KeepAliveSentMessage
			{
				WorkerId = workerId,
				Reason = KeepAliveSentMessage.Reasons.GetJobs
			});

			var workerMessages = await _messageRepository.GetWorkerMessages(workerId);

			if (workerMessages.Any(e => e is WorkerRemovedMessage))
			{
				throw new Exception("Worker disconnected");
			}

			var irrelevantJobs = workerMessages
				.Where(e => e is JobCompletedMessage || e is JobRemovedMessage)
				.Select(e => (IHasJobId)e);

			var jobs = workerMessages
				.OfType<JobAddedMessage>()
				.Where(e => irrelevantJobs.Any(f => f.JobId == e.JobId) == false)
				.Take(maxCount ?? 10)
				.ToList();

			foreach (var jobAddedMessage in jobs)
			{
				await _messageRepository.AddMessage(new JobPickedUpMessage
				{
					JobId = jobAddedMessage.JobId,
					WorkerId = jobAddedMessage.WorkerId
				});
			}

			return jobs;
		}

		public async Task UpdateJob(Guid workerId, Guid jobId, JObject output)
		{
			await _messageRepository.AddMessage(new JobUpdatedMessage
			{
				WorkerId = workerId,
				JobId = jobId,
				Output = output
			});
		}

		public async Task CompleteJob(Guid workerId, Guid jobId, JObject output)
		{
			await _messageRepository.AddMessage(new JobCompletedMessage
			{
				WorkerId = workerId,
				JobId = jobId,
				Output = output
			});
		}

		private void VerifyJobNamesUnique(List<JobDescription> jobDescriptions)
		{
			if (jobDescriptions.Select(e => e.Name).Distinct().Count() !=
				jobDescriptions.Count)
			{
				throw new Exception("Job names must be unique");
			}
		}
	}
}
