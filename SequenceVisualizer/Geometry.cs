using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace LinqVisualizer
{
  public class Geometry
  {
    public static  double GetRadius(int width)
    {
      return width / 2;
    }


    public static  PointF GetCenter(Rectangle r)
    {
      return new PointF(r.Width / 2, r.Height / 2);
    }

    public static Point GetCenter2(Rectangle r)
    {
      return new Point(r.Width / 2, r.Height / 2);
    }

    public static  double GetAngle(PointF p1, PointF p2)
    {
      return Math.Atan((p1.Y - p2.Y) / (p1.X - p2.X)) * 180 / Math.PI;
    }

    public static PointF PointOnRectangle(Rectangle rect, double angle, int adjustX, int adjustY)
    {
      PointF center = GetCenter(rect);
      double x = center.X + rect.Width/2 *  adjustX * Math.Cos(angle * Math.PI / 180);
      double y = center.Y + rect.Height/2 * adjustY * Math.Sin(angle * Math.PI / 180);
      
      return new PointF((float)x, (float)y);  
    }
    
    public static  PointF PointOnCircle(Rectangle rect, double angle, int adjustX, int adjustY)
    {
      double radius = GetRadius(rect.Width);
      PointF center = GetCenter(rect);

      double x = center.X + radius * adjustX * Math.Cos(angle * Math.PI / 180);
      double y = center.Y + radius * adjustY * Math.Sin(angle * Math.PI / 180);
      return new PointF((float)x, (float)y);
    }


    public static Point GetControlLocation(Control control)
    {
      try
      {
        return control.FindForm().PointToClient(control.Parent.PointToScreen(control.Location));
      }
      catch
      {
        return new Point();
      }
    }

  }
}
