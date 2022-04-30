using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace RceRemoteSharpLib.Dtos
{
	public class JobDescription
	{
		public string Name { get; set; }
		public List<string> Description { get; set; }
		public JObject DefaultPayload { get; set; }
	}
}
