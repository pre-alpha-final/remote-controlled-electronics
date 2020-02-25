using System;
using System.Collections.Generic;

namespace RceServer.Domain.Models.Messages
{
	public class WorkerAddedMessage : MessageBase, IRceMessage, IHasWorkerId
	{
		public Guid WorkerId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Base64Logo { get; set; }
		public List<JobDescription> JobDescriptions { get; set; }
		public List<string> Owners { get; set; }
	}
}
