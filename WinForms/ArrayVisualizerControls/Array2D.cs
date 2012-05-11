using System;
using System.Drawing;
using System.Threading;
using WinFormsArrayVisualizerControls.Properties;

namespace WinFormsArrayVisualizerControls
{
#if DEBUG
  public partial class Array2D : ArrayProxy
#else
  public partial class Array2D : ArrayXD
#endif
  {
    private int ySize;
    private int xSize;

    protected override void RenderBlankGrid()
    {
      this.Width = CellSize.Width * this.xSize + 1;
      this.Height = CellSize.Height * this.ySize + 1;
      this.Refresh();

      this.Image = new Bitmap(this.Width, this.Height);

      using (Graphics gr = Graphics.FromImage(this.Image))
      {
        Pen pen = Pens.Black;
        for (int y = 0; y <= this.Height; y = y + CellSize.Height)
          gr.DrawLine(pen, 0, y, this.Width, y);
        for (int x = 0; x <= this.Width; x = x + CellSize.Width)
          gr.DrawLine(pen, x, 0, x, this.Height);
      }
    }

    protected override void DrawContent()
    {
      using (Graphics gr = Graphics.FromImage(this.Image))
      {
        gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
        Brush brush = Brushes.Black;
        double number;
        for (int y = 0; y < ySize; y++)
          for (int x = 0; x < xSize; x++)
          {
            string text = (this.Data.GetValue(y, x) ?? "").ToString();
            if (double.TryParse(text, out number))
              text = number.ToString(this.Formatter, Thread.CurrentThread.CurrentUICulture.NumberFormat);

            SizeF textSize = gr.MeasureString(text, this.RenderFont);

            if (textSize.Width + this.CellPadding > CellSize.Width)
              textSize.Width = CellSize.Width - this.CellPadding;
            if (textSize.Height + this.CellPadding > CellSize.Height)
              textSize.Height = CellSize.Height - this.CellPadding;

            float drawX = x * CellSize.Width + (CellSize.Width - textSize.Width) / 2;
            float drawY = y * CellSize.Height + (CellSize.Height - textSize.Height) / 2;

            PointF textPos = new PointF(drawX, drawY);

            gr.DrawString(text, this.RenderFont, brush, new RectangleF(textPos, textSize));
          }
      }
    }

    protected override void SetAxisSize()
    {
      if (this.Data.Rank != 2)
        throw new ArrayTypeMismatchException(Resources.ArrayNot2DException);

      this.ySize = this.Data.GetLength(0);
      this.xSize = this.Data.GetLength(1);
    }
  }
}

