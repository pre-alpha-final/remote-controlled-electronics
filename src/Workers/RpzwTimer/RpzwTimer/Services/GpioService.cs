using System.Device.Gpio;
using System.Threading.Tasks;

namespace RpzwTimer.Services
{
	public class GpioService
	{
		private static GpioService _instance;
		public static GpioService Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new GpioService();
				}
				return _instance;
			}
		}

		private bool _updating;
		public GpioController GpioController { get; }

		public GpioService()
		{
			GpioController = new GpioController();
			GpioController.OpenPin(25, PinMode.Output);
			GpioController.OpenPin(5, PinMode.Output);
			GpioController.OpenPin(12, PinMode.Output);
			GpioController.OpenPin(6, PinMode.Output);
			GpioController.OpenPin(13, PinMode.Output);
			GpioController.OpenPin(16, PinMode.Output);
			GpioController.OpenPin(19, PinMode.Output);
			GpioController.OpenPin(20, PinMode.Output);
			GpioController.OpenPin(26, PinMode.Output);
			GpioController.OpenPin(21, PinMode.Output);
		}

		public void SetAllHigh()
		{
			GpioController.Write(25, PinValue.High);
			GpioController.Write(5, PinValue.High);
			GpioController.Write(12, PinValue.High);
			GpioController.Write(6, PinValue.High);
			GpioController.Write(13, PinValue.High);
			GpioController.Write(16, PinValue.High);
			GpioController.Write(19, PinValue.High);
			GpioController.Write(20, PinValue.High);
			GpioController.Write(26, PinValue.High);
			GpioController.Write(21, PinValue.High);
		}

		public void SetAllLow()
		{
			GpioController.Write(25, PinValue.Low);
			GpioController.Write(5, PinValue.Low);
			GpioController.Write(12, PinValue.Low);
			GpioController.Write(6, PinValue.Low);
			GpioController.Write(13, PinValue.Low);
			GpioController.Write(16, PinValue.Low);
			GpioController.Write(19, PinValue.Low);
			GpioController.Write(20, PinValue.Low);
			GpioController.Write(26, PinValue.Low);
			GpioController.Write(21, PinValue.Low);
		}

		public void StartUpdating()
		{
			if (_updating)
			{
				return;
			}
			_updating = true;

			_ = Task.Run(() => KeepUpdating());
		}

		private async Task KeepUpdating()
		{
			while (true)
			{
				var elapsedTime = TimerService.Instance.GetElapsedTime();
				var timeToMeasure = TimerService.Instance.GetTimeToMeasure();
				if (timeToMeasure.Ticks == 0)
				{
					SetAllLow();
					continue;
				}

				var progress = elapsedTime.TotalSeconds / timeToMeasure.TotalSeconds;
				if (progress < 1)
				{
					UpdateProgress(progress);
				}
				else
				{
					await FlashComplete();
				}

				await Task.Delay(100);
			}
		}

		private void UpdateProgress(double progress)
		{
			if (progress > 0)
				GpioController.Write(25, PinValue.High);
			else
				GpioController.Write(25, PinValue.Low);

			if (progress > 0.11)
				GpioController.Write(5, PinValue.High);
			else
				GpioController.Write(5, PinValue.Low);

			if (progress > 0.22)
				GpioController.Write(12, PinValue.High);
			else
				GpioController.Write(12, PinValue.Low);

			if (progress > 0.33)
				GpioController.Write(6, PinValue.High);
			else
				GpioController.Write(6, PinValue.Low);

			if (progress > 0.44)
				GpioController.Write(13, PinValue.High);
			else
				GpioController.Write(13, PinValue.Low);

			if (progress > 0.55)
				GpioController.Write(16, PinValue.High);
			else
				GpioController.Write(16, PinValue.Low);

			if (progress > 0.66)
				GpioController.Write(19, PinValue.High);
			else
				GpioController.Write(19, PinValue.Low);

			if (progress > 0.77)
				GpioController.Write(20, PinValue.High);
			else
				GpioController.Write(20, PinValue.Low);

			if (progress > 0.88)
				GpioController.Write(26, PinValue.High);
			else
				GpioController.Write(26, PinValue.Low);

			if (progress > 99)
				GpioController.Write(21, PinValue.High);
			else
				GpioController.Write(21, PinValue.Low);
		}

		private async Task FlashComplete()
		{
			SetAllHigh();
			await Task.Delay(500);
			SetAllLow();
			await Task.Delay(500);
		}
	}
}
