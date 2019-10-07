using System;
using Newtonsoft.Json.Linq;

namespace RceServer.Domain.Models.Messages
{
	public class AddJobMessage : MessageBase, IRceMessage, IHasWorkerId
	{
		public Guid JobId { get; set; }
		public Guid WorkerId { get; set; }
		public string Name { get; set; }
		public JObject Payload { get; set; }
	}
}
