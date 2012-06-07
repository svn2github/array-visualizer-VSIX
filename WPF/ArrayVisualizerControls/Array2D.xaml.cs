using System;
using AvProp = ArrayVisualizerControls.Properties;

namespace ArrayVisualizerControls
{
  public partial class Array2D : ArrayControl
  {
    protected override void RenderBlankGrid()
    {
      if (this.Data.Rank != 2)
        throw new ArrayTypeMismatchException(AvProp.Resources.ArrayNot2DException);

      this.Width = CellSize.Width * base.DimX + 1;
      this.Height = CellSize.Height * base.DimY + 1;
      this.InvalidateVisual();

      for (double y = 0; y <= this.Height; y = y + CellSize.Height)
        AddLine(0, y, this.Width, y);

      for (double x = 0; x <= this.Width; x = x + CellSize.Width)
        AddLine(x, 0, x, this.Height);
    }

    protected override void DrawContent()
    {
      if (this.Data.Rank != 2)
        throw new ArrayTypeMismatchException(AvProp.Resources.ArrayNot2DException);

      string toolTipFmt = "[{0},{1}]";
      for (int y = 0; y < base.DimY; y++)
        for (int x = 0; x < base.DimX; x++)
        {
          string toolTipCoords = string.Format(toolTipFmt, y, x);
          object data = this.Data.GetValue(y, x);
          double labelX = x * CellSize.Width;
          double labelY = y * CellSize.Height;

          if ((data.GetType().IsArray))
            AddLabel(ArrayRenderSection.Front, toolTipCoords,labelX, labelY, (Array)data);
          else
          {
            string text = GetText(data);
            AddLabel(ArrayRenderSection.Front, toolTipCoords, labelX, labelY, text);
          }
        }
    }
  }
}
