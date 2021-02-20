using Newtonsoft.Json.Linq;
using RceSharpLib;
using RceSharpLib.JobHandlers;
using RpzwNeopixels.Neopixel;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RpzwNeopixels.Handlers
{
	public class PixelTestJobHandler : JobHandlerBase
	{
		public PixelTestJobHandler(string baseUrl, Job job) : base(baseUrl, job)
		{
		}

		public override JobDescription JobDescription => new JobDescription
		{
			Name = "Pixel Test",
			Description = new List<string>
			{
				"Test led at specific index"
			},
			DefaultPayload = JObject.Parse(@"{ ""index"": 0 }")
		};

		public override async Task Handle(CancellationToken cancellationToken)
		{
			var pixelTransition = new List<PixelSet>();
			var index = Job.Payload.SelectToken("$.index")?.ToObject<int>();

			pixelTransition.Add(new PixelSet { Pixels = new List<Pixel> { new Pixel(index ?? 0, 255, 0, 0) } });
			pixelTransition.Add(new PixelSet { Pixels = new List<Pixel> { new Pixel(index ?? 0, 0, 255, 0) } });
			pixelTransition.Add(new PixelSet { Pixels = new List<Pixel> { new Pixel(index ?? 0, 0, 0, 255) } });
			pixelTransition.Add(new PixelSet { Pixels = new List<Pixel> { new Pixel(index ?? 0, 0, 0, 0) } });
			NeopixelCompiler.RunPixelTransition(0.5, pixelTransition);

			await CompleteJob(new { jobStatus = Statuses.Success.ToString() });
		}
	}
}
