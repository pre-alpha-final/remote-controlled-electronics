using Newtonsoft.Json;

namespace RceServer.Front.Blazor.Models
{
	public class RefreshTokenResponseModel
	{
		[JsonProperty(PropertyName = "refreshToken")]
		public string RefreshToken { get; set; }
	}
}
