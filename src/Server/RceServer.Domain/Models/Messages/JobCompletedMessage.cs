using System;
using Newtonsoft.Json.Linq;

namespace RceServer.Domain.Models.Messages
{
	public class JobCompletedMessage : MessageBase, IRceMessage, IHasWorkerId, IHasJobId
	{
		public enum Statuses
		{
			Undefined,
			Success,
			Warning,
			Failure
		}

		public Guid JobId { get; set; }
		public Guid WorkerId { get; set; }
		public Statuses JobStatus { get; set; }
		public JObject Output { get; set; }
	}
}
