using System;
using Newtonsoft.Json.Linq;

namespace CSharpSingleThreaded
{
	public class Job
	{
		public Guid WorkerId { get; set; }
		public Guid JobId { get; set; }
		public string JobName { get; set; }
		public JObject Payload { get; set; }
	}
}
