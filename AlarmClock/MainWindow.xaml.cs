using System;
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

		private DispatcherTimer alarmTimer;
		private DispatcherTimer alarmStopTimer;

		public MainWindow()
		{
			InitializeComponent();

			clock = new Clock(clockCanvas, 10, 145, 60, 5);
			secondsClock = new Clock(secondsCanvas, 5, 42, 60, 2);
			dayClock = new Clock(dayCanvas, 5, 42, 7);
			monthClock = new Clock(monthCanvas, 5, 42, 12);

			alarm = new Alarm(alarmCanvas, 5, 52, 60, 2);
			alarm.blinkSpeed = 500;

			// Update timer
			DispatcherTimer updateClockTimer = new DispatcherTimer();
			updateClockTimer.Tick += UpdateClockTimerOnTick;
			updateClockTimer.Interval = TimeSpan.FromMilliseconds(50);
			updateClockTimer.Start();

			// Alarm timer
			alarmTimer = new DispatcherTimer();
			alarmTimer.Tick += AlarmTimerOnTick;
			alarmTimer.Interval = TimeSpan.FromMilliseconds(50);

			// Alarm stop timer
			alarmStopTimer = new DispatcherTimer();
			alarmStopTimer.Tick += AlarmStopTimerOnTick;
			alarmStopTimer.Interval = TimeSpan.FromSeconds(20);
		}

		private void PlayAlarm()
		{
			alarmStopTimer.Start();

			if (muteCheckBox.IsChecked == true)
			{
				alarm.Start(true);
			}
			else
			{
				alarm.Start();
			}
		}

		private void StopAlarm()
		{
			alarmStopTimer.Stop();
			alarmTimer.Stop();

			alarm.Stop();

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

		private void AlarmTimerOnTick(object sender, EventArgs eventArgs)
		{
			var countdown = alarm.GetCountdown();
			alarmHourRun.Text = countdown.ToString("hh");
			alarmMinuteRun.Text = countdown.ToString("mm");
			alarmSecondRun.Text = countdown.ToString("ss");

			if (alarm.CheckAlarm())
			{
				alarmTimer.Stop();
				PlayAlarm();
			}
		}

		private void AlarmStopTimerOnTick(object sender, EventArgs e)
		{
			StopAlarm();
		}

		private void setAlarmButton_Click(object sender, RoutedEventArgs e)
		{
			int hour = -1;
			int minute = -1;

			try
			{
				hour = Convert.ToInt32(hourTextBox.Text);
				minute = Convert.ToInt32(minuteTextBox.Text);
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception);
				MessageBox.Show("Error @ MainWindow.xaml.cs @ Line : 130", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			finally
			{
				if (hour < 24 && hour >= 0 && minute < 60 && minute >= 0)
				{
					alarm.hour = hour;
					alarm.minute = minute;

					alarmTimer.Start();
					alarm.handColor = Colors.Aqua;

					alarm.UpdateAlarm();
				}
				else
				{
					MessageBox.Show("Out of bounds; hour between 0 and 23, minute between 0 and 59");
				}
			}
		}

		private void alarmCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			StopAlarm();
		}
	}
}
