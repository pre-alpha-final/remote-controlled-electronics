using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RceServer.Core.Helpers;
using RceServer.Domain.Models.Messages;
using RceServer.Domain.Services;

namespace RceServer.Core.Services.Implementation
{
	public class ClientService : IClientService
	{
		private readonly IMessageRepository _messageRepository;

		public ClientService(IMessageRepository messageRepository)
		{
			_messageRepository = messageRepository;
		}

		public Task<IList<IRceMessage>> GetState()
		{
			return _messageRepository.GetMessagesAfter(0);
		}
	}
}
