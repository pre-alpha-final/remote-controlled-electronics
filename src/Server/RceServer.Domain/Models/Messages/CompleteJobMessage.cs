using System;
using Newtonsoft.Json.Linq;

namespace RceServer.Domain.Models.Messages
{
	public class CompleteJobMessage : MessageBase, IRceMessage
	{
		public Guid Id { get; set; }
		public JObject Status { get; set; }
	}
}
