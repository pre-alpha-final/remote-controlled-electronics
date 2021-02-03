using System;
using System.Device.Gpio;
using System.Threading.Tasks;

namespace RpzwTimer
{
	class Program
	{
		static async Task Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			var gpioController = new GpioController();
			gpioController.OpenPin(21, PinMode.Output);

			try
			{
				while (true)
				{
					gpioController.Write(21, PinValue.High);
					await Task.Delay(1000);
					gpioController.Write(21, PinValue.Low);
					await Task.Delay(1000);
				}
			}
			finally
			{
				gpioController.ClosePin(21);
			}
		}
	}
}
