using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LinqVisualizer
{
  public class SequenceElement<T> : Control
  {

    private IDrawContainer drawState = null;

    private VisualStyle style = VisualStyle.circle;
    public VisualStyle Style
    {
      get { return style; }
      set { 
        style = value;
        VisualStyleChanged();
      }
    }

    private void VisualStyleChanged()
    {
      if(style == VisualStyle.circle)
        drawState = new DrawEllipse(this);
      else
        drawState = new DrawRectangle(this);

      Invalidate();
    }

    private T data;
    public T Data
    {
      get { return data; }
      set
      {
        data = value;
        this.Text = value != null ? value.ToString() :
          "";
      }
    }

    private bool optimallySizeContainer = true;
    public bool OptimallySizeContainer
    {
      get { return optimallySizeContainer; }
      set
      {
        optimallySizeContainer = value;
        ChangeSize();
      }
    }

    private void ChangeSize()
    {
      if (!optimallySizeContainer) return;
      if (Text == "") return;

      float width = 0f, height = 0f;
      GetWidthHeight(ref width, ref height);

      int left = Left;
      int top = Top;
      MakeSame(ref left, ref top);

      // leave left and top unchanged
      SetBounds(left, top, (int)width + 2,
        (int)height + 2, BoundsSpecified.All);
    }

    

    #region do nothing constructors
    public SequenceElement()
      : base()
    {
      Initialize();
    }

    private void Initialize()
    {
      drawState = new DrawEllipseWithEffects(this);

      SetStyle(ControlStyles.ResizeRedraw
        | ControlStyles.OptimizedDoubleBuffer
        | ControlStyles.AllPaintingInWmPaint 
        | ControlStyles.SupportsTransparentBackColor
        | ControlStyles.UserPaint, true);

      BackColor = Color.Transparent;
    }

    public SequenceElement(string text)
      : base(text)
    {
      Initialize();
    }

    public SequenceElement(Control parent, string text)
      : base(parent, text)
    {
      Initialize();
    }

    public SequenceElement(Control parent, string text, Rectangle rect)
      : base(parent, text, rect.X, rect.Y, rect.Width, rect.Height)
    {
      Initialize();
    }

    public SequenceElement(string text, int left, int top, int width, int height)
      : base(text, left, top, width, height)
    {
      Initialize();
    }

    public SequenceElement(Control parent, string text, int left, int top, int width, int height)
      : base(parent, text, left, top, width, height)
    {
      Initialize();
    }
    #endregion

    protected override void OnPaint(PaintEventArgs e)
    {
      e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
      DrawContainer(e.Graphics);
      if (data == null) return;
      DrawElement(e.Graphics);
      base.OnPaint(e);
      
    }

    public void Draw(Graphics graphics, int x, int y, int width, int height)
    {
      graphics.SmoothingMode = SmoothingMode.AntiAlias;
      DrawContainer(graphics, x,y,width,height);
      DrawElement(graphics, new RectangleF(
        (float)x, (float)y, (float)width, (float)height));
    }

    private void DrawElement(Graphics graphics, RectangleF rect)
    {
      if (Text == "") return;
      graphics.DrawString(data.ToString(), Font, //this.Font,
        Brushes.Black, rect);
    }

    public static void MakeSame(ref float x, ref float y)
    {
      if (x < y) x = y;
      else y = x;
    }

    public static void MakeSame(ref int x, ref int y)
    {
      if (x < y) x = y;
      else y = x;
    }

    public SizeF GetOptimalSize()
    {
      Graphics g = this.CreateGraphics();
      return g.MeasureString(Text, Font);
    }

    public SizeF OptimalArea
    {
      get{
        return GetOptimalSize();
      }
    }

    
    private void GetWidthHeight(ref float width, ref float height)
    {
      SizeF size = GetOptimalSize();
      width = size.Width;
      height = size.Height;
      MakeSame(ref width, ref height);
    }

    private RectangleF GetMiddle()
    {
      Graphics g = this.CreateGraphics();
      SizeF size = g.MeasureString(Text, Font);
      float midX = (Bounds.Width - size.Width) / 2;
      float midY = (Bounds.Height - size.Height) / 2;
      return new RectangleF(midX, midY, size.Width, size.Height);
    }

    private Color fillColor = Color.Cornsilk;
    public Color FillColor
    {
      get { return fillColor; }
      set 
      { 
        fillColor = value; 
        FillColorChanged();
      }
    }

    private void FillColorChanged()
    {
      brush = new SolidBrush(fillColor);
      if(drawState != null)
        drawState.MyBrush = brush;
    }

    protected Brush brush = new SolidBrush(Color.Cornsilk);
    protected Brush MyBrush
    {
      get
      {
        return brush;
      }
    }


    protected override void OnSizeChanged(EventArgs e)
    {
      base.OnSizeChanged(e);
      if(drawState != null)
        drawState.Reset();
    }
 
    protected virtual void DrawContainer(Graphics graphics, int x, int y,
      int width, int height)
    {
      drawState.DrawContainer(graphics, x, y, width, height);
    }

    private void DrawElement(Graphics graphics)
    {
      if (Text == "") return;
      DrawElement(graphics, GetMiddle());
    }

    protected virtual void DrawContainer(Graphics graphics)
    {
      Rectangle rect = GetContainerRect();
      DrawContainer(graphics, rect.X, rect.Y, rect.Width, rect.Height);
    }

    private Rectangle GetContainerRect()
    {
      return new Rectangle(0, 0, Width, Height);
    }

    private Rectangle GetShadowRect()
    {
      return new Rectangle(0, 0, Width - 1, Height - 1);
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.ResumeLayout(false);

    }
    
  }
}