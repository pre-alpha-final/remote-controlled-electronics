using Newtonsoft.Json.Linq;
using RceSharpLib;
using RceSharpLib.JobHandlers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpParallel
{
	public class CounterJobHandler : JobHandlerBase
	{
		public CounterJobHandler(string baseUrl, Job job)
			: base(baseUrl, job)
		{
		}

		public override JobDescription JobDescription => new JobDescription
		{
			Name = "Count",
			Description = new List<string>
			{
				"Counts in one second intervals. Params:",
				" - from [int]",
				" - to [int]"
			},
			DefaultPayload = JObject.Parse("{\"from\":0,\"to\":5}")
		};

		public override async Task Handle(CancellationToken cancellationToken)
		{
			try
			{
				var from = Job.Payload.SelectToken("$.from").ToObject<int>();
				var to = Job.Payload.SelectToken("$.to").ToObject<int>();
				for (var i = from; i < to; i++)
				{
					if (cancellationToken.IsCancellationRequested)
					{
						await FailJob("Job cancelled");
						return;
					}
					await UpdateJob(new { currentCount = i, });
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
