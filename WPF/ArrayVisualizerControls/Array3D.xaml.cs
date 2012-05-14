﻿using System;
using System.Threading;
using AvProp = ArrayVisualizerControls.Properties;

namespace ArrayVisualizerControls
{

  public partial class Array3D : ArrayControl
  {
    protected override void RenderBlankGrid()
    {
      if (this.Data.Rank != 3)
        throw new ArrayTypeMismatchException(AvProp.Resources.ArrayNot3DException);

      double zCellHeight = CellSize.Height * SIZE_FACTOR_3D;
      double zCellWidth = CellSize.Width * SIZE_FACTOR_3D;

      double zSectionHeight = zCellHeight * base.DimZ;
      double zSectionWidth = zCellWidth * base.DimZ;

      double xySectionWidth = CellSize.Width * base.DimX;
      double xySectionHeight = CellSize.Height * base.DimY;

      this.Width = xySectionWidth + zSectionWidth + 1;
      this.Height = xySectionHeight + zSectionHeight + 1;

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

    protected override void DrawContent()
    {
      if (this.Data.Rank != 3)
        throw new ArrayTypeMismatchException(AvProp.Resources.ArrayNot3DException);

      double zCellHeight = CellSize.Height * SIZE_FACTOR_3D;
      double zCellWidth = CellSize.Width * SIZE_FACTOR_3D;

      double zSectionHeight = zCellHeight * base.DimZ;
      double xySectionWidth = CellSize.Width * base.DimX;

      base.SetTransformers();

      //Main grid (front)
      double number;
      string toolTipFmt = "[{0},{1},{2}] : {3}";
      for (int y = 0; y < base.DimY; y++)
        for (int x = 0; x < base.DimX; x++)
        {
          string text = (this.Data.GetValue(0, y, x) ?? "").ToString();          
          if (double.TryParse(text, out number))
            text = number.ToString(this.Formatter, Thread.CurrentThread.CurrentUICulture.NumberFormat);
          string toolTip = string.Format(toolTipFmt, 0, y, x, text);

          double labelX = x * CellSize.Width;
          double labelY = y * CellSize.Height + zSectionHeight;
          AddLabel(ArrayRenderSection.Front, text, toolTip, labelX, labelY);
        }

      //Top section
      for (int z = 0; z < base.DimZ; z++)
        for (int x = 0; x < base.DimX; x++)
        {
          string text = (this.Data.GetValue(z, 0, x) ?? "").ToString();          
          if (double.TryParse(text, out number))
            text = number.ToString(this.Formatter, Thread.CurrentThread.CurrentUICulture.NumberFormat);
          string toolTip = string.Format(toolTipFmt, z, 0, x, text);

          double labelX = (z + 1) * zCellWidth + x * CellSize.Width;
          double labelY = zSectionHeight - (z + 1) * zCellHeight;
          AddLabel(ArrayRenderSection.Top, text, toolTip, labelX, labelY);
        }

      //Right section
      for (int z = 0; z < base.DimZ; z++)
        for (int y = 0; y < base.DimY; y++)
        {
          int x = base.DimX - 1;
          string text = (this.Data.GetValue(z, y, x) ?? "").ToString();          
          if (double.TryParse(text, out number))
            text = number.ToString(this.Formatter, Thread.CurrentThread.CurrentUICulture.NumberFormat);
          string toolTip = string.Format(toolTipFmt, z, y, x, text);

          double labelX = xySectionWidth + z * zCellWidth;
          double labelY = zSectionHeight + y * CellSize.Height - zCellHeight * z;
          AddLabel(ArrayRenderSection.Side, text, toolTip,labelX, labelY);
        }
    }
  }
}
