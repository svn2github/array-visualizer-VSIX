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
  public partial class Array2D : ArrayProxy
#else
  public partial class Array2D : ArrayXD
#endif
  {
    protected int ySize;
    protected int xSize;

    protected override void RenderBlankGrid()
    {
      this.Width = cellSize.Width * this.xSize + 1;
      this.Height = cellSize.Height * this.ySize + 1;
      this.Refresh();

      this.Image = new Bitmap(this.Width, this.Height);

      using (Graphics gr = Graphics.FromImage(this.Image))
      {
        Pen pen = Pens.Black;
        for (int y = 0; y <= this.Height; y = y + cellSize.Height)
          gr.DrawLine(pen, 0, y, this.Width, y);
        for (int x = 0; x <= this.Width; x = x + cellSize.Width)
          gr.DrawLine(pen, x, 0, x, this.Height);
      }
    }

    protected override void DrawContent()
    {
      using (Graphics gr = Graphics.FromImage(this.Image))
      {
        gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
        Pen pen = Pens.Black;
        Brush brush = Brushes.Black;
        for (int y = 0; y < ySize; y++)
          for (int x = 0; x < xSize; x++)
          {
            string textToRender = ((double)this.data.GetValue(y, x)).ToString(this.formatter);
            SizeF textSize = gr.MeasureString(textToRender, this.RederFont);

            if (textSize.Width + this.cellPadding > cellSize.Width)
              textSize.Width = cellSize.Width - this.cellPadding;
            if (textSize.Height + this.cellPadding > cellSize.Height)
              textSize.Height = cellSize.Height - this.cellPadding;

            float drawX = x * cellSize.Width + (cellSize.Width - textSize.Width) / 2;
            float drawY = y * cellSize.Height + (cellSize.Height - textSize.Height) / 2;

            PointF textPos = new PointF(drawX, drawY);

            gr.DrawString(textToRender, this.RederFont, brush, new RectangleF(textPos, textSize));
          }
      }
    }

    protected override void SetAxisSize()
    {
      if (this.Data.Rank != 2)
        throw new ArrayTypeMismatchException("source array must be two dimensional.");

      this.ySize = this.Data.GetLength(0);
      this.xSize = this.Data.GetLength(1);
    }
  }
}

