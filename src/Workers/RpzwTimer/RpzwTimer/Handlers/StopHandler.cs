using Newtonsoft.Json.Linq;
using RceSharpLib;
using RceSharpLib.JobHandlers;
using RpzwTimer.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RpzwTimer.Handlers
{
	public class StopHandler : JobHandlerBase
	{
		public StopHandler(string baseUrl, Job job) : base(baseUrl, job)
		{
		}

		public override JobDescription JobDescription => new JobDescription
		{
			Name = "Stop Timer",
			Description = new List<string>
			{
				"Stops the timer, resets measured time"
			},
			DefaultPayload = JObject.Parse("{}")
		};

		public override async Task Handle(CancellationToken cancellationToken)
		{
			TimerService.Instance.Stop();
			await CompleteJob(new { jobStatus = Statuses.Success.ToString() });
		}
	}
}
