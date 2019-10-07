using System;

namespace RceServer.Domain.Models.Messages
{
	public class RemoveWorkerMessage : MessageBase, IRceMessage, IHasWorkerId
	{
		public Guid WorkerId { get; set; }
	}
}
