// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Array2D.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   The array 2 d.
// </summary>
// --------------------------------------------------------------------------------------------------------------------



using AvProp = ArrayVisualizerControls.Properties;

namespace ArrayVisualizerControls
{
  using System;

  /// <summary>
  /// The array 2 d.
  /// </summary>
  public partial class Array2D : ArrayControl
  {
    #region Methods

    /// <summary>
    /// The draw content.
    /// </summary>
    /// <exception cref="ArrayTypeMismatchException">
    /// </exception>
    protected override void DrawContent()
    {
      if (this.Data.Rank != 2)
      {
        throw new ArrayTypeMismatchException(AvProp.Resources.ArrayNot2DException);
      }

      string toolTipFmt = "[{0},{1}]";
      for (int y = 0; y < base.DimY; y++)
      {
        for (int x = 0; x < base.DimX; x++)
        {
          string toolTipCoords = string.Format(toolTipFmt, y, x);
          object data = this.Data.GetValue(y, x);
          double labelX = x * this.CellSize.Width;
          double labelY = y * this.CellSize.Height;

          if (data.GetType().IsArray)
          {
            this.AddLabel(ArrayRenderSection.Front, toolTipCoords, labelX, labelY, (Array)data);
          }
          else
          {
            string text = this.GetText(data);
            AddLabel(ArrayRenderSection.Front, toolTipCoords, labelX, labelY, text);
          }
        }
      }
    }

    /// <summary>
    /// The render blank grid.
    /// </summary>
    /// <exception cref="ArrayTypeMismatchException">
    /// </exception>
    protected override void RenderBlankGrid()
    {
      if (this.Data.Rank != 2)
      {
        throw new ArrayTypeMismatchException(AvProp.Resources.ArrayNot2DException);
      }

      this.Width = this.CellSize.Width * base.DimX + 1;
      this.Height = this.CellSize.Height * base.DimY + 1;
      this.InvalidateVisual();

      for (double y = 0; y <= this.Height; y = y + this.CellSize.Height)
      {
        this.AddLine(0, y, this.Width, y);
      }

      for (double x = 0; x <= this.Width; x = x + this.CellSize.Width)
      {
        this.AddLine(x, 0, x, this.Height);
      }
    }

    #endregion
  }
}