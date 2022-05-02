using System;
using System.Device.Gpio;

namespace RceRemoteSharpLib
{
	public class GpioService
	{
		private static object _pinValueChangedLock = new object();
		private DateTimeOffset _lastPinValueRise = DateTimeOffset.UtcNow;

		public GpioController GpioController { get; }

		public GpioService()
		{
			GpioController = new GpioController();
		}

		public void Run()
		{
			OpenPin(23);
			OpenPin(24);
			OpenPin(5);
			OpenPin(6);
			OpenPin(13);
		}

		private void OpenPin(int pin)
		{
			if (GpioController.IsPinOpen(pin))
			{
				GpioController.ClosePin(pin);
			}
			GpioController.OpenPin(pin, PinMode.InputPullDown);
			GpioController.RegisterCallbackForPinValueChangedEvent(pin, PinEventTypes.Rising, OnPinValueChanged);
			GpioController.RegisterCallbackForPinValueChangedEvent(pin, PinEventTypes.Falling, OnPinValueChanged);
		}

		private void OnPinValueChanged(object sender, PinValueChangedEventArgs pinValueChangedEventArgs)
		{
			//Console.WriteLine($"{pinValueChangedEventArgs.PinNumber}: {pinValueChangedEventArgs.ChangeType}");
			lock (_pinValueChangedLock)
			{
				if (pinValueChangedEventArgs.ChangeType == PinEventTypes.Rising &&
					_lastPinValueRise + TimeSpan.FromSeconds(1) < DateTimeOffset.UtcNow)
				{
					_lastPinValueRise = DateTimeOffset.UtcNow;
					Console.WriteLine($"{pinValueChangedEventArgs.PinNumber} trigger");
				}
			}
		}
	}
}
