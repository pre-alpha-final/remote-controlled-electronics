using Newtonsoft.Json.Linq;
using RceSharpLib;
using RceSharpLib.JobHandlers;
using RpzwRelay.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RpzwRelay.Handlers
{
	public class TurnOnHandler : JobHandlerBase
	{
		public TurnOnHandler(string baseUrl, Job job) : base(baseUrl, job)
		{
		}

		public override JobDescription JobDescription => new JobDescription
		{
			Name = "Turn on",
			Description = new List<string>
			{
				"Switch the relay to open"
			},
			DefaultPayload = JObject.Parse(@"{}")
		};

		public override async Task Handle(CancellationToken cancellationToken)
		{
			await RelayService.Instance.TurnOn();
			await CompleteJob(new { jobStatus = Statuses.Success.ToString() });
		}
	}
}
