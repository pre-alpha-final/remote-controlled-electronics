using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Threading;
using System.Threading.Tasks;

namespace RceRemoteSharpLib
{
	public class GpioService
	{
		private static object _pinValueChangedLock = new object();
		private List<int> _pins = new List<int>() { 23, 24, 5, 6, 13 };
		private DateTimeOffset[] _pinStateLastChangeUp = new DateTimeOffset[5];
		private DateTimeOffset[] _pinStateLastChangeDown = new DateTimeOffset[5];
		private Action<int, bool> _risen;
		private CancellationTokenSource _monitorCts = new CancellationTokenSource();

		public GpioController GpioController { get; }

		public GpioService()
		{
			GpioController = new GpioController();
		}

		public void Run(Action<int, bool> risen)
		{
			_risen = risen;
			foreach (var pin in _pins)
			{
				OpenPin(pin);
			}

			Task.Run(async () =>
			{
				_monitorCts.Cancel();
				_monitorCts = new CancellationTokenSource();
				var cancellationToken = _monitorCts.Token;
				while (true)
				{
					if (cancellationToken.IsCancellationRequested)
					{
						break;
					}

					Console.WriteLine();
					Console.WriteLine(string.Join("\t", _pins));
					Console.WriteLine(string.Join("\t",
						GpioController.Read(_pins[0]) == PinValue.High,
						GpioController.Read(_pins[1]) == PinValue.High,
						GpioController.Read(_pins[2]) == PinValue.High,
						GpioController.Read(_pins[3]) == PinValue.High,
						GpioController.Read(_pins[4]) == PinValue.High));
					await Task.Delay(250);
				}
			});
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
				var index = _pins.IndexOf(pinValueChangedEventArgs.PinNumber);
				if (pinValueChangedEventArgs.ChangeType == PinEventTypes.Rising &&
					_pinStateLastChangeUp[index] + TimeSpan.FromSeconds(1) < DateTimeOffset.UtcNow)
				{
					_pinStateLastChangeUp[index] = DateTimeOffset.UtcNow;
					_risen(pinValueChangedEventArgs.PinNumber, true);
				}
				if (pinValueChangedEventArgs.ChangeType == PinEventTypes.Falling &&
					_pinStateLastChangeDown[index] + TimeSpan.FromSeconds(1) < DateTimeOffset.UtcNow)
				{
					_pinStateLastChangeDown[index] = DateTimeOffset.UtcNow;
					_risen(pinValueChangedEventArgs.PinNumber, false);
				}
			}
		}
	}
}
