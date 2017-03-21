using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AlarmClock
{
	public class Clock : DrawClock
	{
		private readonly Canvas _canvas;
		private readonly int _amountOfTicks;
		private readonly double _handLength;

		public Clock(Canvas canvas, int tickLength, double radius, int amountOfTicks, int tickOffset = 0, Color? color = null)
		{
			if (color == null)
			{
				color = Colors.Black;
			}

			_canvas = canvas;
			_amountOfTicks = amountOfTicks;
			handColor = (Color)color;
			_handLength = radius - (tickLength + 3);

			DrawClockFrame(canvas, tickLength, tickOffset, radius, amountOfTicks, (Color)color);
		}

		public Color handColor { get; set; }

		public void UpdateClock(int tick, string name = "hand", int handOffset = 0)
		{
			UIElement line = (UIElement)LogicalTreeHelper.FindLogicalNode(_canvas, name);
			_canvas.Children.Remove(line);
			DrawHand(_canvas, _handLength - handOffset, tick, _amountOfTicks, name, handColor);
		}
	}
}
