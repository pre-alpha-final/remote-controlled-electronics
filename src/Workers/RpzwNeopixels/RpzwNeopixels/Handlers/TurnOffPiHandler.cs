using Newtonsoft.Json.Linq;
using RceSharpLib;
using RceSharpLib.JobHandlers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RpzwNeopixels.Handlers
{
	public class TurnOffPiHandler : JobHandlerBase
	{
		public TurnOffPiHandler(string baseUrl, Job job) : base(baseUrl, job)
		{
		}

		public override JobDescription JobDescription => new JobDescription
		{
			Name = "Turn Off Pi",
			Description = new List<string>
			{
				"run 'sudo poweroff'"
			},
			DefaultPayload = JObject.Parse("{}")
		};

		public override async Task Handle(CancellationToken cancellationToken)
		{
			await CompleteJob(new { jobStatus = Statuses.Success.ToString() });
			new Process() { StartInfo = new ProcessStartInfo("/bin/bash", "-c 'sudo poweroff'"), }.Start();
		}
	}
}
