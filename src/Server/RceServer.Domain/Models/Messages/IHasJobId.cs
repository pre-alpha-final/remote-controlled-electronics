using System;

namespace RceServer.Domain.Models.Messages
{
	public interface IHasJobId
	{
		Guid JobId { get; set; }
	}
}
