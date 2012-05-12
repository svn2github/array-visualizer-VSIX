using System;
using System.Threading;
using AvProp = ArrayVisualizerControls.Properties;

namespace ArrayVisualizerControls
{

  public partial class Array4D : ArrayControl
  {
    private const int SPACE_4D = 15;

    private int arraySizeX;
    private int arraySizeY;
    private int arraySizeZ;
    private int arraySizeA;

    protected override void RenderBlankGrid()
    {
      double zCellHeight = CellSize.Height * SIZE_FACTOR_3D;
      double zCellWidth = CellSize.Width * SIZE_FACTOR_3D;

      double zSectionHeight = zCellHeight * this.arraySizeZ;
      double zSectionWidth = zCellWidth * this.arraySizeZ;

      double xySectionWidth = CellSize.Width * this.arraySizeX;
      double xySectionHeight = CellSize.Height * this.arraySizeY;

      this.Width = xySectionWidth + zSectionWidth + 1;
      this.Height = xySectionHeight + zSectionHeight + 1;

      double sectionWidth = (xySectionWidth + zSectionWidth + 1);
      this.Width = sectionWidth * this.arraySizeA + SPACE_4D * (this.arraySizeA - 1);
      this.Height = xySectionHeight + zSectionHeight + 1;


      for (int a = 0; a < this.arraySizeA; a++)
      {
        double aOffset = a * (sectionWidth + SPACE_4D);

        //Front
        for (double y = zSectionHeight; y <= this.Height; y = y + CellSize.Height)
          AddLine(aOffset, y, aOffset + xySectionWidth, y);

        for (double x = 0; x <= xySectionWidth; x = x + CellSize.Width)
          AddLine(aOffset + x, zSectionHeight, aOffset + x, this.Height);

        //Top section
        for (double x = 0; x <= xySectionWidth; x = x + CellSize.Width)
          AddLine(aOffset + x, zSectionHeight, aOffset + x + zSectionWidth, 0);

        double tempX = 0;
        for (double y = zSectionHeight - zCellHeight; y >= 0; y = y - zCellHeight)
        {
          tempX += zCellWidth;
          AddLine(aOffset + tempX, y, aOffset + tempX + xySectionWidth, y);
        }

        //Right section
        for (double y = zSectionHeight + this.CellHeight; y <= this.Height; y = y + CellSize.Height)
          AddLine(aOffset + sectionWidth - zSectionWidth, y, aOffset + sectionWidth, y - zSectionHeight);

        double tempY = 0;
        for (double x = xySectionWidth + zSectionWidth; x >= xySectionWidth + zCellWidth; x = x - zCellWidth)
        {
          AddLine(aOffset + x, tempY, aOffset + x, tempY + xySectionHeight);
          tempY += zCellHeight;
        }
      }
    }

    protected override void DrawContent()
    {
      double zCellHeight = CellSize.Height * SIZE_FACTOR_3D;
      double zCellWidth = CellSize.Width * SIZE_FACTOR_3D;

      double zSectionHeight = zCellHeight * this.arraySizeZ;
      double zSectionWidth = zCellWidth * this.arraySizeZ;

      double xySectionWidth = CellSize.Width * this.arraySizeX;

      base.SetTransformers();
      double number;

      for (int a = 0; a < this.arraySizeA; a++)
      {
        double aOffset = a * (xySectionWidth + SPACE_4D + zSectionWidth);

        //Main grid (front)
        for (int y = 0; y < this.arraySizeY; y++)
          for (int x = 0; x < this.arraySizeX; x++)
          {
            string text = (this.Data.GetValue(a, 0, y, x) ?? "").ToString();
            if (double.TryParse(text, out number))
              text = number.ToString(this.Formatter, Thread.CurrentThread.CurrentUICulture.NumberFormat);

            double labelX = aOffset +x * CellSize.Width;
            double labelY = y * CellSize.Height + zSectionHeight;
            AddLabel(ArrayRenderSection.Front, text, labelX, labelY);
          }

        //Top section                    
        for (int z = 0; z < this.arraySizeZ; z++)
          for (int x = 0; x < this.arraySizeX; x++)
          {
            string text = (this.Data.GetValue(a, z, 0, x) ?? "").ToString();
            if (double.TryParse(text, out number))
              text = number.ToString(this.Formatter, Thread.CurrentThread.CurrentUICulture.NumberFormat);

            double labelX = aOffset + (z + 1) * zCellWidth + x * CellSize.Width;
            double labelY = zSectionHeight - (z + 1) * zCellHeight;
            AddLabel(ArrayRenderSection.Top, text, labelX, labelY);
          }

        //Right section
        for (int z = 0; z < this.arraySizeZ; z++)
          for (int y = 0; y < this.arraySizeY; y++)
          {
            string text = (this.Data.GetValue(a, z, y, this.arraySizeX - 1) ?? "").ToString();
            if (double.TryParse(text, out number))
              text = number.ToString(this.Formatter, Thread.CurrentThread.CurrentUICulture.NumberFormat);

            double labelX =aOffset+ xySectionWidth + z * zCellWidth;
            double labelY = zSectionHeight + y * CellSize.Height - zCellHeight * z;
            AddLabel(ArrayRenderSection.Side, text, labelX, labelY);
          }
      }
    }

    protected override void SetAxisSize()
    {
      if (this.Data.Rank != 4)
        throw new ArrayTypeMismatchException(AvProp.Resources.ArrayNot4DException);

      this.arraySizeA = this.Data.GetLength(0);
      this.arraySizeZ = this.Data.GetLength(1);
      this.arraySizeY = this.Data.GetLength(2);
      this.arraySizeX = this.Data.GetLength(3);
    }
  }
}
