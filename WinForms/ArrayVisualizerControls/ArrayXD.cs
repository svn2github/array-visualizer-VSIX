using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsArrayVisualizerControls
{
  public abstract class ArrayXD : PictureBox
  {
    #region Fields

    private Size cellSize;
    private Array data;
    private Font font;

    #endregion

    #region Constructors and Destructors

    protected ArrayXD()
    {
      this.cellSize = new Size(80, 55);
      this.CellPadding = 10;
    }

    #endregion

    #region Public Properties

    public int CellHeight
    {
      get { return this.cellSize.Height; }
      set { this.cellSize.Height = value; }
    }

    public int CellPadding { get; set; }

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

    public Array Data
    {
      get { return this.data; }
      set
      {
        this.data = value;
        if (value == null)
          this.Refresh();
        else
        {
          this.SetAxisSize();
          this.Render();
        }
      }
    }

    public string Formatter { get; set; }

    public Font RenderFont
    {
      get
      {
        if (this.font == null)
          return base.Font;
        else
          return this.font;
      }
      set { this.font = value; }
    }

    #endregion

    #region Public Methods and Operators

    public void Render()
    {
      this.RenderBlankGrid();
      this.DrawContent();
    }

    #endregion

    #region Methods

    protected abstract void DrawContent();

    protected abstract void RenderBlankGrid();

    protected abstract void SetAxisSize();

    #endregion
  }

#if DEBUG

  public class ArrayProxy : ArrayXD
  {
    #region Methods

    protected override void DrawContent()
    {
      throw new NotImplementedException();
    }

    protected override void RenderBlankGrid()
    {
      throw new NotImplementedException();
    }

    protected override void SetAxisSize()
    {
      throw new NotImplementedException();
    }

    #endregion
  }
#endif
}