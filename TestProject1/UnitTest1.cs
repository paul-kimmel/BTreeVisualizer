using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using BTree;
using BTreeVisualizer;
using LinqVisualizer;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using Microsoft.VisualBasic.PowerPacks;

namespace TestProject1
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class UnitTest1
  {
    public UnitTest1()
    {
      //
      // TODO: Add constructor logic here
      //
    }

    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    #region Additional test attributes
    //
    // You can use the following additional attributes as you write your tests:
    //
    // Use ClassInitialize to run code before running the first test in the class
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // Use ClassCleanup to run code after all tests in a class have run
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    // Use TestInitialize to run code before running each test 
    // [TestInitialize()]
    // public void MyTestInitialize() { }
    //
    // Use TestCleanup to run code after each test has run
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //
    #endregion

    [TestMethod]
    public void DrawLineBetweenTwoSequenceElementsTest()
    {
      using(Form form = new Form())
      {
        //var numbers = Enumerable.Range(1, 100);
        var names = new string[] { "Paul",  "Carolyn" };//, "Camie", "Noah", "Alex", "Leda", "Rocky", "Random", "Zero Mostel" };

        BinaryTree<string> tree = new BinaryTree<string>();

        foreach (var name in names)
          tree.Insert(new Node<string>(name, name), name);

        VisualBinaryTree<string> vbt = new VisualBinaryTree<string>(tree, form);

        form.MouseClick += delegate(object sender, MouseEventArgs e){
          form.Invalidate();
          form.Controls[0].Location = new Point(e.X, e.Y);
          form.Controls[0].SetBounds(e.X, e.Y, form.Controls[0].Width + 2, form.Controls[0].Height + 2);

        };
        form.TopMost = true;
        form.ShowDialog();
      }
    }

    [TestMethod]
    public void ConnectionPointTest()
    {
      using(Form form = new Form())
      {
        TextBox tb1 = new TextBox();
        tb1.Parent = form;
        tb1.Text = "TextBox 1";

        ConnectionPoint ConnectionPoint = new ConnectionPoint(form, "", 0, 0, 5, 5);
        ConnectionPoint.BringToFront();
        tb1.Location = new Point(50, 50);
        ConnectionPoint.FocusControl = tb1;
        tb1.SendToBack();

        TextBox tb2 = new TextBox();
        tb2.Parent = form;
        tb2.Text = "TextBox 2";
        tb2.Location = new Point(100, 100);

        
        ConnectionPoint ConnectionPoint2 = new ConnectionPoint(form, "", 10, 10, 5, 5);
        ConnectionPoint2.BringToFront();
        ConnectionPoint2.FocusControl = tb2;

        ConnectionPoint.SetConnections(ConnectionPoint, ConnectionPoint2);

        ShapeContainer container = new ShapeContainer();
        container.Parent = form;


        LineConnectionPoint lcp = new LineConnectionPoint(container);
        lcp.EndPoint1 = ConnectionPoint;
        lcp.EndPoint2 = ConnectionPoint2;
        
        form.MouseDown += delegate(object sender, MouseEventArgs e)
        {
          tb1.Location = new Point(e.X, e.Y);
        };

        Button b1 = new Button();
        b1.Text = "Push";
        b1.Parent = form;
        b1.Click += delegate{ 
          ConnectionPoint.ConnectAt = new Point(tb1.Left, tb1.Top+tb1.Height);
          };


        form.TopMost = true;
        form.ShowDialog();
      }

    }

  

  
  }
}
