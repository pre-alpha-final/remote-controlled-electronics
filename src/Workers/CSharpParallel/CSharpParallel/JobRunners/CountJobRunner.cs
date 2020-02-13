using System;
using System.Threading.Tasks;

namespace CSharpParallel.JobRunners
{
	public class CountJobRunner : JobRunnerBase
	{
		private readonly Job _job;

		public CountJobRunner(Job job) : base(job)
		{
			_job = job;
		}

		public async Task Run()
		{
			Console.WriteLine($"Running job: '{_job.JobName}' '{_job.JobId}'");

			try
			{
				var from = _job.Payload.SelectToken("$.from").ToObject<int>();
				var to = _job.Payload.SelectToken("$.to").ToObject<int>();
				for (var i = from; i < to; i++)
				{
					await UpdateJob(new
					{
						currentCount = i,
					});
					await Task.Delay(1000);
				}

				await CompleteJob(new
				{
					currentCount = to,
					jobStatus = Statuses.Success.ToString()
				});
			}
			catch (Exception e)
			{
				await FailJob(e.Message);
			}
		}
	}
}
