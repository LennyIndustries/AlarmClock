using System;
using System.Media;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace AlarmClock
{
	public class Alarm : Clock
	{
		private SoundPlayer _buzzer;
		private DispatcherTimer _alarmTimer;
		private readonly Canvas canvas;
		private readonly SolidColorBrush defaultColor;

		public Alarm(Canvas canvas, int tickLength, double radius, int amountOfTicks, int tickOffset = 0, Color? color = null)
			: base(canvas, tickLength, radius, amountOfTicks, tickOffset, color)
		{
			minute = 60;
			hour = 12;
			this.canvas = canvas;
			blinkSpeed = 50;

			defaultColor = new SolidColorBrush();
			defaultColor = (SolidColorBrush) canvas.Background;
			blinkColor = new SolidColorBrush(Colors.OrangeRed);

			_buzzer = new SoundPlayer("Sounds\\Buzzer.wav");

			_alarmTimer = new DispatcherTimer();
			_alarmTimer.Tick += AlarmTimer_Tick;
			_alarmTimer.Interval = TimeSpan.FromMilliseconds(blinkSpeed);

			UpdateAlarm();
		}

		public int minute { get; set; }
		public int hour { get; set; }
		public int blinkSpeed { get; set; }
		public SolidColorBrush blinkColor { get; set; }

		public void UpdateAlarm()
		{
			UpdateClock(hour * 5, "alarm_hour", 10);
			UpdateClock(minute, "alarm_minute");

			_alarmTimer.Interval = TimeSpan.FromMilliseconds(blinkSpeed);
		}

		public TimeSpan GetCountdown()
		{
			int currentHour = DateTime.Now.Hour;
			int currentMinute = DateTime.Now.Minute;
			int currentSecond = DateTime.Now.Second;
			
			TimeSpan currentTime = new TimeSpan(currentHour, currentMinute, currentSecond);
			TimeSpan alarmTime = new TimeSpan(hour, minute, 0);

			if (currentTime > alarmTime)
			{
				alarmTime += TimeSpan.FromHours(24);
			}

			TimeSpan countdown = alarmTime.Subtract(currentTime);
			return countdown;
		}

		public bool CheckAlarm()
		{
			DateTime alarmTime = new DateTime();
			DateTime currentTime = new DateTime();

			alarmTime = alarmTime.AddHours(hour);
			alarmTime = alarmTime.AddMinutes(minute);

			currentTime = currentTime.AddHours(DateTime.Now.Hour);
			currentTime = currentTime.AddMinutes(DateTime.Now.Minute);

			if (currentTime > alarmTime)
			{
				alarmTime = alarmTime.AddHours(24);
			}

			if (alarmTime <= currentTime)
			{
				return true;
			}

			return false;
		}

		public void Start(bool mute = false)
		{
			if (!mute)
			{
				_buzzer.PlayLooping();
			}

			_alarmTimer.Start();
		}

		public void Stop()
		{
			_buzzer.Stop();
			_alarmTimer.Stop();

			canvas.Background = defaultColor;
		}

		private void Blink()
		{
			SolidColorBrush currentColor = new SolidColorBrush();

			currentColor = (SolidColorBrush) canvas.Background;

			if (currentColor.Color == defaultColor.Color)
			{
				canvas.Background = blinkColor;
			}
			else
			{
				canvas.Background = defaultColor;
			}
		}

		private void AlarmTimer_Tick(object sender, EventArgs e)
		{
			Blink();
		}

	}
}
