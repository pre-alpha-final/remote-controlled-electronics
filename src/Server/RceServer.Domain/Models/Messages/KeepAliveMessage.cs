using System;

namespace RceServer.Domain.Models.Messages
{
	public class KeepAliveMessage : MessageBase, IRceMessage, IHasWorkerId
	{
		public enum Reasons
		{
			Undefined,
			GetFeed,
		}

		public Guid WorkerId { get; set; }
		public Reasons Reason { get; set; }
	}
}
