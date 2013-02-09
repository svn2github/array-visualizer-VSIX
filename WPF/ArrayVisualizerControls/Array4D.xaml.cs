using System;
using AvProp = ArrayVisualizerControls.Properties;

namespace ArrayVisualizerControls
{
  public partial class Array4D : ArrayControl
  {
    #region Constants

    private const int SPACE_4D = 15;

    #endregion

    #region Methods

    protected override void DrawContent()
    {
      if (this.Data.Rank != 4)
        throw new ArrayTypeMismatchException(AvProp.Resources.ArrayNot4DException);


      double zCellHeight = this.CellSize.Height * SIZE_FACTOR_3D;
      double zCellWidth = this.CellSize.Width * SIZE_FACTOR_3D;

      double zSectionHeight = zCellHeight * base.DimZ;
      double zSectionWidth = zCellWidth * base.DimZ;

      double xySectionWidth = this.CellSize.Width * base.DimX;

      this.SetTransformers();

      string toolTipFmt = "[{0},{1},{2},{3}]";
      for (int a = 0; a < base.DimA; a++)
      {
        double aOffset = a * (xySectionWidth + SPACE_4D + zSectionWidth);

        // Main grid (front)
        for (int y = 0; y < base.DimY; y++)
          for (int x = 0; x < base.DimX; x++)
          {
            object data = this.Data.GetValue(a, 0, y, x);
            double labelX = aOffset + x * this.CellSize.Width;
            double labelY = y * this.CellSize.Height + zSectionHeight;

            string toolTipCoords = string.Format(toolTipFmt, a, 0, y, x);

            if (data.GetType().IsArray)
              this.AddLabel(ArrayRenderSection.Front, toolTipCoords, labelX, labelY, (Array)data);
            else
            {
              string text = this.GetText(data);
              AddLabel(ArrayRenderSection.Front, toolTipCoords, labelX, labelY, text);
            }
          }

        // Top section                    
        for (int z = 0; z < base.DimZ; z++)
          for (int x = 0; x < base.DimX; x++)
          {
            object data = this.Data.GetValue(a, z, 0, x);
            double labelX = aOffset + (z + 1) * zCellWidth + x * this.CellSize.Width;
            double labelY = zSectionHeight - (z + 1) * zCellHeight;

            string toolTipCoords = string.Format(toolTipFmt, a, z, 0, x);

            if (data.GetType().IsArray)
              this.AddLabel(ArrayRenderSection.Front, toolTipCoords, labelX, labelY, (Array)data);
            else
            {
              string text = this.GetText(data);
              AddLabel(ArrayRenderSection.Top, toolTipCoords, labelX, labelY, text);
            }
          }

        // Right section
        for (int z = 0; z < base.DimZ; z++)
          for (int y = 0; y < base.DimY; y++)
          {
            int x = base.DimX - 1;
            object data = this.Data.GetValue(a, z, y, x);
            double labelX = aOffset + xySectionWidth + z * zCellWidth;
            double labelY = zSectionHeight + y * this.CellSize.Height - zCellHeight * z;

            string toolTipCoords = string.Format(toolTipFmt, a, z, y, x);

            if (data.GetType().IsArray)
              this.AddLabel(ArrayRenderSection.Front, toolTipCoords, labelX, labelY, (Array)data);
            else
            {
              string text = this.GetText(data);
              AddLabel(ArrayRenderSection.Side, toolTipCoords, labelX, labelY, text);
            }
          }
      }
    }

    protected override void RenderBlankGrid()
    {
      if (this.Data.Rank != 4)
        throw new ArrayTypeMismatchException(AvProp.Resources.ArrayNot4DException);

      double zCellHeight = this.CellSize.Height * SIZE_FACTOR_3D;
      double zCellWidth = this.CellSize.Width * SIZE_FACTOR_3D;

      double zSectionHeight = zCellHeight * base.DimZ;
      double zSectionWidth = zCellWidth * base.DimZ;

      double xySectionWidth = this.CellSize.Width * base.DimX;
      double xySectionHeight = this.CellSize.Height * base.DimY;

      this.Width = xySectionWidth + zSectionWidth + 1;
      this.Height = xySectionHeight + zSectionHeight + 1;

      double sectionWidth = xySectionWidth + zSectionWidth + 1;
      this.Width = sectionWidth * base.DimA + SPACE_4D * (base.DimA - 1);
      this.Height = xySectionHeight + zSectionHeight + 1;

      for (int a = 0; a < base.DimA; a++)
      {
        double aOffset = a * (sectionWidth + SPACE_4D);

        // Front
        for (double y = zSectionHeight; y <= this.Height; y = y + this.CellSize.Height)
          this.AddLine(aOffset, y, aOffset + xySectionWidth, y);

        for (double x = 0; x <= xySectionWidth; x = x + this.CellSize.Width)
          this.AddLine(aOffset + x, zSectionHeight, aOffset + x, this.Height);

        // Top section
        for (double x = 0; x <= xySectionWidth; x = x + this.CellSize.Width)
          this.AddLine(aOffset + x, zSectionHeight, aOffset + x + zSectionWidth, 0);

        double tempX = 0;
        for (double y = zSectionHeight - zCellHeight; y >= 0; y = y - zCellHeight)
        {
          tempX += zCellWidth;
          this.AddLine(aOffset + tempX, y, aOffset + tempX + xySectionWidth, y);
        }

        // Right section
        for (double y = zSectionHeight + this.CellHeight; y <= this.Height; y = y + this.CellSize.Height)
          this.AddLine(aOffset + sectionWidth - zSectionWidth, y, aOffset + sectionWidth, y - zSectionHeight);

        double tempY = 0;
        for (double x = xySectionWidth + zSectionWidth; x >= xySectionWidth + zCellWidth; x = x - zCellWidth)
        {
          this.AddLine(aOffset + x, tempY, aOffset + x, tempY + xySectionHeight);
          tempY += zCellHeight;
        }
      }
    }

    #endregion
  }
}