using Newtonsoft.Json.Linq;
using RceSharpLib;
using RceSharpLib.JobHandlers;
using RpzwRelay.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RpzwRelay.Handlers
{
	public class TimerHandler : JobHandlerBase
	{
		private readonly Job _job;

		public TimerHandler(string baseUrl, Job job) : base(baseUrl, job)
		{
			_job = job;
		}

		public override JobDescription JobDescription => new JobDescription
		{
			Name = "Timer",
			Description = new List<string>
			{
				"Turn on for a certain amount of time"
			},
			DefaultPayload = JObject.Parse(@"{""hours"":0,""minutes"":0,""seconds"":5}")
		};

		public override async Task Handle(CancellationToken cancellationToken)
		{
			var time = _job.Payload.ToObject<Time>();
			await RelayService.Instance.RunTimer(time.Hours * 3600 + time.Minutes * 60 + time.Seconds);
			await CompleteJob(new { jobStatus = Statuses.Success.ToString() });
		}
	}
}
