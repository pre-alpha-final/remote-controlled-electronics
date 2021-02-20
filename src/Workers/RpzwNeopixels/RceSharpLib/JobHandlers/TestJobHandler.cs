using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace RceSharpLib.JobHandlers
{
	public class TestJobHandler : JobHandlerBase
	{
		public TestJobHandler(string baseUrl, Job job)
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

		public override async Task Handle(CancellationToken cancellationToken)
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
