using System;
using System.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace AlarmClock
{
	/// <summary>
	/// Created by: Leander Dumas
	/// github.com/LennyIndustries
	/// </summary>
	public partial class MainWindow : Window
	{
		private Clock clock, secondsClock, dayClock, monthClock;
		private Alarm alarm;

		private DispatcherTimer alarmDispatcherTimer;
		private DispatcherTimer alarmStopDispatcherTimer;
		private SoundPlayer buzzer;

		public MainWindow()
		{
			InitializeComponent();

			Color color = Colors.Black; 

			clock = new Clock(clockCanvas, 10, 5, 145, 60, color);
			secondsClock = new Clock(secondsCanvas, 5, 2, 42, 60, color);
			dayClock = new Clock(dayCanvas, 5, 0, 42, 7, color);
			monthClock = new Clock(monthCanvas, 5, 0, 42, 12, color);

			alarm = new Alarm(alarmCanvas, 5, 2, 52, 60, color);
			alarm.UpdateAlarm();

			buzzer = new SoundPlayer("Buzzer.wav");

			// Update timer
			DispatcherTimer updateClockTimer = new DispatcherTimer();
			updateClockTimer.Tick += UpdateClockTimerOnTick;
			updateClockTimer.Interval = TimeSpan.FromMilliseconds(50);
			updateClockTimer.Start();

			// Alarm timer
			alarmDispatcherTimer = new DispatcherTimer();
			alarmDispatcherTimer.Tick += AlarmDispatcherTimerOnTick;
			alarmDispatcherTimer.Interval = TimeSpan.FromMilliseconds(50);

			// Alarm stop timer
			alarmStopDispatcherTimer = new DispatcherTimer();
			alarmStopDispatcherTimer.Tick += AlarmStopDispatcherTimerOnTick;
			alarmStopDispatcherTimer.Interval = TimeSpan.FromSeconds(20);
		}

		private void PlayAlarm()
		{
			alarmStopDispatcherTimer.Start();
			buzzer.Play();
		}

		private void StopAlarm()
		{
			alarmStopDispatcherTimer.Stop();
			alarmDispatcherTimer.Stop();

			buzzer.Stop();

			alarm.handColor = Colors.Black;
			alarm.UpdateAlarm();

			alarmHourRun.Text = "HH";
			alarmMinuteRun.Text = "MM";
			alarmSecondRun.Text = "SS";
		}

		private void UpdateClockTimerOnTick(object sender, EventArgs eventArgs)
		{
			int second = DateTime.Now.Second;
			int minute = DateTime.Now.Minute;
			int hour = DateTime.Now.Hour;
			int day = (int)DateTime.Now.DayOfWeek;
			int month = DateTime.Now.Month;

			secondsClock.UpdateClock(second);
			clock.UpdateClock(minute, "minuteHand");
			clock.UpdateClock(hour * 5, "hourHand", 25);
			dayClock.UpdateClock(day);
			monthClock.UpdateClock(month);

			dayTextBlock.Text = DateTime.Now.ToString("dddd");
			monthTextBlock.Text = DateTime.Now.ToString("dd MMMM");

			timeHourRun.Text = DateTime.Now.ToString("HH");
			timeMinuteRun.Text = DateTime.Now.ToString("mm");
			timeSecondRun.Text = DateTime.Now.ToString("ss");
		}

		private void AlarmDispatcherTimerOnTick(object sender, EventArgs eventArgs)
		{
			TimeSpan countdown;
			countdown = alarm.GetCountdown();
			alarmHourRun.Text = countdown.ToString("hh");
			alarmMinuteRun.Text = countdown.ToString("mm");
			alarmSecondRun.Text = countdown.ToString("ss");

			if (countdown.Seconds == 0 && countdown.Minutes == 0 && countdown.Hours == 0)
			{
				alarmDispatcherTimer.Stop();
				PlayAlarm();
			}
		}

		private void AlarmStopDispatcherTimerOnTick(object sender, EventArgs e)
		{
			StopAlarm();
		}

		private void setAlarmButton_Click(object sender, RoutedEventArgs e)
		{
			int hour = Convert.ToInt32(hourTextBox.Text);
			int minute = Convert.ToInt32(minuteTextBox.Text);

			if (hour < 24 && hour >= 0 && minute < 60 && minute >= 0)
			{
				alarm.hour = hour;
				alarm.minute = minute;
			}
			else
			{
				MessageBox.Show("Out of bounds; hour between 0 and 23, minute between 0 and 59");
			}

			alarmDispatcherTimer.Start();
			alarm.handColor = Colors.Red;

			alarm.UpdateAlarm();
		}

		private void alarmCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			StopAlarm();
		}
	}
}
