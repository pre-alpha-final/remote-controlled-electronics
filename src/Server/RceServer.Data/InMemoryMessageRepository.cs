using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using RceServer.Core.Hubs;
using RceServer.Domain.Models.Messages;
using RceServer.Domain.Services;

namespace RceServer.Data
{
	// TODO make a persistent version
	public class InMemoryMessageRepository : IMessageRepository
	{
		private static readonly object RceMessagesLock = new object();
		private readonly IHubContext<RceHub, IRceHub> _rceHubContext;
		private static List<IRceMessage> RceMessages { get; set; } = new List<IRceMessage>();

		public InMemoryMessageRepository(IHubContext<RceHub, IRceHub> rceHubContext)
		{
			_rceHubContext = rceHubContext;
		}

		public async Task AddMessage(IRceMessage message)
		{
			if (message is IHasWorkerId hasWorkerId &&
				await IsDisconnected(hasWorkerId.WorkerId))
			{
				throw new Exception("Worker disconnected");
			}

			lock (RceMessagesLock)
			{
				RceMessages.Add(message);
				var _ = _rceHubContext.Clients.All.MessageReceived(message);
			}
		}

		public async Task<IList<IRceMessage>> GetMessagesBefore(long timestamp)
		{
			lock (RceMessagesLock)
			{
				return RceMessages.Where(e => e.MessageTimestamp < timestamp).ToList();
			}
		}

		public async Task<IList<IRceMessage>> GetMessagesAfter(long timestamp)
		{
			lock (RceMessagesLock)
			{
				return RceMessages.Where(e => e.MessageTimestamp >= timestamp).ToList();
			}
		}

		public async Task<IList<IRceMessage>> GetWorkerMessages(Guid workerId)
		{
			lock (RceMessagesLock)
			{
				return RceMessages.Where(e =>
					e is IHasWorkerId hasWorkerId &&
					hasWorkerId.WorkerId == workerId).ToList();
			}
		}

		public Task RemoveMessages(IEnumerable<Guid> messages)
		{
			lock (RceMessagesLock)
			{
				RceMessages = RceMessages
					.Where(e => messages.Contains(e.MessageId) == false)
					.ToList();
			}

			return Task.CompletedTask;
		}

		public async Task<bool> IsDisconnected(Guid workerId)
		{
			var workerMessages = RceMessages.Where(e =>
				e is IHasWorkerId hasWorkerId &&
				hasWorkerId.WorkerId == workerId).ToList();

			return workerMessages.Any(e => e is WorkerRemovedMessage);
		}
	}
}
