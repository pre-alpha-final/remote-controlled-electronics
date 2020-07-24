using Newtonsoft.Json;
using RceSharpLib.JobExecutors;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RceSharpLib.States
{
	internal class RunJobsState : StateBase, IState
	{
		private List<Job> _jobs;

		public RunJobsState(StateBase previousState, List<Job> jobs)
			: base(previousState)
		{
			_jobs = jobs;
		}

		public Task Handle()
		{
			foreach (var job in _jobs)
			{
				if (RceJobRunner.JobRunnerContext.JobExecutorDictionary.TryGetValue(job.JobName, out var jobExecutorType) == false)
				{
					_ = Task.Run(() => FailJob(job, $"No job named '{job?.JobName}'"));
					continue;
				}

				var jobExecutor = (JobExecutorBase)Activator.CreateInstance(jobExecutorType, new object[] { RceJobRunner.JobRunnerContext.BaseUrl, job });
				_ = Task.Run(() => jobExecutor.Execute());
			}

			RceJobRunner.State = new GetJobsState(this);

			return Task.CompletedTask;
		}

		private async Task FailJob(Job rceJob, string reason)
		{
			Console.WriteLine($"Job failed: '{rceJob?.JobName}' '{rceJob?.JobId}' '{reason}'");

			try
			{
				var completeJobAddressSuffix = Consts.CompleteJobAddressSuffix
					.Replace("WORKER_ID", rceJob.WorkerId.ToString())
					.Replace("JOB_ID", rceJob.JobId.ToString());
				var requestUri = $"{RceJobRunner.JobRunnerContext.BaseUrl}{completeJobAddressSuffix}";

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
				Console.WriteLine($"Reporting job failure failed: '{rceJob?.JobName}' '{rceJob?.JobId}' '{e.Message}'");
			}
		}
	}
}
