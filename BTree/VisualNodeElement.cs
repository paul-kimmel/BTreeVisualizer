using System;
using LinqVisualizer;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;


namespace BTree
{
  public class VisualNodeElement<T> : SequenceElement<T>
  {

    #region Constructors
      public VisualNodeElement(Control parent, string text)
      : base(parent, text)
      {


      }
      public VisualNodeElement()
      : base()
      {
      }

      public VisualNodeElement(string text)
     		: base(text)
     		{
     				
     		}
      public VisualNodeElement(Control parent, string text, Rectangle rect)
     		: base(parent, text, rect)
     		{
     				
     		}
      public VisualNodeElement(string text, int left, int top, int width, int height)
     		: base(text, left, top, width, height)
     		{
     				
     		}
      public VisualNodeElement(Control parent, string text, int left, int top, int width, int height)
     		  : base(parent, text, left, top, width, height)
     		  {
     				
     		  }
      #endregion

      public static double GetAngle(VisualNodeElement<T> source, VisualNodeElement<T> target)
      {
        return Geometry.GetAngle(source.Location, target.Location);
      }

      
      private bool isMoving;
      public bool IsMoving
      {
        get { return isMoving; }
        set { isMoving = value; }
      }

      protected override void OnMouseDown(MouseEventArgs e)
      {
        base.OnMouseDown(e);
        if(e.Button == MouseButtons.Left)
        isMoving = true;
      }

      protected override void OnMouseUp(MouseEventArgs e)
      {
        base.OnMouseUp(e);
        if(e.Button == MouseButtons.Left)
        isMoving = false;
      }


      protected override void OnMouseMove(MouseEventArgs e)
      {
      
        base.OnMouseMove(e);
        if(isMoving)
        {
          this.Location = new Point(Location.X - (int)Center.X + e.X, Location.Y - (int)Center.Y + e.Y);
        }
      }

      protected override void OnDoubleClick(EventArgs e)
      {
        base.OnDoubleClick(e);
        if(Data != null)
          MessageBox.Show(this.Data.ToString(), "Contents", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }


      public PointF Center
      {
        get{ return Geometry.GetCenter(Bounds); }
      }


      /// <summary>
      /// Connection to my parent
      /// </summary>
      private ConnectionPoint parentConnection = null;
      public ConnectionPoint ParentConnection
      {
        get 
        {
          if(parentConnection == null)
          {
            parentConnection = new ConnectionPoint(this.Parent, "", 0, 0, 5, 5);
            parentConnection.FocusControl = this;
            parentConnection.ConnectAt = new PointF(Left + Center.X, Top - 3);
          }
           
          return parentConnection; 
        }
        set { parentConnection = value; }
      }

      private ConnectionPoint leftConnection = null;
      public ConnectionPoint LeftConnection
      {
        get 
        { 
          if(leftConnection == null)
          {
            leftConnection = new ConnectionPoint(this.Parent, "", 0, 0, 5, 5);
            leftConnection.FocusControl = this;
            PointF edge = Geometry.PointOnCircle(Bounds, -30, -1, -1);
            leftConnection.ConnectAt = new PointF(Left + edge.X, Top + edge.Y);
          }
          return leftConnection; 
        }
        set { leftConnection = value; }
      }


      private ConnectionPoint rightConnection = null;
      public ConnectionPoint RightConnection
      {
        get 
        { 
          if(rightConnection == null)
          {
            rightConnection = new ConnectionPoint(this.Parent, "", 0, 0, 5, 5);
            rightConnection.FocusControl = this;
            PointF edge = Geometry.PointOnCircle(Bounds, 30, 1, 1);
            rightConnection.ConnectAt = new PointF(Left + edge.X - 2, Top + edge.Y);
          }

          return rightConnection; 
        }
        set { rightConnection = value; }
      }
    }
}
