using System;

namespace RceServer.Domain.Models.Messages
{
	public class RemoveWorkerMessage : MessageBase, IRceMessage, IHasWorkerId
	{
		public enum Status
		{
			Undefined,
			ClosedByWorker,
			ClosedByServer,
			ConnectionLost
		}

		public Guid WorkerId { get; set; }
		public Status ConnectionStatus { get; set; }
	}
}
