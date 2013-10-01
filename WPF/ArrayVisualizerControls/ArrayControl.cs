using System;
using System.Globalization;
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

    public const double SIZE_FACTOR_3_D = .75;

    #endregion

    #region Fields

    private readonly Grid arrayGrid;
    private ScrollViewer arrayContainer;
    private Size cellSize;
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
      arrayGrid = new Grid();
      Init();
    }

    private void Init()
    {
      AddChild(arrayGrid);
      cellSize = new Size(80, 55);
      formatter = string.Empty;
      captionBuilder = DefaultCaptionBuilder;
      tooltipPrefix = string.Empty;
      LeftBracket = '[';
      RightBracket = ']';
    }

    #endregion

    #region Public Properties

    public double CellHeight
    {
      get { return cellSize.Height; }
      set { cellSize.Height = value; }
    }

    public Size CellSize
    {
      get { return cellSize; }
      set { cellSize = value; }
    }

    public double CellWidth
    {
      get { return cellSize.Width; }
      set { cellSize.Width = value; }
    }

    public ArrayControl ChildArray
    {
      get
      {
        if (arrayContainer != null)
          return (ArrayControl)arrayContainer.Content;

        return null;
      }
    }

    public Array Data { get; private set; }

    public int ElementsCount
    {
      get { return DimX * DimY * DimZ * DimA; }
    }

    public string Formatter
    {
      get { return formatter; }
      set { formatter = value; }
    }

    public Func<object, string, string> CaptionBuilder
    {
      get { return captionBuilder; }
      set
      {
        captionBuilder = value ?? DefaultCaptionBuilder;
      }
    }

    public bool Truncated { get; protected set; }

    #endregion

    #region Properties

    protected int DimX { get; set; }
    protected int DimY { get; set; }
    protected int DimZ { get; set; }
    protected int DimA { get; set; }

    public char LeftBracket { get; set; }
    public char RightBracket { get; set; }

    #endregion

    #region Public Methods and Operators


    public void Render()
    {
      try
      {
        if (Data.Length > 500)
          Mouse.OverrideCursor = Cursors.Wait;

        arrayGrid.Children.Clear();
        RenderBlankGrid();
        DrawContent();
      }
      finally
      {
        Mouse.OverrideCursor = null;
      }
    }

    public void SetControlData(Array data)
    {
      SetControlData(data, string.Empty);
    }

    public void SetControlData(Array data, string popupTooltipPrefix)
    {
      Data = data;
      tooltipPrefix = popupTooltipPrefix;
      if (data == null)
        arrayGrid.Children.Clear();
      else
      {
        SetAxisSize();
        Render();
      }
    }

    #endregion

    #region Methods

    internal void SetTransformers()
    {
      topTransformer = GetTopTransformation();
      sideTransformer = GetSideTransformation();
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
          label.RenderTransform = topTransformer;
          break;
        case ArrayRenderSection.Side:
          label.Margin = new Thickness(x, y, 0, 0);
          label.RenderTransform = sideTransformer;
          break;
      }

      label.Width = cellSize.Width;
      label.Height = cellSize.Height;

      label.HorizontalAlignment = HorizontalAlignment.Left;
      label.VerticalAlignment = VerticalAlignment.Top;
      label.HorizontalContentAlignment = HorizontalAlignment.Center;
      label.VerticalContentAlignment = VerticalAlignment.Center;

      if (dataType.IsArray)
        label.Content = ArrayCaptionBuilder((Array)data);
      else
        label.Content = captionBuilder(data, formatter);

      label.ToolTip = string.Format("{0}{1} : {2}", tooltipPrefix, toolTipCoords, label.Content);

      if (!dataType.IsPrimitive && dataType != typeof(string))
      {
        label.Tag = data;
        label.Cursor = Cursors.Hand;
        label.MouseUp += label_MouseUp;
      }

      arrayGrid.Children.Add(label);

      return label;
    }

    protected void AddLine(double x1, double y1, double x2, double y2)
    {
      var line = new Line
      {
        Stroke = Brushes.Black,
        StrokeThickness = 1,
        X1 = x1,
        X2 = x2,
        Y1 = y1,
        Y2 = y2
      };
      arrayGrid.Children.Add(line);
    }

    protected abstract void DrawContent();

    protected Transform GetSideTransformation()
    {
      double angle = Math.Atan((CellHeight / CellWidth) * SIZE_FACTOR_3_D) * 180 / Math.PI;
      var skt = new SkewTransform(0, -angle);
      var sct = new ScaleTransform(SIZE_FACTOR_3_D, 1, 0, 0);
      var tg = new TransformGroup();
      tg.Children.Add(skt);
      tg.Children.Add(sct);
      return tg;
    }

    protected Transform GetTopTransformation()
    {
      double angle = Math.Atan((CellWidth / CellHeight) * SIZE_FACTOR_3_D) * 180 / Math.PI;
      var skt = new SkewTransform(-angle, 0);
      var sct = new ScaleTransform(1, SIZE_FACTOR_3_D, 0, 0);
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
      if (popup != null)
      {
        popup.IsOpen = false;
        ArrayControl child = ChildArray;
        if (child != null)
          child.HideSelfAndChildren();
      }
    }

    private void InitPopup(Color backgroundColor)
    {
      popup = new Popup
      {
        Placement = PlacementMode.MousePoint,
        StaysOpen = false
      };
      Grid popupGrid = new Grid
      {
        Background = new SolidColorBrush(backgroundColor)
      };

      popup.Child = popupGrid;
      popupGrid.Children.Add(new Border { BorderBrush = new SolidColorBrush(Colors.Black), BorderThickness = new Thickness(.25) });

      arrayContainer = new ScrollViewer
                              {
                                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
                              };
      popupGrid.Children.Add(arrayContainer);

      popup.MaxWidth = SystemParameters.PrimaryScreenWidth * .85;
      popup.MaxHeight = SystemParameters.PrimaryScreenHeight * .85;
    }

    private void SetAxisSize() //!!!!!
    {
      int ranks = Data.Rank;

      DimX = DimY = DimZ = DimA = 1;

      DimX = Data.GetLength(ranks - 1);
      if (ranks > 1)
      {
        DimY = Data.GetLength(ranks - 2);
        if (ranks > 2)
        {
          DimZ = Data.GetLength(ranks - 3);
          if (ranks > 3)
            DimA = Data.GetLength(ranks - 4);
        }
      }

      Truncated = Data.Length > MAX_ELEMENTS;

      if (Truncated)
      {
        double r = Math.Pow(Data.Length / MAX_ELEMENTS, 1.0 / ranks);
        DimA = AdjustDimensionSize(DimA, r);
        DimZ = AdjustDimensionSize(DimZ, r);
        DimY = AdjustDimensionSize(DimY, r);
        DimX = AdjustDimensionSize(DimX, r);
      }
    }

    public void ShowArrayPopup(UIElement target, Array data, string popupTooltipPrefix, Color backgroundColor)
    {
      if (popup == null)
        InitPopup(backgroundColor);

      HideSelfAndChildren();

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

      arrCtl.LeftBracket = LeftBracket;
      arrCtl.RightBracket = RightBracket;

      arrCtl.CaptionBuilder = captionBuilder;
      arrCtl.CellClick = CellClick;
      arrCtl.Formatter = formatter;
      arrCtl.CellHeight = CellHeight;
      arrCtl.CellWidth = CellWidth;
      arrCtl.SetControlData(data, popupTooltipPrefix);

      arrCtl.Padding = new Thickness(8);
      arrCtl.Width += 16;
      arrCtl.Height += 16;

      arrayContainer.Content = arrCtl;
      if (popup != null)
      {
        popup.PlacementTarget = target;
        popup.IsOpen = true;
      }
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
        int pos = toolTip.IndexOf(":", StringComparison.Ordinal);
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
            depObj = element.GetValue(BackgroundProperty);
          }
          Color color = ((SolidColorBrush)depObj).Color;
          ShowArrayPopup((UIElement)sender, data, toolTip, color);
        }
      }
      else
        CellClick(this, new CellClickEventArgs(fe.Tag, toolTip, e.RoutedEvent, sender));
    }

    #endregion

    private string DefaultCaptionBuilder(object data, string numberFormatter)
    {
      double number;
      string text = (data ?? "").ToString();
      if (double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out number))
        text = number.ToString(numberFormatter, CultureInfo.InvariantCulture);
      return text;
    }

    private string ArrayCaptionBuilder(Array data)
    {
      int[] dims = data.GetDimensions();
      string dimsText = string.Join(", ", dims);
      string text = string.Format("{{{0}}}", data.GetType().Name);
      int pos1 = text.IndexOf(RightBracket);
      int pos2 = text.IndexOf(LeftBracket);
      text = text.Substring(0, pos1) + LeftBracket + dimsText + RightBracket + text.Substring(pos2 + 1);
      return text;
    }
  }
}


