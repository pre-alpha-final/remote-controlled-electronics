using System;
using System.Collections.Generic;

namespace RceServer.Domain.Models.Messages
{
	public class AddWorkerMessage : MessageBase, IRceMessage
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
        public string Base64Logo { get; set; }
        public List<JobDescription> JobDescriptions { get; set; }
	}
}
