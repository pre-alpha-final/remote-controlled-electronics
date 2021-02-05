using Newtonsoft.Json.Linq;
using RceSharpLib;
using RceSharpLib.JobHandlers;
using RpzwTimer.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RpzwTimer.Handlers
{
	public class StartHandler : JobHandlerBase
	{
		public StartHandler(string baseUrl, Job job) : base(baseUrl, job)
		{
		}

		public override JobDescription JobDescription => new JobDescription
		{
			Name = "Start timer",
			Description = new List<string>
			{
				"Starts the timer (default 10 seconds)"
			},
			DefaultPayload = JObject.Parse(@"{""hours"": 0, ""minutes"": 0, ""seconds"": 10}")
		};

		public override async Task Handle(CancellationToken cancellationToken)
		{
			long seconds =
				Job.Payload.SelectToken("$.hours").ToObject<long>() * 3600 +
				Job.Payload.SelectToken("$.minutes").ToObject<long>() * 60 +
				Job.Payload.SelectToken("$.seconds").ToObject<long>();
			TimerService.Instance.Start(TimeSpan.FromSeconds(seconds));
			await CompleteJob(new { jobStatus = Statuses.Success.ToString() });
		}
	}
}
