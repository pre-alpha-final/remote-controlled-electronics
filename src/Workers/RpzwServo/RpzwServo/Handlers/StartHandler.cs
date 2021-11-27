using Newtonsoft.Json.Linq;
using RceSharpLib;
using RceSharpLib.JobHandlers;
using RpzwServo.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RpzwServo.Handlers
{
	public class StartHandler : JobHandlerBase
	{
		public StartHandler(string baseUrl, Job job) : base(baseUrl, job)
		{
		}

		public override JobDescription JobDescription => new JobDescription
		{
			Name = "Start moving",
			Description = new List<string>
			{
				"Starts moving back and forth"
			},
			DefaultPayload = JObject.Parse(@"{}")
		};

		public override async Task Handle(CancellationToken cancellationToken)
		{
			ServoService.Instance.Start();
			await CompleteJob(new { jobStatus = Statuses.Success.ToString() });
		}
	}
}
