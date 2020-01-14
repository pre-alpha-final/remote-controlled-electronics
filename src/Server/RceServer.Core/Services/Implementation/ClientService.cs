using System.Collections.Generic;
using System.Threading.Tasks;
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

		public Task<IList<IRceMessage>> GetMessages()
		{
			return _messageRepository.GetMessagesAfter(0);
		}
	}
}
