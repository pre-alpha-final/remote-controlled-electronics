using Newtonsoft.Json.Linq;
using RceSharpLib;
using RceSharpLib.JobHandlers;
using RpzwNeopixels.Neopixel;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RpzwNeopixels.Handlers
{
	public class KillTheLightsJobHandler : JobHandlerBase
	{
		public KillTheLightsJobHandler(string baseUrl, Job job) : base(baseUrl, job)
		{
		}

		public override JobDescription JobDescription => new JobDescription
		{
			Name = "Kill The Lights",
			Description = new List<string>
			{
				"Shut off all leds"
			},
			DefaultPayload = JObject.Parse(@"{}")
		};

		public override async Task Handle(CancellationToken cancellationToken)
		{
			var zeroPixelSet = new PixelSet();
			for (var i = 0; i < Consts.PixelCount; i++)
			{
				zeroPixelSet.Pixels.Add(new Pixel(i, 0, 0, 0));
			}
			NeopixelCompiler.SetPixels(zeroPixelSet.Pixels);

			await CompleteJob(new { jobStatus = Statuses.Success.ToString() });
		}
	}
}
