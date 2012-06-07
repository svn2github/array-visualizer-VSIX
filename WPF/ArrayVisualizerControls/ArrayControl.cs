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
    #region Local Fields

    public const double SIZE_FACTOR_3D = .75;
    public const double MAX_ELEMENTS = 10000;

    private Grid arrayGrid;
    private System.Array controlData;
    private Size cellSize;
    private string formatter;
    private bool truncated;

    private Transform topTransformer;
    private Transform sideTransformer;

    Popup popup;
    ScrollViewer arrayContainer;

    public ArrayControl ChildArray
    {
      get
      {
        if (arrayContainer != null)
          return (ArrayControl)arrayContainer.Content;
        else
          return null;
      }
    }

    #endregion

    protected ArrayControl()
    {
      arrayGrid = new Grid();
      base.AddChild(arrayGrid);
      this.cellSize = new Size(80, 55);
      this.formatter = "";
    }

    private void InitPopup()
    {
      popup = new Popup();
      popup.Placement = PlacementMode.MousePoint;
      popup.StaysOpen = false;
      Grid popupGrid = new Grid();

      popupGrid.Background = new SolidColorBrush(Colors.SkyBlue);
      popup.Child = popupGrid;
      popupGrid.Children.Add(new Border() { BorderBrush = new SolidColorBrush(Colors.Black), BorderThickness = new Thickness(.25) });


      arrayContainer = new ScrollViewer { HorizontalScrollBarVisibility = ScrollBarVisibility.Auto, VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
      popupGrid.Children.Add(arrayContainer);

      popup.MaxWidth = SystemParameters.PrimaryScreenWidth * .85;
      popup.MaxHeight = SystemParameters.PrimaryScreenHeight * .85;

      // popup.MouseLeave += new MouseEventHandler(popup_MouseLeave);

      //popup.AllowsTransparency = true;
      //popup.

    }

    #region Properties

    public System.Array Data
    {
      get { return controlData; }
      set
      {
        this.controlData = value;
        if (value == null)
          arrayGrid.Children.Clear();
        else
        {
          SetAxisSize();
          Render();
        }
      }
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

    public double CellHeight
    {
      get { return this.cellSize.Height; }
      set { this.cellSize.Height = value; }
    }

    public string Formatter
    {
      get { return this.formatter; }
      set { this.formatter = value; }
    }

    public bool Truncated
    {
      get { return this.truncated; }
      protected set { this.truncated = value; }
    }

    protected int DimX { get; set; }
    protected int DimY { get; set; }
    protected int DimZ { get; set; }
    protected int DimA { get; set; }

    public int ElementsCount { get { return DimX * DimY * DimZ * DimA; } }

    #endregion

    protected abstract void RenderBlankGrid();
    protected abstract void DrawContent();

    public void Render()
    {
      try
      {
        if (this.controlData.Length > 500)
          Mouse.OverrideCursor = Cursors.Wait;
        arrayGrid.Children.Clear();
        RenderBlankGrid();
        DrawContent();
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

    protected void AddLine(double x1, double y1, double x2, double y2)
    {
      Line line = new Line();
      line.Stroke = System.Windows.Media.Brushes.Black;
      line.StrokeThickness = 1;
      line.X1 = x1;
      line.X2 = x2;
      line.Y1 = y1;
      line.Y2 = y2;
      arrayGrid.Children.Add(line);
    }

    protected string GetText(object data)
    {
      double number;
      string text = (data ?? "").ToString();
      if (double.TryParse(text, out number))
        text = number.ToString(this.Formatter, Thread.CurrentThread.CurrentUICulture.NumberFormat);

      return text;
    }

    protected void AddLabel(ArrayRenderSection section, string toolTipCoords, double x, double y, Array data)
    {
      int[] dims = data.GetDimensions();
      string dimsText = string.Join(", ", dims);
      string text = string.Format("{{{0}}}", data.GetType().Name);
      int pos1 = text.IndexOf("[");
      int pos2 = text.IndexOf("]");
      text = text.Substring(0, pos1) + "[" + dimsText + "]" + text.Substring(pos2 + 1);

      Label label = AddLabel(section, "", x, y, text);

      label.ToolTip = string.Format("{0} : {1}\r\nClick to zoom in.", toolTipCoords, text);

      label.Tag = data;
      label.Cursor = Cursors.Hand;
      label.MouseUp += new MouseButtonEventHandler(label_MouseUp);
    }

    protected Label AddLabel(ArrayRenderSection section, string toolTipCoords, double x, double y, string text)
    {
      Label label = new Label();
      switch (section)
      {
        case ArrayRenderSection.Front:
          label.Margin = new Thickness(x, y, 0, 0);
          break;
        case ArrayRenderSection.Top:
          label.Margin = new Thickness(x + 1, y - 1, 0, 0);
          label.RenderTransform = topTransformer;
          label.FontWeight = FontWeight.FromOpenTypeWeight(700);
          break;
        case ArrayRenderSection.Side:
          label.Margin = new Thickness(x, y, 0, 0);
          label.RenderTransform = sideTransformer;
          break;
        default:
          break;
      }

      label.Width = this.cellSize.Width;
      label.Height = this.cellSize.Height;

      label.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
      label.VerticalAlignment = System.Windows.VerticalAlignment.Top;
      label.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
      label.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;

      label.Content = text;
      label.ToolTip = string.Format("{0} : {1}", toolTipCoords, text);

      arrayGrid.Children.Add(label);

      return label;
    }

    protected Transform GetSideTransformation()
    {
      double angle = Math.Atan((this.CellHeight / this.CellWidth) * SIZE_FACTOR_3D) * 180 / Math.PI;
      SkewTransform skt = new SkewTransform(0, -angle);
      ScaleTransform sct = new ScaleTransform(SIZE_FACTOR_3D, 1, 0, 0);
      TransformGroup tg = new TransformGroup();
      tg.Children.Add(skt);
      tg.Children.Add(sct);
      return tg;
    }

    protected Transform GetTopTransformation()
    {
      double angle = Math.Atan((this.CellWidth / this.CellHeight) * SIZE_FACTOR_3D) * 180 / Math.PI;
      SkewTransform skt = new SkewTransform(-angle, 0);
      ScaleTransform sct = new ScaleTransform(1, SIZE_FACTOR_3D, 0, 0);
      TransformGroup tg = new TransformGroup();
      tg.Children.Add(skt);
      tg.Children.Add(sct);
      return tg;
    }

    internal void SetTransformers()
    {
      topTransformer = GetTopTransformation();
      sideTransformer = GetSideTransformation();
    }

    private void SetAxisSize()
    {
      int ranks = controlData.Rank;

      this.DimX = this.DimY = this.DimZ = this.DimA = 1;

      this.DimX = this.Data.GetLength(ranks - 1);
      if (ranks > 1)
      {
        this.DimY = this.Data.GetLength(ranks - 2);
        if (ranks > 2)
        {
          this.DimZ = this.Data.GetLength(ranks - 3);
          if (ranks > 3)
            this.DimA = this.Data.GetLength(ranks - 4);
        }
      }

      this.Truncated = this.Data.Length > MAX_ELEMENTS;

      if (this.Truncated)
      {
        double r = Math.Pow((double)this.Data.Length / MAX_ELEMENTS, 1.0 / ranks);
        this.DimA = AdjustDimensionSize(this.DimA, r);
        this.DimZ = AdjustDimensionSize(this.DimZ, r);
        this.DimY = AdjustDimensionSize(this.DimY, r);
        this.DimX = AdjustDimensionSize(this.DimX, r);
      }
    }

    private static int AdjustDimensionSize(int originalSize, double ratio)
    {
      int size = (int)(originalSize / ratio + .5);
      return Math.Max(1, size);
    }

    private void label_MouseUp(object sender, MouseButtonEventArgs e)
    {
      if (popup == null)
        InitPopup();

      HideSelfAndChildren();

      Array data = (Array)((FrameworkElement)sender).Tag;

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
      arrCtl.Data = data;

      arrCtl.Padding = new Thickness(8);
      arrCtl.Width += 16;
      arrCtl.Height += 16;

      arrayContainer.Content = arrCtl;
      popup.PlacementTarget = (UIElement)sender;
      popup.IsOpen = true;
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
  }
}

