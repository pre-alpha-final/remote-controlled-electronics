using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CSharpParallel.JobRunners
{
	public class JobRunnerBase
	{
		private readonly Job _job;

		public JobRunnerBase(Job job)
		{
			_job = job;
		}

		public Task FailJob(string reason)
		{
			Console.WriteLine($"Job failed: '{_job.JobName}' '{_job.JobId}' '{reason}'");

			return CompleteJob(new
			{
				failure = reason,
				jobStatus = Statuses.Failure.ToString()
			});
		}

		protected async Task UpdateJob(object payload)
		{
			Console.WriteLine($"Updating job: '{_job.JobName}' '{_job.JobId}'");

			try
			{
				var updateJobAddressSuffix = Consts.UpdateJobAddressSuffix
					.Replace("WORKER_ID", _job.WorkerId.ToString())
					.Replace("JOB_ID", _job.JobId.ToString());
				var requestUri = $"{Program.UrlBase}{updateJobAddressSuffix}";

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
				// ignore
			}
		}

		protected async Task CompleteJob(object payload)
		{
			Console.WriteLine($"Completing job: '{_job.JobName}' '{_job.JobId}'");

			try
			{
				var completeJobAddressSuffix = Consts.CompleteJobAddressSuffix
					.Replace("WORKER_ID", _job.WorkerId.ToString())
					.Replace("JOB_ID", _job.JobId.ToString());
				var requestUri = $"{Program.UrlBase}{completeJobAddressSuffix}";

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
			}
		}

		protected enum Statuses
		{
			Undefined,
			Success,
			Warning,
			Failure
		}
	}
}
