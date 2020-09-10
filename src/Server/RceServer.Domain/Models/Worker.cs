using System.Collections.Generic;

namespace RceServer.Domain.Models
{
	public class Worker
	{
		public string WorkerId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Base64Logo { get; set; }
		public List<JobDescription> JobDescriptions { get; set; }
		public List<Job> Jobs { get; set; }
		public string Error { get; set; }
	}
}
