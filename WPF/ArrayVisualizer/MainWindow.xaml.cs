using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using LinqLib.Array;
using LinqLib.Sequence;
using ArrayVisualizerControls;
using System;

namespace ArrayVisualizer
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();

      for (int i = 1; i <= 12; i++)
      {
        xDimComboBox.Items.Add(i);
        yDimComboBox.Items.Add(i);
        zDimComboBox.Items.Add(i);
        aDimComboBox.Items.Add(i);

        xResizeComboBox.Items.Add(i);
        yResizeComboBox.Items.Add(i);
        zResizeComboBox.Items.Add(i);
        aResizeComboBox.Items.Add(i);
      }

      xDimComboBox.SelectedItem = 3;
      yDimComboBox.SelectedItem = 4;
      zDimComboBox.SelectedItem = 5;
      aDimComboBox.SelectedItem = 6;

      xResizeComboBox.SelectedItem = 5;
      yResizeComboBox.SelectedItem = 5;
      zResizeComboBox.SelectedItem = 5;
      aResizeComboBox.SelectedItem = 5;

    }

    private ArrayControl arrayCtl;
    private int dims;

    private void PrepairGrid(ArrayControl arrayControl)
    {
      if (arrayCtl != null)
        mainPanel.Children.Remove(arrayCtl);
      arrayCtl = arrayControl;
      arrayCtl.Margin = new Thickness(12, 12, 0, 0);
      arrayCtl.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
      arrayCtl.Width = 285;
      arrayCtl.Height = 251;
      arrayCtl.VerticalAlignment = System.Windows.VerticalAlignment.Top;      
      mainPanel.Children.Add(arrayCtl);
    }

    private void button2_Click(object sender, RoutedEventArgs e)
    {
      PrepairGrid(new Array3D());
      arrayCtl.Data = Enumerator.Generate<double>(1, 1, 120).ToArray(4, 5, 6);
    }

    private void button3_Click(object sender, RoutedEventArgs e)
    {
      PrepairGrid(new Array4D());
      arrayCtl.Data = Enumerator.Generate<double>(1, 1, 360).ToArray(3, 4, 5, 6);

    }

    private void renderButton_Click(object sender, RoutedEventArgs e)
    {
      int x = (int)xDimComboBox.SelectedItem;
      int y = (int)yDimComboBox.SelectedItem;
      int z = (int)zDimComboBox.SelectedItem;
      int a = (int)aDimComboBox.SelectedItem;

      switch (dims)
      {
        case 2:
          PrepairGrid(new Array2D());
          arrayCtl.Data = Enumerator.Generate<double>(1, 1, x * y).ToArray(y, x);
          break;
        case 3:
          PrepairGrid(new Array3D());
          arrayCtl.Data = Enumerator.Generate<double>(1, 1, x * y * z).ToArray(z, y, x);
          break;
        case 4:
          PrepairGrid(new Array4D());
          arrayCtl.Data = Enumerator.Generate<double>(1, 1, x * y * z * a).ToArray(a, z, y, x);
          break;
      }

      rotateGrid.IsEnabled = true;
      resizeGrid.IsEnabled = true;
    }

    private void dimenstionsTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      ArrangeFrames();
    }

    private void ArrangeFrames()
    {
      int temp = int.Parse((string)((TabItem)dimenstionsTab.SelectedItem).Tag);
      if (temp != dims)
      {
        dims = temp;

        this.angelComboBox.Items.Clear();
        this.axisComboBox.Items.Clear();

        this.angelComboBox.Items.Add(90);
        this.angelComboBox.Items.Add(180);
        this.angelComboBox.Items.Add(270);

        this.axisComboBox.Items.Add("X");
        this.axisComboBox.Items.Add("Y");
        this.axisComboBox.Items.Add("Z");


        if (dims >= 3)
        {
          zDimComboBox.Visibility = System.Windows.Visibility.Visible;
          z1Label.Visibility = System.Windows.Visibility.Visible;

          axisComboBox.Visibility = System.Windows.Visibility.Visible;
          axisLabel.Visibility = System.Windows.Visibility.Visible;

          zResizeComboBox.Visibility = System.Windows.Visibility.Visible;
          z2Label.Visibility = System.Windows.Visibility.Visible;
        }
        else //2
        {
          zDimComboBox.Visibility = System.Windows.Visibility.Hidden;
          z1Label.Visibility = System.Windows.Visibility.Hidden;

          axisComboBox.Visibility = System.Windows.Visibility.Hidden;
          axisLabel.Visibility = System.Windows.Visibility.Hidden;

          zResizeComboBox.Visibility = System.Windows.Visibility.Hidden;
          z2Label.Visibility = System.Windows.Visibility.Hidden;
        }

        if (dims >= 4)
        {
          this.angelComboBox.Items.Add(360);
          this.angelComboBox.Items.Add(450);

          this.axisComboBox.Items.Add("A");

          aDimComboBox.Visibility = System.Windows.Visibility.Visible;
          a1Label.Visibility = System.Windows.Visibility.Visible;

          aResizeComboBox.Visibility = System.Windows.Visibility.Visible;
          a2Label.Visibility = System.Windows.Visibility.Visible;
        }
        else //3 or 2
        {
          aDimComboBox.Visibility = System.Windows.Visibility.Hidden;
          a1Label.Visibility = System.Windows.Visibility.Hidden;

          aResizeComboBox.Visibility = System.Windows.Visibility.Hidden;
          a2Label.Visibility = System.Windows.Visibility.Hidden;
        }

        this.angelComboBox.SelectedItem = 90;
        this.axisComboBox.SelectedItem = "Z";

        rotateGrid.IsEnabled = false;
        resizeGrid.IsEnabled = false;
      }
    }

    private void rotateButton_Click(object sender, RoutedEventArgs e)
    {
      RotateAxis r = RotateAxis.RotateNone;
      int angle = (int)angelComboBox.SelectedItem;

      switch ((string)axisComboBox.SelectedItem)
      {
        case "X":
          r = RotateAxis.RotateX;
          break;
        case "Y":
          r = RotateAxis.RotateY;
          break;
        case "Z":
          r = RotateAxis.RotateZ;
          break;
        case "A":
          r = RotateAxis.RotateA;
          break;
      }

      switch (dims)
      {
        case 2:
          arrayCtl.Data = ((double[,])arrayCtl.Data).Rotate(angle);
          break;
        case 3:
          arrayCtl.Data = ((double[, ,])arrayCtl.Data).Rotate(r, angle);
          break;
        case 4:
          arrayCtl.Data = ((double[, , ,])arrayCtl.Data).Rotate(r, angle);
          break;
      }
    }

    private void resizeButton_Click(object sender, RoutedEventArgs e)
    {
      int x = (int)xResizeComboBox.SelectedItem;
      int y = (int)yResizeComboBox.SelectedItem;
      int z = (int)zResizeComboBox.SelectedItem;
      int a = (int)aResizeComboBox.SelectedItem;

      switch (dims)
      {
        case 2:
          arrayCtl.Data = ((double[,])arrayCtl.Data).Resize(y, x);
          break;
        case 3:
          arrayCtl.Data = ((double[, ,])arrayCtl.Data).Resize(z, y, x);
          break;
        case 4:
          arrayCtl.Data = ((double[, , ,])arrayCtl.Data).Resize(a, z, y, x);
          break;
        default:
          throw new ArrayTypeMismatchException();
      }

    }
  }
}
