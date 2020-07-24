using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace RceSharpLib.JobExecutors
{
	public class TestJobExecutor : JobExecutorBase
	{
		public TestJobExecutor(string baseUrl, Job job)
			: base(baseUrl, job)
		{
		}

		public override JobDescription JobDescription => new JobDescription
		{
			Name = "_TEST_JOB",
			Description = new List<string>
			{
				"Test job"
			},
			DefaultPayload = JObject.Parse("{}")
		};

		public override async Task Execute()
		{
			try
			{
				await CompleteJob(new { jobStatus = Statuses.Success.ToString() });
			}
			catch (Exception e)
			{
				await FailJob(e.Message);
			}
		}
	}
}
