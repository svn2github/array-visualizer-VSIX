using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System;
using System.Windows.Input;

namespace ArrayVisualizerControls
{
  public abstract class ArrayControl : UserControl
  {
    #region Local Fields

    public const double SIZE_FACTOR_3D = .75;
    public const double MAX_ELEMENTS = 10000;

    private Grid arrayGrid;
    private System.Array data;
    private Size cellSize;
    private string formatter;

    #endregion
    private bool truncated;

    protected ArrayControl()
    {
      arrayGrid = new Grid();
      base.AddChild(arrayGrid);
      this.cellSize = new Size(80, 55);
      this.formatter = "";
    }

    #region Properties

    public System.Array Data
    {
      get { return data; }
      set
      {
        this.data = value;
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

    #endregion

    public void Render()
    {
      //Cursor cur = this.Cursor;
      try
      {
        if (this.data.Length > 500)
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

    //static System.DateTime lastStop = System.DateTime.Now;
    private Transform topTransformer;
    private Transform sideTransformer;

    protected void AddLabel(ArrayRenderSection section, string text, string toolTip, double x, double y)
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
      label.ToolTip = toolTip;

      arrayGrid.Children.Add(label);
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

    protected abstract void RenderBlankGrid();
    protected abstract void DrawContent();

    private void SetAxisSize()
    {
      int ranks = data.Rank;

      this.DimX = this.Data.GetLength(ranks - 1);
      this.DimY = this.Data.GetLength(ranks - 2);

      if (ranks > 2)
      {
        this.DimZ = this.Data.GetLength(ranks - 3);
        if (ranks > 3)
          this.DimA = this.Data.GetLength(ranks - 4);
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

    internal void SetTransformers()
    {
      topTransformer = GetTopTransformation();
      sideTransformer = GetSideTransformation();
    }
  }
}
