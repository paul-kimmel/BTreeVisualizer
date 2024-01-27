using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTree
{
  public class BinaryTree<T>
  {
    
    public BinaryTree<T>()
    {
      nodes = new HashSet<T>();
    }

    private HashSet<T> nodes = null;

    private Node<T> rootNode;
    public Node<T> RootNode
    {
      get { return rootNode; }
      set { rootNode = value; }
    }

    public Node<T> Find(Node<T> node, string key)
    {
      if(node == null) return null;

      if(node.Key.Equals(key)) return node;

      if(key.CompareTo(node.Key) < 0)
        return Find(node.LeftNode, key);
      else
        return Find(node.RightNode, key);
    }

    /// <summary>
    /// Insert the node in the tree and keep track of the levels
    /// </summary>
    /// <param name="node"></param>
    /// <param name="key"></param>
    public void Insert(Node<T> node, string key)
    {
      int level = 0;
      Node<T> current = rootNode;
      Node<T> parent = rootNode;

      while(true)
      {
        if(current == null)
        {
          current = node;
          node.Parent = parent;
          node.Level = level;
          return;
        }

        // already exists
        if(current.Key.Equals(key)) return;

        if(current.Key.CompareTo(key) < 0)
        {
          parent = current;
          current = current.LeftNode;
        }
        else
        {
          parent = current;
          current = current.RightNode;
        }

        level++;
      }
    }



    public void Walk(Node<T> node, Action<T> action)
    {


    }


  }


  public class Node<T>
  {
    private Node<T> leftNode;
    public Node<T> LeftNode
    {
      get { return leftNode; }
      set { leftNode = value; }
    }

    private Node<T> rightNode;
    public Node<T> RightNode
    {
      get { return rightNode; }
      set { rightNode = value; }
    }


    private Node<T> parent;
    public Node<T> Parent
    {
      get { return parent; }
      set { parent = value; }
    }

    private string key;
    public string Key
    {
      get { return key; }
      set
      {
        key = value;
      }
    }

    private int level;
    public int Level
    {
      get { return level; }
      set
      {
        level = value;
      }
    }
    

    private T nodeValue;
    public T NodeValue
    {
      get { return nodeValue; }
      set { nodeValue = value; }
    }
  }
}
