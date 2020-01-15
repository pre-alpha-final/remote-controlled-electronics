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

		public Task AddMessage(IRceMessage message)
		{
			lock (RceMessagesLock)
			{
				CheckActive(message as IHasWorkerId);

				RceMessages.Add(message);
				_rceHubContext.Clients.All.MessageReceived(message);
			}

			return Task.CompletedTask;
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

		private void CheckActive(IHasWorkerId message)
		{
			if (message == null)
			{
				return;
			}

			var workerMessages = RceMessages.Where(e =>
				e is IHasWorkerId hasWorkerId &&
				hasWorkerId.WorkerId == message.WorkerId).ToList();
			if (workerMessages.Any(e => e is WorkerRemovedMessage))
			{
				throw new Exception("Worker disconnected");
			}
		}
	}
}
