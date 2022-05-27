using Newtonsoft.Json.Linq;
using RceSharpLib;
using RceSharpLib.JobHandlers;
using RpzwRelay.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RpzwRelay.Handlers
{
	public class TurnOffHandler : JobHandlerBase
	{
		public TurnOffHandler(string baseUrl, Job job) : base(baseUrl, job)
		{
		}

		public override JobDescription JobDescription => new JobDescription
		{
			Name = "Turn off",
			Description = new List<string>
			{
				"Switch the relay to closed"
			},
			DefaultPayload = JObject.Parse(@"{}")
		};

		public override async Task Handle(CancellationToken cancellationToken)
		{
			await RelayService.Instance.TurnOff();
			await CompleteJob(new { jobStatus = Statuses.Success.ToString() });
		}
	}
}
