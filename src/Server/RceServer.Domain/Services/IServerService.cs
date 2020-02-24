using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RceServer.Domain.Models.Messages;

namespace RceServer.Domain.Services
{
	public interface IServerService
	{
		Task<IList<IRceMessage>> GetMyMessages();
		Task RunJob(Guid workerId, string jobName, string jobPayload);
		Task RemoveJob(Guid workerId, Guid jobId);
	}
}
