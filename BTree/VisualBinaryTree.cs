using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using LinqVisualizer;
using System.Diagnostics;
using Microsoft.VisualBasic.PowerPacks;

namespace BTree
{
  public class VisualBinaryTree<T>
  {
    private BinaryTree<T> tree = null;
    private Control parent = null;
    private ShapeContainer shapeContainer = null;


    public VisualBinaryTree(BinaryTree<T> tree, Control parent )
    {
      if(tree == null)
        throw new ArgumentNullException("tree");
      if(parent == null)
        throw new ArgumentNullException("parent");

      this.tree = tree;
      this.parent = parent;
      shapeContainer = new ShapeContainer();
      shapeContainer.Parent = parent;
      LineConnectionPoint.SetContainer(shapeContainer);

      parent.Resize += new EventHandler(parent_Resize);
      parent.Paint += new PaintEventHandler(parent_Paint);
      CreateSequenceElements();
    }

    private void CreateSequenceElements()
    {
      SizeF size = FindBiggest();
      float height = size.Height;
      float width = size.Width;
      VisualNodeElement<T>.MakeSame(ref width, ref height);
      
      
      int top = 0;
      foreach(var node in tree.Nodes)
      {
        VisualNodeElement<Node<T>> elem = new VisualNodeElement<Node<T>>(parent, node.Key);
        elem.OptimallySizeContainer = false;
        elem.SetBounds(0, top, (int)width, (int) height);
        elem.Data = node;
      }
      LayoutAll();
    }
    
 

    /// <summary>
    /// parent was resized so we need to layout all of the sequence elements
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void parent_Resize(object sender, EventArgs e)
    {
      if(tree == null) return;
      parent.Invalidate();
      LayoutAll();
    }

    void parent_Paint(object sender, PaintEventArgs e)
    {
      if(tree == null) return;
      shapeContainer.Invalidate();
    }

    private VisualNodeElement<Node<T>> GetElem(Node<T> node)
    {
      return (VisualNodeElement<Node<T>>)(from Control control in parent.Controls
                                          where control is VisualNodeElement<Node<T>>
                                          && (control as VisualNodeElement<Node<T>>).Data == node
                                          select control).First();
    }


    /// <summary>
    /// This will walk the tree in order and draw the nodes. This is the initial layout only
    /// </summary>
    public void LayoutAll()
    {
      Action<Node<T>> layout = (node)=>
        {
          // get my control - should exist or we aren't here
          VisualNodeElement<Node<T>> elem = GetElem(node);
          LayoutElem(elem);
        };

      tree.Walk(layout);
    }

    private void LayoutElem(VisualNodeElement<Node<T>> elem)
    {
      if(elem.Data == elem.Data.Parent)
        LayoutRoot(elem);
      else
        Layout(elem, elem.Data.Parent.LeftNode == elem.Data);
    }


    private VisualNodeElement<Node<T>> GetParentElem(Node<T> parentNode)
    {
      return (VisualNodeElement<Node<T>>)(from Control control in parent.Controls
             where control is VisualNodeElement<Node<T>>
             && (control as VisualNodeElement<Node<T>>).Data == parentNode
             select control).First();
    }

    /// <summary>
    /// Layout the element to the left or right of the parent
    /// </summary>
    /// <param name="elem"></param>
    private void Layout(VisualNodeElement<Node<T>> elem, bool left)
    {
      Node<T> parentNode = elem.Data.Parent;
      VisualNodeElement<Node<T>> parentElem = GetParentElem(parentNode);
      Rectangle parentRect = parentElem.Bounds;

      int x = 0;
      if(left)
        x = parentRect.X - parentRect.Width;
      else
        x = parentRect.X + parentRect.Width;

     elem.SetBounds(x, parentRect.Y + parentRect.Height, elem.Width, elem.Height);

      if(left)
      {
        ConnectionPoint.SetConnections(elem.ParentConnection, parentElem.LeftConnection);
      }
      else
      {
        ConnectionPoint.SetConnections(elem.ParentConnection, parentElem.RightConnection);
      }
    }

    private static readonly Rectangle starter = new Rectangle(0,0,5,5);
    
     private void LayoutRoot(VisualNodeElement<Node<T>> root)
    {
      int top = 10;
      int left = (parent.Bounds.Width - root.Bounds.Width) / 2;
      root.SetBounds(left, top, root.Width, root.Height);
    }


    public SizeF FindBiggest()
    {
      SizeF biggest = new SizeF();
      foreach(var elem in tree.Nodes)
      {
        SizeF size = GetOptimalSize(elem.ToString());
        if(size.Width * size.Height > biggest.Width * biggest.Height)
          biggest = size;
      }
    return biggest;
    }

    public SizeF GetOptimalSize(string text)
    {
      Graphics g = parent.CreateGraphics();
      return g.MeasureString(text, parent.Font);
    }
  }
}
