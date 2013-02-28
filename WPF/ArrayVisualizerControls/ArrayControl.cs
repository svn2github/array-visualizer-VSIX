using System;
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

    public event EventHandler<CellClickEventArgs> CellClick;

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
    private Func<object, string, string> captionBuilder;
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
      this.captionBuilder = this.DefaultCaptionBuilder;
      this.tooltipPrefix = string.Empty;
      this.LeftBracket = '[';
      this.RightBracket = ']';
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
      set { this.formatter = value; }
    }

    public Func<object, string, string> CaptionBuilder
    {
      get { return this.captionBuilder; }
      set
      {
        if (value == null)
          this.captionBuilder = this.DefaultCaptionBuilder;
        else
          this.captionBuilder = value;
      }
    }

    public bool Truncated { get; protected set; }

    #endregion

    #region Properties

    protected int DimA { get; set; }
    protected int DimX { get; set; }
    protected int DimY { get; set; }
    protected int DimZ { get; set; }

    public char LeftBracket { get; set; }
    public char RightBracket { get; set; }

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

    protected Label AddLabel(ArrayRenderSection section, string toolTipCoords, double x, double y, object data)
    {
      Type dataType = data.GetType();
      Label label = new Label();
      switch (section)
      {
        case ArrayRenderSection.Front:
          label.Margin = new Thickness(x, y, 0, 0);
          break;
        case ArrayRenderSection.Top:
          label.Margin = new Thickness(x + 1, y - 1, 0, 0);
          label.RenderTransform = this.topTransformer;
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

      if (dataType.IsArray)
        label.Content = this.ArrayCaptionBuilder((Array)data);
      else
        label.Content = this.captionBuilder(data, this.formatter);

      label.ToolTip = string.Format("{0}{1} : {2}", this.tooltipPrefix, toolTipCoords, label.Content);

      if (!dataType.IsPrimitive && dataType != typeof(string))
      {
        label.Tag = data;
        label.Cursor = Cursors.Hand;
        label.MouseUp += new MouseButtonEventHandler(label_MouseUp);
      }

      arrayGrid.Children.Add(label);

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

    private void InitPopup(Color backgroundColor)
    {
      this.popup = new Popup();
      this.popup.Placement = PlacementMode.MousePoint;
      this.popup.StaysOpen = false;
      Grid popupGrid = new Grid();

      popupGrid.Background = new SolidColorBrush(backgroundColor);
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

    public void ShowArrayPopup(UIElement target, Array data, string tooltipPrefix, Color backgroundColor)
    {
      if (this.popup == null)
        this.InitPopup(backgroundColor);

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

      arrCtl.LeftBracket = this.LeftBracket;
      arrCtl.RightBracket = this.RightBracket;

      arrCtl.CaptionBuilder = this.captionBuilder;
      arrCtl.CellClick = this.CellClick;
      arrCtl.Formatter = this.formatter;
      arrCtl.CellHeight = this.CellHeight;
      arrCtl.CellWidth = this.CellWidth;
      arrCtl.SetControlData(data, tooltipPrefix);

      arrCtl.Padding = new Thickness(8);
      arrCtl.Width += 16;
      arrCtl.Height += 16;

      this.arrayContainer.Content = arrCtl;
      this.popup.PlacementTarget = target;
      this.popup.IsOpen = true;

    }

    private void label_MouseUp(object sender, MouseButtonEventArgs e)
    {
      OnCellClick(sender, e);
    }

    private void OnCellClick(object sender, RoutedEventArgs e)
    {
      FrameworkElement fe = (FrameworkElement)sender;
      string toolTip = "";

      if (fe.ToolTip != null)
      {
        toolTip = (string)fe.ToolTip;
        int pos = toolTip.IndexOf(":");
        if (pos > 0)
          toolTip = toolTip.Substring(0, pos - 1);
      }

      if (CellClick == null)
      {
        FrameworkElement element = fe;
        if (fe.Tag.GetType().IsArray)
        {
          Array data = (Array)fe.Tag;
          object depObj = null;
          while (depObj == null)
          {
            element = (FrameworkElement)element.Parent;
            depObj = element.GetValue(Control.BackgroundProperty);
          }
          Color color = ((SolidColorBrush)depObj).Color;
          ShowArrayPopup((UIElement)sender, data, toolTip, color);
        }
      }
      else
        CellClick(this, new CellClickEventArgs(fe.Tag, toolTip, e.RoutedEvent, sender));
    }

    #endregion

    private string DefaultCaptionBuilder(object data, string formatter)
    {
      double number;
      string text = (data ?? "").ToString();
      if (double.TryParse(text, out number))
        text = number.ToString(formatter, System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat);
      return text;
    }

    private string ArrayCaptionBuilder(Array data)
    {
      int[] dims = data.GetDimensions();
      string dimsText = string.Join(", ", dims);
      string text = string.Format("{{{0}}}", data.GetType().Name);
      int pos1 = text.IndexOf(this.RightBracket);
      int pos2 = text.IndexOf(this.LeftBracket);
      text = text.Substring(0, pos1) + this.LeftBracket + dimsText + this.RightBracket + text.Substring(pos2 + 1);
      return text;
    }
  }
}


