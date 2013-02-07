using System;
using System.Windows.Forms;
using LinqLib.Array;
using LinqLib.Sequence;

namespace CsTester
{
  using SharpDX;

  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }


    int[,] mArr1;
    System.Array mArr2;
    long[, ,] mArrUnused1;

    private void button1_Click(object sender, EventArgs e)
    {
      int[, ,] arr1;
      System.Array arr2;
      long[, ,] arrUnused1;
      TestType[,] arr3;
      System.Array arr4;

      mArr1 = Enumerator.Generate<int>(1, 1, 20).Shuffle().ToArray(4, 5);
      mArr2 = Enumerator.Generate<long>(1, 1, 360).Shuffle().ToArray(3, 3, 4, 5);

      arr1 = Enumerator.Generate<int>(1, 1, 100).Shuffle().ToArray(5, 5, 4);
      arr2 = Enumerator.Generate<int>(1, 1, 49).Shuffle().ToArray(7, 7);
      arr3 = Enumerator.Generate<TestType>(15, (X) => GetNewType(X)).Shuffle().ToArray(5, 3);
      arr4 = Enumerator.Generate<TestType>(27, (X) => GetNewType(X)).Shuffle().ToArray(9, 3);

      var v1 = new Vector2(1, 2);
      var v2 = new Vector3(1,2,3);
      var v3 = new Vector4(1, 2, 3,4);
      var m1 = Matrix.Identity;
      var m2 = Matrix3x2.Identity;
      var m3 = Matrix5x4.Identity;

      var o = new other { w = new Vector3(1, 2, 3) };

      System.Diagnostics.Debugger.Break();
    }

    private TestType GetNewType(int X)
    {
      return new TestType() { a = X, b = X * 2 };
    }
  }

  public class TestType
  {
    public int a;
    public int b;

    public override string ToString()
    {
      return string.Format("{0} of {1}", a, b);
    }
  }

  public class other
  {
    public Vector3 w { get; set; }
  }
}

