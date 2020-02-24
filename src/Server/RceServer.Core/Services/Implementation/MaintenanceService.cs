using System;
using System.Linq;
using System.Threading.Tasks;
using RceServer.Core.Helpers;
using RceServer.Domain.Models.Messages;
using RceServer.Domain.Services;

namespace RceServer.Core.Services.Implementation
{
	public class MaintenanceService : IMaintenanceService
	{
		private const int OldActivityCheckDelayMinutes = 10;
		private const int DeclareOldAfterMinutes = 60;
		private const int DisconnectedWorkersCheckDelayMinutes = 1;
		private const int DeclareDisconnectedAfterMinutes = 3;
		private readonly IMessageRepository _messageRepository;

		public MaintenanceService(IMessageRepository messageRepository)
		{
			_messageRepository = messageRepository;
		}

		public void Start()
		{
			Task.Factory.StartNew(RemoveOldActivityLoop, TaskCreationOptions.LongRunning);
			Task.Factory.StartNew(MarkDisconnectedWorkersLoop, TaskCreationOptions.LongRunning);
		}

		public async Task RemoveOldActivity()
		{
			try
			{
				var pastTimestamp =
					DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() -
					(long)TimeSpan.FromMinutes(DeclareOldAfterMinutes).TotalMilliseconds;
				var messages = await _messageRepository.GetMessagesBefore(pastTimestamp);
				var redundantMessages = RceMessageHelpers.GetRedundantMessages(messages).ToList();
				await _messageRepository.RemoveMessages(redundantMessages.Select(e => e.MessageId));
				await _messageRepository.RemoveOwnership(redundantMessages
					.Where(e => e is WorkerRemovedMessage)
					.Select(e => ((WorkerRemovedMessage)e).WorkerId)
					.Distinct());
			}
			catch (Exception e)
			{
				// ignore
			}
		}

		public async Task MarkDisconnectedWorkers()
		{
			try
			{
				var pastTimestamp =
					DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() -
					(long)TimeSpan.FromMinutes(DeclareDisconnectedAfterMinutes).TotalMilliseconds;
				var oldMessages = await _messageRepository.GetMessagesBefore(pastTimestamp);
				var newMessages = await _messageRepository.GetMessagesAfter(pastTimestamp);
				var previouslyActiveWorkers = RceMessageHelpers.GetActiveWorkerIds(oldMessages).ToList();
				var currentlyActiveWorkers = RceMessageHelpers.GetActiveWorkerIds(newMessages).ToList();

				foreach (var disconnectedWorkerId in previouslyActiveWorkers.Except(currentlyActiveWorkers))
				{
					if (await _messageRepository.IsDisconnected(disconnectedWorkerId))
					{
						continue;
					}

					await _messageRepository.AddMessage(new WorkerRemovedMessage
					{
						WorkerId = disconnectedWorkerId,
						ConnectionStatus = WorkerRemovedMessage.Statuses.ConnectionLost
					});
				}
			}
			catch (Exception e)
			{
				// ignore
			}
		}

		private async Task RemoveOldActivityLoop()
		{
			while (true)
			{
				await RemoveOldActivity();
				await Task.Delay(TimeSpan.FromMinutes(OldActivityCheckDelayMinutes));
			}
		}

		private async Task MarkDisconnectedWorkersLoop()
		{
			while (true)
			{
				await MarkDisconnectedWorkers();
				await Task.Delay(TimeSpan.FromMinutes(DisconnectedWorkersCheckDelayMinutes));
			}
		}
	}
}
