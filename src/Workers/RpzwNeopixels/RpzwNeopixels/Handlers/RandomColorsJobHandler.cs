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
	public class RandomColorsJobHandler : JobHandlerBase
	{
		public RandomColorsJobHandler(string baseUrl, Job job) : base(baseUrl, job)
		{
		}

		public override JobDescription JobDescription => new JobDescription
		{
			Name = "Random Colors",
			Description = new List<string>
			{
				"Set all leds to random colors"
			},
			DefaultPayload = JObject.Parse(@"{}")
		};

		public override async Task Handle(CancellationToken cancellationToken)
		{
			var pixelTransition = new List<PixelSet>();
			var zeroPixelSet = new PixelSet();
			var pixelSet = new PixelSet();
			var random = new Random();

			for (var i = 0; i < Consts.PixelCount; i++)
			{
				zeroPixelSet.Pixels.Add(new Pixel(i, 0, 0, 0));
				pixelSet.Pixels.Add(new Pixel(i, random.Next(21), random.Next(21), random.Next(21)));
			}
			pixelTransition.Add(pixelSet);
			pixelTransition.Add(zeroPixelSet);
			NeopixelCompiler.RunPixelTransition(5, pixelTransition);

			await CompleteJob(new { jobStatus = Statuses.Success.ToString() });
		}
	}
}
