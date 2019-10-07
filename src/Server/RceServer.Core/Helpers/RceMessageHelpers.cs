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
			var workerIdsToRemove = new HashSet<Guid>();
			var messageIdsToRemove = new HashSet<Guid>();

			foreach (var message in messages)
			{
				if (message is RemoveWorkerMessage removeWorkerMessage)
				{
					// Remove only if has corresponding AddWorkerMessage
					var hasCorrespondingAddMessage = messages.Any(e =>
						message is AddWorkerMessage addWorkerMessage &&
						addWorkerMessage.WorkerId == removeWorkerMessage.WorkerId);
					if (hasCorrespondingAddMessage)
					{
						workerIdsToRemove.Add(removeWorkerMessage.WorkerId);
					}
				}
			}

			foreach (var message in messages)
			{
				if (message is IHasWorkerId hasWorkerIdMessage)
				{
					if (workerIdsToRemove.Contains(hasWorkerIdMessage.WorkerId))
					{
						messageIdsToRemove.Add((hasWorkerIdMessage as IRceMessage).MessageId);
					}
				}
			}

			foreach (var messageIdToRemove in messageIdsToRemove.Distinct())
			{
				messages.RemoveAll(e => e.MessageId == messageIdToRemove);
			}
		}
	}
}
