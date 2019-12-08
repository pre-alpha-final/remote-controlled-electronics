using System;
using System.Collections.Generic;
using System.Linq;
using RceServer.Domain.Models.Messages;

namespace RceServer.Core.Helpers
{
	public static class RceMessageHelpers
	{
		public static void Minimize(List<IRceMessage> messages)
		{
			var redundantMessages = GetRedundantMessages(messages);
			messages.RemoveAll(e => redundantMessages.Contains(e.MessageId));
		}

		public static IEnumerable<Guid> GetRedundantMessages(IList<IRceMessage> messages)
		{
			var workerIdsToRemove = new HashSet<Guid>();
			var jobIdsToRemove = new HashSet<Guid>();
			var messageIdsToRemove = new HashSet<Guid>();
			var lastUpdate = new Dictionary<Guid, (Guid messageId, long timestamp)>();
			var lastKeepAlive = new Dictionary<Guid, (Guid messageId, long timestamp)>();

			// Mark messages for removal
			foreach (var message in messages)
			{
				// Mark completed workers with all their messages for removal
				if (message is RemoveWorkerMessage removeWorkerMessage)
				{
					// Only if has corresponding AddWorkerMessage
					var hasCorrespondingAddMessage = messages.Any(e =>
						e is AddWorkerMessage addWorkerMessage &&
						addWorkerMessage.WorkerId == removeWorkerMessage.WorkerId);
					if (hasCorrespondingAddMessage)
					{
						workerIdsToRemove.Add(removeWorkerMessage.WorkerId);
					}
				}

				// Mark removed jobs with all their messages for removal
				if (message is RemoveJobMessage removeJobMessage)
				{
					// Only if has corresponding AddJobMessage
					var hasCorrespondingAddMessage = messages.Any(e =>
						e is AddJobMessage addJobMessage &&
						addJobMessage.JobId == removeJobMessage.JobId);
					if (hasCorrespondingAddMessage)
					{
						jobIdsToRemove.Add(removeJobMessage.JobId);
					}
				}

				// Mark old update messages for removal
				if (message is UpdateJobMessage updateJobMessage)
				{
					if (lastUpdate.ContainsKey(updateJobMessage.WorkerId) == false)
					{
						lastUpdate.Add(updateJobMessage.WorkerId,
							(updateJobMessage.MessageId, updateJobMessage.MessageTimestamp));
						continue;
					}

					if (updateJobMessage.MessageTimestamp > lastUpdate[updateJobMessage.WorkerId].timestamp)
					{
						messageIdsToRemove.Add(lastUpdate[updateJobMessage.WorkerId].messageId);
						lastUpdate[updateJobMessage.WorkerId] =
							(updateJobMessage.MessageId, updateJobMessage.MessageTimestamp);
					}
					else
					{
						messageIdsToRemove.Add(updateJobMessage.MessageId);
					}
				}

				// Mark old keep alive messages for removal
				if (message is KeepAliveMessage keepAliveMessage)
				{
					if (lastKeepAlive.ContainsKey(keepAliveMessage.WorkerId) == false)
					{
						lastKeepAlive.Add(keepAliveMessage.WorkerId,
							(keepAliveMessage.MessageId, keepAliveMessage.MessageTimestamp));
						continue;
					}

					if (keepAliveMessage.MessageTimestamp > lastKeepAlive[keepAliveMessage.WorkerId].timestamp)
					{
						messageIdsToRemove.Add(lastKeepAlive[keepAliveMessage.WorkerId].messageId);
						lastKeepAlive[keepAliveMessage.WorkerId] =
							(keepAliveMessage.MessageId, keepAliveMessage.MessageTimestamp);
					}
					else
					{
						messageIdsToRemove.Add(keepAliveMessage.MessageId);
					}
				}
			}

			return messages.Where(e =>
					workerIdsToRemove.Contains((e as IHasWorkerId)?.WorkerId ?? Guid.Empty) ||
					jobIdsToRemove.Contains((e as IHasJobId)?.JobId ?? Guid.Empty) ||
					messageIdsToRemove.Contains(e.MessageId))
				.Select(e => e.MessageId);
		}

		public static IEnumerable<Guid> GetActiveWorkers(IList<IRceMessage> messages)
		{
			var workers = new HashSet<Guid>();
			var removedWorkers = new HashSet<Guid>();

			foreach (var message in messages)
			{
				if (message is IHasWorkerId workerMessage)
				{
					workers.Add(workerMessage.WorkerId);
				}

				if (message is RemoveWorkerMessage removeWorkerMessage)
				{
					removedWorkers.Add(removeWorkerMessage.WorkerId);
				}
			}

			return workers.Except(removedWorkers);
		}
	}
}
