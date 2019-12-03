using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace RceServer.Domain.Models
{
	public class JobDescription
	{
		public string Name { get; set; }
		public List<string> Description { get; set; }
		public JObject DefaultPayload { get; set; }
	}
}
