using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CSharpSingleThreaded.States
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
				var getJobAddressSuffix = Consts.GetJobAddressSuffix.Replace("WORKER_ID", jobRunnerStateMachine.WorkerId.ToString());
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

				var jobJson = JArray.Parse(result)?.FirstOrDefault();
				if (jobJson == null)
				{
					_jobRunnerStateMachine.State = new GetJobsState();
					return;
				}

				var job = jobJson.ToObject<Job>();
				job.WorkerId = _jobRunnerStateMachine.WorkerId;
				_jobRunnerStateMachine.State = new RunJobState(job);
			}
		}
	}
}
