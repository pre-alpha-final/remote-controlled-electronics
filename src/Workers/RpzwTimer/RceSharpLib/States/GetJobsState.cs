using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RceSharpLib.States
{
	internal class GetJobsState : StateBase, IState
	{
		public GetJobsState(StateBase previousState)
			: base(previousState)
		{
		}

		public async Task Handle()
		{
			Console.WriteLine("Getting jobs");

			try
			{
				var getJobAddressSuffix = Consts.GetJobsAddressSuffix.Replace("WORKER_ID", WorkerId.ToString());
				var requestUri = $"{RceJobRunner.JobRunnerContext.BaseUrl}{getJobAddressSuffix}";
				using (var client = new HttpClient())
				{
					using (var response = await client.GetAsync(requestUri, RceJobRunner.CancellationTokenSource.Token))
					{
						await HandleResponse(response);
					}
				}
			}
			catch (Exception e)
			{
				RceJobRunner.State = new FailedState(this, e.Message);
				Console.WriteLine($"Getting jobs failed: '{WorkerId}' '{e.Message}'");
			}
		}

		private async Task HandleResponse(HttpResponseMessage httpResponseMessage)
		{
			using (var content = httpResponseMessage.Content)
			{
				var result = await content.ReadAsStringAsync();
				if (httpResponseMessage.StatusCode == HttpStatusCode.InternalServerError)
				{
					RceJobRunner.State = new FailedState(this, result);
					return;
				}

				var jobsJson = JArray.Parse(result);
				if (jobsJson.Count == 0)
				{
					RceJobRunner.State = new GetJobsState(this);
					return;
				}

				var jobs = jobsJson.ToObject<List<Job>>();
				jobs.ForEach(e => e.WorkerId = WorkerId);
				RceJobRunner.State = new RunJobsState(this, jobs);
			}
		}
	}
}
