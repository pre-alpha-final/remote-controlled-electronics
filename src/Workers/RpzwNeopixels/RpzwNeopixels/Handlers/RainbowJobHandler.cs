using Newtonsoft.Json.Linq;
using RceSharpLib;
using RceSharpLib.JobHandlers;
using RpzwNeopixels.Neopixel;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RpzwNeopixels.Handlers
{
	/*
	 * Max 5s & limited brightness not to blow the 3A power supply
	 */
	public class RainbowJobHandler : JobHandlerBase
	{
		public RainbowJobHandler(string baseUrl, Job job) : base(baseUrl, job)
		{
		}

		public override JobDescription JobDescription => new JobDescription
		{
			Name = "Rainbow",
			Description = new List<string>
			{
				"Run rainbow pattern"
			},
			DefaultPayload = JObject.Parse(@"{}")
		};

		public override async Task Handle(CancellationToken cancellationToken)
		{
			const int TransitionSize = 60;
			const int Transition1Break = 60;
			const int Transition2Break = 120;
			const int Transition3Break = 180;
			const int Transition4Break = 240;
			const int Transition5Break = 300;

			var pixelTransition = new List<PixelSet>();
			var zeroPixelSet = new PixelSet();
			var pixelSet = new PixelSet();
			var brightness = 0.25;

			for (var pixelIndex = 0; pixelIndex < Consts.PixelCount; pixelIndex++)
			{
				zeroPixelSet.Pixels.Add(new Pixel(pixelIndex, 0, 0, 0));

				if (pixelIndex < Transition1Break)
				{
					pixelSet.Pixels.Add(new Pixel(pixelIndex, 255, 0 + 127 * pixelIndex / TransitionSize, 0, brightness));
				}
				else if (pixelIndex < Transition2Break)
				{
					var transitionRelativePixelIndex = pixelIndex - Transition1Break;
					pixelSet.Pixels.Add(new Pixel(pixelIndex, 255, 127 + 128 * transitionRelativePixelIndex / TransitionSize, 0, brightness));
				}
				else if (pixelIndex < Transition3Break)
				{
					var transitionRelativePixelIndex = pixelIndex - Transition2Break;
					pixelSet.Pixels.Add(new Pixel(pixelIndex, 255 - 255 * transitionRelativePixelIndex / TransitionSize, 255, 0, brightness));
				}
				else if (pixelIndex < Transition4Break)
				{
					var transitionRelativePixelIndex = pixelIndex - Transition3Break;
					pixelSet.Pixels.Add(new Pixel(pixelIndex, 0, 255 - 255 * transitionRelativePixelIndex / TransitionSize, 0 + 255 * transitionRelativePixelIndex / TransitionSize, brightness));
				}
				else if (pixelIndex < Transition5Break)
				{
					var transitionRelativePixelIndex = pixelIndex - Transition4Break;
					pixelSet.Pixels.Add(new Pixel(pixelIndex, 0 + 143 * transitionRelativePixelIndex / TransitionSize, 0, 255, brightness));
				}
			}
			pixelTransition.Add(pixelSet);
			pixelTransition.Add(zeroPixelSet);
			NeopixelCompiler.RunPixelTransition(5, pixelTransition);

			await CompleteJob(new { jobStatus = Statuses.Success.ToString() });
		}
	}
}
