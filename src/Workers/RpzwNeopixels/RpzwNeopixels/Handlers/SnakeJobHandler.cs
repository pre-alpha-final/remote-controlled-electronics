using Newtonsoft.Json.Linq;
using RceSharpLib;
using RceSharpLib.JobHandlers;
using RpzwNeopixels.Neopixel;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RpzwNeopixels.Handlers
{
	public class SnakeJobHandler : JobHandlerBase
	{
		public SnakeJobHandler(string baseUrl, Job job) : base(baseUrl, job)
		{
		}

		public override JobDescription JobDescription => new JobDescription
		{
			Name = "Snake",
			Description = new List<string>
			{
				"Run the snake pattern"
			},
			DefaultPayload = JObject.Parse(@"{ ""length"": 3, ""red"": 0, ""green"": 255, ""blue"": 255 }")
		};

		public override async Task Handle(CancellationToken cancellationToken)
		{
			var length = Job.Payload.SelectToken("$.length").ToObject<int>();
			var red = Job.Payload.SelectToken("$.red").ToObject<int>();
			var green = Job.Payload.SelectToken("$.green").ToObject<int>();
			var blue = Job.Payload.SelectToken("$.blue").ToObject<int>();
			var pixelTransition = new List<PixelSet>();
			for (var leadPixelIndex = 0; leadPixelIndex < Consts.PixelCount + length; leadPixelIndex++)
			{
				var pixelSet = new PixelSet();
				pixelSet.Pixels.AddRange(GetPixels(leadPixelIndex, red, green, blue, length));
				pixelTransition.Add(pixelSet);
			}
			NeopixelController.RunPixelTransition(0, pixelTransition);

			await CompleteJob(new { jobStatus = Statuses.Success.ToString() });
		}

		private IEnumerable<Pixel> GetPixels(int leadPixelIndex, int red, int green, int blue, int length)
		{
			var pixels = new List<Pixel>();
			for (var shift = 0; shift <= length; shift++)
			{
				var pixelIndex = leadPixelIndex - shift;
				if (pixelIndex < 0)
				{
					break;
				}
				if (pixelIndex >= Consts.PixelCount)
				{
					continue;
				}

				pixels.Add(new Pixel
				{
					Index = pixelIndex,
					Red = red,
					Green = green,
					Blue = blue,
					Brightness = leadPixelIndex == pixelIndex ? 1 : (1 - (double)shift / length) / 2
				});
			}

			return pixels;
		}
	}
}
