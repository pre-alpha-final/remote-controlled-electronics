using Newtonsoft.Json.Linq;

namespace RceServer.Domain.Models
{
	public class Job
	{
		public string JobId { get; set; }
		public string WorkerId { get; set; }
		public string Name { get; set; }
		public JObject Payload { get; set; }
		public JObject Output { get; set; }
		public JobStates JobState { get; set; }
	}
}
