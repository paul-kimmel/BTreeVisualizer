using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace LinqVisualizer
{
  public class VisualSequence<T> 
  {
    private static readonly int space = 10;
    private List<SequenceElement<T>> sequenceElements;

    /// <summary>
    /// starting top position of this sequenceTop
    /// </summary>
    private int sequenceTop = 10;
    public int SequenceTop
    {
      get { return sequenceTop; }
      set { sequenceTop = value; }
    }

    /// <summary>
    /// starting left position of this sequenceTop
    /// </summary>
    private int sequenceLeft = 10;
    public int SequenceLeft
    {
      get { return sequenceLeft; }
      set { sequenceLeft = value; }
    }

    public int GetSuggestedYOffset()
    {
      return (int)FindBiggest().Height;
    }

    public int Length
    {
      get { return sequenceElements.Count; }
    }

    public List<SequenceElement<T>> SequenceElements
    {
      get { return sequenceElements; }
    }

    public static VisualSequence<T> Create(Control parent, 
      IEnumerable<T> sequence, int top, int left)
    {
      VisualSequence<T> o = new VisualSequence<T>();
      o.sequenceElements = new List<SequenceElement<T>>();
      o.sequenceTop = top;
      o.sequenceLeft = left;

      for (int i = 0; i < sequence.Count(); i++)
      {
        SequenceElement<T> se = new SequenceElement<T>();
        se.Parent = parent;
        se.OptimallySizeContainer = false;
        se.Data = sequence.ElementAt(i);
        o.sequenceElements.Add(se);
        parent.Controls.Add(se);
        o.LayoutAll();
      }
      return o;
    }
     
    public static VisualSequence<T> Create(Control parent,
      IEnumerable<T> sequence)
    {
      return Create(parent, sequence, 10, 10);
    }
    
    public void LayoutAll(SizeF size)
    {
      // make dimension symmetric
      float width = size.Width;
      float height = size.Height;
      SequenceElement<T>.MakeSame(ref width, ref height);

      // size and position everything in the sequence
      int i = sequenceLeft;
      foreach (var elem in sequenceElements)
      {
        elem.SetBounds(i, sequenceTop,
          (int)width, (int)height, BoundsSpecified.All);

        i += (int)size.Width + space;
      }
    }

    public void LayoutAll()
    {
      // find the largest element needed
      LayoutAll(FindBiggest());
    }
     
    public SizeF FindBiggest()
    {
      SizeF biggest = new SizeF();
      foreach(var elem in sequenceElements)
      {
        SizeF size = elem.GetOptimalSize();
        if (size.Width * size.Height
          > biggest.Width * biggest.Height)
          biggest = size;
      }

      return biggest;
    }

    protected VisualSequence()
    {
    
    }

    public void DrawToClipboard()
    {
      SizeF biggest = FindBiggest();
      
      // make dimension symmetric
      float width = biggest.Width;
      float height = biggest.Height;
      SequenceElement<T>.MakeSame(ref width, ref height);

      Bitmap bmp = new Bitmap(
        (int)(width + space) * sequenceElements.Count + 10, 
        (int)height + 10);

      Graphics graphics = Graphics.FromImage(bmp);
      graphics.FillRectangle(Brushes.White, 
        new Rectangle(0, 0, bmp.Width, bmp.Height));

      int i = space;
      foreach(var elem in sequenceElements)
      {
        elem.DrawToBitmap(bmp, new Rectangle(i, 5, (int)width, (int)height));
        i += (int)width + space;
      }
      Clipboard.SetImage(bmp);
      MessageBox.Show("Sequence copied to clipboard");
      Process.Start("clipbrd.exe");
    }
  }
}
