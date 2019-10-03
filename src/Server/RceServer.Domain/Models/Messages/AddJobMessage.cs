using System;
using Newtonsoft.Json.Linq;

namespace RceServer.Domain.Models.Messages
{
	public class AddJobMessage : MessageBase, IRceMessage
	{
		public Guid Id { get; set; }
		public Guid WorkerId { get; set; }
		public string Name { get; set; }
		public JObject Payload { get; set; }
	}
}
