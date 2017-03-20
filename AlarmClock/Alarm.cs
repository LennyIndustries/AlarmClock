using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace AlarmClock
{
	public class Alarm : Clock
	{
		public Alarm(Canvas canvas, int tickLength, int tickOffset, double radius, int amountOfTicks, Color color)
			: base(canvas, tickLength, tickOffset, radius, amountOfTicks, color)
		{
			minute = 60;
			hour = 12;
		}

		public int minute { get; set; }
		public int hour { get; set; }

		public void UpdateAlarm()
		{
			UpdateClock(hour * 5, "alarm_hour", 10);
			UpdateClock(minute, "alarm_minute");
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
	}
}
