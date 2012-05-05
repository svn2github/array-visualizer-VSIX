using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace ArrayVisualizer
{
#if DEBUG
  public partial class Array4D : ArrayProxy
#else
  public partial class Array4D : ArrayXD
#endif

  {
    private const int SPACE_4D = 15;

    protected int xSize;
    protected int ySize;
    protected int zSize;
    protected int aSize;

    protected override void RenderBlankGrid()
    {
      int zCellHeight = cellSize.Height / 4 * 3;
      int zCellWidth = cellSize.Width / 4 * 3;

      int zSectionHeight = zCellHeight * this.zSize;
      int zSectionWidth = zCellWidth * this.zSize;

      int xySectionWidth = cellSize.Width * this.xSize;
      int xySectionHeight = cellSize.Height * this.ySize;

      int sectionWidth = (xySectionWidth + zSectionWidth + 1);
      this.Width = sectionWidth * this.aSize + SPACE_4D * (this.aSize - 1);
      this.Height = xySectionHeight + zSectionHeight + 1;

      this.Refresh();

      this.Image = new Bitmap(this.Width, this.Height);

      using (Graphics gr = Graphics.FromImage(this.Image))
      {
        Pen pen = Pens.Black;
        //Main grid (front)
        for (int a = 0; a < aSize; a++)
        {
          int aOffset = a * (sectionWidth + SPACE_4D);

          for (int y = zSectionHeight; y <= this.Height; y = y + cellSize.Height)
            gr.DrawLine(pen, aOffset, y, aOffset + xySectionWidth, y);
          for (int x = 0; x <= xySectionWidth; x = x + cellSize.Width)
            gr.DrawLine(pen, aOffset + x, zSectionHeight, aOffset + x, this.Height);

          //Top section
          for (int x = 0; x <= xySectionWidth; x = x + cellSize.Width)
            gr.DrawLine(pen, aOffset + x, zSectionHeight, aOffset + x + zSectionWidth, 0);

          int tempX = 0;
          for (int y = zSectionHeight - zCellHeight; y >= 0; y = y - zCellHeight)
          {
            tempX += zCellWidth;
            gr.DrawLine(pen, aOffset + tempX, y, aOffset + tempX + xySectionWidth, y);
          }

          //Right section
          for (int y = zSectionHeight + this.CellHeight; y <= this.Height; y = y + cellSize.Height)
            gr.DrawLine(pen, aOffset + sectionWidth - zSectionWidth, y, aOffset + sectionWidth, y - zSectionHeight);

          int tempY = 0;
          for (int x = xySectionWidth + zSectionWidth; x >= xySectionWidth + zCellWidth; x = x - zCellWidth)
          {
            gr.DrawLine(pen, aOffset + x, tempY, aOffset + x, tempY + xySectionHeight);
            tempY += zCellHeight;
          }
        }
      }
    }

    protected override void DrawContent()
    {
      int zCellHeight = cellSize.Height / 4 * 3;
      int zCellWidth = cellSize.Width / 4 * 3;

      int zSectionHeight = zCellHeight * this.zSize;
      int zSectionWidth = zCellWidth * this.zSize;

      int xySectionWidth = cellSize.Width * this.xSize;
      int xySectionHeight = cellSize.Height * this.ySize;

      using (Graphics gr = Graphics.FromImage(this.Image))
      {
        gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
        Pen pen = Pens.Black;
        Brush brush = Brushes.Black;

        for (int a = 0; a < aSize; a++)
        {
          gr.ResetTransform();
          int aOffset = a * (xySectionWidth + SPACE_4D + zSectionWidth);

          //Main grid (front)
          for (int y = 0; y < ySize; y++)
            for (int x = 0; x < xSize; x++)
            {
              string textToRender = ((double)this.data.GetValue(a, 0, y, x)).ToString(this.formatter);
              SizeF textSize = gr.MeasureString(textToRender, this.RederFont);

              if (textSize.Width + this.cellPadding > cellSize.Width)
                textSize.Width = cellSize.Width - this.cellPadding;
              if (textSize.Height + this.cellPadding > cellSize.Height)
                textSize.Height = cellSize.Height - this.cellPadding;

              float drawX = aOffset + x * cellSize.Width + (cellSize.Width - textSize.Width) / 2;
              float drawY = y * cellSize.Height + (cellSize.Height - textSize.Height) / 2 + zSectionHeight;

              PointF textPos = new PointF(drawX, drawY);

              gr.DrawString(textToRender, this.RederFont, brush, new RectangleF(textPos, textSize));
            }

          //Top section                    
          for (int z = 0; z < zSize; z++)
            for (int x = 0; x < xSize; x++)
            {
              string textToRender = ((double)this.data.GetValue(a, z, 0, x)).ToString(this.formatter);
              SizeF textSize = gr.MeasureString(textToRender, this.RederFont);

              if (textSize.Width + this.cellPadding > zCellWidth)
                textSize.Width = zCellWidth - this.cellPadding;
              if (textSize.Height + this.cellPadding > zCellHeight)
                textSize.Height = zCellHeight - this.cellPadding;

              float drawX = aOffset + z * zCellWidth + zCellWidth + x * cellSize.Width;
              float drawY = zSectionHeight - (z * zCellHeight + (cellSize.Height) / 2);

              PointF textPos = new PointF(drawX, drawY);

              gr.DrawString(textToRender, this.RederFont, brush, new RectangleF(textPos, textSize));
            }

          //Right section
          PointF point00 = new PointF(-15, 0);
          for (int z = 0; z < zSize; z++)
            for (int y = 0; y < ySize; y++)
            {
              string textToRender = ((double)this.data.GetValue(a, z, y, xSize - 1)).ToString(this.formatter);
              SizeF textSize = gr.MeasureString(textToRender, this.RederFont);

              if (textSize.Width + this.cellPadding > zCellWidth)
                textSize.Width = zCellWidth - this.cellPadding;
              if (textSize.Height + this.cellPadding > zCellHeight)
                textSize.Height = zCellHeight - this.cellPadding;

              float drawX = xySectionWidth + z * zCellWidth + zCellWidth / 2;
              float drawY = zSectionHeight + y * cellSize.Height - zCellHeight * z;

              PointF textPos = new PointF(drawX, drawY);

              gr.ResetTransform();
              gr.TranslateTransform(aOffset + textPos.X, textPos.Y);
              gr.RotateTransform(-30);

              gr.DrawString(textToRender, this.RederFont, brush, new RectangleF(point00, textSize));
            }
        }
      }

    }
    protected override void SetAxisSize()
    {
      if (this.Data.Rank != 4)
        throw new ArrayTypeMismatchException("source array must be four dimensional.");

      this.aSize = this.Data.GetLength(0);
      this.zSize = this.Data.GetLength(1);
      this.ySize = this.Data.GetLength(2);
      this.xSize = this.Data.GetLength(3);
    }
  }
}



