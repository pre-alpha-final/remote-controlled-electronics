using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RceSharpLib.States
{
	internal class RegistrationState : StateBase, IState
	{
		public RegistrationState(RceJobRunner rceJobRunner)
			: base(null)
		{
			RceJobRunner = rceJobRunner;
		}

		public async Task Handle()
		{
			Console.WriteLine("Registering worker");

			try
			{
				var requestUri = $"{RceJobRunner.JobRunnerContext.BaseUrl}{Consts.RegisterAddressSuffix}";
				using var client = new HttpClient();
				using var response = await client.PostAsync(requestUri, new StringContent(
					JsonConvert.SerializeObject(RceJobRunner.JobRunnerContext.RegistrationModel),
					Encoding.UTF8,
					"application/json"));
				await HandleResponse(response);
			}
			catch (Exception e)
			{
				RceJobRunner.State = new FailedState(this, e.Message);
				Console.WriteLine($"Worker registration failed: '{e.Message}'");
			}
		}

		private async Task HandleResponse(HttpResponseMessage httpResponseMessage)
		{
			using var content = httpResponseMessage.Content;
			var result = await content.ReadAsStringAsync();
			if (httpResponseMessage.StatusCode == HttpStatusCode.InternalServerError)
			{
				RceJobRunner.State = new FailedState(this, result);
				return;
			}

			if (Guid.TryParse(result.Replace("\"", ""), out var workerId))
			{
				WorkerId = workerId;
				RceJobRunner.State = new GetJobsState(this);
			}
			else
			{
				RceJobRunner.State = new FailedState(this, result);
			}
		}
	}
}
