using Newtonsoft.Json;

namespace RceServer.Front.Controllers.Models
{
	public class ErrorResponse
	{
		[JsonProperty(PropertyName = "error")]
		public string Error { get; set; }
	}
}
