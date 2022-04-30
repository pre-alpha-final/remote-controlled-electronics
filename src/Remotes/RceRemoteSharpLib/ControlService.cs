using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using RceRemoteSharpLib.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RceRemoteSharpLib
{
	public class ControlService
	{
		private readonly HttpClient _client = new HttpClient();
		private readonly LogInService _logInService;

		public ControlService(LogInService logInService)
		{
			_logInService = logInService;
		}

		public async Task<List<Worker>> GetList(string getMessagesUrl)
		{
			var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, getMessagesUrl)
			{
				Headers =
				{
					{ HeaderNames.Authorization, $"Bearer {_logInService.GetBearerToken()}" },
				},
			};
			var logInResponse = await _client.SendAsync(httpRequestMessage);
			var content = await logInResponse.Content.ReadAsStringAsync();
			var workers = JsonConvert.DeserializeObject<List<Worker>>(content);

			return workers.Where(e => e.Name != null).ToList();
		}
	}
}
