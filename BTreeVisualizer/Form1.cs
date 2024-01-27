using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LinqVisualizer;
using BTree;


namespace BTreeVisualizer
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    VisualBinaryTree<string> vbt = null;
    private void Form1_Load(object sender, EventArgs e)
    {

      //var numbers = Enumerable.Range(1, 100);
      var names = new string[]{"Paul", "Carolyn", "Camie", "Noah", "Alex", "Leda", "Rocky", "Random", "Zero Mostel", "Leda", "Pollux", "Fred Flintstone"};

      BinaryTree<string> tree = new BinaryTree<string>();

      foreach(var name in names)
        tree.Insert(new Node<string>(name, name), name);

      vbt = new VisualBinaryTree<string>(tree, this);
      
    }

    private void Form1_Resize(object sender, EventArgs e)
    {
    }
  }
}
