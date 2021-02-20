using System.Collections.Generic;

namespace RpzwNeopixels.Neopixel
{
	public static class NeopixelCompiler
	{
		public static void SetPixels(IEnumerable<Pixel> pixels)
		{
			var commands = new List<string>
			{
				$"pixels = neopixel.NeoPixel({Consts.ControlPin}, {Consts.PixelCount}, pixel_order = {Consts.PixelOrder})",
			};

			foreach (var pixel in pixels)
			{
				commands.Add($"pixels[{pixel.Index}] = ({pixel.Red}, {pixel.Green}, {pixel.Blue})");
			}

			PythonRunner.Run(commands);
		}

		public static void RunPixelTransition(double secondsDelay, IEnumerable<PixelSet> pixelTransition)
		{
			var commands = new List<string>
			{
				$"pixels = neopixel.NeoPixel({Consts.ControlPin}, {Consts.PixelCount}, pixel_order = {Consts.PixelOrder}, auto_write = False)",
			};

			foreach (var pixelSet in pixelTransition)
			{
				foreach (var pixel in pixelSet.Pixels)
				{
					commands.Add($"pixels[{pixel.Index}] = ({pixel.Red}, {pixel.Green}, {pixel.Blue})");
				}
				commands.Add("pixels.show()");
				commands.Add($"sleep({secondsDelay})");
			}
			commands.RemoveAt(commands.Count - 1);

			PythonRunner.Run(commands);
		}
	}
}
