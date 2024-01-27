using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTree
{
  public class Node<T>
  {

    public Node(T value, string key)
    {
      NodeValue = value;
      this.Key = key;
    }

    public Node<T> LeftNode { get; set; }
    public Node<T> RightNode { get; set; }
    public Node<T> Parent { get; set; }
    public string Key { get; set; }
    public int Level { get; set; }
    public T NodeValue { get; set; }

    public override string ToString()
    {
      return string.Format("{0}/{1}", Key, Level);
    }

  }
}
