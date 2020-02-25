using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RceServer.Domain.Models.Messages;

namespace RceServer.Domain.Services
{
	public interface IMessageRepository
	{
		Task AddMessage(IRceMessage message);
		Task<IList<IRceMessage>> GetMyMessages();
		Task<IList<IRceMessage>> GetMessagesBefore(long timestamp);
		Task<IList<IRceMessage>> GetMessagesAfter(long timestamp);
		Task<IList<IRceMessage>> GetWorkerMessages(Guid workerId);
		Task RemoveMessages(IEnumerable<Guid> messageIds);
		Task RemoveOwnership(IEnumerable<Guid> workerIds);
		Task<bool> IsDisconnected(Guid workerId);
	}
}
