using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RceServer.Domain.Models.Messages;
using RceServer.Domain.Services;

namespace RceServer.Core.Services.Implementation
{
	public class ServerService : IServerService
	{
		private readonly IMessageRepository _messageRepository;

		public ServerService(IMessageRepository messageRepository)
		{
			_messageRepository = messageRepository;
		}

		public Task<IList<IRceMessage>> GetMyMessages()
		{
			return _messageRepository.GetMyMessages();
		}

		public Task RunJob(Guid workerId, string jobName, string jobPayload)
		{
			return _messageRepository.AddMessage(new JobAddedMessage
			{
				JobId = Guid.NewGuid(),
				WorkerId = workerId,
				Name = jobName,
				Payload = JObject.Parse(jobPayload)
			});
		}

		public Task RemoveJob(Guid workerId, Guid jobId)
		{
			return _messageRepository.AddMessage(new JobRemovedMessage
			{
				JobId = jobId,
				WorkerId = workerId
			});
		}
	}
}
