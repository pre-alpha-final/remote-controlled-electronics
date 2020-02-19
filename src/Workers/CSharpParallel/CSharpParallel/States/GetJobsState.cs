using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CSharpParallel.States
{
	public class GetJobsState : IState
	{
		private JobRunnerStateMachine _jobRunnerStateMachine;

		public async Task Handle(JobRunnerStateMachine jobRunnerStateMachine)
		{
			Console.WriteLine("Getting jobs");

			_jobRunnerStateMachine = jobRunnerStateMachine;
			try
			{
				var getJobAddressSuffix = Consts.GetJobsAddressSuffix.Replace("WORKER_ID", jobRunnerStateMachine.WorkerId.ToString());
				var requestUri = $"{Program.UrlBase}{getJobAddressSuffix}";
				using (var client = new HttpClient())
				using (var response = await client.GetAsync(requestUri))
				{
					await HandleResponse(response);
				}
			}
			catch (Exception e)
			{
				jobRunnerStateMachine.State = new FailedState(e.Message);
			}
		}

		private async Task HandleResponse(HttpResponseMessage httpResponseMessage)
		{
			using (var content = httpResponseMessage.Content)
			{
				var result = await content.ReadAsStringAsync();
				if (httpResponseMessage.StatusCode == HttpStatusCode.InternalServerError)
				{
					_jobRunnerStateMachine.State = new FailedState(result);
					return;
				}

				var jobsJson = JArray.Parse(result);
				if (jobsJson.Count == 0)
				{
					_jobRunnerStateMachine.State = new GetJobsState();
					return;
				}

				var jobs = jobsJson.ToObject<List<Job>>();
				jobs.ForEach(e => e.WorkerId = _jobRunnerStateMachine.WorkerId);
				_jobRunnerStateMachine.State = new RunJobsState(jobs);
			}
		}
	}
}
