using System;
using System.Threading;
using AvProp = ArrayVisualizerControls.Properties;

namespace ArrayVisualizerControls
{

  public partial class Array2D : ArrayControl
  {
    private int arraySizeX;
    private int arraySizeY;

    protected override void RenderBlankGrid()
    {
      this.Width = CellSize.Width * this.arraySizeX + 1;
      this.Height = CellSize.Height * this.arraySizeY + 1;
      this.InvalidateVisual();

      for (double y = 0; y <= this.Height; y = y + CellSize.Height)
        AddLine(0, y, this.Width, y);

      for (double x = 0; x <= this.Width; x = x + CellSize.Width)
        AddLine(x, 0, x, this.Height);
    }

    protected override void DrawContent()
    {
      double number;
      for (int y = 0; y < arraySizeY; y++)
        for (int x = 0; x < arraySizeX; x++)
        {
          string text = (this.Data.GetValue(y, x) ?? "").ToString();          
          if (double.TryParse(text, out number))
            text = number.ToString(this.Formatter, Thread.CurrentThread.CurrentUICulture.NumberFormat);
          double labelX = x * CellSize.Width;
          double labelY = y * CellSize.Height;
          AddLabel(text, labelX, labelY);
        }
    }

    protected override void SetAxisSize()
    {
      if (this.Data.Rank != 2)
        throw new ArrayTypeMismatchException(AvProp.Resources.ArrayNot2DException);

      this.arraySizeY = this.Data.GetLength(0);
      this.arraySizeX = this.Data.GetLength(1);
    }
  }
}
