using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ArrayVisualizerControls;
using LinqLib.Array;
using LinqLib.Sequence;
using Microsoft.Win32;
using AvProp = ArrayVisualizer.Properties;

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

      for (int i = 1; i <= 100; i++)
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

      fillOptionsTabControlWidth = fillOptionsTabControl.Width;
    }

    #region Local Fields

    private ArrayControl arrayCtl;
    private int dims;
    private double fillOptionsTabControlWidth;

    #endregion

    #region Events

    private void renderButton_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        int x = (int)xDimComboBox.SelectedItem;
        int y = (int)yDimComboBox.SelectedItem;
        int z = (int)zDimComboBox.SelectedItem;
        int a = (int)aDimComboBox.SelectedItem;

        switch (dims)
        {
          case 2:
            PrepairGrid(new Array2D());
            arrayCtl.Data = Get2DArray(x, y);
            break;
          case 3:
            PrepairGrid(new Array3D());
            arrayCtl.Data = Get3DArray(x, y, z);
            break;
          case 4:
            PrepairGrid(new Array4D());
            arrayCtl.Data = Get4DArray(x, y, z, a);
            break;
        }

        rotateGrid.IsEnabled = true;
        resizeGrid.IsEnabled = true;
        saveButton.IsEnabled = true;
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, AvProp. Resources.Error, MessageBoxButton.OK,MessageBoxImage.Error);
      }

    }

    private void dimenstionsTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      ArrangeFrames();
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

    private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      TextBox textBox = sender as TextBox;
      if (textBox != null)
      {
        string text = textBox.Text + e.Text;
        double temp;        
        bool ok = double.TryParse(text, out temp);
        e.Handled = !ok;        
      }
    }

    private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Space)
        e.Handled = true;
    }

    private void manualItemsTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
      fillOptionsTabControl.Width = fillOptionsTabControlWidth;
    }

    private void manualItemsTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
      fillOptionsTabControlWidth = fillOptionsTabControl.Width;
      fillOptionsTabControl.Width = this.Width - 53;
    }

    private void openFileButton_Click(object sender, RoutedEventArgs e)
    {
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.Filter = "Text Files|*.txt|Csv Files|*.csv|All Files|*.*";
      ofd.Title = "Select Input File";

      bool? res = ofd.ShowDialog();
      if (res.HasValue && res.Value)
      {
        string name = ofd.FileName;
        fileLabel.Tag = name;
        fileLabel.Content = Path.GetFileName(name);
        fileLabel.ToolTip = name;
      }
    }

    private void saveButton_Click(object sender, RoutedEventArgs e)
    {
      SaveFileDialog sfd = new SaveFileDialog();
      sfd.Filter = "Text Files|*.txt|Csv Files|*.csv|All Files|*.*";
      sfd.Title = "Select Input File";

      bool? res = sfd.ShowDialog();
      if (res.HasValue && res.Value)
        SaveToFile(sfd.FileName);
    }

    #endregion

    #region Methods

    private void PrepairGrid(ArrayControl arrayControl)
    {
      if (arrayCtl != null)
        mainPanel.Children.Remove(arrayCtl);
      arrayCtl = arrayControl;
      arrayCtl.CellWidth = double.Parse(cellWidthTextBox.Text);
      arrayCtl.CellHeight = double.Parse(cellHeightTextBox.Text);
      arrayCtl.Formatter = formatterTextBox.Text;
      arrayCtl.Margin = new Thickness(12, 12, 0, 0);
      arrayCtl.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
      arrayCtl.Width = 285;
      arrayCtl.Height = 251;
      arrayCtl.VerticalAlignment = System.Windows.VerticalAlignment.Top;
      mainPanel.Children.Add(arrayCtl);
    }

    private Array Get2DArray(int x, int y)
    {
      if (autoFillTab.IsSelected)
        return Enumerator.Generate(double.Parse(startValueTextBox.Text), double.Parse(stepTextBox.Text), x * y).Select(V => (double)V).ToArray(y, x);
      else if (manualTab.IsSelected)
      {
        try
        {
          string[] items = GetManualItems();
          return items.Select(X => double.Parse(X, Thread.CurrentThread.CurrentUICulture.NumberFormat)).ToArray(y, x);
        }
        catch (Exception ex)
        {
          throw new FormatException(AvProp.Resources.InvalidInputFormat, ex);
        }
      }
      else //file
      {
        try
        {
          string[] items = GetFileItems();
          return items.Select(X => double.Parse(X, Thread.CurrentThread.CurrentUICulture.NumberFormat)).ToArray(y, x);
        }
        catch (FormatException ex)
        {
          throw new FormatException(AvProp.Resources.InvalidFileContent, ex);
        }
        catch
        {
          throw;
        }
      }
    }

    private Array Get3DArray(int x, int y, int z)
    {
      if (autoFillTab.IsSelected)
        return Enumerator.Generate(double.Parse(startValueTextBox.Text), double.Parse(stepTextBox.Text), x * y * z).Select(V => (double)V).ToArray(z, y, x);
      else if (manualTab.IsSelected)
      {
        try
        {
          string[] items = GetManualItems();
          return items.Select(X => double.Parse(X, Thread.CurrentThread.CurrentUICulture.NumberFormat)).ToArray(z, y, x);
        }
        catch (Exception ex)
        {
          throw new FormatException(AvProp.Resources.InvalidInputFormat, ex);
        }
      }
      else //file
      {
        try
        {
          string[] items = GetFileItems();
          return items.Select(X => double.Parse(X, Thread.CurrentThread.CurrentUICulture.NumberFormat)).ToArray(z, y, x);
        }
        catch (FormatException ex)
        {
          throw new FormatException(AvProp.Resources.InvalidFileContent, ex);
        }
        catch
        {
          throw;
        }
      }
    }

    private Array Get4DArray(int x, int y, int z, int a)
    {
      if (autoFillTab.IsSelected)
        return Enumerator.Generate(double.Parse(startValueTextBox.Text), double.Parse(stepTextBox.Text), x * y * z * a).Select(V => (double)V).ToArray(a, z, y, x);
      else if (manualTab.IsSelected)
      {
        try
        {
          string[] items = GetManualItems();
          return items.Select(X => double.Parse(X, Thread.CurrentThread.CurrentUICulture.NumberFormat)).ToArray(a, z, y, x);
        }
        catch
        {
          throw new FormatException(AvProp.Resources.InvalidInputFormat);
        }
      }
      else //file
      {
        try
        {
          string[] items = GetFileItems();
          return items.Select(X => double.Parse(X, Thread.CurrentThread.CurrentUICulture.NumberFormat)).ToArray(a, z, y, x);
        }
        catch (FormatException)
        {
          throw new FormatException(AvProp.Resources.InvalidFileContent);
        }
        catch
        {
          throw;
        }
      }
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

    private string[] GetManualItems()
    {
      return GetItems(manualItemsTextBox.Text);
    }

    private static string[] GetItems(string list)
    {
      list = list.Replace(" ", "");
      list = list.Replace('\r', ',');
      list = list.Replace('\n', ',');

      while (list.IndexOf(",,", StringComparison.CurrentCulture) != -1)
        list = list.Replace(",,", ",");

      list = list.Trim(',');

      return list.Split(new char[] { ',' });
    }

    private void SaveToFile(string fileName)
    {
      double[] values = arrayCtl.Data.AsEnumerable<double>().ToArray();
      string list = string.Join(",", values);

      File.WriteAllText(fileName, list);
    }

    private string[] GetFileItems()
    {
      string name = (string)fileLabel.Tag;
      string list = string.Join(",", File.ReadAllLines(name));
      return GetItems(list);
    }

    #endregion
  }
}
