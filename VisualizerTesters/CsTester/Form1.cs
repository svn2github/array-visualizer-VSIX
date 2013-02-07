// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Form1.cs" company="">
//   
// </copyright>
// <summary>
//   The form 1.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CsTester
{
  using System;
  using System.Diagnostics;
  using System.Windows.Forms;

  using LinqLib.Array;
  using LinqLib.Sequence;

  using SharpDX;

  /// <summary>
  /// The form 1.
  /// </summary>
  public partial class Form1 : Form
  {
    #region Fields

    /// <summary>
    /// The m arr 1.
    /// </summary>
    private int[,] mArr1;

    /// <summary>
    /// The m arr 2.
    /// </summary>
    private Array mArr2;

    /// <summary>
    /// The m arr unused 1.
    /// </summary>
    private long[,,] mArrUnused1;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Form1"/> class.
    /// </summary>
    public Form1()
    {
      this.InitializeComponent();
    }

    #endregion

    #region Methods

    /// <summary>
    /// The get new type.
    /// </summary>
    /// <param name="X">
    /// The x.
    /// </param>
    /// <returns>
    /// The <see cref="TestType"/>.
    /// </returns>
    private TestType GetNewType(int X)
    {
      return new TestType { a = X, b = X * 2 };
    }

    /// <summary>
    /// The button 1_ click.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    private void button1_Click(object sender, EventArgs e)
    {
      int[,,] arr1;
      Array arr2;
      long[,,] arrUnused1;
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

  /// <summary>
  /// The test type.
  /// </summary>
  public class TestType
  {
    #region Fields

    /// <summary>
    /// The a.
    /// </summary>
    public int a;

    /// <summary>
    /// The b.
    /// </summary>
    public int b;

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The to string.
    /// </summary>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public override string ToString()
    {
      return string.Format("{0} of {1}", this.a, this.b);
    }

    #endregion
  }

  /// <summary>
  /// The other.
  /// </summary>
  public class other
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the w.
    /// </summary>
    public Vector3 w { get; set; }

    #endregion
  }
}