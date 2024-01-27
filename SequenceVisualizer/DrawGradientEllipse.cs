using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LinqVisualizer
{
  public class DrawGradientEllipse : DrawEllipse
  {
    public DrawGradientEllipse(Control parent) : base(parent)
    {
      MyBrush = new LinearGradientBrush(new Rectangle(0, 0, 40, 40), Color.AliceBlue, Color.White, 30);
    }
  }
}
