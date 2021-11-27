using System.Device.Pwm;
using System.Threading.Tasks;

namespace RpzwServo.Services
{
	public class ServoService
	{
		private const double StartPosition = 0.035;
		private const double StopPosition = 0.11;
		private const double Step = 0.0005;

		private bool _moving;
		private bool _movingClockwise;
		private double _position = StartPosition;

		private static ServoService _instance;
		public static ServoService Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ServoService();
				}
				return _instance;
			}
		}

		public void Initialize()
		{
			PwmChannel pwmChannel = PwmChannel.Create(0, 0, 50, StartPosition);
			pwmChannel.Start();
			Task.Run(() => Loop(pwmChannel));
		}

		public void Start()
		{
			_moving = true;
		}

		public void Stop()
		{
			_moving = false;
		}

		public async Task Loop(PwmChannel pwmChannel)
		{
			while (true)
			{
				if (_moving)
				{
					if (_movingClockwise)
					{
						_position -= Step;
						if (_position <= StartPosition)
						{
							_movingClockwise = false;
						}
					}
					else
					{
						_position += Step;
						if (_position >= StopPosition)
						{
							_movingClockwise = true;
						}
					}
					pwmChannel.DutyCycle = _position;
				}
				else
				{
					if (_position > StartPosition)
					{
						_position -= Step;
						pwmChannel.DutyCycle = _position;
					}
					else
					{
						pwmChannel.DutyCycle = 0;
					}
				}
				await Task.Delay(25);
			}
		}
	}
}
