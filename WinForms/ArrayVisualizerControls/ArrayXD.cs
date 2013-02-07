// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayXD.cs" company="">
//   
// </copyright>
// <summary>
//   The array xd.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WinFormsArrayVisualizerControls
{
  using System;
  using System.Drawing;
  using System.Windows.Forms;

  /// <summary>
  /// The array xd.
  /// </summary>
  public abstract class ArrayXD : PictureBox
  {
    #region Fields

    /// <summary>
    /// The cell size.
    /// </summary>
    private Size cellSize;

    /// <summary>
    /// The data.
    /// </summary>
    private Array data;

    /// <summary>
    /// The font.
    /// </summary>
    private Font font;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ArrayXD"/> class.
    /// </summary>
    protected ArrayXD()
    {
      this.cellSize = new Size(80, 55);
      this.CellPadding = 10;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets or sets the cell height.
    /// </summary>
    public int CellHeight
    {
      get
      {
        return this.cellSize.Height;
      }

      set
      {
        this.cellSize.Height = value;
      }
    }

    /// <summary>
    /// Gets or sets the cell padding.
    /// </summary>
    public int CellPadding { get; set; }

    /// <summary>
    /// Gets or sets the cell size.
    /// </summary>
    public Size CellSize
    {
      get
      {
        return this.cellSize;
      }

      set
      {
        this.cellSize = value;
      }
    }

    /// <summary>
    /// Gets or sets the cell width.
    /// </summary>
    public int CellWidth
    {
      get
      {
        return this.cellSize.Width;
      }

      set
      {
        this.cellSize.Width = value;
      }
    }

    /// <summary>
    /// Gets or sets the data.
    /// </summary>
    public Array Data
    {
      get
      {
        return this.data;
      }

      set
      {
        this.data = value;
        if (value == null)
        {
          this.Refresh();
        }
        else
        {
          this.SetAxisSize();
          this.Render();
        }
      }
    }

    /// <summary>
    /// Gets or sets the formatter.
    /// </summary>
    public string Formatter { get; set; }

    /// <summary>
    /// Gets or sets the render font.
    /// </summary>
    public Font RenderFont
    {
      get
      {
        if (this.font == null)
        {
          return base.Font;
        }
        else
        {
          return this.font;
        }
      }

      set
      {
        this.font = value;
        
      }
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The render.
    /// </summary>
    public void Render()
    {
      this.RenderBlankGrid();
      this.DrawContent();
    }

    #endregion

    #region Methods

    /// <summary>
    /// The draw content.
    /// </summary>
    protected abstract void DrawContent();

    /// <summary>
    /// The render blank grid.
    /// </summary>
    protected abstract void RenderBlankGrid();

    /// <summary>
    /// The set axis size.
    /// </summary>
    protected abstract void SetAxisSize();

    #endregion
  }

#if DEBUG

  /// <summary>
  /// The array proxy.
  /// </summary>
  public class ArrayProxy : ArrayXD
  {
    #region Methods

    /// <summary>
    /// The draw content.
    /// </summary>
    /// <exception cref="NotImplementedException">
    /// </exception>
    protected override void DrawContent()
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// The render blank grid.
    /// </summary>
    /// <exception cref="NotImplementedException">
    /// </exception>
    protected override void RenderBlankGrid()
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// The set axis size.
    /// </summary>
    /// <exception cref="NotImplementedException">
    /// </exception>
    protected override void SetAxisSize()
    {
      throw new NotImplementedException();
    }

    #endregion
  }
#endif
}