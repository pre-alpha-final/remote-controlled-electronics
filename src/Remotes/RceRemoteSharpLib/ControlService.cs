using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using RceRemoteSharpLib.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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

		public async Task<List<Message>> GetWorkerList(string getMessagesUrl)
		{
			var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, getMessagesUrl)
			{
				Headers =
				{
					{ HeaderNames.Authorization, $"Bearer {_logInService.GetBearerToken()}" },
				},
			};
			var getListResponse = await _client.SendAsync(httpRequestMessage);
			if (getListResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
			{
				throw new Exception("Unauthorized");
			}

			var content = await getListResponse.Content.ReadAsStringAsync();
			var messages = JsonConvert.DeserializeObject<List<Message>>(content);

			return messages.Where(e => e.MessageType == "WorkerAddedMessage").ToList();
		}

		public async Task RunJob(string runJobUrl, string jobName, string jobPayload)
		{
			var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, runJobUrl)
			{
				Headers =
				{
					{ HeaderNames.Authorization, $"Bearer {_logInService.GetBearerToken()}" },
				},
				Content = new StringContent(JsonConvert.SerializeObject(new JobBody { JobName = jobName, JobPayload = jobPayload }),
					Encoding.UTF8, "application/json")
			};
			var runJobResponse = await _client.SendAsync(httpRequestMessage);
			if (runJobResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
			{
				throw new Exception("Unauthorized");
			}
		}
	}
}
