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
			GpioController.OpenPin(23, PinMode.InputPullDown);
			GpioController.RegisterCallbackForPinValueChangedEvent(23, PinEventTypes.Rising, OnPinValueChanged);
			GpioController.RegisterCallbackForPinValueChangedEvent(23, PinEventTypes.Falling, OnPinValueChanged);

			GpioController.OpenPin(24, PinMode.InputPullDown);
			GpioController.RegisterCallbackForPinValueChangedEvent(24, PinEventTypes.Rising, OnPinValueChanged);
			GpioController.RegisterCallbackForPinValueChangedEvent(24, PinEventTypes.Falling, OnPinValueChanged);

			GpioController.OpenPin(5, PinMode.InputPullDown);
			GpioController.RegisterCallbackForPinValueChangedEvent(5, PinEventTypes.Rising, OnPinValueChanged);
			GpioController.RegisterCallbackForPinValueChangedEvent(5, PinEventTypes.Falling, OnPinValueChanged);

			GpioController.OpenPin(6, PinMode.InputPullDown);
			GpioController.RegisterCallbackForPinValueChangedEvent(6, PinEventTypes.Rising, OnPinValueChanged);
			GpioController.RegisterCallbackForPinValueChangedEvent(6, PinEventTypes.Falling, OnPinValueChanged);

			GpioController.OpenPin(13, PinMode.InputPullDown);
			GpioController.RegisterCallbackForPinValueChangedEvent(13, PinEventTypes.Rising, OnPinValueChanged);
			GpioController.RegisterCallbackForPinValueChangedEvent(13, PinEventTypes.Falling, OnPinValueChanged);
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
