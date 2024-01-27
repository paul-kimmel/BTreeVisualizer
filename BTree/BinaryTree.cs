using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTree
{
  public class BinaryTree<T>
  {
    private List<Node<T>> nodes = new List<Node<T>>();
    public List<Node<T>> Nodes
    {
      get { return nodes; }
    }

    private int depth = 0;
    public int Depth
    {
      get { return depth; }
      protected set
      { 
        if(value > depth)
          depth = value;
      }
    }
    
    public List<Node<T>> GetNodesAtLevel(int level)
    {
      return (from Node<T> node in nodes
                    where node.Level == level
                    select node).ToList<Node<T>>();
    }


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
      if(rootNode == null)
      {
        rootNode = node;
        Insert(rootNode, node, level);
        return;
      }

      level++;
      Node<T> current = rootNode;
      Node<T> parent = null;

      while(true)
      {
        
        // already exists
        if(current.Key.Equals(key)) return;

        parent = current;
        if(current.Key.CompareTo(key) > 0)
        {
          if(current.LeftNode == null)
          {
            InsertLeft(current, node, level);
            return;
          }
          current = current.LeftNode;
        }
        else
        {
          if(current.RightNode == null)
          {
            InsertRight(current, node, level);
            return;
          }
          current = current.RightNode;
        }

        level++;
      }
    }


    private void Insert(Node<T> parent, Node<T> nodeToInsert, int level)
    {
      nodeToInsert.Parent = parent;
      nodeToInsert.Level = level;
      nodes.Add(nodeToInsert);
      Depth = level;
    }

    private void InsertLeft(Node<T> parent, Node<T> nodeToInsert, int level)
    {
      parent.LeftNode = nodeToInsert;
      Insert(parent, nodeToInsert, level);
    }

    private void InsertRight(Node<T> parent, Node<T> nodeToInsert, int level)
    {
      parent.RightNode = nodeToInsert;
      Insert(parent, nodeToInsert, level);
    }

    public void Walk(Action<Node<T>> action)
    {
      Walk(rootNode, action);
    }


    public void Walk(Node<T> node, Action<Node<T>> action)
    {
      if (node == null) return;
      action(node);
      Walk(node.LeftNode, action);
      Walk(node.RightNode, action);
    }


  }
}
