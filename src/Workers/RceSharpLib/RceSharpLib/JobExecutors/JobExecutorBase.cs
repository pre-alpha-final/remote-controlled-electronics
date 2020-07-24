using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RceSharpLib.JobExecutors
{
	public abstract class JobExecutorBase
	{
		private string _baseUrl { get; set; }
		protected Job RceJob { get; set; }
		public abstract JobDescription JobDescription { get; }
		public abstract Task Execute();

		public JobExecutorBase(string baseUrl, Job rceJob)
		{
			_baseUrl = baseUrl;
			RceJob = rceJob;
		}

		protected async Task UpdateJob(object payload)
		{
			Console.WriteLine($"Updating job: '{RceJob.JobName}' '{RceJob.JobId}'");

			try
			{
				var updateJobAddressSuffix = Consts.UpdateJobAddressSuffix
					.Replace("WORKER_ID", RceJob.WorkerId.ToString())
					.Replace("JOB_ID", RceJob.JobId.ToString());
				var requestUri = $"{_baseUrl}{updateJobAddressSuffix}";

				using (var client = new HttpClient())
				using (await client.PostAsync(requestUri, new StringContent(
					JsonConvert.SerializeObject(payload),
					Encoding.UTF8,
					"application/json")))
				{
				}
			}
			catch (Exception e)
			{
				Console.WriteLine($"Updating job failed: '{RceJob?.JobName}' '{RceJob?.JobId}' '{e.Message}'");
			}
		}

		protected async Task CompleteJob(object payload)
		{
			Console.WriteLine($"Completing job: '{RceJob.JobName}' '{RceJob.JobId}'");

			try
			{
				var completeJobAddressSuffix = Consts.CompleteJobAddressSuffix
					.Replace("WORKER_ID", RceJob.WorkerId.ToString())
					.Replace("JOB_ID", RceJob.JobId.ToString());
				var requestUri = $"{_baseUrl}{completeJobAddressSuffix}";

				using (var client = new HttpClient())
				using (await client.PostAsync(requestUri, new StringContent(
					JsonConvert.SerializeObject(payload),
					Encoding.UTF8,
					"application/json")))
				{
				}
			}
			catch (Exception e)
			{
				await FailJob(e.Message);
				Console.WriteLine($"Completing job failed: '{RceJob?.JobName}' '{RceJob?.JobId}' '{e.Message}'");
			}
		}

		protected async Task FailJob(string reason)
		{
			Console.WriteLine($"Job failed: '{RceJob.JobName}' '{RceJob.JobId}' '{reason}'");

			try
			{
				var completeJobAddressSuffix = Consts.CompleteJobAddressSuffix
					.Replace("WORKER_ID", RceJob.WorkerId.ToString())
					.Replace("JOB_ID", RceJob.JobId.ToString());
				var requestUri = $"{_baseUrl}{completeJobAddressSuffix}";

				using (var client = new HttpClient())
				using (await client.PostAsync(requestUri, new StringContent(
					JsonConvert.SerializeObject(new
					{
						failure = reason,
						jobStatus = Statuses.Failure.ToString()
					}),
					Encoding.UTF8,
					"application/json")))
				{
				}
			}
			catch (Exception e)
			{
				Console.WriteLine($"Reporting job failure failed: '{RceJob?.JobName}' '{RceJob?.JobId}' '{e.Message}'");
			}
		}
	}
}
