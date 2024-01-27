using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LinqVisualizer
{
  public class DrawEllipse : IDrawContainer
  {

    protected Control control = null;
    protected static Pen pen = new Pen(Brushes.Black, 1.6f);
    public DrawEllipse(Control parent)
    {
      if (parent == null)
        throw new ArgumentNullException("parent");

      control = parent;
    }

    private bool clipOnce = true;
    private void ClipRegionOneTime(int x, int y, int width, int height)
    {
      if (!clipOnce) return;
      clipOnce = false;
      DrawContainerHelper.ClipRegionOneTime(control, x, y, width, height);
    }

    #region IDrawContainer Members

    public virtual void DrawContainer(Graphics graphics, int x, int y, int width, int height)
    {

#if DEBUG
      DrawContainerHelper.DrawDebugOutline(graphics, width, height);
#endif

      ClipRegionOneTime(x, y, width, height);

      width -= 1;
      height -= 1;
      graphics.FillEllipse(Brushes.DarkGray, x + 2, y + 2, width, height);
      graphics.FillEllipse(myBrush, x, y, width - 2, height - 2);
      graphics.DrawEllipse(pen, x, y, width - 2, height - 2);
    }

    public void Reset()
    {
      clipOnce = true;
    }

    protected Brush myBrush = new SolidBrush(Color.Cornsilk);
    public Brush MyBrush
    {
      set { myBrush = value; }
    }

    #endregion
  }
}
