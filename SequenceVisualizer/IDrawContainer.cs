using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LinqVisualizer
{
  public interface IDrawContainer
  {
    void DrawContainer(Graphics graphics, int x, int y,
      int width, int height);
    Brush MyBrush { set; }
    void Reset();
  }
}
