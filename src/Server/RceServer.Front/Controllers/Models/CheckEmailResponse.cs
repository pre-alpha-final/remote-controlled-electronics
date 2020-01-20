using Newtonsoft.Json;

namespace RceServer.Front.Controllers.Models
{
	public class CheckEmailResponse
	{
		[JsonProperty(PropertyName = "result")]
		public string Result { get; set; }
	}
}
