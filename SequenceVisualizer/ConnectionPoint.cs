using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using Microsoft.VisualBasic.PowerPacks;


namespace LinqVisualizer
{

  public class ConnectionPoint : Control
  {
    #region Constructors
    public ConnectionPoint()
    {

    }
    public ConnectionPoint(string text)
      : base(text)
    {

    }
    public ConnectionPoint(string text, int left, int top, int width, int height)
      : base(text, left, top, width, height)
    {

    }
    public ConnectionPoint(Control parent, string text)
      : base(parent, text)
    {

    }
    public ConnectionPoint(Control parent, string text, int left, int top, int width, int height)
      : base(parent, text, left, top, width, height)
    {

    }
    #endregion

    private PointF connectAt;
    public PointF ConnectAt
    {
      get { return connectAt; }
      set
      {
        connectAt = value;
        ConnectAtChanged();
      }
    }

    public static void SetConnections(ConnectionPoint endPoint1, ConnectionPoint endPoint2)
    {
      if(endPoint1.ConnectedTo != null && endPoint2.ConnectedTo != null) return;
      LineConnectionPoint lcp = new LineConnectionPoint();
      
      lcp.EndPoint1 = endPoint1;
      lcp.EndPoint2 = endPoint2;
      
      endPoint1.ConnectedTo = endPoint2;
      endPoint2.ConnectedTo = endPoint1;
    }

    private void ConnectAtChanged()
    {
      Location = new Point((int)connectAt.X, (int)connectAt.Y);
    }


    private Control focusControl;
    public Control FocusControl
    {
      get { return focusControl; }
      set
      {
        focusControl = value;
        FocusControlChanged();
      }
    }

    private void FocusControlChanged()
    {
      Action<object, EventArgs> ev = (sender, e) =>
      {
        if (sender != focusControl) return;
        int DX = focusControl.Location.X - focusControlLocation.X;
        int DY = focusControl.Location.Y - focusControlLocation.Y;

        this.Location = new Point(Left + DX, Top + DY);
        focusControlLocation = focusControl.Location;
      };

      if (focusControl == null) return;
      focusControl.LocationChanged += new EventHandler(ev);
      focusControl.SizeChanged += new EventHandler(ev);

      // default connectAt
      ConnectAt = new PointF(focusControl.Left + focusControl.Width, focusControl.Top + focusControl.Height);
      focusControlLocation = focusControl.Location;

    }

    private Point focusControlLocation = Point.Empty;

    private ConnectionPoint connectedTo;
    public ConnectionPoint ConnectedTo
    {
      get { return connectedTo; }
      set
      {
        connectedTo = value;
        ConnectedToChanged();
      }
    }

    private void ConnectedToChanged()
    {
      Action<object, EventArgs> ev = (sender, e) =>
      {
        if (sender != connectedTo) return;
        Invalidate();
      };

      if (focusControl == null) return;
      connectedTo.LocationChanged += new EventHandler(ev);
      connectedTo.SizeChanged += new EventHandler(ev);

    }


    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      e.Graphics.DrawRectangle(Pens.Black, new Rectangle(0, 0, Width - 1, Height - 1));
    }
  }
}
