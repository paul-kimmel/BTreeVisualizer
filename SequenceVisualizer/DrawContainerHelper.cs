using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LinqVisualizer
{
  public static class DrawContainerHelper
  {
    public static void DrawDebugOutline(Graphics graphics, int width, int height)
    {
      float[] dashValues = { 5, 2, 15, 4 };
      Pen p = new Pen(Color.Red, 1);
      p.DashPattern = dashValues;
      graphics.DrawRectangle(p, 0, 0, width - 1, height - 1);
    }

    public static void ClipRegionOneTime(Control control, int x, int y, int width, int height)
    {
      GraphicsPath path = new GraphicsPath();
      path.StartFigure();
      path.AddEllipse(x, y, width, height);
      path.CloseFigure();
      control.Region = new Region(path);
    }
  }
}
