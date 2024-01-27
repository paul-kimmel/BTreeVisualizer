using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LinqVisualizer
{
  public class DrawRectangle : IDrawContainer
  {
    private Control control = null;
    public DrawRectangle(Control parent)
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

    public void DrawContainer(Graphics graphics, int x, int y, int width, int height)
    {
#if DEBUG
      DrawContainerHelper.DrawDebugOutline(graphics, width, height);
#endif

      ClipRegionOneTime(x, y, width, height);

      width -= 1;
      height -= 1;
      graphics.FillRectangle(myBrush, x, y, width, height);
      graphics.DrawRectangle(Pens.Black, x, y, width, height);

    }

    private Brush myBrush = new SolidBrush(Color.Cornsilk);
    public Brush MyBrush
    {
      set { myBrush = value; }
    }

    public void Reset()
    {
      clipOnce = true;
    }


    #endregion
  }
}
