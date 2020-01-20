using System;

namespace RceServer.Domain.Models.Messages
{
	public class WorkerRemovedMessage : MessageBase, IRceMessage, IHasWorkerId
	{
		public enum Statuses
		{
			Undefined,
			ClosedByWorker,
			ClosedByServer,
			ConnectionLost
		}

		public Guid WorkerId { get; set; }
		public Statuses ConnectionStatus { get; set; }
	}
}
