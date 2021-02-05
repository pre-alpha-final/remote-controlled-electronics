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
	public class ShowTimeHandler : JobHandlerBase
	{
		public ShowTimeHandler(string baseUrl, Job job) : base(baseUrl, job)
		{
		}

		public override JobDescription JobDescription => new JobDescription
		{
			Name = "Show Time",
			Description = new List<string>
			{
				"Shows elapsed/measure/remaining Times"
			},
			DefaultPayload = JObject.Parse("{}")
		};

		public override async Task Handle(CancellationToken cancellationToken)
		{
			await CompleteJob(new
			{
				elapsedTime = (TimerService.Instance.GetElapsedTime() + TimeSpan.FromSeconds(1)).ToString(@"hh\:mm\:ss"),
				timeToMeasure = TimerService.Instance.GetTimeToMeasure().ToString(@"hh\:mm\:ss"),
				remainingTime = (TimerService.Instance.GetTimeToMeasure() - TimerService.Instance.GetElapsedTime()).ToString(@"hh\:mm\:ss"),
				jobStatus = Statuses.Success.ToString()
			});
		}
	}
}
