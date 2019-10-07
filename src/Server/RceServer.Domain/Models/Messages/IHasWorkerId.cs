using System;

namespace RceServer.Domain.Models.Messages
{
	public interface IHasWorkerId
	{
		Guid WorkerId { get; set; }
	}
}
