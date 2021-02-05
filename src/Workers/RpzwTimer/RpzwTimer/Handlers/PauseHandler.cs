using Newtonsoft.Json.Linq;
using RceSharpLib;
using RceSharpLib.JobHandlers;
using RpzwTimer.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RpzwTimer.Handlers
{
	public class PauseHandler : JobHandlerBase
	{
		public PauseHandler(string baseUrl, Job job) : base(baseUrl, job)
		{
		}

		public override JobDescription JobDescription => new JobDescription
		{
			Name = "Pause/Resume Timer",
			Description = new List<string>
			{
				"Toogles pause on the timer"
			},
			DefaultPayload = JObject.Parse("{}")
		};

		public override async Task Handle(CancellationToken cancellationToken)
		{
			var pause = TimerService.Instance.Pause();
			await CompleteJob(new
			{
				status = pause ? "Paused" : "Resumed",
				jobStatus = Statuses.Success.ToString()
			});
		}
	}
}
