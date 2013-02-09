using System;
using AvProp = ArrayVisualizerControls.Properties;

namespace ArrayVisualizerControls
{
  public partial class Array1D : ArrayControl
  {
    #region Methods

    protected override void DrawContent()
    {
      if (this.Data.Rank != 1)
        throw new ArrayTypeMismatchException(AvProp.Resources.ArrayNot1DException);

      string toolTipFmt = "[{0}]";
      for (int x = 0; x < base.DimX; x++)
      {
        object data = this.Data.GetValue(x);
        double labelX = x * this.CellSize.Width;

        string toolTipCoords = string.Format(toolTipFmt, x);

        if (data.GetType().IsArray)
          this.AddLabel(ArrayRenderSection.Front, toolTipCoords, labelX, 1, (Array)data);
        else
        {
          string text = this.GetText(data);
          AddLabel(ArrayRenderSection.Front, toolTipCoords, labelX, 1, text);
        }
      }
    }

    protected override void RenderBlankGrid()
    {
      if (this.Data.Rank != 1)
        throw new ArrayTypeMismatchException(AvProp.Resources.ArrayNot1DException);

      this.Width = this.CellSize.Width * base.DimX + 1;
      this.Height = this.CellSize.Height + 1;
      this.InvalidateVisual();

      for (double y = 0; y <= this.Height; y = y + this.CellSize.Height)
        this.AddLine(0, y, this.Width, y);

      for (double x = 0; x <= this.Width; x = x + this.CellSize.Width)
        this.AddLine(x, 0, x, this.Height);
    }

    #endregion
  }
}