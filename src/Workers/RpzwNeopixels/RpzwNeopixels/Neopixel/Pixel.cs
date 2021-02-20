namespace RpzwNeopixels.Neopixel
{
	public class Pixel
	{
		public int Index { get; set; }
		public double Brightness { get; set; } = 1;

		private int _red;
		public int Red
		{
			get
			{
				return (int)(_red * Brightness);
			}
			set => _red = value;
		}

		private int _green;
		public int Green
		{
			get
			{
				return (int)(_green * Brightness);
			}
			set => _green = value;
		}

		private int _blue;
		public int Blue
		{
			get
			{
				return (int)(_blue * Brightness);
			}
			set => _blue = value;
		}

		public Pixel()
		{
		}

		public Pixel(int index, int red, int green, int blue)
			: this(index, red, green, blue, 1)
		{
		}

		public Pixel(int index, int red, int green, int blue, double brightness)
		{
			Index = index;
			Red = red;
			Green = green;
			Blue = blue;
			Brightness = brightness;
		}
	}
}
