using System;
using Newtonsoft.Json.Linq;

namespace RceServer.Domain.Models.Messages
{
	public class CompleteJobMessage : MessageBase, IRceMessage, IHasWorkerId
	{
		public enum Status
		{
			Undefined,
			Success,
			Warning,
			Failure
		}

		public Guid JobId { get; set; }
		public Guid WorkerId { get; set; }
		public Status JobStatus { get; set; }
		public JObject Output { get; set; }
	}
}
