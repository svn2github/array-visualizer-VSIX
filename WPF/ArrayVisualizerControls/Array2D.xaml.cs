using System;
using System.Threading;
using AvProp = ArrayVisualizerControls.Properties;

namespace ArrayVisualizerControls
{

  public partial class Array2D : ArrayControl
  {
    //private int arraySizeX;
    //private int arraySizeY;

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

      double number;
      string toolTipFmt = "[{0},{1}] : {2}";
      for (int y = 0; y < base.DimY; y++)
        for (int x = 0; x < base.DimX; x++)
        {
          string text = (this.Data.GetValue(y, x) ?? "").ToString();
          if (double.TryParse(text, out number))
            text = number.ToString(this.Formatter, Thread.CurrentThread.CurrentUICulture.NumberFormat);
          string toolTip = string.Format(toolTipFmt, y, x, text);

          double labelX = x * CellSize.Width;
          double labelY = y * CellSize.Height;
          AddLabel(ArrayRenderSection.Front, text, toolTip, labelX, labelY);
        }
    }
  }
}
