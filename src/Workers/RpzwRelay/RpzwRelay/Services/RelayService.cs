using System;
using System.Device.Gpio;
using System.Threading;
using System.Threading.Tasks;

namespace RpzwRelay.Services
{
	public class RelayService
	{
		private CancellationTokenSource _timerCts;
		private static RelayService _instance;
		public static RelayService Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new RelayService();
				}
				return _instance;
			}
		}

		public GpioController GpioController { get; }

		public RelayService()
		{
			GpioController = new GpioController();
			GpioController.OpenPin(18, PinMode.Output);
		}

		public Task TurnOn()
		{
			GpioController.Write(18, PinValue.High);
			return Task.CompletedTask;
		}

		public Task TurnOff()
		{
			GpioController.Write(18, PinValue.Low);
			return Task.CompletedTask;
		}

		public async Task RunTimer(double seconds)
		{
			_timerCts?.Cancel();
			_timerCts = new CancellationTokenSource();

			var startTime = DateTimeOffset.Now;

			GpioController.Write(18, PinValue.High);
			while (true)
			{
				await Task.Delay(1000);
				if (startTime + TimeSpan.FromSeconds(seconds) > DateTimeOffset.Now)
				{
					break;
				}
				if (_timerCts.Token.IsCancellationRequested)
				{
					return;
				}
			}
			GpioController.Write(18, PinValue.Low);
		}
	}
}
