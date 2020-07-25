using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RceSharpLib.JobExecutors
{
	public abstract class JobExecutorBase
	{
		private readonly string _baseUrl;
		protected Job Job { get; set; }
		public abstract JobDescription JobDescription { get; }
		public abstract Task Execute(CancellationToken cancellationToken);

		public JobExecutorBase(string baseUrl, Job job)
		{
			_baseUrl = baseUrl;
			Job = job;
		}

		protected async Task UpdateJob(object payload)
		{
			Console.WriteLine($"Updating job: '{Job?.JobName}' '{Job?.JobId}'");

			try
			{
				var updateJobAddressSuffix = Consts.UpdateJobAddressSuffix
					.Replace("WORKER_ID", Job.WorkerId.ToString())
					.Replace("JOB_ID", Job.JobId.ToString());
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
				Console.WriteLine($"Updating job failed: '{Job?.JobName}' '{Job?.JobId}' '{e.Message}'");
			}
		}

		protected async Task CompleteJob(object payload)
		{
			Console.WriteLine($"Completing job: '{Job?.JobName}' '{Job?.JobId}'");

			try
			{
				var completeJobAddressSuffix = Consts.CompleteJobAddressSuffix
					.Replace("WORKER_ID", Job.WorkerId.ToString())
					.Replace("JOB_ID", Job.JobId.ToString());
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
				Console.WriteLine($"Completing job failed: '{Job?.JobName}' '{Job?.JobId}' '{e.Message}'");
			}
		}

		protected async Task FailJob(string reason)
		{
			Console.WriteLine($"Job failed: '{Job?.JobName}' '{Job?.JobId}' '{reason}'");

			try
			{
				var completeJobAddressSuffix = Consts.CompleteJobAddressSuffix
					.Replace("WORKER_ID", Job.WorkerId.ToString())
					.Replace("JOB_ID", Job.JobId.ToString());
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
				Console.WriteLine($"Reporting job failure failed: '{Job?.JobName}' '{Job?.JobId}' '{e.Message}'");
			}
		}
	}
}
