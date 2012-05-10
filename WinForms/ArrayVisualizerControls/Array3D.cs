using System;
using System.Drawing;
using System.Threading;
using WinFormsArrayVisualizerControls.Properties;

namespace WinFormsArrayVisualizerControls
{
#if DEBUG
  public partial class Array3D : ArrayProxy
#else
  public partial class Array3D : ArrayXD
#endif

  {
    private int xSize;
    private int ySize;
    private int zSize;

    protected override void RenderBlankGrid()
    {
      int zCellHeight = CellSize.Height / 4 * 3;
      int zCellWidth = CellSize.Width / 4 * 3;

      int zSectionHeight = zCellHeight * this.zSize;
      int zSectionWidth = zCellWidth * this.zSize;

      int xySectionWidth = CellSize.Width * this.xSize;
      int xySectionHeight = CellSize.Height * this.ySize;

      this.Width = xySectionWidth + zSectionWidth + 1;
      this.Height = xySectionHeight + zSectionHeight + 1;

      this.Refresh();

      this.Image = new Bitmap(this.Width, this.Height);

      using (Graphics gr = Graphics.FromImage(this.Image))
      {
        Pen pen = Pens.Black;
        //Main grid (front)
        for (int y = zSectionHeight; y <= this.Height; y = y + CellSize.Height)
          gr.DrawLine(pen, 0, y, xySectionWidth, y);
        for (int x = 0; x <= xySectionWidth; x = x + CellSize.Width)
          gr.DrawLine(pen, x, zSectionHeight, x, this.Height);

        //Top section
        for (int x = 0; x <= xySectionWidth; x = x + CellSize.Width)
          gr.DrawLine(pen, x, zSectionHeight, x + zSectionWidth, 0);

        int tempX = 0;
        for (int y = zSectionHeight - zCellHeight; y >= 0; y = y - zCellHeight)
        {
          tempX += zCellWidth;
          gr.DrawLine(pen, tempX, y, tempX + xySectionWidth, y);
        }

        //Right section
        for (int y = zSectionHeight + this.CellHeight; y <= this.Height; y = y + CellSize.Height)
          gr.DrawLine(pen, this.Width - zSectionWidth, y, this.Width, y - zSectionHeight);

        int tempY = 0;
        for (int x = xySectionWidth + zSectionWidth; x >= xySectionWidth + zCellWidth; x = x - zCellWidth)
        {
          gr.DrawLine(pen, x, tempY, x, tempY + xySectionHeight);
          tempY += zCellHeight;
        }

      }
    }

    protected override void DrawContent()
    {
      int zCellHeight = CellSize.Height / 4 * 3;
      int zCellWidth = CellSize.Width / 4 * 3;

      int zSectionHeight = zCellHeight * this.zSize;

      int xySectionWidth = CellSize.Width * this.xSize;

      using (Graphics gr = Graphics.FromImage(this.Image))
      {
        gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
        Brush brush = Brushes.Black;

        //Main grid (front)
        for (int y = 0; y < ySize; y++)
          for (int x = 0; x < xSize; x++)
          {
            string textToRender = ((double)this.Data.GetValue(0, y, x)).ToString(this.Formatter, Thread.CurrentThread.CurrentUICulture.NumberFormat);
            SizeF textSize = gr.MeasureString(textToRender, this.RenderFont);

            if (textSize.Width + this.CellPadding > CellSize.Width)
              textSize.Width = CellSize.Width - this.CellPadding;
            if (textSize.Height + this.CellPadding > CellSize.Height)
              textSize.Height = CellSize.Height - this.CellPadding;

            float drawX = x * CellSize.Width + (CellSize.Width - textSize.Width) / 2;
            float drawY = y * CellSize.Height + (CellSize.Height - textSize.Height) / 2 + zSectionHeight;

            PointF textPos = new PointF(drawX, drawY);
            gr.DrawString(textToRender, this.RenderFont, brush, new RectangleF(textPos, textSize));
          }

        //Top section
        for (int z = 0; z < zSize; z++)
          for (int x = 0; x < xSize; x++)
          {
            string textToRender = ((double)this.Data.GetValue(z, 0, x)).ToString(this.Formatter, Thread.CurrentThread.CurrentUICulture.NumberFormat);
            SizeF textSize = gr.MeasureString(textToRender, this.RenderFont);

            if (textSize.Width + this.CellPadding > zCellWidth)
              textSize.Width = zCellWidth - this.CellPadding;
            if (textSize.Height + this.CellPadding > zCellHeight)
              textSize.Height = zCellHeight - this.CellPadding;

            float drawX = z * zCellWidth + zCellWidth + x * CellSize.Width;
            float drawY = zSectionHeight - (z * zCellHeight + (CellSize.Height) / 2);

            PointF textPos = new PointF(drawX, drawY);

            gr.DrawString(textToRender, this.RenderFont, brush, new RectangleF(textPos, textSize));
          }

        //Right section
        PointF point00 = new PointF(-15, 0);
        for (int z = 0; z < zSize; z++)
          for (int y = 0; y < ySize; y++)
          {
            string textToRender = ((double)this.Data.GetValue(z, y, xSize - 1)).ToString(this.Formatter, Thread.CurrentThread.CurrentUICulture.NumberFormat);
            SizeF textSize = gr.MeasureString(textToRender, this.RenderFont);

            if (textSize.Width + this.CellPadding > zCellWidth)
              textSize.Width = zCellWidth - this.CellPadding;
            if (textSize.Height + this.CellPadding > zCellHeight)
              textSize.Height = zCellHeight - this.CellPadding;

            float drawX = xySectionWidth + z * zCellWidth + zCellWidth / 2;
            float drawY = zSectionHeight + y * CellSize.Height - zCellHeight * z;

            PointF textPos = new PointF(drawX, drawY);

            gr.ResetTransform();
            gr.TranslateTransform(textPos.X, textPos.Y);
            gr.RotateTransform(-30);

            gr.DrawString(textToRender, this.RenderFont, brush, new RectangleF(point00, textSize));
          }
      }
    }

    protected override void SetAxisSize()
    {
      if (this.Data.Rank != 3)
        throw new ArrayTypeMismatchException(Resources.ArrayNot3DException);

      this.zSize = this.Data.GetLength(0);
      this.ySize = this.Data.GetLength(1);
      this.xSize = this.Data.GetLength(2);
    }
  }
}



