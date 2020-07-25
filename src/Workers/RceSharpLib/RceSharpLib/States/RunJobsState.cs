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

		public async Task Handle()
		{
			foreach (var job in _jobs)
			{
				if (RceJobRunner.JobRunnerContext.JobExecutorDictionary.TryGetValue(job.JobName, out var jobExecutorType) == false)
				{
					_ = Task.Run(() => FailJob(job, $"No job named '{job?.JobName}'"));
					continue;
				}

				Console.WriteLine($"Running job: '{job?.JobName}' '{job?.JobId}'");
				try
				{
					var jobExecutor = (JobExecutorBase)Activator.CreateInstance(jobExecutorType, new object[] { RceJobRunner.JobRunnerContext.BaseUrl, job });
					if (RceJobRunner.JobRunnerContext.RunInParallel)
					{
						_ = Task.Run(() => jobExecutor.Execute(RceJobRunner.CancellationTokenSource.Token));
					}
					else
					{
						await jobExecutor.Execute(RceJobRunner.CancellationTokenSource.Token);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine($"Running job failed: '{job?.JobName}' '{job.JobId}' '{e.Message}'");
				}
			}

			RceJobRunner.State = new GetJobsState(this);
		}

		private async Task FailJob(Job job, string reason)
		{
			Console.WriteLine($"Job failed: '{job?.JobName}' '{job?.JobId}' '{reason}'");

			try
			{
				var completeJobAddressSuffix = Consts.CompleteJobAddressSuffix
					.Replace("WORKER_ID", job.WorkerId.ToString())
					.Replace("JOB_ID", job.JobId.ToString());
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
				Console.WriteLine($"Reporting job failure failed: '{job?.JobName}' '{job?.JobId}' '{e.Message}'");
			}
		}
	}
}
