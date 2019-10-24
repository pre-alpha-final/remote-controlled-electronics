﻿using System;
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

		public static IEnumerable<Guid> GetRedundantMessages(List<IRceMessage> messages)
		{
			var workerIdsToRemove = new HashSet<Guid>();
			var messageIdsToRemove = new HashSet<Guid>();
			var lastUpdate = new Dictionary<Guid, (Guid messageId, long timestamp)>();

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
			}

			return messages.Where(e =>
					workerIdsToRemove.Contains((e as IHasWorkerId)?.WorkerId ?? Guid.Empty) ||
					messageIdsToRemove.Contains(e.MessageId))
				.Select(e => e.MessageId);
		}
	}
}
