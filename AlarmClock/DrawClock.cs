using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AlarmClock
{
	public abstract class DrawClock
	{
		protected void DrawClockFrame(Canvas canvas, int tickLength, int tickOffset, double radius, int amountOfTicks, Color color)
		{
			DrawTicks(canvas, tickLength, tickOffset, radius, amountOfTicks, color);
			DrawCircle(canvas, color, radius + 1, false);
			DrawCircle(canvas, color, 2.5, true);
		}

		protected void DrawHand(Canvas canvas, double radius, int tick, int totalTicks, string name, Color color)
		{
			double centerY = canvas.Height / 2;
			double centerX = canvas.Width / 2;

			double xPosI, yPosI;
			double xPosO, yPosO;

			GetXY(centerX, centerY, radius, tick, totalTicks, out xPosO, out yPosO);
			GetXY(centerX, centerY, 2.5, tick, totalTicks, out xPosI, out yPosI);

			DrawLine(canvas, xPosI, xPosO, yPosI, yPosO, color, name);
		}

		private void DrawTicks(Canvas canvas, int length, int offset, double rad, int amount, Color color)
		// canvas = what canvas to draw on
		// length = length of one tick; offset = (only used for the seconds/minutes/hours clock) what offset to use every 5th tick
		// rad = radius of circle
		// amount = total amount of ticks
		{
			double centerY = canvas.Height / 2;
			double centerX = canvas.Width / 2;

			double xPosI, xPosO;
			double yPosI, yPosO;

			int tmp = length;

			for (int x = 0; x < amount; x += 1)
			{
				if (x % 5 != 0)
				{
					length -= offset;
				}

				GetXY(centerX, centerY, rad, x, amount, out xPosO, out yPosO);
				GetXY(centerX, centerY, rad - length, x, amount, out xPosI, out yPosI);

				DrawLine(canvas, xPosI, xPosO, yPosI, yPosO, color);

				length = tmp;
			} //END FOR
		}

		private void DrawCircle(Canvas canvas, Color color, double rad, bool fill)
		// canvas = what canvas to draw on
		// color = what color to use
		// rad = radius of th circle
		// fill = whether or not to fill the circle
		{
			Ellipse circle = new Ellipse();
			SolidColorBrush brush = new SolidColorBrush(color);

			double x = canvas.Width / 2 - rad;
			double y = canvas.Height / 2 - rad;

			circle.Width = rad * 2;
			circle.Height = rad * 2;
			circle.Margin = new Thickness(x, y, 0, 0);
			circle.Stroke = brush;

			if (fill)
			{
				circle.Fill = brush;
			}

			canvas.Children.Add(circle);
		}

		private void GetXY(double xCen, double yCen, double rad, int tick, int amount, out double xPos, out double yPos)
		// xCen = x center of circle; yCen = y center of circle
		// rad = radius of circle
		// tick = what tick to get (e.g. tick = 5 of the 5th minute); amount = total amount of ticks (e.g. 60 for 60 ticks (1 full hour of 60 minutes))
		{
			xPos = xCen + Math.Cos(-2 * Math.PI * tick / amount + (0.5 * Math.PI)) * rad;
			yPos = yCen - Math.Sin(-2 * Math.PI * tick / amount + (0.5 * Math.PI)) * rad;
		}

		private void DrawLine(Canvas canvas, double xPosI, double xPosO, double yPosI, double yPosO, Color color, string name = null)
		{
			Line line = new Line();
			line.X1 = xPosI;
			line.X2 = xPosO;
			line.Y1 = yPosI;
			line.Y2 = yPosO;

			line.Stroke = new SolidColorBrush(color);
			line.StrokeThickness = 1;

			if (name != null)
			{
				line.Name = name;
			}

			canvas.Children.Add(line);
		}
	}
}
