using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace ArrayVisualizer
{
  public abstract class ArrayXD : PictureBox
  {
    protected System.Array data;
    protected Font font;

    protected Size cellSize;
    protected string formatter;
    protected int cellPadding;

    public ArrayXD()
    {
      this.cellSize = new Size(80, 55);
      this.cellPadding = 10;
    }

    #region Properties

    public System.Array Data
    {
      get { return data; }
      set
      {
        this.data = value;
        if (value == null)
          this.Refresh();
        else
        {
          SetAxisSize();
          Render();
        }
      }
    }

    public Font RederFont
    {
      get
      {
        if (this.font == null)
          return base.Font;
        else
          return this.font;
      }
      set
      {
        this.font = value; ;
      }
    }

    public Size CellSize
    {
      get { return this.cellSize; }
      set { this.cellSize = value; }
    }

    public int CellWidth
    {
      get { return this.cellSize.Width; }
      set { this.cellSize.Width = value; }
    }

    public int CellHeight
    {
      get { return this.cellSize.Height; }
      set { this.cellSize.Height = value; }
    }

    public string Formatter
    {
      get { return this.formatter; }
      set { this.formatter = value; }
    }

    public int CellPadding
    {
      get { return this.cellPadding; }
      set { this.cellPadding = value; }
    }

    #endregion

    public void Render()
    {
      RenderBlankGrid();
      DrawContent();
    }

    protected abstract void RenderBlankGrid();
    protected abstract void DrawContent();
    protected abstract void SetAxisSize();
  }

#if DEBUG
  public class ArrayProxy : ArrayXD
  {
    protected override void RenderBlankGrid()
    {
      throw new NotImplementedException();
    }

    protected override void DrawContent()
    {
      throw new NotImplementedException();
    }

    protected override void SetAxisSize()
    {
      throw new NotImplementedException();
    }
  }
#endif
}
