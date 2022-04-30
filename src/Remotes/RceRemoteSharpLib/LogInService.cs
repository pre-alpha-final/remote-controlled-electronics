using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RceRemoteSharpLib
{
	public class LogInService
	{
		private readonly HttpClient _client = new HttpClient();
		private string _bearer;

		public string LogInUrl { get; set; }
		public string User { get; set; }
		public string Password { get; set; }

		public Task ReLogIn()
		{
			return LogIn(LogInUrl, User, Password);
		}

		public async Task LogIn(string logInUrl, string user, string password)
		{
			LogInUrl = logInUrl;
			User = user;
			Password = password;

			var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, LogInUrl)
			{
				Content = new StringContent($"{{\"login\":\"{User}\",\"password\":\"{Password}\"}}",
					Encoding.UTF8, "application/json")
			};
			var logInResponse = await _client.SendAsync(httpRequestMessage);
			var content = await logInResponse.Content.ReadAsStringAsync();
			_bearer = JObject.Parse(content).SelectToken($"access_token").ToString();
		}

		public string GetBearerToken()
		{
			return _bearer;
		}
	}
}
