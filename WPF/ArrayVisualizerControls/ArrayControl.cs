// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayControl.cs" company="">
//   
// </copyright>
// <summary>
//   The array control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ArrayVisualizerControls
{
  using System;
  using System.Threading;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Controls.Primitives;
  using System.Windows.Input;
  using System.Windows.Media;
  using System.Windows.Shapes;

  using LinqLib.Array;

  /// <summary>
  /// The array control.
  /// </summary>
  public abstract class ArrayControl : UserControl
  {
    #region Constants

    /// <summary>
    /// The ma x_ elements.
    /// </summary>
    public const double MAX_ELEMENTS = 10000;

    /// <summary>
    /// The siz e_ facto r_3 d.
    /// </summary>
    public const double SIZE_FACTOR_3D = .75;

    #endregion

    #region Fields

    /// <summary>
    /// The array grid.
    /// </summary>
    private readonly Grid arrayGrid;

    /// <summary>
    /// The array container.
    /// </summary>
    private ScrollViewer arrayContainer;

    /// <summary>
    /// The cell size.
    /// </summary>
    private Size cellSize;

    /// <summary>
    /// The control data.
    /// </summary>
    private Array controlData;

    /// <summary>
    /// The formatter.
    /// </summary>
    private string formatter;

    /// <summary>
    /// The popup.
    /// </summary>
    private Popup popup;

    /// <summary>
    /// The side transformer.
    /// </summary>
    private Transform sideTransformer;

    /// <summary>
    /// The tooltip prefix.
    /// </summary>
    private string tooltipPrefix;

    /// <summary>
    /// The top transformer.
    /// </summary>
    private Transform topTransformer;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ArrayControl"/> class.
    /// </summary>
    protected ArrayControl()
    {
      this.arrayGrid = new Grid();
      this.AddChild(this.arrayGrid);
      this.cellSize = new Size(80, 55);
      this.formatter = string.Empty;
      this.tooltipPrefix = string.Empty;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets or sets the cell height.
    /// </summary>
    public double CellHeight
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
    public double CellWidth
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
    /// Gets the child array.
    /// </summary>
    public ArrayControl ChildArray
    {
      get
      {
        if (this.arrayContainer != null)
        {
          return (ArrayControl)this.arrayContainer.Content;
        }
        else
        {
          return null;
        }
      }
    }

    /// <summary>
    /// Gets the data.
    /// </summary>
    public Array Data
    {
      get
      {
        return this.controlData;
      }
    }

    /// <summary>
    /// Gets the elements count.
    /// </summary>
    public int ElementsCount
    {
      get
      {
        return this.DimX * this.DimY * this.DimZ * this.DimA;
      }
    }

    /// <summary>
    /// Gets or sets the formatter.
    /// </summary>
    public string Formatter
    {
      get
      {
        return this.formatter;
      }

      set
      {
        this.formatter = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether truncated.
    /// </summary>
    public bool Truncated { get; protected set; }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the dim a.
    /// </summary>
    protected int DimA { get; set; }

    /// <summary>
    /// Gets or sets the dim x.
    /// </summary>
    protected int DimX { get; set; }

    /// <summary>
    /// Gets or sets the dim y.
    /// </summary>
    protected int DimY { get; set; }

    /// <summary>
    /// Gets or sets the dim z.
    /// </summary>
    protected int DimZ { get; set; }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The render.
    /// </summary>
    public void Render()
    {
      try
      {
        if (this.controlData.Length > 500)
        {
          Mouse.OverrideCursor = Cursors.Wait;
        }

        this.arrayGrid.Children.Clear();
        this.RenderBlankGrid();
        this.DrawContent();
      }
      catch (Exception)
      {
        throw;
      }
      finally
      {
        Mouse.OverrideCursor = null;
      }
    }

    /// <summary>
    /// The set control data.
    /// </summary>
    /// <param name="data">
    /// The data.
    /// </param>
    public void SetControlData(Array data)
    {
      this.SetControlData(data, string.Empty);
    }

    /// <summary>
    /// The set control data.
    /// </summary>
    /// <param name="data">
    /// The data.
    /// </param>
    /// <param name="tooltipPrefix">
    /// The tooltip prefix.
    /// </param>
    public void SetControlData(Array data, string tooltipPrefix)
    {
      this.controlData = data;
      this.tooltipPrefix = tooltipPrefix;
      if (data == null)
      {
        this.arrayGrid.Children.Clear();
      }
      else
      {
        this.SetAxisSize();
        this.Render();
      }
    }

    #endregion

    #region Methods

    /// <summary>
    /// The set transformers.
    /// </summary>
    internal void SetTransformers()
    {
      this.topTransformer = this.GetTopTransformation();
      this.sideTransformer = this.GetSideTransformation();
    }

    /// <summary>
    /// The add label.
    /// </summary>
    /// <param name="section">
    /// The section.
    /// </param>
    /// <param name="toolTipCoords">
    /// The tool tip coords.
    /// </param>
    /// <param name="x">
    /// The x.
    /// </param>
    /// <param name="y">
    /// The y.
    /// </param>
    /// <param name="data">
    /// The data.
    /// </param>
    protected void AddLabel(ArrayRenderSection section, string toolTipCoords, double x, double y, Array data)
    {
      int[] dims = data.GetDimensions();
      string dimsText = string.Join(", ", dims);
      string text = string.Format("{{{0}}}", data.GetType().Name);
      int pos1 = text.IndexOf("[");
      int pos2 = text.IndexOf("]");
      text = text.Substring(0, pos1) + "[" + dimsText + "]" + text.Substring(pos2 + 1);

      Label label = AddLabel(section, string.Empty, x, y, text);

      label.ToolTip = string.Format("{0}{1} : {2}\r\nClick to zoom in.", this.tooltipPrefix, toolTipCoords, text);

      label.Tag = data;
      label.Cursor = Cursors.Hand;
      label.MouseUp += this.label_MouseUp;
    }

    /// <summary>
    /// The add label.
    /// </summary>
    /// <param name="section">
    /// The section.
    /// </param>
    /// <param name="toolTipCoords">
    /// The tool tip coords.
    /// </param>
    /// <param name="x">
    /// The x.
    /// </param>
    /// <param name="y">
    /// The y.
    /// </param>
    /// <param name="text">
    /// The text.
    /// </param>
    /// <returns>
    /// The <see cref="Label"/>.
    /// </returns>
    protected Label AddLabel(ArrayRenderSection section, string toolTipCoords, double x, double y, string text)
    {
      var label = new Label();
      switch (section)
      {
        case ArrayRenderSection.Front:
          label.Margin = new Thickness(x, y, 0, 0);
          break;
        case ArrayRenderSection.Top:
          label.Margin = new Thickness(x + 1, y - 1, 0, 0);
          label.RenderTransform = this.topTransformer;
          label.FontWeight = FontWeight.FromOpenTypeWeight(700);
          break;
        case ArrayRenderSection.Side:
          label.Margin = new Thickness(x, y, 0, 0);
          label.RenderTransform = this.sideTransformer;
          break;
        default:
          break;
      }

      label.Width = this.cellSize.Width;
      label.Height = this.cellSize.Height;

      label.HorizontalAlignment = HorizontalAlignment.Left;
      label.VerticalAlignment = VerticalAlignment.Top;
      label.HorizontalContentAlignment = HorizontalAlignment.Center;
      label.VerticalContentAlignment = VerticalAlignment.Center;

      label.Content = text;
      label.ToolTip = string.Format("{0}{1} : {2}", this.tooltipPrefix, toolTipCoords, text);

      this.arrayGrid.Children.Add(label);

      return label;
    }

    /// <summary>
    /// The add line.
    /// </summary>
    /// <param name="x1">
    /// The x 1.
    /// </param>
    /// <param name="y1">
    /// The y 1.
    /// </param>
    /// <param name="x2">
    /// The x 2.
    /// </param>
    /// <param name="y2">
    /// The y 2.
    /// </param>
    protected void AddLine(double x1, double y1, double x2, double y2)
    {
      var line = new Line();
      line.Stroke = Brushes.Black;
      line.StrokeThickness = 1;
      line.X1 = x1;
      line.X2 = x2;
      line.Y1 = y1;
      line.Y2 = y2;
      this.arrayGrid.Children.Add(line);
    }

    /// <summary>
    /// The draw content.
    /// </summary>
    protected abstract void DrawContent();

    /// <summary>
    /// The get side transformation.
    /// </summary>
    /// <returns>
    /// The <see cref="Transform"/>.
    /// </returns>
    protected Transform GetSideTransformation()
    {
      double angle = Math.Atan((this.CellHeight / this.CellWidth) * SIZE_FACTOR_3D) * 180 / Math.PI;
      var skt = new SkewTransform(0, -angle);
      var sct = new ScaleTransform(SIZE_FACTOR_3D, 1, 0, 0);
      var tg = new TransformGroup();
      tg.Children.Add(skt);
      tg.Children.Add(sct);
      return tg;
    }

    /// <summary>
    /// The get text.
    /// </summary>
    /// <param name="data">
    /// The data.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    protected string GetText(object data)
    {
      double number;
      string text = (data ?? string.Empty).ToString();
      if (double.TryParse(text, out number))
      {
        text = number.ToString(this.Formatter, Thread.CurrentThread.CurrentUICulture.NumberFormat);
      }

      return text;
    }

    /// <summary>
    /// The get top transformation.
    /// </summary>
    /// <returns>
    /// The <see cref="Transform"/>.
    /// </returns>
    protected Transform GetTopTransformation()
    {
      double angle = Math.Atan((this.CellWidth / this.CellHeight) * SIZE_FACTOR_3D) * 180 / Math.PI;
      var skt = new SkewTransform(-angle, 0);
      var sct = new ScaleTransform(1, SIZE_FACTOR_3D, 0, 0);
      var tg = new TransformGroup();
      tg.Children.Add(skt);
      tg.Children.Add(sct);
      return tg;
    }

    /// <summary>
    /// The render blank grid.
    /// </summary>
    protected abstract void RenderBlankGrid();

    /// <summary>
    /// The adjust dimension size.
    /// </summary>
    /// <param name="originalSize">
    /// The original size.
    /// </param>
    /// <param name="ratio">
    /// The ratio.
    /// </param>
    /// <returns>
    /// The <see cref="int"/>.
    /// </returns>
    private static int AdjustDimensionSize(int originalSize, double ratio)
    {
      var size = (int)(originalSize / ratio + .5);
      return Math.Max(1, size);
    }

    /// <summary>
    /// The hide self and children.
    /// </summary>
    private void HideSelfAndChildren()
    {
      if (this.popup != null)
      {
        this.popup.IsOpen = false;
        ArrayControl child = this.ChildArray;
        if (child != null)
        {
          child.HideSelfAndChildren();
        }
      }
    }

    /// <summary>
    /// The init popup.
    /// </summary>
    private void InitPopup()
    {
      this.popup = new Popup();
      this.popup.Placement = PlacementMode.MousePoint;
      this.popup.StaysOpen = false;
      var popupGrid = new Grid();

      popupGrid.Background = new SolidColorBrush(Colors.SkyBlue);
      this.popup.Child = popupGrid;
      popupGrid.Children.Add(
        new Border { BorderBrush = new SolidColorBrush(Colors.Black), BorderThickness = new Thickness(.25) });

      this.arrayContainer = new ScrollViewer
                              {
                                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto, 
                                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
                              };
      popupGrid.Children.Add(this.arrayContainer);

      this.popup.MaxWidth = SystemParameters.PrimaryScreenWidth * .85;
      this.popup.MaxHeight = SystemParameters.PrimaryScreenHeight * .85;
    }

    /// <summary>
    /// The set axis size.
    /// </summary>
    private void SetAxisSize()
    {
      int ranks = this.controlData.Rank;

      this.DimX = this.DimY = this.DimZ = this.DimA = 1;

      this.DimX = this.controlData.GetLength(ranks - 1);
      if (ranks > 1)
      {
        this.DimY = this.controlData.GetLength(ranks - 2);
        if (ranks > 2)
        {
          this.DimZ = this.controlData.GetLength(ranks - 3);
          if (ranks > 3)
          {
            this.DimA = this.controlData.GetLength(ranks - 4);
          }
        }
      }

      this.Truncated = this.controlData.Length > MAX_ELEMENTS;

      if (this.Truncated)
      {
        double r = Math.Pow(this.controlData.Length / MAX_ELEMENTS, 1.0 / ranks);
        this.DimA = AdjustDimensionSize(this.DimA, r);
        this.DimZ = AdjustDimensionSize(this.DimZ, r);
        this.DimY = AdjustDimensionSize(this.DimY, r);
        this.DimX = AdjustDimensionSize(this.DimX, r);
      }
    }

    /// <summary>
    /// The show array popup.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="data">
    /// The data.
    /// </param>
    /// <param name="tooltipPrefix">
    /// The tooltip prefix.
    /// </param>
    private void ShowArrayPopup(object sender, Array data, string tooltipPrefix)
    {
      if (this.popup == null)
      {
        this.InitPopup();
      }

      this.HideSelfAndChildren();

      ArrayControl arrCtl;
      switch (data.Rank)
      {
        case 1:
          arrCtl = new Array1D();
          break;
        case 2:
          arrCtl = new Array2D();
          break;
        case 3:
          arrCtl = new Array3D();
          break;
        case 4:
          arrCtl = new Array4D();
          break;
        default:
          return;
      }

      arrCtl.Formatter = this.formatter;
      arrCtl.CellHeight = this.CellHeight;
      arrCtl.CellWidth = this.CellWidth;
      arrCtl.SetControlData(data, tooltipPrefix);

      arrCtl.Padding = new Thickness(8);
      arrCtl.Width += 16;
      arrCtl.Height += 16;

      this.arrayContainer.Content = arrCtl;
      this.popup.PlacementTarget = (UIElement)sender;
      this.popup.IsOpen = true;
    }

    /// <summary>
    /// The label_ mouse up.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    private void label_MouseUp(object sender, MouseButtonEventArgs e)
    {
      var fe = (FrameworkElement)sender;
      var data = (Array)fe.Tag;

      string toolTip = string.Empty;

      if (fe.ToolTip != null)
      {
        toolTip = (string)fe.ToolTip;
        int pos = toolTip.IndexOf(":");
        if (pos > 0)
        {
          toolTip = toolTip.Substring(0, pos - 1);
        }
      }

      this.ShowArrayPopup(sender, data, toolTip);
    }

    #endregion
  }
}