using System;

namespace RceServer.Domain.Models.Messages
{
	public interface IRceMessage
	{
		Guid MessageId { get; set; }
		long MessageTimestamp { get; set; }
		string MessageType { get; }
	}
}
