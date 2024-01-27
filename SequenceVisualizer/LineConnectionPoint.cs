using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic.PowerPacks;
using System.Drawing;

namespace LinqVisualizer
{
  public class LineConnectionPoint : LineShape
  {
    private static ShapeContainer container;
    public static void SetContainer(ShapeContainer container)
    {
      LineConnectionPoint.container = container;
    }

    private ConnectionPoint endPoint1;
    public ConnectionPoint EndPoint1
    {
      get { return endPoint1; }
      set
      {
        endPoint1 = value;
        OnEndPointChanged(endPoint1);
      }
    }
    #region Constructors
    private ConnectionPoint endPoint2;
    public LineConnectionPoint()
    {
      Parent = container;
    }
    public LineConnectionPoint(ShapeContainer parent)
      : base(parent)
    {
      SetContainer(parent);
    }

    public LineConnectionPoint(int x1, int y1, int x2, int y2)
      : base(x1, y1, x2, y2)
    {

    }
    #endregion

    public ConnectionPoint EndPoint2
    {
      get { return endPoint2; }
      set
      {
        endPoint2 = value;
        OnEndPointChanged(endPoint2);
      }
    }

    private void OnEndPointChanged(ConnectionPoint endPoint)
    {
      if (endPoint == null) return;

      Action<object, EventArgs> ev = (sender, e) =>
      {
        if (endPoint1 == null || endPoint2 == null) return;
        Point ep1 = Geometry.GetCenter2(endPoint1.Bounds);
        Point ep2 = Geometry.GetCenter2(endPoint2.Bounds);
        StartPoint = new Point(endPoint1.Left + ep1.X, endPoint1.Top + ep1.Y);
        EndPoint = new Point(endPoint2.Left + ep2.X, endPoint2.Top + ep2.Y);
      };

      endPoint.LocationChanged += new EventHandler(ev);
      endPoint.SizeChanged += new EventHandler(ev);
      ev(this, EventArgs.Empty);
    }
  }


}
