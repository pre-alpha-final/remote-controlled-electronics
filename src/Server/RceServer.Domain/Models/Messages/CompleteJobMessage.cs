using System;
using Newtonsoft.Json.Linq;

namespace RceServer.Domain.Models.Messages
{
	public class CompleteJobMessage : MessageBase, IRceMessage, IHasWorkerId
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
