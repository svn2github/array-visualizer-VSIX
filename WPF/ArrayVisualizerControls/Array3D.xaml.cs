using System;
using System.Threading;
using AvProp = ArrayVisualizerControls.Properties;

namespace ArrayVisualizerControls
{

  public partial class Array3D : ArrayControl
  {
    private int arraySizeX;
    private int arraySizeY;
    private int arraySizeZ;

    protected override void RenderBlankGrid()
    {
      double zCellHeight = CellSize.Height * .75;
      double zCellWidth = CellSize.Width * .75;

      double zSectionHeight = zCellHeight * this.arraySizeZ;
      double zSectionWidth = zCellWidth * this.arraySizeZ;

      double xySectionWidth = CellSize.Width * this.arraySizeX;
      double xySectionHeight = CellSize.Height * this.arraySizeY;

      this.Width = xySectionWidth + zSectionWidth + 1;
      this.Height = xySectionHeight + zSectionHeight + 1;

      {
        for (double y = zSectionHeight; y <= this.Height; y = y + CellSize.Height)
          AddLine(0, y, xySectionWidth, y);

        for (double x = 0; x <= xySectionWidth; x = x + CellSize.Width)
          AddLine(x, zSectionHeight, x, this.Height);

        //Top section
        for (double x = 0; x <= xySectionWidth; x = x + CellSize.Width)
          AddLine(x, zSectionHeight, x + zSectionWidth, 0);

        double tempX = 0;
        for (double y = zSectionHeight - zCellHeight; y >= 0; y = y - zCellHeight)
        {
          tempX += zCellWidth;
          AddLine(tempX, y, tempX + xySectionWidth, y);
        }

        //Right section
        for (double y = zSectionHeight + this.CellHeight; y <= this.Height; y = y + CellSize.Height)
          AddLine(this.Width - zSectionWidth, y, this.Width, y - zSectionHeight);

        double tempY = 0;
        for (double x = xySectionWidth + zSectionWidth; x >= xySectionWidth + zCellWidth; x = x - zCellWidth)
        {
          AddLine(x, tempY, x, tempY + xySectionHeight);
          tempY += zCellHeight;
        }
      }
    }

    protected override void DrawContent()
    {
      double zCellHeight = CellSize.Height * .75;
      double zCellWidth = CellSize.Width * .75;

      double zSectionHeight = zCellHeight * this.arraySizeZ;

      double xySectionWidth = CellSize.Width * this.arraySizeX;

      //Main grid (front)
      double num;
      for (int y = 0; y < this.arraySizeY; y++)
        for (int x = 0; x < this.arraySizeX; x++)
        {
          string text = this.Data.GetValue(0, y, x).ToString();          
          if (double.TryParse(text, out num))
            text = num.ToString(this.Formatter, Thread.CurrentThread.CurrentUICulture.NumberFormat);

          double labelX = x * CellSize.Width;
          double labelY = y * CellSize.Height + zSectionHeight;
          AddLabel(text, labelX, labelY);
        }

      //Top section
      for (int z = 0; z < this.arraySizeZ; z++)
        for (int x = 0; x < this.arraySizeX; x++)
        {
          string text = this.Data.GetValue(z, 0, x).ToString();
          if (double.TryParse(text, out num))
            text = num.ToString(this.Formatter, Thread.CurrentThread.CurrentUICulture.NumberFormat);

          double labelX = z * zCellWidth + zCellWidth + x * CellSize.Width;
          double labelY = zSectionHeight - (z * zCellHeight + (CellSize.Height) / 2);
          AddLabel(text, labelX, labelY);
        }

      //Right section
      for (int z = 0; z < this.arraySizeZ; z++)
        for (int y = 0; y < this.arraySizeY; y++)
        {
          string text = this.Data.GetValue(z, y, this.arraySizeX - 1).ToString();
          if (double.TryParse(text, out num))
            text = num.ToString(this.Formatter, Thread.CurrentThread.CurrentUICulture.NumberFormat);
          
          double labelX = xySectionWidth + z * zCellWidth + zCellWidth / 2;
          double labelY = zSectionHeight + y * CellSize.Height - zCellHeight * z;
          AddLabel(text, labelX, labelY);
        }
    }

    protected override void SetAxisSize()
    {
      if (this.Data.Rank != 3)
        throw new ArrayTypeMismatchException(AvProp.Resources.ArrayNot3DException);

      this.arraySizeZ = this.Data.GetLength(0);
      this.arraySizeY = this.Data.GetLength(1);
      this.arraySizeX = this.Data.GetLength(2);
    }
  }
}
