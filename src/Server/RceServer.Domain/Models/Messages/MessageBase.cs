using System;

namespace RceServer.Domain.Models.Messages
{
	public abstract class MessageBase
	{
		public Guid MessageId { get; set; } = Guid.NewGuid();
		public long MessageTimestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		public string MessageType => GetType().Name;
	}
}
