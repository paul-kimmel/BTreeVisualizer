using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace LinqVisualizer
{
  public class DrawEllipseWithEffects : DrawGradientEllipse
  {

    protected static Brush shadowBrush = new SolidBrush(Color.DarkGray);
    public DrawEllipseWithEffects(Control parent) : base(parent)
    {
      parent.MouseDown += new MouseEventHandler(parent_MouseDown);
      parent.MouseUp += new MouseEventHandler(parent_MouseUp);
    }

    private bool down = false;

    void parent_MouseUp(object sender, MouseEventArgs e)
    {
      down = false;
      (sender as Control).Invalidate();
    }

    void parent_MouseDown(object sender, MouseEventArgs e)
    {
      down = true;
      (sender as Control).Invalidate();
    }

    private Color GetColor(bool buttonState)
    {
      Color[] colors = {Color.Silver, Color.Gray};
      return colors[Convert.ToInt32(buttonState)];
    }

    private Brush falseBrush = null;
    private Brush trueBrush = null;

    private Brush GetBrush(bool buttonState)
    {
      if(falseBrush == null)
        falseBrush = new LinearGradientBrush(new Point(2,2), new Point(50, 50), Color.White, GetColor(false));

      if (trueBrush == null)
        trueBrush = new LinearGradientBrush(new Point(2, 2), new Point(50, 50), Color.White, GetColor(true));
      
      return buttonState ? trueBrush : falseBrush;
    }


    private int GetPenWidth(bool buttonState)
    {
      return 1 + Convert.ToInt32(buttonState);
    }
    
    private Pen penFalse = null;
    private Pen penTrue = null;
    private Pen GetPen()
    {
      if(penFalse == null)
        penFalse = new Pen(Brushes.Black, GetPenWidth(false));

      if(penTrue == null)
        penTrue = new Pen(Brushes.Black, GetPenWidth(true));

      return down == false ? penFalse : penTrue;
    }

 
    private bool shadowed = true;
    public bool Shadowed
    {
      get { return shadowed; }
      set { shadowed = value; }
    }


    private int shadowDepth = 2;
    public int ShadowDepth
    {
      get { return shadowDepth; }
      set { shadowDepth = value; }
    }

    private Color shadowColor = Color.DarkGray;
    public Color ShadowColor
    {
      get { return shadowColor; }
      set 
      { 
        shadowColor = value; 
        shadowBrush = new SolidBrush(shadowColor);
      }
    }

    private bool clipOnce = true;
    private void ClipRegionOneTime(int x, int y, int width, int height)
    {
      if (!clipOnce) return;
      clipOnce = false;
      DrawContainerHelper.ClipRegionOneTime(control, x, y, width, height);
    }

    private void DrawButtonOutline(Graphics graphics, int width, int height)
    {
      graphics.DrawEllipse(GetPen(), 1, 1, width - shadowDepth,
        height - shadowDepth);
    }

    private void DrawButton(Graphics graphics, int width, int height)
    {
      graphics.FillEllipse(shadowBrush, 0 + shadowDepth, 0 + shadowDepth, width, height);
      graphics.FillEllipse(GetBrush(down), 0, 0, width, height);
      DrawButtonOutline(graphics, width, height);
    }
    
    private void DrawGraphic(Graphics graphics)
    {
      if(down) DrawDownGraphic(graphics);
    }

    private void DrawDownGraphic(Graphics graphics)
    {
      Matrix m = new Matrix();
      m.Scale(1.03f, 1.03f);
      graphics.Transform = m;
    }


    public override void DrawContainer(Graphics graphics, int x, int y, int width, int height)
    {

#if DEBUG
      DrawContainerHelper.DrawDebugOutline(graphics, width, height);
#endif

      ClipRegionOneTime(x, y, width, height);

      width -= 1;
      height -= 1;
      DrawButton(graphics, width, height);
      DrawGraphic(graphics);
    }
  }
}
