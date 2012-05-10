using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

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

    protected void AddLabel(string text, double x, double y)
    {
      Label label = new Label();
      label.Content = label.ToolTip = text;
      label.Margin = new Thickness(x, y, 0, 0);
      arrayGrid.Children.Add(label);
    }

    protected abstract void RenderBlankGrid();
    protected abstract void DrawContent();
    protected abstract void SetAxisSize();
  }
}
