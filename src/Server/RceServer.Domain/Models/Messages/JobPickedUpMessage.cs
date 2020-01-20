using System;

namespace RceServer.Domain.Models.Messages
{
	public class JobPickedUpMessage : MessageBase, IRceMessage, IHasWorkerId, IHasJobId
	{
		public Guid JobId { get; set; }
		public Guid WorkerId { get; set; }
	}
}
