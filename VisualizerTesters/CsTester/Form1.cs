using System;
using System.Diagnostics;
using System.Windows.Forms;

using LinqLib.Array;
using LinqLib.Sequence;

using SharpDX;

namespace CsTester
{
  public partial class Form1 : Form
  {
    #region Fields

    private int[,] mArr1;
    private Array mArr2;
    private long[, ,] mArrUnused1;

    #endregion

    #region Constructors and Destructors

    public Form1()
    {
      this.InitializeComponent();
    }

    #endregion

    #region Methods

    private TestType GetNewType(int X)
    {
      return new TestType { a = X, b = X * 2 };
    }

    private void button1_Click(object sender, EventArgs e)
    {
      int[, ,] arr1;
      Array arr2;
      long[, ,] arrUnused1;
      TestType[,] arr3;
      Array arr4;

      this.mArr1 = Enumerator.Generate(1, 1, 20).Shuffle().ToArray(4, 5);
      this.mArr2 = Enumerator.Generate<long>(1, 1, 360).Shuffle().ToArray(3, 3, 4, 5);

      arr1 = Enumerator.Generate(1, 1, 100).Shuffle().ToArray(5, 5, 4);
      arr2 = Enumerator.Generate(1, 1, 49).Shuffle().ToArray(7, 7);
      arr3 = Enumerator.Generate(15, (X) => this.GetNewType(X)).Shuffle().ToArray(5, 3);
      arr4 = Enumerator.Generate(27, (X) => this.GetNewType(X)).Shuffle().ToArray(9, 3);

      var v1 = new Vector2(1, 2);
      var v2 = new Vector3(1, 2, 3);
      var v3 = new Vector4(1, 2, 3, 4);
      Matrix m1 = Matrix.Identity;
      Matrix3x2 m2 = Matrix3x2.Identity;
      Matrix5x4 m3 = Matrix5x4.Identity;

      var o = new other { w = new Vector3(1, 2, 3) };

      Debugger.Break();
    }

    #endregion
  }

  public class TestType
  {
    #region Fields

    public int a;
    public int b;

    #endregion

    #region Public Methods and Operators

    public override string ToString()
    {
      return string.Format("{0} of {1}", this.a, this.b);
    }

    #endregion
  }

  public class other
  {
    #region Public Properties

    public Vector3 w { get; set; }

    #endregion
  }
}