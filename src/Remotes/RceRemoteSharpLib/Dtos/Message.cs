using System;
using System.Collections.Generic;

namespace RceRemoteSharpLib.Dtos
{
	public class Message
	{
		public string MessageType { get; set; }
		public Guid WorkerId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public List<JobDescription> JobDescriptions { get; set; }
	}
}
