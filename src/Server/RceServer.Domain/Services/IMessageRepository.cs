using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RceServer.Domain.Models.Messages;

namespace RceServer.Domain.Services
{
	public interface IMessageRepository
	{
		Task AddMessage(IRceMessage message);
		Task<List<IRceMessage>> GetMessagesBefore(long timestamp);
		Task<List<IRceMessage>> GetMessagesAfter(long timestamp);
		Task RemoveMessages(IEnumerable<Guid> messages);
	}
}
