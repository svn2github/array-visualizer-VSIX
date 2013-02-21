using System;
using AvProp = ArrayVisualizerControls.Properties;

namespace ArrayVisualizerControls
{
  public partial class Array3D : ArrayControl
  {
    #region Methods

    protected override void DrawContent()
    {
      if (this.Data.Rank != 3)
        throw new ArrayTypeMismatchException(AvProp.Resources.ArrayNot3DException);

      double zCellHeight = this.CellSize.Height * SIZE_FACTOR_3D;
      double zCellWidth = this.CellSize.Width * SIZE_FACTOR_3D;

      double zSectionHeight = zCellHeight * base.DimZ;
      double xySectionWidth = this.CellSize.Width * base.DimX;

      this.SetTransformers();

      // Main grid (front)
      string toolTipFmt = string.Format("{0}{{0}},{{1}},{{2}}{1}", this.LeftBracket, this.RightBracket);
      for (int y = 0; y < base.DimY; y++)
        for (int x = 0; x < base.DimX; x++)
        {
          object data = this.Data.GetValue(0, y, x);
          double labelX = x * this.CellSize.Width;
          double labelY = y * this.CellSize.Height + zSectionHeight;

          string toolTipCoords = string.Format(toolTipFmt, 0, y, x);

            AddLabel(ArrayRenderSection.Front, toolTipCoords, labelX, labelY, data);
        }

      // Top section
      for (int z = 0; z < base.DimZ; z++)
        for (int x = 0; x < base.DimX; x++)
        {
          object data = this.Data.GetValue(z, 0, x);
          double labelX = (z + 1) * zCellWidth + x * this.CellSize.Width;
          double labelY = zSectionHeight - (z + 1) * zCellHeight;

          string toolTipCoords = string.Format(toolTipFmt, z, 0, x);

            AddLabel(ArrayRenderSection.Top, toolTipCoords, labelX, labelY, data);
        }

      // Right section
      for (int z = 0; z < base.DimZ; z++)
        for (int y = 0; y < base.DimY; y++)
        {
          int x = base.DimX - 1;
          object data = this.Data.GetValue(z, y, x);
          double labelX = xySectionWidth + z * zCellWidth;
          double labelY = zSectionHeight + y * this.CellSize.Height - zCellHeight * z;

          string toolTipCoords = string.Format(toolTipFmt, z, y, x);

            AddLabel(ArrayRenderSection.Side, toolTipCoords, labelX, labelY, data);
        }
    }

    //Highlight color: #FFFDBF3A
    protected override void RenderBlankGrid()
    {
      if (this.Data.Rank != 3)
        throw new ArrayTypeMismatchException(AvProp.Resources.ArrayNot3DException);


      double zCellHeight = this.CellSize.Height * SIZE_FACTOR_3D;
      double zCellWidth = this.CellSize.Width * SIZE_FACTOR_3D;

      double zSectionHeight = zCellHeight * base.DimZ;
      double zSectionWidth = zCellWidth * base.DimZ;

      double xySectionWidth = this.CellSize.Width * base.DimX;
      double xySectionHeight = this.CellSize.Height * base.DimY;

      this.Width = xySectionWidth + zSectionWidth + 1;
      this.Height = xySectionHeight + zSectionHeight + 1;

      for (double y = zSectionHeight; y <= this.Height; y = y + this.CellSize.Height)
        this.AddLine(0, y, xySectionWidth, y);

      for (double x = 0; x <= xySectionWidth; x = x + this.CellSize.Width)
        this.AddLine(x, zSectionHeight, x, this.Height);

      // Top section
      for (double x = 0; x <= xySectionWidth; x = x + this.CellSize.Width)
        this.AddLine(x, zSectionHeight, x + zSectionWidth, 0);

      double tempX = 0;
      for (double y = zSectionHeight - zCellHeight; y >= 0; y = y - zCellHeight)
      {
        tempX += zCellWidth;
        this.AddLine(tempX, y, tempX + xySectionWidth, y);
      }

      // Right section
      for (double y = zSectionHeight + this.CellHeight; y <= this.Height; y = y + this.CellSize.Height)
        this.AddLine(this.Width - zSectionWidth, y, this.Width, y - zSectionHeight);

      double tempY = 0;
      for (double x = xySectionWidth + zSectionWidth; x >= xySectionWidth + zCellWidth; x = x - zCellWidth)
      {
        this.AddLine(x, tempY, x, tempY + xySectionHeight);
        tempY += zCellHeight;
      }
    }

    #endregion
  }
}