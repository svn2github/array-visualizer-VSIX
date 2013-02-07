// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CsTester
{
  using System;
  using System.Windows.Forms;

  /// <summary>
  /// The program.
  /// </summary>
  internal static class Program
  {
    #region Methods

    /// <summary>
    ///   The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new Form1());
    }

    #endregion
  }
}