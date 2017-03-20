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

		public Color handColor { get; set; }

		public Clock(Canvas canvas, int tickLength, int tickOffset, double radius, int amountOfTicks, Color color)
		{
			_canvas = canvas;
			_amountOfTicks = amountOfTicks;
			handColor = color;
			_handLength = radius - (tickLength + 3);

			DrawClockFrame(canvas, tickLength, tickOffset, radius, amountOfTicks, color);
		}

		public void UpdateClock(int tick, string name = "hand", int handOffset = 0)
		{
			UIElement line = (UIElement)LogicalTreeHelper.FindLogicalNode(_canvas, name);
			_canvas.Children.Remove(line);
			DrawHand(_canvas, _handLength - handOffset, tick, _amountOfTicks, handColor, name);
		}
	}
}
