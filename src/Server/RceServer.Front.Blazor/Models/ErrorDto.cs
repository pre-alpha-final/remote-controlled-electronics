using Newtonsoft.Json;

namespace RceServer.Front.Blazor.Models
{
	public class ErrorDto
	{
		[JsonProperty(PropertyName = "error")]
		public string Error { get; set; }
	}
}
