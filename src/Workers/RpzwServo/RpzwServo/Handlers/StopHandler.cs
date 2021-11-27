using Newtonsoft.Json.Linq;
using RceSharpLib;
using RceSharpLib.JobHandlers;
using RpzwServo.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RpzwServo.Handlers
{
	public class StopHandler : JobHandlerBase
	{
		public StopHandler(string baseUrl, Job job) : base(baseUrl, job)
		{
		}

		public override JobDescription JobDescription => new JobDescription
		{
			Name = "Stop moving",
			Description = new List<string>
			{
				"Moves to starting position and stops"
			},
			DefaultPayload = JObject.Parse(@"{}")
		};

		public override async Task Handle(CancellationToken cancellationToken)
		{
			ServoService.Instance.Stop();
			await CompleteJob(new { jobStatus = Statuses.Success.ToString() });
		}
	}
}
