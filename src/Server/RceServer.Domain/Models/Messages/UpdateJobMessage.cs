using System;
using Newtonsoft.Json.Linq;

namespace RceServer.Domain.Models.Messages
{
	public class UpdateJobMessage : MessageBase, IRceMessage, IHasWorkerId
	{
		public Guid JobId { get; set; }
		public Guid WorkerId { get; set; }
		public JObject Output { get; set; }
	}
}
