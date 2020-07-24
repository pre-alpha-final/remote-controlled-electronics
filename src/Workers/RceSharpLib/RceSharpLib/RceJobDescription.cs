using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace RceSharpLib
{
	public class RceJobDescription
	{
		public string Name { get; set; }
		public List<string> Description { get; set; }
		public JObject DefaultPayload { get; set; }
	}
}
