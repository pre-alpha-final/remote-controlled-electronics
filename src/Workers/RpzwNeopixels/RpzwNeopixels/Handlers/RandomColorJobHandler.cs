using Newtonsoft.Json.Linq;
using RceSharpLib;
using RceSharpLib.JobHandlers;
using RpzwNeopixels.Neopixel;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RpzwNeopixels.Handlers
{
	/*
	 * Max 5s & 20/255 per pixel brightness not to blow the 3A power supply
	 */
	public class RandomColorJobHandler : JobHandlerBase
	{
		private readonly int[] possiblePixelValues = { 0, 10, 20 };

		public RandomColorJobHandler(string baseUrl, Job job) : base(baseUrl, job)
		{
		}

		public override JobDescription JobDescription => new JobDescription
		{
			Name = "Random Color",
			Description = new List<string>
			{
				"Set all leds to a random color"
			},
			DefaultPayload = JObject.Parse(@"{}")
		};

		public override async Task Handle(CancellationToken cancellationToken)
		{
			var pixelTransition = new List<PixelSet>();
			var zeroPixelSet = new PixelSet();
			var pixelSet = new PixelSet();
			var random = new Random();
			int red, green, blue;
			do
			{
				red = possiblePixelValues[random.Next(3)];
				green = possiblePixelValues[random.Next(3)];
				blue = possiblePixelValues[random.Next(3)];
			} while ((red == 0 && green == 0 && blue == 0) || (red == 20 && green == 20 && blue == 20));

			for (var i = 0; i < Consts.PixelCount; i++)
			{
				zeroPixelSet.Pixels.Add(new Pixel(i, 0, 0, 0));
				pixelSet.Pixels.Add(new Pixel(i, red, green, blue));
			}
			pixelTransition.Add(pixelSet);
			pixelTransition.Add(zeroPixelSet);
			NeopixelCompiler.RunPixelTransition(5, pixelTransition);

			await CompleteJob(new { jobStatus = Statuses.Success.ToString() });
		}
	}
}
