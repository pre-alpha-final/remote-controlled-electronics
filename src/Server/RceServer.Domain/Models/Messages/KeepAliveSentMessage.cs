using System;

namespace RceServer.Domain.Models.Messages
{
	public class KeepAliveSentMessage : MessageBase, IRceMessage, IHasWorkerId
	{
		public enum Reasons
		{
			Undefined,
			GetJobs
		}

		public Guid WorkerId { get; set; }
		public Reasons Reason { get; set; }
	}
}
