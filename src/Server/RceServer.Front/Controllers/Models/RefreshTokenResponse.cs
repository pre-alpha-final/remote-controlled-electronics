using Newtonsoft.Json;

namespace RceServer.Front.Controllers.Models
{
	public class RefreshTokenResponse
	{
		[JsonProperty(PropertyName = "refreshToken")]
		public string RefreshToken { get; set; }
	}
}
