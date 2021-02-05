using System;
using System.Diagnostics;

namespace RpzwTimer.Services
{
	public class TimerService
	{
		private static TimerService _instance;
		public static TimerService Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new TimerService();
				}
				return _instance;
			}
		}

		private TimeSpan _timeToMeasure;
		private Stopwatch _stopwatch = new Stopwatch();

		public void Start(TimeSpan timeToMeasure)
		{
			_timeToMeasure = timeToMeasure;
			_stopwatch.Restart();
		}

		public bool Pause()
		{
			if (_stopwatch.IsRunning)
			{
				_stopwatch.Stop();
				return true;
			}
			else
			{
				_stopwatch.Start();
				return false;
			}
		}

		public void Stop()
		{
			_stopwatch.Reset();
			_timeToMeasure = TimeSpan.Zero;
		}

		public TimeSpan GetElapsedTime()
		{
			return _stopwatch.Elapsed;
		}

		public TimeSpan GetTimeToMeasure()
		{
			return _timeToMeasure;
		}
	}
}
