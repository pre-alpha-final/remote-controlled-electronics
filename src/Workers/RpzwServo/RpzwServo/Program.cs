using System;
using System.Device.Pwm;
using System.Threading.Tasks;

namespace RpzwServo
{
	class Program
	{
		static async Task Main(string[] args)
		{
			PwmChannel pwmChannel = PwmChannel.Create(0, 0, 50, 0);
			pwmChannel.Start();

			while (true)
			{
				for (var i = 0.035; i < 0.11; i += 0.002)
				{
					pwmChannel.DutyCycle = i;
					Console.WriteLine(i);
					await Task.Delay(100);
				}
			}

			pwmChannel.Stop();
		}
	}
}
