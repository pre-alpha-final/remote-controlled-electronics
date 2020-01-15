using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

		public Task<IList<IRceMessage>> GetMessages()
		{
			return _messageRepository.GetMessagesAfter(0);
		}

		public Task RunJob(Guid workerId, string jobName, string jobPayload)
		{
			throw new NotImplementedException();
		}

		public Task RemoveJob(Guid workerId, Guid jobId)
		{
			throw new NotImplementedException();
		}
	}
}
