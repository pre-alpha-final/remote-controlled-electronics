using System.Collections.Generic;
using RceServer.Domain.Models;

namespace RceServer.Front.Controllers.Models
{
	public class RegisterWorkerModel
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Base64Logo { get; set; }
		public List<JobDescription> JobDescriptions { get; set; }
	}
}
