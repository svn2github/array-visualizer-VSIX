using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArrayVisualizerExt
{
  /// <summary>
  /// Interaction logic for LargeArrayHandler.xaml
  /// </summary>
  public partial class LargeArrayHandler : UserControl
  {
    public event EventHandler<RoutedEventArgs> LoadArrayRequest;

    public LargeArrayHandler()
    {
      InitializeComponent();
    }

    public LargeArrayHandler(int itemsCount, int autoMax, int absoluteMax)
      : this()
    {
      if (itemsCount > absoluteMax)
        ButtonLoadArray.Visibility = System.Windows.Visibility.Hidden;
      else
        ButtonLoadArray.Visibility = System.Windows.Visibility.Visible;

      MessageBlock.Text = string.Format("Array Visualizer is limited to arrays of {0:N0} elements or less.\r\nThe selected array contains {1:N0} elements.", autoMax, itemsCount);              
    }

    private void LoadArray_Click(object sender, RoutedEventArgs e)
    {
      OnLoadArrayRequest();
    }

    protected void OnLoadArrayRequest()
    {
      if (LoadArrayRequest != null)
        LoadArrayRequest(this, new RoutedEventArgs());
    }
  }
}
