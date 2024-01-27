using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace LinqVisualizer
{
  public class VisualSequences<T> 
  {
    private static readonly int space = 10;
    private List<VisualSequence<T>> sequences;

    public void Add(Control parent, IEnumerable<T> sequence)
    {
      sequences.Add(VisualSequence<T>.Create(parent, sequence));
      LayoutAll();
    }
    
    public void Add(VisualSequence<T> item)
    {
      sequences.Add(item);
      LayoutAll();
    }

    public void LayoutAll()
    {
      int top = 10;
      SizeF size = FindBiggest();
      foreach (var sequence in sequences)
      {
        sequence.SequenceLeft = 10;
        sequence.SequenceTop = top;
        top += (int)size.Height + 20;
        sequence.LayoutAll(size);
      }
    }

    /// <summary>
    /// Initializes a new instance of the VisualSequences class.
    /// </summary>
    public VisualSequences()
    {
      sequences = new List<VisualSequence<T>>();
    }

 
    private SizeF FindBiggest()
    {
      SizeF biggest = new SizeF(0,0);
      foreach(var sequence in sequences)
      {
        SizeF size = sequence.FindBiggest();
        if (size.Width * size.Height > biggest.Width * biggest.Height)
          biggest = size;
      }
      return biggest; 
    }

    private int FindLongest()
    {
      int longest = 0;
      foreach (var sequence in sequences)
        if (sequence.Length > longest)
          longest = sequence.Length;
      return longest;
    }

    public void DrawToClipboard()
    {
      SizeF biggest = FindBiggest();
      int longest = FindLongest();
      Bitmap bmp = null;
      int y = 5;

      // make dimension symmetric
      float width = biggest.Width;
      float height = biggest.Height;
      SequenceElement<T>.MakeSame(ref width, ref height);

      bmp = new Bitmap(
        (int)(width + space) * longest + 10,
        (int)(height + 20) * sequences.Count + 20);

      Graphics graphics = Graphics.FromImage(bmp);
      graphics.FillRectangle(Brushes.White,
        new Rectangle(0, 0, bmp.Width, bmp.Height));

      foreach (var sequence in sequences)
      {
        int x = 5; 
        foreach (var elem in sequence.SequenceElements)
        {
          elem.DrawToBitmap(bmp, new Rectangle(x, y, (int)width, (int)height));
          x += (int)width + space;
        }
        y += (int)height + 20;
      }

      Clipboard.SetImage(bmp);
      MessageBox.Show("Sequences copied to clipboard");

    }
  }
}
