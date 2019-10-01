using System;

namespace RceServer.Domain.Models
{
	public interface IRceMessage
	{
		Guid Id { get; set; }
		long Timestamp { get; set; }
	}
}
