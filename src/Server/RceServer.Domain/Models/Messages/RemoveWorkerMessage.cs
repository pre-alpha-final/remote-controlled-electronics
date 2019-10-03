using System;

namespace RceServer.Domain.Models.Messages
{
	public class RemoveWorkerMessage : MessageBase, IRceMessage
	{
		public Guid Id { get; set; }
	}
}
