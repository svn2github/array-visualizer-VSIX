using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System;

namespace ArrayVisualizerControls
{
  public abstract class ArrayControl : UserControl
  {
    #region Local Fields

    private Grid arrayGrid;
    private System.Array data;
    private Size cellSize;
    private string formatter;

    #endregion

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

    #endregion

    public void Render()
    {
      arrayGrid.Children.Clear();
      RenderBlankGrid();
      DrawContent();
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

    static System.DateTime lastStop = System.DateTime.Now;
    protected void AddLabel(ArrayRenderSection section, string text, double x, double y)
    {
      //if (lastStop.AddSeconds(1) < System.DateTime.Now)
      //{
      // // System.Diagnostics.Debugger.Break();
      //}
      lastStop = System.DateTime.Now;
      Label label = new Label();


      switch (section)
      {
        case ArrayRenderSection.Front:
          label.Margin = new Thickness(x, y, 0, 0);
          break;
        case ArrayRenderSection.Top:
          label.Margin = new Thickness(x + 1, y - 1, 0, 0);
          label.RenderTransform = GetTopTransformation();
          label.FontWeight = FontWeight.FromOpenTypeWeight(700);
          break;
        case ArrayRenderSection.Side:
          label.Margin = new Thickness(x, y, 0, 0);
          label.RenderTransform = GetSideTransformation();
          break;
        default:
          break;
      }

      label.Content = label.ToolTip = text;

      label.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
      label.VerticalAlignment = System.Windows.VerticalAlignment.Top;
      label.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
      label.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
      label.Width = this.cellSize.Width;
      label.Height = this.cellSize.Height;

      arrayGrid.Children.Add(label);
    }

    private Transform GetSideTransformation()
    {
      double angle = Math.Atan((this.CellHeight / this.CellWidth) * .75) * 180 / Math.PI;
      SkewTransform skt = new SkewTransform(0, -angle);
      ScaleTransform sct = new ScaleTransform(.75, 1, 0, 0);
      TransformGroup tg = new TransformGroup();
      tg.Children.Add(skt);
      tg.Children.Add(sct);
      return tg;
    }


    private Transform GetTopTransformation()
    {
      double angle = Math.Atan((this.CellWidth / this.CellHeight) * .75) * 180 / Math.PI;
      SkewTransform skt = new SkewTransform(-angle, 0);
      ScaleTransform sct = new ScaleTransform(1, .75, 0, 0);
      TransformGroup tg = new TransformGroup();
      tg.Children.Add(skt);
      tg.Children.Add(sct);
      return tg;
    }

    protected abstract void RenderBlankGrid();
    protected abstract void DrawContent();
    protected abstract void SetAxisSize();
  }
}
