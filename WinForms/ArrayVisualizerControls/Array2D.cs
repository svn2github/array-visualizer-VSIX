// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Array2D.cs" company="">
//   
// </copyright>
// <summary>
//   The array 2 d.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WinFormsArrayVisualizerControls
{
  using System;
  using System.Drawing;
  using System.Drawing.Text;
  using System.Threading;

  using WinFormsArrayVisualizerControls.Properties;

#if DEBUG

  /// <summary>
  /// The array 2 d.
  /// </summary>
  public class Array2D : ArrayProxy
#else
  public partial class Array2D : ArrayXD
#endif
  {
    /// <summary>
    /// The y size.
    /// </summary>
    private int ySize;

    /// <summary>
    /// The x size.
    /// </summary>
    private int xSize;

    /// <summary>
    /// The render blank grid.
    /// </summary>
    protected override void RenderBlankGrid()
    {
      this.Width = this.CellSize.Width * this.xSize + 1;
      this.Height = this.CellSize.Height * this.ySize + 1;
      this.Refresh();

      this.Image = new Bitmap(this.Width, this.Height);

      using (Graphics gr = Graphics.FromImage(this.Image))
      {
        Pen pen = Pens.Black;
        for (int y = 0; y <= this.Height; y = y + this.CellSize.Height)
        {
          gr.DrawLine(pen, 0, y, this.Width, y);
        }

        for (int x = 0; x <= this.Width; x = x + this.CellSize.Width)
        {
          gr.DrawLine(pen, x, 0, x, this.Height);
        }
      }
    }

    /// <summary>
    /// The draw content.
    /// </summary>
    protected override void DrawContent()
    {
      using (Graphics gr = Graphics.FromImage(this.Image))
      {
        gr.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
        Brush brush = Brushes.Black;
        double number;
        for (int y = 0; y < this.ySize; y++)
        {
          for (int x = 0; x < this.xSize; x++)
          {
            string text = (this.Data.GetValue(y, x) ?? string.Empty).ToString();
            if (double.TryParse(text, out number))
            {
              text = number.ToString(this.Formatter, Thread.CurrentThread.CurrentUICulture.NumberFormat);
            }

            SizeF textSize = gr.MeasureString(text, this.RenderFont);

            if (textSize.Width + this.CellPadding > this.CellSize.Width)
            {
              textSize.Width = this.CellSize.Width - this.CellPadding;
            }

            if (textSize.Height + this.CellPadding > this.CellSize.Height)
            {
              textSize.Height = this.CellSize.Height - this.CellPadding;
            }

            float drawX = x * this.CellSize.Width + (this.CellSize.Width - textSize.Width) / 2;
            float drawY = y * this.CellSize.Height + (this.CellSize.Height - textSize.Height) / 2;

            var textPos = new PointF(drawX, drawY);

            gr.DrawString(text, this.RenderFont, brush, new RectangleF(textPos, textSize));
          }
        }
      }
    }

    /// <summary>
    /// The set axis size.
    /// </summary>
    /// <exception cref="ArrayTypeMismatchException">
    /// </exception>
    protected override void SetAxisSize()
    {
      if (this.Data.Rank != 2)
      {
        throw new ArrayTypeMismatchException(Resources.ArrayNot2DException);
      }

      this.ySize = this.Data.GetLength(0);
      this.xSize = this.Data.GetLength(1);
    }
  }
}