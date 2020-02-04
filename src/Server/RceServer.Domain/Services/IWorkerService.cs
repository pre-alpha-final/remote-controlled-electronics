using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RceServer.Domain.Models;
using RceServer.Domain.Models.Messages;

namespace RceServer.Domain.Services
{
	public interface IWorkerService
	{
		Task<Guid> Register(string name, string description,
			string base64Logo, List<JobDescription> jobDescriptions);
		Task<List<JobAddedMessage>> GetJobs(Guid workerId, int? maxCount);
		Task UpdateJob(Guid workerId, Guid jobId, JObject output);
		Task CompleteJob(Guid workerId, Guid jobId, JObject output);
		Task CloseWorker(Guid workerId);
	}
}
