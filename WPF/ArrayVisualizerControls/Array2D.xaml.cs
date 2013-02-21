using System;
using AvProp = ArrayVisualizerControls.Properties;

namespace ArrayVisualizerControls
{
  public partial class Array2D : ArrayControl
  {
    #region Methods

    protected override void DrawContent()
    {
      if (this.Data.Rank != 2)
      {
        throw new ArrayTypeMismatchException(AvProp.Resources.ArrayNot2DException);
      }


      string toolTipFmt = string.Format("{0}{{0}},{{1}}{1}", this.LeftBracket, this.RightBracket);
      for (int y = 0; y < base.DimY; y++)
        for (int x = 0; x < base.DimX; x++)
        {
          string toolTipCoords = string.Format(toolTipFmt, y, x);
          object data = this.Data.GetValue(y, x);
          double labelX = x * this.CellSize.Width;
          double labelY = y * this.CellSize.Height;

          AddLabel(ArrayRenderSection.Front, toolTipCoords, labelX, labelY, data);
        }
    }


    protected override void RenderBlankGrid()
    {
      if (this.Data.Rank != 2)
        throw new ArrayTypeMismatchException(AvProp.Resources.ArrayNot2DException);

      this.Width = this.CellSize.Width * base.DimX + 1;
      this.Height = this.CellSize.Height * base.DimY + 1;
      this.InvalidateVisual();

      for (double y = 0; y <= this.Height; y = y + this.CellSize.Height)
        this.AddLine(0, y, this.Width, y);

      for (double x = 0; x <= this.Width; x = x + this.CellSize.Width)
        this.AddLine(x, 0, x, this.Height);
    }

    #endregion
  }
}