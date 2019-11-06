using System;

namespace RceServer.Domain.Models.Messages
{
	public class RemoveJobMessage : MessageBase, IRceMessage, IHasWorkerId, IHasJobId
	{
		public Guid JobId { get; set; }
		public Guid WorkerId { get; set; }
	}
}
