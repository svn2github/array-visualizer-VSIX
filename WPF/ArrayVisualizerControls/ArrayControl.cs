using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

using LinqLib.Array;

namespace ArrayVisualizerControls
{
  public abstract class ArrayControl : UserControl
  {
    #region Constants

    public const double MAX_ELEMENTS = 10000;

    public const double SIZE_FACTOR_3D = .75;

    #endregion

    #region Fields

    private readonly Grid arrayGrid;
    private ScrollViewer arrayContainer;
    private Size cellSize;
    private Array controlData;
    private string formatter;
    private Popup popup;
    private Transform sideTransformer;
    private string tooltipPrefix;
    private Transform topTransformer;

    #endregion

    #region Constructors and Destructors

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

    public double CellHeight
    {
      get { return this.cellSize.Height; }
      set { this.cellSize.Height = value; }
    }

    public Size CellSize
    {
      get { return this.cellSize; }
      set { this.cellSize = value; }
    }

    public double CellWidth
    {
      get { return this.cellSize.Width; }
      set { this.cellSize.Width = value; }
    }

    public ArrayControl ChildArray
    {
      get
      {
        if (this.arrayContainer != null)
          return (ArrayControl)this.arrayContainer.Content;
        else
          return null;
      }
    }

    public Array Data
    {
      get { return this.controlData; }
    }

    public int ElementsCount
    {
      get { return this.DimX * this.DimY * this.DimZ * this.DimA; }
    }

    public string Formatter
    {
      get { return this.formatter; }

      set
      {
        this.formatter = value;
      }
    }

    public bool Truncated { get; protected set; }

    #endregion

    #region Properties

    protected int DimA { get; set; }
    protected int DimX { get; set; }
    protected int DimY { get; set; }
    protected int DimZ { get; set; }

    #endregion

    #region Public Methods and Operators


    public void Render()
    {
      try
      {
        if (this.controlData.Length > 500)
          Mouse.OverrideCursor = Cursors.Wait;

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

    public void SetControlData(Array data)
    {
      this.SetControlData(data, string.Empty);
    }

    public void SetControlData(Array data, string tooltipPrefix)
    {
      this.controlData = data;
      this.tooltipPrefix = tooltipPrefix;
      if (data == null)
        this.arrayGrid.Children.Clear();
      else
      {
        this.SetAxisSize();
        this.Render();
      }
    }

    #endregion

    #region Methods

    internal void SetTransformers()
    {
      this.topTransformer = this.GetTopTransformation();
      this.sideTransformer = this.GetSideTransformation();
    }

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

    protected abstract void DrawContent();

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

    protected abstract void RenderBlankGrid();

    private static int AdjustDimensionSize(int originalSize, double ratio)
    {
      var size = (int)(originalSize / ratio + .5);
      return Math.Max(1, size);
    }

    private void HideSelfAndChildren()
    {
      if (this.popup != null)
      {
        this.popup.IsOpen = false;
        ArrayControl child = this.ChildArray;
        if (child != null)
          child.HideSelfAndChildren();
      }
    }

    private void InitPopup()
    {
      this.popup = new Popup();
      this.popup.Placement = PlacementMode.MousePoint;
      this.popup.StaysOpen = false;
      var popupGrid = new Grid();

      popupGrid.Background = new SolidColorBrush(Colors.SkyBlue);
      this.popup.Child = popupGrid;
      popupGrid.Children.Add(new Border { BorderBrush = new SolidColorBrush(Colors.Black), BorderThickness = new Thickness(.25) });

      this.arrayContainer = new ScrollViewer
                              {
                                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
                              };
      popupGrid.Children.Add(this.arrayContainer);

      this.popup.MaxWidth = SystemParameters.PrimaryScreenWidth * .85;
      this.popup.MaxHeight = SystemParameters.PrimaryScreenHeight * .85;
    }

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
            this.DimA = this.controlData.GetLength(ranks - 4);
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

    private void ShowArrayPopup(object sender, Array data, string tooltipPrefix)
    {
      if (this.popup == null)
        this.InitPopup();

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
          toolTip = toolTip.Substring(0, pos - 1);
      }

      this.ShowArrayPopup(sender, data, toolTip);
    }

    #endregion
  }
}